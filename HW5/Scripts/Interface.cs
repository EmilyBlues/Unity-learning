using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}

public interface ISceneController
{
    void LoadResources();
}

public interface IUserAction
{
    void Restart();
    void Hit(Vector3 pos);
    void GameOver();
    int GetScore();
    void BeginGame();
}
