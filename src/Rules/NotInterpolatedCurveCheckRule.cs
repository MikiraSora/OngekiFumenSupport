﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using OngekiFumenEditor.Modules.FumenCheckerListViewer.Base.DefaultNavigateBehaviorImpl;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Modules.FumenCheckerListViewer.Base.DefaultRulesImpl
{
    [Export(typeof(IFumenCheckRule))]
    internal class NotInterpolatedCurveCheckRule : IFumenCheckRule
    {
        public IEnumerable<ICheckResult> CheckRule(OngekiFumen fumen, FumenVisualEditorViewModel fumenHostViewModel)
        {
            IEnumerable<ICheckResult> CheckList(IEnumerable<ConnectableChildObjectBase> objs)
            {
                const string RuleName = "[Ongeki]NotInterpolatedCurve";

                foreach (var obj in objs.Where(x => x.IsCurvePath))
                {
                    yield return new CommonCheckResult()
                    {
                        Severity = RuleSeverity.Error,
                        Description = $"轨道物件曲线还没被插值",
                        LocationDescription = $"{obj.XGrid} {obj.TGrid}",
                        NavigateBehavior = new NavigateToTGridBehavior(obj.TGrid),
                        RuleName = RuleName,
                    };
                }
            }

            foreach (var result in CheckList(fumen.GetAllDisplayableObjects().OfType<ConnectableChildObjectBase>()))
                yield return result;
        }
    }
}

