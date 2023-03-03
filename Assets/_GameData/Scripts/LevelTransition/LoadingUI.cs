using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TSGameDev.LevelManagment
{
    public class LoadingUI : MonoBehaviour
    {
        [SerializeField] private Slider levelProgressSlider;
        [SerializeField] private TMP_Text levelProgressTxt;

        public void Awake()
        {
            gameObject.SetActive(false);
        }

        public void LoadLevel(int _LevelIndex)
        {
            gameObject.SetActive(true);
            StartCoroutine(LoadLevelAsync(_LevelIndex));
        }

        IEnumerator LoadLevelAsync(int _LevelIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(_LevelIndex);

            while(!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                levelProgressSlider.value = progress;
                levelProgressTxt.text = $"{progress * 100}%";

                yield return null;
            }
        }
    }
}
