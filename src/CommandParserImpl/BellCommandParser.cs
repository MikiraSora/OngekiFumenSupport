﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using OngekiFumenEditor.Parser;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl
{
    [Export(typeof(ICommandParser))]
    public class BellCommandParser : CommandParserBase
    {
        public override string CommandLineHeader => Bell.CommandName;

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var dataArr = args.GetDataArray<float>();
            var bell = new Bell();

            bell.TGrid.Unit = dataArr[1];
            bell.TGrid.Grid = (int)dataArr[2];
            bell.XGrid.Unit = dataArr[3];

            var palleteId = args.GetData<string>(4);
            if (!string.IsNullOrWhiteSpace(palleteId) && palleteId != "--")
                bell.ReferenceBulletPallete = fumen.BulletPalleteList.FirstOrDefault(x => x.StrID == palleteId);

            return bell;
        }
    }
}
