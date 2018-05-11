using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public interface IUserAction
{
    void heroMove(int dir);
}

public interface IAddAction
{
    void addRandomMovement(GameObject sourceObj, bool isActive);
    void addDirectMovement(GameObject sourceObj);
}

public interface IGameStatusOp
{
    int getHeroStandOnArea();
    void heroEscapeAndScore();
    void patrolHitHeroAndGameover();
}

public enum SSActionEventType : int { Started, Completed }
public enum SSActionTargetType : int { Normal, Catching }    //与原来不同的地方

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
        SSActionEventType eventType = SSActionEventType.Completed,
        SSActionTargetType intParam = SSActionTargetType.Normal,     //动作结束时回调，需要告知是哪种动作
        string strParam = null,
        Object objParam = null);
}