﻿using Gemini.Framework.Commands;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace OngekiFumenEditor.Kernel.MiscMenu.Commands
{
    [CommandDefinition]
    public class FastOpenFumenCommandDefinition : CommandDefinition
    {
        public const string CommandName = "OngekiFumen.FastOpenFumen";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "快速打开.ogkr谱面"; }
        }

        public override string ToolTip
        {
            get { return Text; }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<FastOpenFumenCommandDefinition>(new(Key.F, ModifierKeys.Control));
    }
}