using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArAvailableCheck : MonoBehaviour
{
    //经度最大值
    private const float MAX_LONGITUDE = 0;
    //经度最小值
    private const float MIN_LONGITUDE = 0;
    //纬度最大值
    private const float MAX_LATITUDE = 0;
    //纬度最小值
    private const float MIN_LATITUDE = 0;

    private ARSession arSession;

    public GameObject UnsupportTip;
    public GameObject NotLocateTip;
    // Start is called before the first frame update
    void Start()
    {
        arSession = this.GetComponent<ARSession>();
        StartCoroutine(Check());
    }
    private IEnumerator Check()
    {
        if ((ARSession.state == ARSessionState.None) || (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            UnsupportTip.SetActive(true);
        }
        else
        {
            if (CheckPosAvailable())
            {
                arSession.enabled = true;
            }
            else
            {
                NotLocateTip.SetActive(true);
            }

        }

    }
    /// <summary>
    /// 监测位置是否在商场内
    /// </summary>
    /// <returns></returns>
    private bool CheckPosAvailable()
    {
        Vector2 v = AppBridge.GetLongitudeAndLatitude();
        if (v.x >= MIN_LONGITUDE && v.x <= MAX_LONGITUDE && v.y >= MIN_LATITUDE && v.y <= MAX_LATITUDE)
        {
            return true;
        }
        return false;
    }
}
