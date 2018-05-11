using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public GameObject target;
    public float followingSpeed = 0.06f;
    Vector3 distance;

	// Use this for initialization
	void Start () {
        distance = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + distance, followingSpeed * Time.deltaTime);
	}
}
