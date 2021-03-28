using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerManager : MonoBehaviour
{
    public Dictionary<int, Transform> Trackers;

    public List<int> TrackerIDs;
    public List<Transform> TrackerTransforms;
    public int PlayerTrackerID;
    public int HCOriginTrackerID;
    public Transform PlayerTrackerModel;
    public Transform HCTrackerModel;

    public bool ShouldSetCamera = true;
    public float ResetDuration = 0.1f;

    public OVRCameraRig ovrRig;
    public static TrackerManager instance;

    public bool StartReceiveTrandform = false;

    Quaternion negateRotation;
    Vector3 negatePosition;
    public float ResetThresholdTranslation;
    //public Quaternion ResetThresholdRotation;
    public bool HaveSpawnedRoom = false;
    bool startSpawnRoom;
    
    public void OnReceiveCameraTrackerInfo(Quaternion trackerQ, Vector3 trackerV)
    {
        if (!StartReceiveTrandform)
        {
            StartResetCamera();
            StartReceiveTrandform = true;

        }

        if (ShouldSetCamera)
        {
            Trackers[PlayerTrackerID].position = trackerV;
            Trackers[PlayerTrackerID].rotation = trackerQ;
        }
        else 
        {
            PlayerTrackerModel.position = trackerV;
            PlayerTrackerModel.rotation = trackerQ;
        }
    }

    public bool ShouldResetCamera(Quaternion rotation, Vector3 translation)
    {
        float distance = Vector3.Distance(translation, ovrRig.centerEyeAnchor.position);
        //Debug.Log($"{distance}, {translation - ovrRig.centerEyeAnchor.position}");
        if (distance > ResetThresholdTranslation)
        {
            return true;
        }
        return false;
    }

    void NegateCameraTransform(OVRCameraRig cameraRig)
    {
        //Debug.Log("Update anchor callback");

        if (ShouldSetCamera)
        { 
            Matrix4x4 m = cameraRig.centerEyeAnchor.parent.worldToLocalMatrix * cameraRig.centerEyeAnchor.localToWorldMatrix;
            negateRotation = m.inverse.rotation;
            negatePosition.x = m.inverse.m03;
            negatePosition.y = m.inverse.m13;
            negatePosition.z = m.inverse.m23;
        }



        cameraRig.centerEyeAnchor.parent.localRotation = negateRotation;
        cameraRig.centerEyeAnchor.parent.localPosition = negatePosition;

    }


    public void StartResetCamera()
    {
        Debug.Log("Start reset cam");
        StartCoroutine(ResetCamCoroutine());
        PlayerTrackerModel.localPosition = Vector3.zero;
        PlayerTrackerModel.localRotation = Quaternion.identity;

    }
    IEnumerator ResetCamCoroutine()
    {
        ShouldSetCamera = true;
        yield return new WaitForSeconds(ResetDuration);
        ShouldSetCamera = false;

    }



    public void SetHCOrigin(Quaternion rot, Vector3 pos)
    {
        Trackers[HCOriginTrackerID].position = pos;
        Trackers[HCOriginTrackerID].rotation = rot;
    }


    //public IEnumerator StartSetHCOrigin()
    //{
    //    HCTrackerModel.localPosition = Vector3.zero;
    //    HCTrackerModel.localRotation = Quaternion.identity;
    //    HaveSpawnedRoom = false;
    //    yield return new WaitForSeconds(ResetDuration);
    //    HaveSpawnedRoom = true;

    //}

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        Trackers = new Dictionary<int, Transform>();
        for (int i = 0; i < TrackerTransforms.Count; ++i)
        {
            Trackers.Add(TrackerIDs[i], TrackerTransforms[i]);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        ovrRig.UpdatedAnchors += NegateCameraTransform;

    }

    // Update is called once per frame
    void Update()
    {
    }
}
