using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveToAction : SSAction {
    public Vector3 target;
    public float speed;
    public bool isCatching;
    //判定此动作是否为追捕（与原来不同）

    public static CCMoveToAction CreateSSAction(Vector3 _target, float _speed, bool _isCatching)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = _target;
        action.speed = _speed;
        action.isCatching = _isCatching;
        return action;
    }

    public override void Start() { }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed);
        if (this.transform.position == target)
        {
            this.destroy = true;
            //根据不同的动作类型回调函数传递不同的参数
            if (!isCatching)
                this.callBack.SSActionEvent(this);
            else
                this.callBack.SSActionEvent(this, SSActionEventType.Completed, SSActionTargetType.Catching);
        }
    }
}
