using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMatchTarget : MonoBehaviour {

	public float moveSpeed; //needed here??

	private GameObject fish;
	private GameObject fishParent;

	// Use this for initialization
	void Start () {
		fish = GameObject.FindGameObjectWithTag("Fishy");
		fishParent = GameObject.FindGameObjectWithTag("FishParent");
		moveSpeed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = fishParent.transform.position;
	}

	void LateUpdate () {
		// gameObject is the Main Camera, which this script is attached to
		Quaternion rotation = gameObject.transform.rotation;
		fishParent.transform.rotation = rotation;
		// what if fish rotation changes due to its parent rotation chaning?
		fish.transform.rotation = gameObject.transform.rotation;
	}
}
