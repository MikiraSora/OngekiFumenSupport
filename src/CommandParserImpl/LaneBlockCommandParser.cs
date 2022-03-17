using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using OngekiFumenEditor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OngekiFumenEditor.Base.OngekiObjects.LaneBlockArea;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl
{
    [Export(typeof(ICommandParser))]
    public class LaneBlockCommandParser : CommandParserBase
    {
        public override string CommandLineHeader => "LBK";

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var dataArr = args.GetDataArray<float>();
            var lbk = new LaneBlockArea();
            var laneRecId = (int)dataArr[1];

            var refLaneType = fumen.Lanes.FirstOrDefault(x => x.RecordId == laneRecId)?.LaneType;
            lbk.Direction = refLaneType == LaneType.WallLeft ? BlockDirection.Left : BlockDirection.Right;

            lbk.TGrid.Unit = dataArr[2];
            lbk.TGrid.Grid = (int)dataArr[3];

            lbk.EndIndicator.TGrid.Unit = dataArr[6];
            lbk.EndIndicator.TGrid.Grid = (int)dataArr[7];

            return lbk;
        }
    }
}
