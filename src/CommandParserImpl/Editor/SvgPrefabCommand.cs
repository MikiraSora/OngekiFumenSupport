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

            svg.ColorSimilar.CurrentValue = args.GetData<float>(1);
            svg.Rotation.CurrentValue = args.GetData<float>(2);
            svg.EnableColorfulLaneSimilar = args.GetData<bool>(3);
            svg.OffsetX.CurrentValue = args.GetData<float>(4);
            svg.OffsetY.CurrentValue = args.GetData<float>(5);
            svg.ShowOriginColor = args.GetData<bool>(6);
            svg.Opacity.CurrentValue = args.GetData<float>(7);
            svg.Scale = args.GetData<float>(8);
            svg.Tolerance.CurrentValue = args.GetData<float>(9);
            svg.TGrid = new TGrid(args.GetData<float>(10), args.GetData<int>(11));
            svg.XGrid = new XGrid(args.GetData<float>(12), args.GetData<int>(13));
            svg.SvgFile = new System.IO.FileInfo(Encoding.UTF8.GetString(Convert.FromBase64String(args.GetData<string>(14))));

            return svg;
        }
    }
}
