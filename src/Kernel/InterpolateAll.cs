﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.Kernel
{
    public static class InterpolateAll
    {
        public static IEnumerable<(ConnectableStartObject beforeStart, IEnumerable<ConnectableStartObject> genStarts)> Calculate(OngekiFumen fumen)
        {
            var curveStarts = fumen.Lanes.Where(x => x.Children.Any(x => x.PathControls.Count > 0)).ToList();

            var laneMap = curveStarts.ToDictionary(
                x => x.RecordId,
                x => x.InterpolateCurve().ToArray());

            foreach (var item in laneMap)
            {
                var beforeLane = curveStarts.FirstOrDefault(x => x.RecordId == item.Key);
                var afterLanes = item.Value;
                yield return (beforeLane, afterLanes);
            }
        }

        public static IEnumerable<ILaneDockable> CalculateAffectedDockableObjects(OngekiFumen fumen, IEnumerable<ConnectableStartObject> curveStarts)
        {
            return fumen.Taps
                .AsEnumerable<ILaneDockable>()
                .Concat(fumen.Holds.SelectMany(x => new ILaneDockable[] { x, x.HoldEnd }))
                .Where(x => x.ReferenceLaneStart?.RecordId is int id && curveStarts.Any(y => y.RecordId == id));
        }
    }
}