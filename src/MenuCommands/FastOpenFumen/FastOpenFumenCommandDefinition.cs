using Gemini.Framework.Commands;

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
    }
}