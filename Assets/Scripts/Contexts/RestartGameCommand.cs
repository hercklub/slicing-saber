using Enemy.Models;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace Contexts
{
    public class RestartGameSignal : Signal
    {
    }

    public class RestartGameCommand : Command
    {
        [Inject] public IEnemyDataModels EnemyDataModels { get; set; }

        public override void Execute()
        {
            EnemyDataModels.Init();
        }
    }
}