using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    // Use this for initialization
    private bool isShowPlay = false;
    void Start()
    {

    }
    private void OnEnable()
    {
        Color c = this.GetComponent<RawImage>().color;
        c.a = 0;
        this.GetComponent<RawImage>().color = c;
    }
    public void Play(string Url)
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        rawImage = this.GetComponent<RawImage>();
        videoPlayer.url = Url;
        videoPlayer.Play();
    }
    private void Update()
    {
        if (!isShowPlay && videoPlayer.isPlaying && videoPlayer.texture != null)
        {
            rawImage.texture = videoPlayer.texture;
            Color c = rawImage.color;
            c.a = 1;
            rawImage.color = c;
            isShowPlay = true;
        }


    }
}
