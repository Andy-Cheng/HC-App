using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float FlyingSpeed;
    public float FlyingTime;
    public float AdjustLerpFactor = 0.8f;
    Transform gunTracker;
    Transform otherPlayerTransform;
    Transform targetTransform; // the other player's shield
    Transform playerTransform;
    float distanceToPlayer;
    bool  adjustPosition;
    public bool canExplode;
    //public Transform AimTransform;
    //public Material AimMat;

    // Follow the direction: otherPlayer to player
    IEnumerator Flying()
    {
        float acc = 0f;
        while (acc < FlyingTime)
        {
            transform.Translate(Vector3.Normalize(SpaceShipManager.instance.RoomCenter.position - otherPlayerTransform.position) * Time.deltaTime * FlyingSpeed, Space.World);
            transform.LookAt(playerTransform);
            acc += Time.deltaTime;
            yield return null;
        }
        distanceToPlayer = Vector3.Magnitude(transform.position - playerTransform.position);
        adjustPosition = true;
    }
    


    // Start is called before the first frame update
    void Start()
    {
        canExplode = false;
        adjustPosition = false;
        gunTracker = SpaceShipManager.instance.GunTracker;
        targetTransform = SpaceShipManager.instance.TargetTransform;
        otherPlayerTransform = SpaceShipManager.instance.OtherPlayerTransform;
        playerTransform = SpaceShipManager.instance.PlayerTransform;

        StartCoroutine(Flying());


    }



    // Update is called once per frame
    void Update()
    {
        // OnSpawned: flying --> 3 second
        // OnStartShoot: driffting (change the position according to gun's and target's positions)
        if (adjustPosition)
        {
            Vector3 endPosition = gunTracker.position + Vector3.Normalize(targetTransform.position - gunTracker.position) * distanceToPlayer;
            transform.position = Vector3.Lerp(transform.position, endPosition, AdjustLerpFactor);
            //AimTransform.position = gunTracker.position +  Vector3.Normalize(targetTransform.position - gunTracker.position) * distanceToPlayer;
            transform.LookAt(playerTransform);
        }

    }

    public IEnumerator Explode()
    {
        Debug.Log("Explode in");

        if (canExplode)
        {
            Debug.Log("Explode");
            ParticleSystem exp = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            exp.Play();
            yield return new WaitForSeconds(3);
            MeshRenderer rend = GetComponent<MeshRenderer>();
            rend.enabled = false;
            exp = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
            exp.Play();
            yield return new WaitForSeconds(2);
            canExplode = false;
            Destroy(gameObject);
            SpaceShipManager.instance.OnEnemyDie();
        }
        else {
            yield return null;
        }
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LaserBullet")
        {
            Debug.Log("Hit Enemy");
            MeshRenderer rend= other.gameObject.GetComponent<MeshRenderer>();
            rend.enabled = false;
            StartCoroutine(Explode());
        }

    }

    public void Aimed()
    {
        Debug.Log("Aim");
        //AimMat.EnableKeyword("_EMISSION");
    }

    public void DisableAimed()
    {
        Debug.Log("Not Aim");
        //AimMat.DisableKeyword("_EMISSION");

    }


    private void OnDestroy()
    {
        SpaceShipManager.instance.activeEnemy = null;
        //SpaceShipManager.instance.NextSpawningPoint();


    }






}
