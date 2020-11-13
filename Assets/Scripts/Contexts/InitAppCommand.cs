using AppFsm.State;
using Framewerk.AppStateMachine;
using strange.extensions.command.impl;

namespace Contexts
{
    public class InitAppCommand : Command
    {
        [Inject] public IAppFsm AppFsm{ get; set; }

        public override void Execute()
        {
            AppFsm.SwitchState(new InitAppState());

        }
    }
}