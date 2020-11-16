using LevelEditor.EventsList;
using LevelEditor.IO;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace LevelEditor
{
    public class LoadLevelSignal : Signal
    {
    }

    public class LoadLevelCommand : Command
    {
        [Inject] public ShowEditorLevelMenuSignal ShowEditorLevelMenuSignal { get; set; }

        
        public override void Execute()
        {
            // show ui
            ShowEditorLevelMenuSignal.Dispatch(true);
            
        }
    }
}