using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App
{
    public class Persistent : MonoBehaviour
    {
        public const string SceneName = "Persistent";

        /// <summary>
        /// 常駐シーンのロード
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadPersistentSceneAdditive()
        {
            // 既にロード済みかチェック
            var sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; ++i)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == SceneName)
                {
                    // 読み込み済みであれば、何もしない
                    return;
                }
            }

            // Additiveでロード
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

    }

}