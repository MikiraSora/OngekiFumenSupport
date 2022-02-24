﻿using OngekiFumenEditor.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using OngekiFumenEditor.Parser;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenParser.CommandParserImpl.MetaInfo
{
    [Export(typeof(ICommandParser))]
    class TResolutionCommandParser : MetaInfoCommandParserBase
    {
        public override string CommandLineHeader => "TRESOLUTION";

        public override void ParseMetaInfo(CommandArgs args, OngekiFumen fumen)
        {
            fumen.MetaInfo.TRESOLUTION = args.GetData<int>(1);
        }
    }
}
