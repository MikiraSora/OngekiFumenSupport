﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using OngekiFumenEditor.Parser;
using System.Threading.Tasks;
using static OngekiFumenEditor.Base.OngekiObjects.BulletPallete;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport.CommandParserImpl
{
    [Export(typeof(ICommandParser))]
    public class BulletPalleteCommandParser : CommandParserBase
    {
        public override string CommandLineHeader => CommandName;

        public override OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen)
        {
            var dataIntArr = args.GetDataArray<float>();
            var dataStrArr = args.GetDataArray<string>();
            var bpl = new BulletPallete();

            bpl.StrID = dataStrArr.ElementAtOrDefault(1);
            bpl.ShooterValue = dataStrArr.ElementAtOrDefault(2)?.ToUpper() switch
            {
                "UPS" => Shooter.TargetHead,
                "ENE" => Shooter.Enemy,
                "CEN" => Shooter.Center,
                _ => throw new NotImplementedException(),
            };
            bpl.PlaceOffset = (int)dataIntArr.ElementAtOrDefault(3);
            bpl.TargetValue = dataStrArr.ElementAtOrDefault(4)?.ToUpper() switch
            {
                "PLR" => Target.Player,
                "FIX" => Target.FixField,
                _ => throw new NotImplementedException(),
            };
            bpl.Speed = dataIntArr.ElementAtOrDefault(5);
            bpl.SizeValue = dataStrArr.ElementAtOrDefault(6)?.ToUpper() switch
            {
                "L" => BulletSize.Large,
                "N" or _ => BulletSize.Normal,
            };
            bpl.TypeValue = dataStrArr.ElementAtOrDefault(7)?.ToUpper() switch
            {
                "SQR" => BulletType.Square,
                "NDL" => BulletType.Needle,
                "CIR" or _=> BulletType.Circle,
            };

            return bpl;
        }
    }
}
