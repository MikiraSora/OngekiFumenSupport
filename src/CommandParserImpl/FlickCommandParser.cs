﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OngekiFumenEditor.Parser;

namespace OngekiFumenEditorPlugins.OngekiFumenParser.CommandParserImpl
{
    [Export(typeof(ICommandParser))]
    public class FlickCommandParser : CommandParserBase
    {
        public override string CommandLineHeader => "FLK";

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var dataArr = args.GetDataArray<float>();
            var flick = new Flick();

            flick.IsCritical = args.GetData<string>(0) == "CFK";

            flick.TGrid.Unit = dataArr[1];
            flick.TGrid.Grid = (int)dataArr[2];
            flick.XGrid.Unit = dataArr[3];

            flick.Direction = args.GetData<string>(4) == "L" ? Flick.FlickDirection.Left : Flick.FlickDirection.Right;

            return flick;
        }
    }

    [Export(typeof(ICommandParser))]
    public class CriticalFlickCommandParser : FlickCommandParser
    {
        public override string CommandLineHeader => "CFK";
    }
}
