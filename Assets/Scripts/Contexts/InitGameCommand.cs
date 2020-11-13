using System.Collections;
using AppFsm.State;
using Framewerk.AppStateMachine;
using Framewerk.Managers;

using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts
{
    public class InitGameSignal : Signal
    {
    }

    public class InitGameCommand : Command
    {
        
        [Inject] public IAppFsm AppFsm{ get; set; }
        
        public override void Execute()
        {
            AppFsm.SwitchState(new GameAppState());
        }
        

    }
}