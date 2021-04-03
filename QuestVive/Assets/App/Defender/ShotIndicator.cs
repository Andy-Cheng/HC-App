using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotIndicator : MonoBehaviour
{
    public ParticleSystem AsteroidExplosion;
    public List<GameObject> Arrows;
    public Material ArrowMat;
    Transform CenterEye;
    public float GlowingStarDistance = 100f;
    public GameObject GlowingStar;
    public float GlowingStarSpeed = 10f;
    public float GlowingStarDownSpeed = 20f;

    float ArrowGlowingSpeed = 10f;
    float arrowAlpha = 0;
    int activeDirection = -1;
    Vector3 arrowColor;
    bool startMovingTowardSelf = false;
    bool shouldCheckDirection = false;


    void AnimateArrowAlpha()
    {

        arrowAlpha += ArrowGlowingSpeed * Time.deltaTime;
        arrowAlpha %= 360;
        ArrowMat.SetColor("_Color", new Color(arrowColor.x, arrowColor.y, arrowColor.z, 1f * Mathf.Abs(Mathf.Sin(arrowAlpha / 180 * Mathf.PI))));
        //Debug.Log(ArrowMat.color);

    }

    // 0: left, 1: right
    void OpenIndicator(int direction)
    {
        Arrows[direction].SetActive(true);
        activeDirection = direction;
    }



    void CloseIndicator()
    {
        if (activeDirection == -1)
        {
            return;
        }
        Arrows[activeDirection].SetActive(false);
        activeDirection = -1;

    }

    IEnumerator CheckDirection()
    {
        while (shouldCheckDirection)
        {
            Vector3 startToEye = CenterEye.position - GlowingStar.transform.position;
            if (startToEye.magnitude > 2f && startMovingTowardSelf)
            {
                GlowingStar.transform.Translate(Vector3.Normalize(startToEye) * Time.deltaTime * GlowingStarSpeed, Space.World);
            }

            Vector3 glowingStarScreenPoint = CenterEye.gameObject.GetComponent<Camera>().WorldToViewportPoint(GlowingStar.transform.position);
            if (glowingStarScreenPoint.z > 0 && glowingStarScreenPoint.x > 0 && glowingStarScreenPoint.x < 1 && glowingStarScreenPoint.y > 0 && glowingStarScreenPoint.y < 1)
            {
                CloseIndicator();
                yield return null;
                continue;
            }

            Vector3 playerRightNormal = CenterEye.right;
            Vector3 selfToOther = DefenderManager.instance.OtherPlayerTransform.position - CenterEye.position;
            if (Vector3.Dot(playerRightNormal, selfToOther) > 0)
            {
                CloseIndicator();
                OpenIndicator(1);
            }
            else
            {
                CloseIndicator();
                OpenIndicator(0);
            }

            yield return null;
        }
        startMovingTowardSelf = false;

    }

    IEnumerator MoveAsteroidDown()
    {
        float acc = 0f;
        while (acc < 5f)
        { 
            GlowingStar.transform.Translate(Time.deltaTime * -CenterEye.up * GlowingStarDownSpeed, Space.World);
            acc += Time.deltaTime;
            yield return null;
        }
        startMovingTowardSelf = true;
    }

    void StartCheckDirection()
    {
        GlowingStar.SetActive(true);

        Vector3 selfToOther = DefenderManager.instance.OtherPlayerTransform.position - CenterEye.position;
        Vector3 selfToStar = GlowingStarDistance * Vector3.Normalize(Vector3.Normalize(Vector3.ProjectOnPlane(selfToOther, CenterEye.up)) + CenterEye.transform.up);

        GlowingStar.transform.position = CenterEye.transform.position + selfToStar;
        GlowingStar.GetComponent<ParticleSystem>().Play();
        shouldCheckDirection = true;
        StartCoroutine(MoveAsteroidDown());
        StartCoroutine(CheckDirection());
    }

     void StopCheckDirection()
    {
        shouldCheckDirection = false;
        CloseIndicator();
        GlowingStar.SetActive(false);
        AsteroidExplosion.Play();
    }

    private void Awake()
    {
        CenterEye = transform.parent;
        Arrows[0].SetActive(false);
        Arrows[1].SetActive(false);
        GlowingStar = DefenderManager.instance.Asteroid;
        AsteroidExplosion.gameObject.SetActive(true);

    }
    void Start()
    {

        DefenderManager.instance.OnUserEnterTarget += StartCheckDirection;
        DefenderManager.instance.OnOtherPlayerShoot += StopCheckDirection;

    }
}
