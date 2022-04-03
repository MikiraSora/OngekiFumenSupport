using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Microsoft.Win32;
using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using OngekiFumenEditor.Base.OngekiObjects.Lane.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.Kernel;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels;
using OngekiFumenEditor.Parser;
using OngekiFumenEditor.Utils;
using OngekiFumenEditorPlugins.OngekiFumenSupport.Kernel;

namespace OngekiFumenEditor.Kernel.MiscMenu.Commands
{
    [CommandHandler]
    public class StandardizeFormatCommandHandler : CommandHandlerBase<StandardizeFormatCommandDefinition>
    {
        public override void Update(Command command)
        {
            base.Update(command);
            command.Enabled = IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not null;
        }

        public override async Task Run(Command command)
        {
            if (IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not FumenVisualEditorViewModel editor)
                return;
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FileDialogFilterHelper.BuildExtensionFilter((".ogkr", "已标准化的音击谱面"));
            saveFileDialog.Title = "新的谱面文件输出保存路径";

            if (saveFileDialog.ShowDialog() != true)
                return;

            editor.LockAllUserInteraction();

            var newFilePath = saveFileDialog.FileName;

            var fumen = await StandardizeFormat.Process(newFilePath, editor.Fumen);

            editor.UnlockAllUserInteraction();

            var serializer = IoC.Get<IFumenParserManager>().GetSerializer(newFilePath);
            await File.WriteAllBytesAsync(newFilePath, await serializer.SerializeAsync(fumen));

            if (MessageBox.Show("音击谱面标准化输出,处理完成\n是否立即打开输出文件夹", "生成标准音击谱面", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start(new ProcessStartInfo(Path.GetDirectoryName(newFilePath))
                {
                    UseShellExecute = true
                });
            }

            return;
        }

    }
}