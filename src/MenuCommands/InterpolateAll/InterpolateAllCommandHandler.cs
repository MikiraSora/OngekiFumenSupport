using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects.Lane.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.Kernel;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels;
using OngekiFumenEditor.Utils;

namespace OngekiFumenEditor.Kernel.MiscMenu.Commands
{
    [CommandHandler]
    public class InterpolateAllCommandHandler : CommandHandlerBase<InterpolateAllCommandDefinition>
    {
        public override void Update(Command command)
        {
            base.Update(command);
            command.Enabled = IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not null;
        }

        public override Task Run(Command command)
        {
            if (IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not FumenVisualEditorViewModel editor)
                return TaskUtility.Completed;
            if (MessageBox.Show("是否插值所有包含曲线的轨道物件?\n可能将会删除并重新生成已经插值好的,不含曲线的轨道物件\n部分高度重叠的Tap/Hold物件可能会因此改变它依赖的轨道物件", "提醒", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return TaskUtility.Completed;
            editor.LockAllUserInteraction();

            Process(editor);

            editor.UnlockAllUserInteraction();
            return TaskUtility.Completed;
        }

        private void Process(FumenVisualEditorViewModel editor)
        {
            var fumen = editor.Fumen;
            var curveStarts = fumen.Lanes.Where(x => x.Children.Any(x => x.PathControls.Count > 0)).ToList();
            var affactObjects = fumen.Taps
                .AsEnumerable<ILaneDockable>()
                .Concat(fumen.Holds.SelectMany(x => new ILaneDockable[] { x, x.HoldEnd }))
                .Where(x => x.ReferenceLaneStart?.RecordId is int id && curveStarts.Any(y => y.RecordId == id)).ToList();

            var laneMap = curveStarts.ToDictionary(
                x => x.RecordId,
                x => x.InterpolateCurve().ToArray());

            var redoAction = new System.Action(() => { });

            var undoAction = new System.Action(() => { });

            foreach (var item in laneMap)
            {
                var beforeLane = curveStarts.FirstOrDefault(x => x.RecordId == item.Key);
                var afterLanes = item.Value;

                redoAction += () =>
                {
                    fumen.RemoveObject(beforeLane);
                    fumen.AddObjects(afterLanes);
                };

                undoAction += () =>
                {
                    fumen.AddObject(beforeLane);
                    fumen.RemoveObjects(afterLanes);
                };
            }

            foreach (var obj in affactObjects)
            {
                var tGrid = obj.TGrid;
                var beforeXGrid = obj.XGrid;
                var beforeLane = obj.ReferenceLaneStart;

                (var afterLane, var afterXGrid) = laneMap[obj.ReferenceLaneStart.RecordId]
                    .Where(x => tGrid >= x.MinTGrid && tGrid <= x.MaxTGrid)
                    .Select(x => (x, x.CalulateXGrid(tGrid)))
                    .Where(x => x.Item2 is not null)
                    .OrderBy(x => x.Item2)
                    .FirstOrDefault();

                redoAction += () =>
                {
                    obj.ReferenceLaneStart = afterLane as LaneStartBase;
                    //obj.XGrid = afterXGrid;
                };

                undoAction += () =>
                {
                    obj.ReferenceLaneStart = beforeLane;
                    //obj.XGrid = beforeXGrid;
                };
            }

            redoAction += () =>
            {
                editor.Redraw(RedrawTarget.OngekiObjects);
            };

            undoAction += () =>
            {
                editor.Redraw(RedrawTarget.OngekiObjects);
            };

            editor.UndoRedoManager.ExecuteAction(LambdaUndoAction.Create("插值所有曲线轨道", redoAction, undoAction));
            Log.LogInfo($"插值计算完成,一共对 {curveStarts.Count} 条符合条件的轨道进行插值,生成了 {laneMap.Values.Select(x => x.Length).Sum()} 条新的轨道,对应 {affactObjects.Count()} 个受到影响的Tap/Hold等物件进行重新计算。");
        }
    }
}