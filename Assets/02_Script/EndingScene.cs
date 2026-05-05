using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingScene : MonoBehaviour
{
    [SerializeField] private VideoPlayer _endingVideo;
    [SerializeField] private GameObject _panelEnding;

    private void Start()
    {
        _panelEnding.SetActive(false);
        _endingVideo.Play();
        _endingVideo.loopPointReached += OnVideoEnd;
    }

    private void OnDisable()
    {
        _endingVideo.loopPointReached -= OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        _panelEnding.SetActive(true);
    }
}
