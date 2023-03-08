using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Wonnasmith
{

    public class LoadManager : MonoBehaviour
    {
        [SerializeField]
        private Slider loadingSlider;

        [SerializeField]
        private TMP_Text loaderText;

        private const string strLoad = "% ";

        private void Start()
        {
            StartCoroutine(StartLoading());
        }

        private IEnumerator StartLoading()
        {
            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("DeveloperScene");

            if (async == null)
            {
                yield break;
            }

            while (!async.isDone)
            {
                loadingSlider.value = Mathf.Clamp01(async.progress / 0.9f);

                loaderText.text = strLoad + (loadingSlider.value * 100);

                yield return null;
            }
        }
    }
}