using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

public class UserInterface : MonoBehaviour {
    public IUserAction action;

	// Use this for initialization
	void Start () {
        action = SceneController.getInstance() as IUserAction;
	}
	
	// Update is called once per frame
	void Update () {
        detectKeyInput();
	}

    void detectKeyInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            action.heroMove(Direction.UP);
        if (Input.GetKey(KeyCode.DownArrow))
            action.heroMove(Direction.DOWN);
        if (Input.GetKey(KeyCode.RightArrow))
            action.heroMove(Direction.RIGHT);
        if (Input.GetKey(KeyCode.LeftArrow))
            action.heroMove(Direction.LEFT);
    }
}
