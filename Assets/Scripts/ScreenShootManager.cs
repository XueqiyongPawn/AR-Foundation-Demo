using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum SharePlatform
{
    wechat=1,
    WechatFriend=2,
    Weibo=3,
    //QQ,
    system = 10,
}

public unsafe class ScreenShootManager : MonoBehaviour
{
    public static ScreenShootManager Instance;
    Texture2D texture;
    private byte[] imageData;
    GameObject canvas;

    private string sdPath = "/storage/emulated/0/";
    private string floder = "WanXiang/";
    private string Screenfloder = "Screenshots/";
    private string fileName;
#if !UNITY_EDITOR && UNITY_IOS
    [DllImport("__Internal")]
    private static extern void md_saveImageToPhotosAlbum(void * data,UInt64 length);
   
#endif
    // Use this for initialization
    void Start()
    {
        Instance = this;
        canvas = GameObject.Find("Canvas").gameObject;
        //Mvc.MvcTool.addNoticeListener(MessageKeys.ScreenShot, SceenShootBtnClick);
        //Mvc.MvcTool.addNoticeListener(MessageKeys.ShareScreenShot, Share);
        //Mvc.MvcTool.addNoticeListener(MessageKeys.SaveScreenShot, SaveBtnClick);
        //Mvc.MvcTool.addNoticeListener(MessageKeys.CancelScreenShot, DeleteScreenShot);

    }

    private void OnDestroy()
    {
        //Mvc.MvcTool.removeNoticeListener(MessageKeys.ScreenShot, SceenShootBtnClick);
        //Mvc.MvcTool.removeNoticeListener(MessageKeys.ShareScreenShot, Share);
        //Mvc.MvcTool.removeNoticeListener(MessageKeys.SaveScreenShot, SaveBtnClick);
        //Mvc.MvcTool.removeNoticeListener(MessageKeys.CancelScreenShot, DeleteScreenShot);
    }
    /// <summary>
    /// Deletes the screen shot.
    /// </summary>
    /// <param name="noticeType">Notice type.</param>
    /// <param name="data">Data.</param>
    public void CancelSaveClick()
    {
        DeleteScreenShot();
    }

    public void SaveBtnClick()
    {
#if !UNITY_EDITOR && UNITY_IOS
        md_saveImageToPhotosAlbum((void*)GetPtr(imageData), Convert.ToUInt64(imageData.Length));
#endif
        if (Application.platform == RuntimePlatform.Android)
        {
            CheckPath();
            string path = string.Format("{0}{1}{2}{3}.png", sdPath, floder, Screenfloder, fileName);
            File.Copy(GetTmpPath(),path);
            //OriginBridge.CallJaveMethod(OriginBridge.md_saveImageToPhotosAlbum,path);

        }

        DeleteScreenShot();
    }
    /// <summary>
    /// 分享
    /// </summary>
    /// <returns>The share.</returns>
    /// <param name="noticeType">Notice type.</param>
    /// <param name="data">Data.</param>
    private void Share(string noticeType, object data)
    {

        SharePlatform platform = (SharePlatform)data;
#if !UNITY_EDITOR && UNITY_IOS
        md_shareImageTo((int)platform, (void*)GetPtr(imageData), Convert.ToUInt64(imageData.Length));
#elif !UNITY_EDITOR &&UNITY_ANDROID
        //OriginBridge.CallJaveMethod(OriginBridge.md_shareImageTo,(int)platform,GetTmpPath());
#endif
        DeleteScreenShot();
    }

    // Update is called once per frame
	void Update()
    {
        //if (Input.touchCount > 0 && shareSreenShootBg.activeSelf)
        //{
            //PointerEventData pointer = new PointerEventData(EventSystem.current);
            //pointer.position = Input.GetTouch(0).position;

            //List<RaycastResult> raycastResults = new List<RaycastResult>();
            //EventSystem.current.RaycastAll(pointer, raycastResults);
            //if (raycastResults.Count == 1 && raycastResults[0].gameObject == shareSreenShootBg)
            //{
            //    cancelBtnClick();
            //}
        //}

    }
    /// <summary>
    /// 截图 禁用自动旋转
    /// </summary>
    /// <param name="notice">Notice.</param>
    /// <param name="data">Data.</param>
    public void SceenShootBtnClick()
    {
        //canvas.SetActive(false);
        canvas.SetActive(false);
        StartCoroutine(CapTexture());
        //Manager._instance.SetScreenDirt();
    }

    /// <summary>
    /// 截图
    /// </summary>
    /// <returns>The texture.</returns>
    IEnumerator CapTexture()
    {
        
        yield return new WaitForEndOfFrame();
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply(false, false);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);

        yield return new WaitForEndOfFrame();

        // 最后将这些纹理数据，成一个png图片文件 
        imageData = texture.EncodeToPNG();

        canvas.SetActive(true);
        PhotoAndVideoManager.Instance.ShowPhotoSave(sprite);
        //Mvc.MvcTool.sendNotice(MessageKeys.ShowUI, UIForm.ScreenSHot);
        //Mvc.MvcTool.sendNotice(MessageKeys.SetScreenImg, sprite);
//#if !UNITY_EDITOR &&UNITY_ANDROID
        AndroidSaveImage(imageData);
//#endif
    }

    void AndroidSaveImage(byte[] data)
    {
        CheckPath();
        fileName = GetTimeStr();
        File.WriteAllBytes(GetTmpPath(),data);
    }

    string GetTimeStr()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmssms");
    }
    void CheckPath()
    {
        if (!Directory.Exists(string.Format("{0}{1}",sdPath,floder)))
        {
            Directory.CreateDirectory(string.Format("{0}{1}",sdPath,floder));
        }

        if (!Directory.Exists(string.Format("{0}{1}{2}",sdPath,floder,Screenfloder)))
        {
            Directory.CreateDirectory(string.Format("{0}{1}{2}",sdPath,floder,Screenfloder));
        }
    }
    
    /// <summary>
    /// 删除截图、启用自动旋转
    /// </summary>
    private void DeleteScreenShot()
    {
        Destroy(texture);
        //Mvc.MvcTool.sendNotice(MessageKeys.ShowUI, UIForm.Home);
        if (Application.platform == RuntimePlatform.Android)
        {
            File.Delete(GetTmpPath());
        }
        //Manager._instance.AutoRotate();
    }
    /// <summary>
    /// Gets the ptr.
    /// </summary>
    /// <returns>The ptr.</returns>
    /// <param name="data">Data.</param>
    private IntPtr GetPtr(byte[] data)
    {
        IntPtr ptr = Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, ptr, data.Length);
        return ptr;
    }

    string GetTmpPath()
    {
        return string.Format("{0}/{1}.png", Application.persistentDataPath, fileName);
    }

}
