using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using OngekiFumenEditor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl
{
    [Export(typeof(ICommandParser))]
    public class LaneBlockCommandParser : CommandParserBase
    {
        public override string CommandLineHeader => "LBK";

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var dataArr = args.GetDataArray<float>();
            var hold = new LaneBlockArea();

            hold.TGrid.Unit = dataArr[2];
            hold.TGrid.Grid = (int)dataArr[3];

            hold.EndIndicator.TGrid.Unit = dataArr[6];
            hold.EndIndicator.TGrid.Grid = (int)dataArr[7];

            return hold;
        }
    }
}
