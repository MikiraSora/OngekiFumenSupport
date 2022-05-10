﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.EditorObjects.Svg;
using OngekiFumenEditor.Base.OngekiObjects;
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
    public abstract class SvgPrefabCommandBase : ICommandParser
    {
        public abstract string CommandLineHeader { get; }

        public void AfterParse(OngekiObjectBase obj, OngekiFumen fumen)
        {

        }

        public OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var svg = CreateAndParseSvgObject(args, fumen);

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
            svg.ColorfulLaneBrightness.CurrentValue = args.GetData<int>(14);

            return svg;
        }

        public abstract SvgPrefabBase CreateAndParseSvgObject(CommandArgs args, OngekiFumen fumen);
    }

    [Export(typeof(ICommandParser))]
    public class SvgImageFilePrefabCommand : SvgPrefabCommandBase
    {
        public override string CommandLineHeader => "SVG_IMG";

        public override SvgPrefabBase CreateAndParseSvgObject(CommandArgs args, OngekiFumen fumen)
        {
            var svg = new SvgImageFilePrefab();
            svg.SvgFile = new System.IO.FileInfo(Encoding.UTF8.GetString(Convert.FromBase64String(args.GetData<string>(14))));
            return svg;
        }
    }

    [Export(typeof(ICommandParser))]
    public class SvgStringPrefabCommand : SvgPrefabCommandBase
    {
        public override string CommandLineHeader => "SVG_STR";

        public override SvgPrefabBase CreateAndParseSvgObject(CommandArgs args, OngekiFumen fumen)
        {
            var svg = new SvgStringPrefab();
            var i = 15;

            svg.Content = Encoding.UTF8.GetString(Convert.FromBase64String(args.GetData<string>(i++)));
            svg.FontSize = args.GetData<double>(i++);
            var colorId = args.GetData<int>(i++);
            svg.FontColor = ColorIdConst.AllColors.FirstOrDefault(x => x.Id == colorId);
            svg.TypefaceName = Base64.Decode(args.GetData<string>(i++));
            svg.ContentFlowDirection = Enum.Parse<SvgStringPrefab.FlowDirection>(args.GetData<string>(i++));
            svg.ContentLineHeight = args.GetData<double>(i++);
            return svg;
        }
    }
}
