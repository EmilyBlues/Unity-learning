using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour {

    public int score;
	// Use this for initialization
	void Start () {
        score = 0;
	}

    public void Record(GameObject disk)
    {
        int temp = disk.GetComponent<DiskData>().score;
        score += temp;
    }

    public void Reset()
    {
        score = 0;
    }
}
