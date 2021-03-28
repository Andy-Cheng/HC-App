using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

	public float DieTime = 10;
	float acc;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * 100f;
		acc += Time.deltaTime;
		if (acc > DieTime)
		{
			Destroy(gameObject);
		}
	}
}
