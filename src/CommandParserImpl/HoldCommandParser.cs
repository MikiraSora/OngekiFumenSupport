﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using OngekiFumenEditor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using OngekiFumenEditor.Parser;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl
{
    [Export(typeof(ICommandParser))]
    public class HoldCommandParser : CommandParserBase
    {
        public override string CommandLineHeader => "HLD";

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var dataArr = args.GetDataArray<float>();

            var laneId = args.GetData<int>(1);
            var refLaneStart = fumen.Lanes.FirstOrDefault(x => x.RecordId == laneId);
            if (refLaneStart is null)
            {
                Log.LogWarn($"Tap parse can't find lane/wallLane RecordId = {laneId}");
            }
            var hold = (refLaneStart?.IsWallLane ?? false) ? new WallHold() : new Hold();

            hold.ReferenceLaneStart = refLaneStart;

            hold.IsCritical = args.GetData<string>(0) == "CHD" || args.GetData<string>(0) == "XHD";

            hold.TGrid.Unit = dataArr[2];
            hold.TGrid.Grid = (int)dataArr[3];
            hold.XGrid.Unit = dataArr[4];
            hold.XGrid.Grid = (int)dataArr[5];

            if (dataArr.Length > 6)
            {
                var holdEnd = (refLaneStart?.IsWallLane ?? false) ? new WallHoldEnd() : new HoldEnd();
                hold.AddChildObject(holdEnd);

                holdEnd.TGrid.Unit = dataArr[6];
                holdEnd.TGrid.Grid = (int)dataArr[7];
                holdEnd.XGrid.Unit = dataArr[8];
                holdEnd.XGrid.Grid = (int)dataArr[9];
            }

            return hold;
        }
    }

    [Export(typeof(ICommandParser))]
    public class CriticalHoldCommandParser1 : HoldCommandParser
    {
        public override string CommandLineHeader => "CHD";
    }

    [Export(typeof(ICommandParser))]
    public class CriticalHoldCommandParser2 : HoldCommandParser
    {
        public override string CommandLineHeader => "XHD";
    }
}
