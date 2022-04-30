using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.EditorObjects.Svg;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl.Editor
{
    [Export(typeof(ICommandParser))]
    public class SvgPrefabCommand : CommandParserBase
    {
        public override string CommandLineHeader => "Svg";

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var svg = new SvgPrefab();

            svg.LimitXGridUnitSimply = args.GetData<bool>(1);
            svg.ColorSimilar.CurrentValue = args.GetData<float>(2);
            svg.Rotation.CurrentValue = args.GetData<float>(3);
            svg.EnableColorfulLaneSimilar = args.GetData<bool>(4);
            svg.OffsetX.CurrentValue = args.GetData<float>(5);
            svg.OffsetY.CurrentValue = args.GetData<float>(6);
            svg.ShowOriginColor = args.GetData<bool>(7);
            svg.Opacity.CurrentValue = args.GetData<float>(8);
            svg.Scale = args.GetData<float>(9);
            svg.Tolerance.CurrentValue = args.GetData<float>(10);
            svg.TGrid = new TGrid(args.GetData<float>(11), args.GetData<int>(12));
            svg.XGrid = new XGrid(args.GetData<float>(13), args.GetData<int>(14));
            svg.SvgFile = new System.IO.FileInfo(Encoding.UTF8.GetString(Convert.FromBase64String(args.GetData<string>(15))));

            return svg;
        }
    }
}
