﻿using OngekiFumenEditor.Base;

namespace OngekiFumenEditorPlugins.OngekiFumenSupport
{
    public abstract class CommandParserBase : ICommandParser
    {
        public abstract string CommandLineHeader { get; }

        public virtual void AfterParse(OngekiObjectBase obj, OngekiFumen fumen) { }

        public abstract OngekiObjectBase Parse(CommandArgs args, OngekiFumen fumen);
    }
}
