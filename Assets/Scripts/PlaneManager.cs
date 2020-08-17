using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    public static PlaneManager Instance;
    private ARPlaneManager mARPlaneManger;
    private List<ARPlane> mPlanes;
    public GameObject ModelPrefab;
    public Transform ModelParent;
    [HideInInspector]
    public GameObject ModelClone;
    private bool mIsClone = false;
    private Vector3 mModelParentLasetForward;

    [HideInInspector]
    public bool IsStartScan = false;
    private float mScanTime = 0;

    public GameObject MovePhoneTip;
    public GameObject ClickModelTip;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        mPlanes = new List<ARPlane>();
        mARPlaneManger = this.GetComponent<ARPlaneManager>();
        mARPlaneManger.planesChanged += OnPlaneChanged;
       
    }
    private void OnDisable()
    {
        if (ModelClone == null)
        {
            mARPlaneManger.planesChanged -= OnPlaneChanged;
        }
        
    }

    private void Update()
    {
        if (mIsClone)
        {

        }
        if (IsStartScan && !mIsClone)
        {
            mScanTime += Time.deltaTime;
            if (mScanTime > 5f)
            {
                IsStartScan = false;
                MovePhoneTip.SetActive(true);
            }
        }
    }

    private void OnPlaneChanged(ARPlanesChangedEventArgs arg)
    {
        if (ModelClone != null || arg.added.Count == 0)
        {
            return;
        }
        for (int i = 0; i < arg.added.Count; i++)
        {
            mPlanes.Add(arg.added[i]);
            arg.added[i].gameObject.SetActive(false);
        }
        mARPlaneManger.planesChanged -= OnPlaneChanged;
        mARPlaneManger.enabled = false;
        MovePhoneTip.SetActive(false);
        
        PlaceModel(mPlanes[0]);
    }

    public void SetClickModelState(bool state)
    {
        ClickModelTip.SetActive(state);
    }
    private void PlaceModel(ARPlane plane)
    {
        Debug.Log("start palce model");

        ModelParent.position = plane.transform.position;
        //强制离人1M
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 modelParentPos = ModelParent.position;
        cameraPos.y = modelParentPos.y;
         float dis = Vector3.Distance(cameraPos, modelParentPos);
        ModelParent.position += (1.3f - dis)*(modelParentPos - cameraPos).normalized;


        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        ModelParent.forward = -forward.normalized;
        mModelParentLasetForward = ModelParent.forward;

        ModelClone = Instantiate<GameObject>(ModelPrefab);
        ModelClone.transform.SetParent(ModelParent);
        ModelClone.transform.localPosition = Vector3.zero;
        ModelClone.transform.localRotation = Quaternion.identity;
        ModelClone.transform.localScale = Vector3.one * 0.375f;

        ModelParent.transform.localScale = Vector3.zero;
        StartCoroutine(BornAni());

        mIsClone = true;
    }
    private IEnumerator BornAni()
    {
        Vector3 addScale = Vector3.one / 60f;
        yield return null;
        while (ModelParent.transform.localScale.x < 1)
        {
            ModelParent.transform.localScale += addScale;
            yield return null;
        }
        SetClickModelState(true);
       
    }
}
