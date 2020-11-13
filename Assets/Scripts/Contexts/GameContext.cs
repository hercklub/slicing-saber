using AppFsm.Screen;
using Blade;
using Blades;
using Definitions;
using Framewerk;
using Framewerk.AppStateMachine;
using Framewerk.Managers;
using Framewerk.StrangeCore;
using Plugins.Framewerk;
using strange.extensions.injector.api;
using UI;
using UnityEngine;

namespace Contexts
{
    public enum BindingKeys
    {
        ClientContextId,
    }

    public class GameContext : FramewerkMVCSContext
    {
        private ViewConfig _viewConfig;
        private int _contextId;

        public GameContext(MonoBehaviour view, ViewConfig viewConfig, int contextId) : base(view,
            true)
        {
            _viewConfig = viewConfig;
            _contextId = contextId;
        }

        protected override void mapBindings()
        {
            base.mapBindings();
            
            //general
            injectionBinder.Bind<int>().ToValue(_contextId).ToName(BindingKeys.ClientContextId);

            injectionBinder.Bind<ViewConfig>().ToValue(_viewConfig);
            injectionBinder.Bind<IInjector>().To(injectionBinder.injector);

            injectionBinder.Bind<IUpdater>().To(Updater.Instance);
            injectionBinder.Bind<ICoroutineManager>().To(CoroutineManager.Instance);
            injectionBinder.Bind<IAssetManager>().To<AssetManager>().ToSingleton();
            injectionBinder.Bind<IUiManager>().To<UiManager>().ToSingleton();

            //signals 
            injectionBinder.Bind<ButtonClickedSignal>().ToSingleton();
            injectionBinder.Bind<BladeSelectSignal>().ToSingleton();

            //commands
            commandBinder.Bind<ContextStartSignal>().To<InitAppCommand>();
            commandBinder.Bind<InitGameSignal>().To<InitGameCommand>();
            
            //impl
            injectionBinder.Bind<IBladesEffectsImpl>().To<BladesEffectsImpl>().ToSingleton();

            //models
            injectionBinder.Bind<IBladesModel>().To<BladesModel>().ToSingleton();

            //defs
            injectionBinder.Bind<IBladesDataDefinitions>().To<BladesDataDefinitions>().ToSingleton();

            //fsm 
            injectionBinder.Bind<IAppFsm>().To<Framewerk.AppStateMachine.AppFsm>().ToSingleton();
            injectionBinder.Bind<AppStateEnterSignal>().ToSingleton();
            injectionBinder.Bind<AppStateExitSignal>().ToSingleton();
            injectionBinder.Bind<InitAppScreen>().To<InitAppScreen>();
            injectionBinder.Bind<GameAppScreen>().To<GameAppScreen>();
            

            //view mediation
            //ui
            mediationBinder.Bind<IntroPopupView>().To<IntroPopupMediator>();
            mediationBinder.Bind<BladeListView>().To<BladeListMediator>();
            mediationBinder.Bind<BladeListItemView>().To<BladeListItemMediator>();

           //game
            mediationBinder.Bind<BladeView>().To<BladeMediator>();
            mediationBinder.Bind<BladesEffectView>().To<BladesEffectMediator>();


        }
    }
}