using LevelEditor.IO;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace LevelEditor
{
    public class SaveLevelSignal : Signal
    {
    }

    public class SaveLevelCommand : Command
    {

        [Inject] public ILevelImporter LevelImporter { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }

        public override void Execute()
        {
            // gather data from model
            // convert to json data structure
            // open/save to file
            //LevelImporter.SaveCustomLevelToFile(EnemyEventsModel.AllEnemies);
        }
    }
}