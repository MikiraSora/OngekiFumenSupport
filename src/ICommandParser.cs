﻿using OngekiFumenEditor.Base;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport
{
    public interface ICommandParser
    {
        public string CommandLineHeader { get; }
        public OngekiObjectBase Parse(CommandArgs args,OngekiFumen fumen);
        public void AfterParse(OngekiObjectBase obj, OngekiFumen fumen);
    }
}
