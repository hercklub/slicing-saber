using System;
using Common;
using Framewerk.Managers;
using Plugins.Framewerk;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.extensions.pool.api;
using strange.framework.api;
using UnityEngine;

namespace LevelEditor
{
    public class EventInstanceProvider : IInstanceProvider
    {
        [Inject] public IAssetManager AssetManager { get; set; }
        [Inject] public ViewConfig ViewConfig { get; set; }
        [Inject] public IUiManager UiManager { get; set; }
        [Inject] public IInjector Injector { get; set; }

        public T GetInstance<T>() 
        {
            object instance = GetInstance(typeof(T));
            T retv = (T) instance;
            return retv;
        }

        public object GetInstance(Type key)
        {
            object retv = null;
            if (key == typeof(EventEnemyObjectView))
            {
                retv = InstantiateGamePrefab<EventEnemyObjectView>();
            }
            else if (key == typeof(EventFishObjectView))
            {
                retv = InstantiateGamePrefab<EventFishObjectView>();
            }
            else if (key == typeof(EventGrassObjectView))
            {
                retv = InstantiateGamePrefab<EventGrassObjectView>();
            }
            else if (key == typeof(EventObstacleObjectView))
            {
                retv = InstantiateGamePrefab<EventObstacleObjectView>();
            }
            else if (key == typeof(EventTargetObjectView))
            {
                retv = InstantiateGamePrefab<EventTargetObjectView>();
            }
            Injector.Inject(retv, false);
            
            return retv;
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