using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public Transform MyCenter;

    public float selfSpeed = 1;
    public float speed = 1;

    float ry, rx;

    // Use this for initialization
    void Start () {
        rx = Random.Range(10, 60);
        ry = Random.Range(10, 60);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 axis = new Vector3(rx, ry, 0);
        this.transform.RotateAround(MyCenter.position, axis, speed * Time.deltaTime);
        this.transform.Rotate(Vector3.up * selfSpeed * Time.deltaTime);
    }
}
