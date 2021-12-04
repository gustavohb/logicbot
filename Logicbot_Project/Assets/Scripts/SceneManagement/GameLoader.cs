using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameLoader : MonoBehaviour
{
    
    [SerializeField] private float _fadeDuration = 1f;

    [SerializeField] private CanvasGroup _loadingCanvasGroup;
    
    [SerializeField] private Slider _slider;

    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        _loadingCanvasGroup.alpha = 0f;
    }

    private void Start()
    {
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        FadeIn(() => StartCoroutine(LoadSceneAsynchronously(SceneManager.GetActiveScene().buildIndex + 1)));
    }
    
    private IEnumerator LoadSceneAsynchronously(int sceneIndex)
    {
        yield return null;

        _asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        _asyncOperation.allowSceneActivation = false;


        Debug.LogWarning("ASYNC LOAD STARTED - " +
                         "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");

        while (_asyncOperation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);

            _slider.value = progress;

            yield return new WaitForEndOfFrame();
        }

        _slider.value = 1.0f;
        
        FadeOut(() =>
        {
            _asyncOperation.allowSceneActivation = true;
        });
    }
    
    private void FadeIn(Action callback = null)
    {
        _loadingCanvasGroup.DOFade(1, _fadeDuration).OnComplete(() => callback?.Invoke());
    }

    private void FadeOut(Action callback = null)
    {
        _loadingCanvasGroup.DOFade(0, _fadeDuration).OnComplete(() => callback?.Invoke());
    }
    
}
