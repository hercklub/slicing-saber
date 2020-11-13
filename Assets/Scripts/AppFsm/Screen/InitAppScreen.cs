using Common;
using Framewerk.AppStateMachine;
using UI;

namespace AppFsm.Screen
{
    public class InitAppScreen : AppStateScreen
    {
        protected override void Enter()
        {
            base.Enter();
            InstantiateView<IntroPopupView>(ResourcePath.POPUPS_ROOT, ViewConfig.Popups);
        }
    }

}