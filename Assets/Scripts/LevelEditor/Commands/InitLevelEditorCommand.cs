using Common;
using Framewerk.Managers;
using LevelEditor.EventsList;
using LevelEditor.Interactions;
using LevelEditor.IO;
using LevelEditor.UI.LoadPopup;
using Plugins.Framewerk;
using strange.extensions.command.impl;
using strange.extensions.mediation.api;
using strange.extensions.pool.api;
using UnityEngine;

namespace LevelEditor
{
    public class InitLevelEditorCommand : Command
    {
        [Inject] public IAssetManager AssetManager { get; set; }

        [Inject] public IUiManager UiManager { get; set; }

        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public IPool<EventEnemyObjectView> EventEnemyObjectsPool { get; set; }
        [Inject] public IPool<EventGrassObjectView> EventGrassObjectsPool { get; set; }
        [Inject] public IPool<EventFishObjectView> EventFishObjectsPool { get; set; }
        [Inject] public IPool<EventObstacleObjectView> EventObstacleObjectsPool { get; set; }
        [Inject] public IPool<EventTargetObjectView> EventTargetObjectsPool { get; set; }

        [Inject] public ViewConfig ViewConfig { get; set; }
        [Inject] public EventInstanceProvider EventInstanceProvider { get; set; }
        [Inject] public ILevelImporter LevelImporter { get; set; }
        [Inject] public IEditorLevelsInfoModel LevelsInfoModel { get; set; }

        [Inject] public IEditorLibraryBundleDataModel EditorLibraryBudnle { get; set; }


        public override void Execute()
        {
            LevelEditorControler.Init();
            EnemyEventsModel.Init();

            EventEnemyObjectsPool.instanceProvider = EventInstanceProvider;
            EventGrassObjectsPool.instanceProvider = EventInstanceProvider;
            EventFishObjectsPool.instanceProvider = EventInstanceProvider;
            EventObstacleObjectsPool.instanceProvider = EventInstanceProvider;
            EventTargetObjectsPool.instanceProvider = EventInstanceProvider;

            // load level infos
            var loadedInfos = LevelImporter.LoadAllLevelInfos();
            foreach (var level in loadedInfos)
            {
                LevelsInfoModel.AddLevelInfo(level.Version, level.LevelName, level.DisplayLevelName, level.BeatsPerMinute, level.EventsPerBeat, level.Duration,
                    level.Difficulty, level.LevelType);
            }

            // load preset bundles
            var bundleInfos = LevelImporter.LoadAllBundleInfos();
            foreach (var bundle in bundleInfos)
            {
                EditorLibraryBudnle.AddBundleData(bundle.Version, bundle.Bundlename, bundle.BundleDirectory,
                    bundle.EnemyCount, bundle.GrasCount, bundle.FishCount, bundle.ObstacleCount, bundle.TargetCount);
            }

            EditorLibraryBudnle.UpdateDirectories();


            UiManager.InstantiateView<SaveModePopupView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<AddModePopupView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<ModeControlView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<LevelInfoDataView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);

            UiManager.InstantiateView<EnemyDataView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<GrassDataView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<FishDataView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<TargetDataView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<ObstacleDataView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);
            UiManager.InstantiateView<MultiDataView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);


            UiManager.InstantiateView<EditorLevelMenuView>(ResourcePath.LEVEL_EDITOR, ViewConfig.UiDefault);

            InstantiateGamePrefab<PlaybackControlerView>();
            InstantiateGamePrefab<EventListView>();
            InstantiateGamePrefab<SelectedObjectView>();
            InstantiateGamePrefab<PathSelectedObjectView>();
            InstantiateGamePrefab<GhostObjectView>();
        }

        public T InstantiateGamePrefab<T>(string path = "", Transform parent = null) where T : MonoBehaviour, IView
        {
            if (parent == null)
                parent = ViewConfig.Container3d;

            path += UiManager.GetViewName(typeof(T));
            //TODO: remove magic string
            GameObject gamePrefab = AssetManager.GetAsset<GameObject>(ResourcePath.LEVEL_EDITOR + path);
            gamePrefab.transform.SetParent(parent, false);

            var component = gamePrefab.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError(
                    $"<color=\"aqua\">AppStateScreen.InstantiateGamePrefab() : Cant find component{typeof(T)} on {gamePrefab}</color>");
                return null;
            }

            return component;
        }
    }
}