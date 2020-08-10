using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class AppController : MonoBehaviour
{
    public  GameObject spawnPrefab;
    static List<ARRaycastHit> Hits;
    private ARRaycastManager mRaycastManager;
    private GameObject spawnedObject = null;
    private void Start()
    {
        Hits = new List<ARRaycastHit>();
        mRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;
        var touch = Input.GetTouch(0);
        if (mRaycastManager.Raycast(touch.position, Hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneWithinBounds))
        {
            var hitPose = Hits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(spawnPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }
}
