﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using OngekiFumenEditor.Base.OngekiObjects.Lane.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.Kernel;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels;
using OngekiFumenEditor.Utils;
using OngekiFumenEditorPlugins.OngekiFumenSupport.Kernel;
using OngekiFumenEditorPlugins.OngekiFumenSupport.MenuCommands;

namespace OngekiFumenEditor.Kernel.MiscMenu.Commands
{

    [CommandHandler]
    public class InterpolateAllCommandHandler : InterpolateAllCommandHandlerBase<InterpolateAllCommandDefinition>
    {
        public override void Update(Command command)
        {
            base.Update(command);
            command.Enabled = IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not null;
        }

        public override Task Run(Command command)
        {
            if (IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not FumenVisualEditorViewModel editor)
                return TaskUtility.Completed;
            if (MessageBox.Show("是否插值所有包含曲线的轨道物件?\n可能将会删除并重新生成已经插值好的,不含曲线的轨道物件\n部分高度重叠的Tap/Hold物件可能会因此改变它依赖的轨道物件", "提醒", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return TaskUtility.Completed;
            editor.LockAllUserInteraction();

            Process(editor, false);

            editor.UnlockAllUserInteraction();
            return TaskUtility.Completed;
        }
    }
    [CommandHandler]
    public class InterpolateAllWithXGridLimitCommandHandler : InterpolateAllCommandHandlerBase<InterpolateAllWithXGridLimitCommandDefinition>
    {
        public override void Update(Command command)
        {
            base.Update(command);
            command.Enabled = IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not null;
        }

        public override Task Run(Command command)
        {
            if (IoC.Get<IEditorDocumentManager>().CurrentActivatedEditor is not FumenVisualEditorViewModel editor)
                return TaskUtility.Completed;
            if (MessageBox.Show("是否插值所有包含曲线的轨道物件?\n可能将会删除并重新生成已经插值好的,不含曲线的轨道物件\n部分高度重叠的Tap/Hold物件可能会因此改变它依赖的轨道物件", "提醒", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return TaskUtility.Completed;
            editor.LockAllUserInteraction();

            Process(editor, true);

            editor.UnlockAllUserInteraction();
            return TaskUtility.Completed;
        }
    }
}