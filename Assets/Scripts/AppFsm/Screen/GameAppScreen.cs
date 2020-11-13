using Blade;
using Framewerk.AppStateMachine;

namespace AppFsm.Screen
{
    public class GameAppScreen : AppStateScreen
    {
        protected override void Enter()
        {
            base.Enter();
            InstantiateView<BladeListView>();
            InstantiateGamePrefab<BladeView>();
        }
    }
}