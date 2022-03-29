using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.EditorObjects.LaneCurve;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using OngekiFumenEditor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl.Editor
{
    [Export(typeof(ICommandParser))]
    public class CurvePrecisionCommand : CommandParserBase
    {
        public override string CommandLineHeader => "LCO_PREC";

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var data = args.GetDataArray<float>();

            var starts = fumen.Lanes.AsEnumerable<ConnectableStartObject>().Concat(fumen.Beams);

            var laneId = (int)data[1];
            var childOrder = (int)data[2];
            if (starts.FirstOrDefault(x=>x.RecordId == laneId) is not ConnectableStartObject start)
                throw new Exception($"can't parse LCO_PREC because lane object (laneId:{laneId}) is not found.");
            if (start.Children.ElementAt(childOrder) is not ConnectableChildObjectBase child)
                throw new Exception($"can't parse LCO_PREC because child object (childOrder:{childOrder}) is not found.");

            child.CurvePrecision = data[3];
            return null;
        }
    }
}
