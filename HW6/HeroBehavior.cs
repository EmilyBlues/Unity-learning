using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

public class HeroBehavior : MonoBehaviour {

    public int standOnArea = -1;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () {
        modifyStandOnArea();
	}

    void modifyStandOnArea()
    {
        float posX = this.gameObject.transform.position.x;
        float posZ = this.gameObject.transform.position.z;
        if (posZ >= FenchLocation.FenchHoriUp)
        {
            if (posX < FenchLocation.FenchVertLeft)
                standOnArea = 0;
            else if (posX > FenchLocation.FenchVertRight)
                standOnArea = 2;
            else
                standOnArea = 1;
        }
        else if(posZ < FenchLocation.FenchHoriUp && posZ > FenchLocation.FenchHoriDown)
        {
            if (posX < FenchLocation.FenchVertLeft)
                standOnArea = 3;
            else if (posX > FenchLocation.FenchVertRight)
                standOnArea = 5;
            else
                standOnArea = 4;
        }
        else
        {
            if (posX < FenchLocation.FenchVertLeft)
                standOnArea = 6;
            else if (posX > FenchLocation.FenchVertRight)
                standOnArea = 8;
            else
                standOnArea = 7;
        }
    }
}
