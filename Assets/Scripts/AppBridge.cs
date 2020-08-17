using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppBridge : MonoBehaviour
{
    public static AppBridge Instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    public static Vector2 GetLongitudeAndLatitude()
    {

        return new Vector2();
    }

    public void CloseAr()
    {
        Application.Quit();
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void CallAndroidMethod(string methodName,params object[] args)
    {
        Debug.LogFormat("CallAndroidMethod:{0}", methodName);
        AndroidJavaClass jc = new AndroidJavaClass("com.crland.mixc.ArActivity");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        // 调用Android中写好的public函数
        // 可以传参数，参数类型是params[]，所以~~~
        // 像这样就可以了 jo.Call("u3dCallHideBanner"，参数1，参数2，参数3);
        jo.Call(methodName, args);
    }
}
