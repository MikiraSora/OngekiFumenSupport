using Gemini.Framework.Commands;

namespace OngekiFumenEditor.Kernel.MiscMenu.Commands
{
    [CommandDefinition]
    public class InterpolateAllCommandDefinition : CommandDefinition
    {
        public const string CommandName = "OngekiFumen.InterpolateAll";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "插值所有曲线轨道"; }
        }

        public override string ToolTip
        {
            get { return Text; }
        }
    }
}