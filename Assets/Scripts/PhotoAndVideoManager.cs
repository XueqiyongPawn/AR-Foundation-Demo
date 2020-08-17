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

    public GameObject Btns;
    public GameObject PhotoSavePanel;
    public Image PhotoSavePreviewImg;
    public GameObject VideoPreview;
    public GameObject SaveSuccessTip;

    public static PhotoAndVideoManager Instance;

    private bool mIsPhotoModel = false;
    

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
        mIsPhotoModel = true;
        PhotoModelImg.sprite = PhotoModelSelectedSprite;
        TakePhotoBtn.SetActive(true);
        VideoModelImg.sprite = VideoModelNotSelectedSprite;
        TakeVideoBtn.SetActive(false);

    }

    public void VideoModelClick()
    {
        if (CapVideos.Instance.isRecording)
        {
            return;
        }
        mIsPhotoModel = false;
        PhotoModelImg.sprite = PhotoModelNotSelectedSprite;
        TakePhotoBtn.SetActive(false);
        VideoModelImg.sprite = VideoModelSelectedSprite;
        TakeVideoBtn.SetActive(true);
    }
    public void TakePhotoClick()
    {
        StartCoroutine(TakePhoto());
    }
    private IEnumerator TakePhoto()
    {
        SetBtnState(TakePhotoBtn, false);
        yield return new WaitForSeconds(0.1f);
        SetBtnState(TakePhotoBtn, true);
        ScreenShootManager.Instance.SceenShootBtnClick();
    }

    private void SetBtnState(GameObject g, bool isShow)
    {
        Color color = g.GetComponent<Image>().color;
        if (isShow)
        {
            color.a = 1;
        }
        else
        {
            color.a = 0;
        }
        g.GetComponent<Image>().color = color;
    }


    public void TakeVideoClick()
    {
        CapVideos.Instance.RecVideo();
        if (CapVideos.Instance.isRecording)
        {
            StartCoroutine(TakeVideoAni());
        }
    }

    private IEnumerator TakeVideoAni()
    {
        while (CapVideos.Instance.isRecording)
        {
            SetBtnState(TakeVideoBtn, TakeVideoBtn.GetComponent<Image>().color.a == 0 ? true : false);
            
            yield return new WaitForSeconds(0.2f);
        }
        SetBtnState(TakeVideoBtn, true);
    }



    public void Save()
    {
        if (mIsPhotoModel)
        {
            //ScreenShootManager.Instance.SaveBtnClick();
        }
        PhotoSavePanel.SetActive(false);
        Btns.SetActive(true);
        SaveSuccessTip.SetActive(true);
    }

    public void CancelSave()
    {
        if (mIsPhotoModel)
        {
            ScreenShootManager.Instance.CancelSaveClick();
        }
        PhotoSavePanel.SetActive(false);
        Btns.SetActive(true);
        
    }

    public void ShowPhotoSave(Sprite sprite)
    {
        Btns.SetActive(false);
        PhotoSavePanel.SetActive(true);
        if (mIsPhotoModel)
        {
            PhotoSavePreviewImg.gameObject.SetActive(true);
            VideoPreview.SetActive(false);
            SaveSuccessTip.SetActive(false);
            PhotoSavePreviewImg.sprite = sprite;
        }
        else
        {
            PhotoSavePreviewImg.gameObject.SetActive(false);
            VideoPreview.SetActive(true);
            SaveSuccessTip.SetActive(false);
            VideoPreview.GetComponent<PlayVideo>().Play(CapVideos.Instance.lastVideoPath);
        }

    }

}
