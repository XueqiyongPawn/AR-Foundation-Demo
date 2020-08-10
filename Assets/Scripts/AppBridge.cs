using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
