using Gemini.Framework.Commands;

namespace OngekiFumenEditor.Kernel.MiscMenu.Commands
{
    [CommandDefinition]
    public class StandardizeFormatCommandDefinition : CommandDefinition
    {
        public const string CommandName = "OngekiFumen.StandardizeFormat";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "生成标准音击谱面"; }
        }

        public override string ToolTip
        {
            get { return Text; }
        }
    }
}