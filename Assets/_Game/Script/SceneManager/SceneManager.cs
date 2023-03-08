using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WonnasmithTools;

namespace Wonnasmith
{
    public class SceneManager : Singleton<SceneManager>
    {
        public delegate void SceneManagerLoadedSceneManager(ScneType scneType);
        public static event /*SceneManager.*/SceneManagerLoadedSceneManager LoadedSceneManager;

        public enum ScneType
        {
            NONE,

            LobyScene,
            GameScene
        }

        private readonly Dictionary<ScneType, string> _sceneDictionary = new Dictionary<ScneType, string>
        {
            {ScneType.LobyScene, "LobyScene"},
            {ScneType.GameScene, "GameScene"}
        };


        private string _currentSceneName = null;

        /// <summary> sahneyi yükler </summary>
        /// <returns></returns>
        public void SceneLoader(ScneType scneType)
        {
            if (_sceneDictionary == null)
            {
                return;
            }

            if (!_sceneDictionary.ContainsKey(scneType))
            {
                return;
            }

            string sceneName = _sceneDictionary[scneType];

            if (IsCurrentSceneLoad(sceneName))
            {
                Debug.LogWarning("CurrentSceneLoad::" + sceneName);
                return;
            }

            if (sceneName == null)
            {
                Debug.LogWarning("LoadScene:: " + sceneName + " Not Found");
                return;
            }

            Debug.Log("<color=pink>:::LoadScene:::</color>" + sceneName + "<color=pink>:::</color>");

            GameManager.Instance.SetState(GameState.GAME_SCENE_LOAD);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            LoadedSceneManager?.Invoke(scneType);
        }


        public AsyncOperation SceneLoaderSceneAsync(ScneType scneType)
        {
            if (_sceneDictionary == null)
            {
                Debug.LogWarning("NONE Scene");

                return null;
            }

            if (!_sceneDictionary.ContainsKey(scneType))
            {
                Debug.LogWarning("NONE Scene");

                return null;
            }

            string sceneName = _sceneDictionary[scneType];

            if (IsCurrentSceneLoad(sceneName))
            {
                Debug.LogWarning("CurrentSceneLoad::" + sceneName);
                return null;
            }

            if (sceneName == null)
            {
                Debug.LogWarning("LoadSceneAsync:: " + sceneName + " Not Found");
                return null;
            }

            Debug.Log("<color=pink>:::LoadSceneAsync:::</color>" + sceneName + "<color=pink>:::</color>");

            GameManager.Instance.SetState(GameState.GAME_SCENE_LOAD);
            LoadedSceneManager?.Invoke(scneType);
            return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        }


        private bool IsCurrentSceneLoad(string newSceneName)
        {
            _currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (_currentSceneName.Equals(null))
            {
                return true;
            }

            if (newSceneName.Equals(_currentSceneName))
            {
                return true;
            }

            return false;
        }
    }
}