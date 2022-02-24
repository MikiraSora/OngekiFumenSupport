﻿using OngekiFumenEditor.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using OngekiFumenEditor.Parser;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl.MetaInfo
{
    [Export(typeof(ICommandParser))]
    class XResolutionCommandParser : MetaInfoCommandParserBase
    {
        public override string CommandLineHeader => "XRESOLUTION";

        public override void ParseMetaInfo(CommandArgs args, OngekiFumen fumen)
        {
            fumen.MetaInfo.XRESOLUTION = args.GetData<int>(1);
        }
    }
}
