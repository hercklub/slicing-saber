using Plugins.Framewerk;
using strange.extensions.context.impl;

namespace Contexts
{
    public class Bootstrap : ContextView
    {
        private static int _clientContextId = 0;
        public ViewConfig ViewConfig;
        
        private void Start()
        {
            //Client context
            context = new GameContext(this, ViewConfig, _clientContextId++);
            context.Start();
        }
    }
}