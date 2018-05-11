using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//并发顺序
public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Start(){ }

    protected void Update()
    {
        foreach (SSAction ac in waitingAdd)
        {
            actions[ac.GetInstanceID()] = ac;
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
                waitingDelete.Add(kv.Key);
            else if (ac.enable)
                ac.Update();
        }

        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }

    public void runAction(GameObject gameObj, SSAction action, ISSActionCallback manager)
    {
        //先把该对象现有的动作销毁（与原来不同部分）
        for (int i = 0; i < waitingAdd.Count; i++)
        {
            if (waitingAdd[i].gameObject.Equals(gameObj))
            {
                SSAction ac = waitingAdd[i];
                waitingAdd.RemoveAt(i);
                i--;
                DestroyObject(ac);
            }
        }
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.gameObject.Equals(gameObj))
            {
                ac.destroy = true;
            }
        }

        action.gameObject = gameObj;
        action.transform = gameObj.transform;
        action.callBack = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}
