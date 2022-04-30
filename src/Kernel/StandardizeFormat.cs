using Caliburn.Micro;
using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using OngekiFumenEditor.Base.OngekiObjects.Lane.Base;
using OngekiFumenEditor.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.Kernel
{
    public static class StandardizeFormat
    {
        private static async Task<OngekiFumen> CopyFumenObject(OngekiFumen fumen)
        {
            var tmpFilePath = Path.GetTempFileName() + ".ogkr";
            var serializer = IoC.Get<IFumenParserManager>().GetSerializer(tmpFilePath);
            var deserializer = IoC.Get<IFumenParserManager>().GetDeserializer(tmpFilePath);

            await File.WriteAllBytesAsync(tmpFilePath, await serializer.SerializeAsync(fumen));
            using var stream = File.OpenRead(tmpFilePath);
            var newFumen = await deserializer.DeserializeAsync(stream);

            return newFumen;
        }

        public static async Task<OngekiFumen> Process(OngekiFumen currentFumen)
        {
            var fumen = await CopyFumenObject(currentFumen);

            //directly removes objects which not belong to ongeki.
            fumen.SvgPrefabs.Clear();

            var laneMap = new Dictionary<ConnectableStartObject, List<ConnectableStartObject>>();

            foreach ((var beforeLane, var genLanes) in InterpolateAll.Calculate(fumen))
                laneMap[beforeLane] = genLanes.ToList();

            var curveStarts = laneMap.Keys.ToList();

            var affactObjects = InterpolateAll.CalculateAffectedDockableObjects(fumen, curveStarts).ToArray();

            foreach (var item in laneMap)
            {
                var beforeLane = item.Key;
                var afterLanes = item.Value;

                PostProcessInterpolatedConnectableStart(beforeLane, afterLanes);

                fumen.RemoveObject(beforeLane);
                fumen.AddObjects(afterLanes);
            }

            foreach (var obj in affactObjects)
            {
                var tGrid = obj.TGrid;
                var beforeXGrid = obj.XGrid;
                var beforeLane = obj.ReferenceLaneStart;

                (var afterLane, var afterXGrid) =
                    //考虑到处理HoldEnd的refLane之前，已经被前者Hold处理过了
                    (obj.ReferenceLaneStart is not null && laneMap.TryGetValue(obj.ReferenceLaneStart, out var genStarts) ? genStarts : Enumerable.Empty<ConnectableStartObject>())
                    .Where(x => tGrid >= x.MinTGrid && tGrid <= x.MaxTGrid)
                    .Select(x => (x, x.CalulateXGrid(tGrid)))
                    .Where(x => x.Item2 is not null)
                    .OrderBy(x => x.Item2)
                    .FirstOrDefault();

                obj.ReferenceLaneStart = afterLane as LaneStartBase;
            }

            var grids = fumen.GetAllDisplayableObjects()
                .SelectMany(x => new GridBase[] { (x as IHorizonPositionObject)?.XGrid, (x as ITimelineObject)?.TGrid })
                .OfType<GridBase>();
            foreach (var grid in grids)
            {
                RecalcGrid(grid);
            }

            return fumen;
        }

        private static void RecalcGrid(GridBase grid)
        {
            var fixedPointPart = grid.Unit - (int)grid.Unit;

            grid.Grid = (int)Math.Floor(grid.GridRadix * fixedPointPart + 0.5f) + grid.Grid;
            grid.Unit = (int)grid.Unit;

            grid.NormalizeSelf();
        }

        private static void PostProcessInterpolatedConnectableStart(ConnectableStartObject rawStart, List<ConnectableStartObject> genStarts)
        {

        }
    }
}
