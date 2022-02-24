﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using OngekiFumenEditor.Parser;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenParser.CommandParserImpl
{
    [Export(typeof(ICommandParser))]
    public class BpmCommandParser : CommandParserBase
    {
        public override string CommandLineHeader => BPMChange.CommandName;

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var dataArr = args.GetDataArray<float>();
            var bpm = new BPMChange();

            bpm.TGrid.Unit = dataArr[1];
            bpm.TGrid.Grid = (int)dataArr[2];
            bpm.BPM = dataArr[3];

            return bpm;
        }
    }
}
