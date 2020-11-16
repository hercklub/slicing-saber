using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public interface ISceneLoadControler
    {
        void LoadGameScene(GameObject levelEditorRoot);
        void LoadLevelEditorScene();
        void UnloadGameScene();
    }

    public class SceneLoadControler : ISceneLoadControler
    {
        private const string GAME_SCENE = "GameScene";
        private const string LEVEL_EDITOR = "LevelEditorScene";

        private Scene _gameScene;
        private GameObject _levelEditorRoot;

        public void Init()
        {
        }

        public void LoadGameScene(GameObject levelEditorRoot)
        {
            SceneManager.LoadScene(GAME_SCENE, new LoadSceneParameters(LoadSceneMode.Additive));
            
            // disable editor scene
            _levelEditorRoot = levelEditorRoot;
            _levelEditorRoot.SetActive(false);
        }

        public void LoadLevelEditorScene()
        {
            // disable game scene
            SceneManager.UnloadSceneAsync(GAME_SCENE);
            if (_levelEditorRoot != null)
            {
                _levelEditorRoot.SetActive(true);
            }
        }

        public void UnloadGameScene()
        {
            throw new System.NotImplementedException();
        }
    }
}