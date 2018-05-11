using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

public class GameEventManager : MonoBehaviour {
    public delegate void GameScoreAction();
    public static event GameScoreAction gameScoreAction;
    public delegate void GameOverAction();
    public static event GameOverAction gameOverAction;
    private SceneController sceneController;

	// Use this for initialization
	void Start () {
        sceneController = SceneController.getInstance();
        sceneController.setGameEventManager(this);
	}
	
	// Update is called once per frame
	void Update () { }

    public void heroEscape()
    {
        if (gameScoreAction != null)
            gameScoreAction();
    }

    public void patrolHitHero()
    {
        if (gameOverAction != null)
            gameOverAction();
    }
}
