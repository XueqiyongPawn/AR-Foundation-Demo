using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    private ARPlaneManager mARPlaneManger;
    private List<ARPlane> mPlanes;
    public GameObject ModelPrefab;
    public Transform ModelParent;
    [HideInInspector]
    public GameObject ModelClone;
    private bool mIsClone = false;
    private Vector3 mModelParentLasetForward;
    // Start is called before the first frame update
    void Start()
    {
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
        
        PlaceModel(mPlanes[0]);
    }

    private void PlaceModel(ARPlane plane)
    {
        Debug.Log("start palce model");

        ModelParent.position = plane.transform.position;
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        ModelParent.forward = -forward.normalized;
        mModelParentLasetForward = ModelParent.forward;

        ModelClone = Instantiate<GameObject>(ModelPrefab);
        ModelClone.transform.SetParent(ModelParent);
        ModelClone.transform.localPosition = Vector3.zero;
        ModelClone.transform.localRotation = Quaternion.identity;

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
        mARPlaneManger.enabled = false;
    }
}
