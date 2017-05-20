using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMatchTarget : MonoBehaviour {

	private float moveSpeed;

	private GameObject fish;
	private GameObject fishParent;
	private FishAni fishManager;

	// Use this for initialization
	void Start () {
		fish = GameObject.FindGameObjectWithTag("Fishy");
		fishParent = GameObject.FindGameObjectWithTag("FishParent");
		fishManager = fish.GetComponent<FishAni> ();
		moveSpeed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		moveSpeed = fishManager.moveSpeed;
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
