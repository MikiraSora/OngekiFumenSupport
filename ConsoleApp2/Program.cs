// See https://aka.ms/new-console-template for more information
using Caliburn.Micro;
using OngekiFumenEditor;
using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.Collections;
using OngekiFumenEditor.Modules.FumenVisualEditor;
using OngekiFumenEditor.Utils;
using OngekiFumenEditorPlugins.OngekiFumenSupport;
using System.IO;

var app = new AppBootstrapper(false);

var parser = new DefaultOngekiFumenParser(IoC.GetAll<ICommandParser>());
using var fs = File.OpenRead(@"F:\ongeki bright memory\package\option\A032\music\music8090\8090_10.ogkr");
var fumen = await parser.DeserializeAsync(fs);

var pot = fumen.Soflans.GetCachedSoflanPositionList_PreviewMode(240, fumen.BpmList);

Console.WriteLine();