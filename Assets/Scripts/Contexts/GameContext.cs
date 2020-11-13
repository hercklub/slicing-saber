using AppFsm.Screen;
using Arena;
using Blade;
using Blades;
using Common;
using Definitions;
using Enemies;
using Enemy;
using Enemy.Models;
using Framewerk;
using Framewerk.AppStateMachine;
using Framewerk.Managers;
using Framewerk.StrangeCore;
using Plugins.Framewerk;
using strange.extensions.injector.api;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
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

        private EnemyInstanceProvider _enemyInstanceProvider;
        public GameContext(MonoBehaviour view, ViewConfig viewConfig, int contextId) : base(view,
            true)
        {
            _viewConfig = viewConfig;
            _contextId = contextId;
            _enemyInstanceProvider = new EnemyInstanceProvider();
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
            
            //instance providers
            injectionBinder.Bind<EnemyInstanceProvider>().ToValue(_enemyInstanceProvider);

            //signals 
            injectionBinder.Bind<ButtonClickedSignal>().ToSingleton();
            injectionBinder.Bind<BladeSelectedSignal>().ToSingleton();
            injectionBinder.Bind<EnemyAddedSignal>().ToSingleton();
            injectionBinder.Bind<EnemyRemovedSignal>().ToSingleton();

            //commands
            commandBinder.Bind<ContextStartSignal>().To<InitAppCommand>();
            commandBinder.Bind<InitGameSignal>().To<InitGameCommand>();
            commandBinder.Bind<RestartGameSignal>().To<RestartGameCommand>();
            
            //models
            injectionBinder.Bind<IBladesModel>().To<BladesModel>().ToSingleton();
            injectionBinder.Bind<IEnemyDataModels>().To<EnemyDataModels>().ToSingleton();

            //defs
            injectionBinder.Bind<IBladesDataDefinitions>().To<BladesDataDefinitions>().ToSingleton();

            //fsm 
            injectionBinder.Bind<IAppFsm>().To<Framewerk.AppStateMachine.AppFsm>().ToSingleton();
            injectionBinder.Bind<AppStateEnterSignal>().ToSingleton();
            injectionBinder.Bind<AppStateExitSignal>().ToSingleton();
            injectionBinder.Bind<InitAppScreen>().To<InitAppScreen>();
            injectionBinder.Bind<GameAppScreen>().To<GameAppScreen>();
            
            //controllers
            injectionBinder.Bind<IEnemyFireController>().To<EnemyFireController>().ToSingleton();
            injectionBinder.Bind<IInputController>().To<InputController>().ToSingleton();

            //view mediation
            //ui
            mediationBinder.Bind<IntroPopupView>().To<IntroPopupMediator>();
            mediationBinder.Bind<BladeListView>().To<BladeListMediator>();
            mediationBinder.Bind<BladeListItemView>().To<BladeListItemMediator>();
            mediationBinder.Bind<ScoreView>().To<ScoreMediator>();
            

           //game
            mediationBinder.Bind<BladeView>().To<BladeMediator>();
            mediationBinder.Bind<ArenaView>().To<ArenaMediator>();
            
            injectionBinder.Bind<BladeInteractableView>().To<BladeInteractableView>();
            injectionBinder.Bind<IPool<BladeInteractableView>>().To<Pool<BladeInteractableView>>().ToSingleton();


        }
    }
}