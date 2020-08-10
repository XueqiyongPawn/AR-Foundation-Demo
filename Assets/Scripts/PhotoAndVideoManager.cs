using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAndVideoManager : MonoBehaviour
{
    public Image PhotoModelImg;
    public Image VideoModelImg;
    public GameObject TakePhotoBtn;
    public GameObject TakeVideoBtn;

    public Sprite PhotoModelNotSelectedSprite;
    public Sprite PhotoModelSelectedSprite;

    public Sprite VideoModelNotSelectedSprite;
    public Sprite VideoModelSelectedSprite;

    public GameObject PhotoSavePanel;
    public Image PhotoSavePreviewImg;

    public static PhotoAndVideoManager Instance;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        PhotoModelClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PhotoModelClick()
    {
        if (CapVideos.Instance.isRecording)
        {
            return;
        }
        PhotoModelImg.sprite = PhotoModelSelectedSprite;
        TakePhotoBtn.SetActive(true);
        VideoModelImg.sprite = VideoModelNotSelectedSprite;
        TakeVideoBtn.SetActive(false);
        
    }

    public void VideoModelClick()
    {
        PhotoModelImg.sprite = PhotoModelNotSelectedSprite;
        TakePhotoBtn.SetActive(false);
        VideoModelImg.sprite = VideoModelSelectedSprite;
        TakeVideoBtn.SetActive(true);
    }
    public void TakePhotoClick()
    {
        ScreenShootManager.Instance.SceenShootBtnClick();
    }

    public void TakeVideoClick()
    {
        CapVideos.Instance.RecVideo();
        if (CapVideos.Instance.isRecording)
        {

        }
        else
        { 
        
        }
    }

    public void SavePhoto()
    {
        ScreenShootManager.Instance.SaveBtnClick();
        PhotoSavePanel.SetActive(false);
    }

    public void CancelPhotoSave()
    {
        ScreenShootManager.Instance.CancelSaveClick();
        PhotoSavePanel.SetActive(false);
    }

    public void ShowPhotoSave(Sprite sprite)
    {
        PhotoSavePanel.SetActive(true);
        PhotoSavePreviewImg.sprite = sprite;
    }
    
}
