using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Microsoft.Win32;
using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using OngekiFumenEditor.Base.OngekiObjects.Lane.Base;
using OngekiFumenEditor.Kernel.Audio;
using OngekiFumenEditor.Modules.FumenVisualEditor;
using OngekiFumenEditor.Modules.FumenVisualEditor.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.Kernel;
using OngekiFumenEditor.Modules.FumenVisualEditor.Models;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels;
using OngekiFumenEditor.Parser;
using OngekiFumenEditor.Utils;
using OngekiFumenEditorPlugins.OngekiFumenSupport.Kernel;

namespace OngekiFumenEditor.Kernel.MiscMenu.Commands
{
    [CommandHandler]
    public class FastOpenFumenCommandHandler : CommandHandlerBase<FastOpenFumenCommandDefinition>
    {
        public override async Task Run(Command command)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileDialogHelper.BuildExtensionFilter((".ogkr", "已标准化的音击谱面"));
            openFileDialog.Title = "新的谱面文件输出保存路径";
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() != true)
                return;

            var ogkrFilePath = openFileDialog.FileName;

            (var audioFile, var audioDuration) = await GetAudioFilePath(ogkrFilePath);

            if (!File.Exists(audioFile))
            {
                audioFile = FileDialogHelper.OpenFile("手动选择音频文件", IoC.Get<IAudioManager>().SupportAudioFileExtensionList);
                if (!File.Exists(audioFile))
                    return;
            }

            using var fs = File.OpenRead(ogkrFilePath);
            var fumen = await IoC.Get<IFumenParserManager>().GetDeserializer(ogkrFilePath).DeserializeAsync(fs);

            var newProj = new EditorProjectDataModel();
            newProj.FumenFilePath = ogkrFilePath;
            newProj.Fumen = fumen;
            newProj.AudioFilePath = audioFile;
            newProj.AudioDuration = audioDuration;

            var provider = IoC.Get<IFumenVisualEditorProvider>();
            var editor = IoC.Get<IFumenVisualEditorProvider>().Create();
            var viewAware = (IViewAware)editor;
            viewAware.ViewAttached += (sender, e) =>
            {
                var frameworkElement = (FrameworkElement)e.View;

                RoutedEventHandler loadedHandler = null;
                loadedHandler = async (sender2, e2) =>
                {
                    frameworkElement.Loaded -= loadedHandler;
                    await provider.Open(editor, newProj);
                };
                frameworkElement.Loaded += loadedHandler;
            };

            await IoC.Get<IShell>().OpenDocumentAsync(editor);
        }

        private async Task<(string, float)> GetAudioFilePath(string ogkrFilePath)
        {
            var ogkrFileDir = Path.GetDirectoryName(ogkrFilePath);
            var musicXmlFilePath = Path.Combine(ogkrFileDir, "Music.xml");
            var musicId = -2857;

            if (File.Exists(musicXmlFilePath))
            {
                //从Music.xml读取musicId
                var musicXml = XDocument.Parse(File.ReadAllText(musicXmlFilePath));
                var element = musicXml.XPathSelectElement(@"//MusicSourceName[1]/id[1]");
                if (element != null)
                {
                    musicId = int.Parse(element.Value);
                }
            }

            if (musicId < 0)
            {
                //从文件名读取musicId
                var match = new Regex(@"(\d+)_\d+").Match(Path.GetFileNameWithoutExtension(ogkrFilePath));
                if (match.Success)
                {
                    musicId = int.Parse(match.Groups[0].Value);
                }
            }

            if (musicId < 0)
            {
                return default;
            }

            var musicSourcePath = Path.Combine(ogkrFileDir, "..", "..", "musicsource", $"musicsource{musicId}");
            var audioExts = IoC.Get<IAudioManager>().SupportAudioFileExtensionList.Select(x => x.fileExt.TrimStart('.')).ToArray();
            var audioFile = "";

            if (Directory.Exists(musicSourcePath))
            {
                //去对应的musicsource文件夹检查
                audioFile = Directory.GetFiles(musicSourcePath, $"music{musicId}.*").Where(x => audioExts.Any(t => x.EndsWith(t))).FirstOrDefault();
            }

            if (!File.Exists(audioFile))
            {
                return default;
            }

            using var audio = await IoC.Get<IAudioManager>().LoadAudioAsync(audioFile);
            var durationMs = audio.Duration;

            return (audioFile, durationMs);
        }
    }
}