using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : System.Object{
    private static GameDirector _instance;

    public SceneController currentSceneController { get; set; }
    public static GameDirector getInstance()
    {
        if(_instance == null)
        {
            _instance = new GameDirector();
        }
        return _instance;
    }

    public int getFPS()
    {
        return Application.targetFrameRate;
    }

    public void setFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
}
