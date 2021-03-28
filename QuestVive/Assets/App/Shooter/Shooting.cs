using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject laserPrefab;
    public OVRHand RightHand;
    public Transform shotPos;
    public float ShotInterval = 1f;
    public Transform GunTransform;

    float acc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if ( acc > ShotInterval)
        {
            bool isIndexFingerPinching = RightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            float ringFingerPinchStrength = RightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            //Debug.Log($"index pinching: {isIndexFingerPinching}");
            //Debug.Log($"Pinching strength: {ringFingerPinchStrength}");
            if (isIndexFingerPinching)
            {
                GameObject go = Instantiate(laserPrefab, shotPos);
                go.transform.SetParent(null);
                Destroy(go, 3f);
            }
            acc = 0f;
        }
        acc += Time.deltaTime;
    }


    // Check if hit the shield
    private void FixedUpdate()
    {
        //if (SpaceShipManager.instance.activeEnemy != null && SpaceShipManager.instance.activeEnemy.canExplode)
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        //    {
        //        Debug.Log(hit.collider.name);
        //        if (hit.collider.name == "Shield")
        //        {
        //            Debug.Log("Hit other player shield");
        //            SpaceShipManager.instance.activeEnemy.Aimed();
        //        }
        //        else
        //        {
        //            SpaceShipManager.instance.activeEnemy.DisableAimed();
        //        }

        //    }
        //}

    }

}
