using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

public class GameModel : SSActionManager, ISSActionCallback {
    public GameObject PatrolItem, HeroItem;
    //public GameObject sceneModelItem, canvasItem;
    private SceneController sceneController;
    private GameObject hero, sceneModel, canvas;
    private List<GameObject> patrolSet;
    private List<int> patrolLastDir;
    private const float PERSON_SPEED_NORMAL = 0.05f;
    private const float PERSON_SPEED_CATCHING = 0.06f;

    void Awake()
    {
        PatrolFactory.getInstance().initItem(PatrolItem);
    }

	// Use this for initialization
	protected new void Start () {
        sceneController = SceneController.getInstance();
        sceneController.setGameModel(this);
        GenHero();
        GenPatrols();
       // sceneModel = Instantiate(sceneModelItem);
       // canvas = Instantiate(canvasItem);
	}
	
	// Update is called once per frame
	protected new void Update()
    {
        base.Update();
    }

    void GenHero()
    {
        hero = Instantiate(HeroItem);
    }

    void GenPatrols()
    {
        patrolSet = new List<GameObject>(9);
        patrolLastDir = new List<int>(9);
        Vector3[] posSet = PatrolFactory.getInstance().getPosSet();
        for(int i = 0; i < 9; i++)
        {
            GameObject newPatrol = PatrolFactory.getInstance().getPatrol();
            newPatrol.transform.position = posSet[i];
            newPatrol.name = "Patrol" + i;
            patrolLastDir.Add(-2);
            patrolSet.Add(newPatrol);
            addRandomMovement(newPatrol, true);
        }
    }

    public void SSActionEvent(SSAction source,
        SSActionEventType eventType = SSActionEventType.Completed,
        SSActionTargetType intParam = SSActionTargetType.Normal,     //动作结束时回调，需要告知是哪种动作
        string strParam = null,
        Object objParam = null)
    {
        if (intParam == SSActionTargetType.Normal)
            addRandomMovement(source.gameObject, true);
        else
            addDirectMovement(source.gameObject);
    }

    public void addRandomMovement(GameObject sourceObj, bool isActive)
    {
        int ind = GetIndex(sourceObj);
        int randomDir = GetRandomDirection(ind, isActive);
        patrolLastDir[ind] = randomDir;
        sourceObj.transform.rotation = Quaternion.Euler(new Vector3(0, randomDir * 90, 0));
        Vector3 target = sourceObj.transform.position;
        switch (randomDir)
        {
            case Direction.UP:
                target += new Vector3(0, 0, 1);
                break;
            case Direction.DOWN:
                target += new Vector3(0, 0, -1);
                break;
            case Direction.LEFT:
                target += new Vector3(-1, 0, 0);
                break;
            case Direction.RIGHT:
                target += new Vector3(1, 0, 0);
                break;
        }
        AddSingleMove(sourceObj, target, PERSON_SPEED_NORMAL, false);
    }

    public void addDirectMovement(GameObject sourceObj)
    {
        int index = GetIndex(sourceObj);
        patrolLastDir[index] = -2;

        sourceObj.transform.LookAt(sourceObj.transform);
        Vector3 oriTarget = hero.transform.position - sourceObj.transform.position;
        Vector3 target = new Vector3(oriTarget.x / 4.0f, 0, oriTarget.z / 4.0f);
        target += sourceObj.transform.position;

        AddSingleMove(sourceObj, target, PERSON_SPEED_CATCHING, true);
    }

    int GetIndex(GameObject sourceObj)
    {
        string name = sourceObj.name;
        char ind = name[name.Length - 1];
        int result = ind - '0';
        return result;
    }

    int GetRandomDirection(int ind, bool isActive)
    {
        int randomDir = Random.Range(-1, 3);
        if (!isActive)
        {
            while(patrolLastDir[ind] == randomDir)
            {
                randomDir = Random.Range(-1, 3);
            }
        }
        else
        {
            while (patrolLastDir[ind] == 0 && randomDir == 2
                || patrolLastDir[ind] == 2 && randomDir == 0
                || patrolLastDir[ind] == 1 && randomDir == -1
                || patrolLastDir[ind] == -1 && randomDir == 1
                || PatrolOutOfArea(ind, randomDir))
            {
                randomDir = Random.Range(-1, 3);
            }
        }
        return randomDir;

    }

    bool PatrolOutOfArea(int ind, int randomDir)
    {
        Vector3 patrolPos = patrolSet[ind].transform.position;
        float posX = patrolPos.x;
        float posZ = patrolPos.z;
        switch (ind)
        {
            case 0:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertLeft
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHoriUp)
                    return true;
                break;
            case 1:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertRight
                    || randomDir == -1 && posX - 1 < FenchLocation.FenchVertLeft
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHoriUp)
                    return true;
                break;
            case 2:
                if (randomDir == -1 && posX - 1 < FenchLocation.FenchVertRight
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHoriUp)
                    return true;
                break;
            case 3:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertLeft
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHoriUp
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHoriDown)
                    return true;
                break;
            case 4:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertRight
                    || randomDir == -1 && posX - 1 < FenchLocation.FenchVertLeft
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHoriUp
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHoriDown)
                    return true;
                break;
            case 5:
                if (randomDir == -1 && posX - 1 < FenchLocation.FenchVertRight
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHoriUp
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHoriDown)
                    return true;
                break;
            case 6:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertLeft
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHoriDown)
                    return true;
                break;
            case 7:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertRight
                    || randomDir == -1 && posX - 1 < FenchLocation.FenchVertLeft
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHoriDown)
                    return true;
                break;
            case 8:
                if (randomDir == -1 && posX - 1 < FenchLocation.FenchVertRight
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHoriDown)
                    return true;
                break;
        }
        return false;
    }

    void AddSingleMove(GameObject sourceObj, Vector3 target, float speed, bool isCatching)
    {
        this.runAction(sourceObj, CCMoveToAction.CreateSSAction(target, speed, isCatching), this);
    }

    void AddCombinedMoving(GameObject sourceObj, Vector3[] target, float[] speed, bool isCatching)
    {
        List<SSAction> action = new List<SSAction>();
        for(int i = 0; i < target.Length; i++)
        {
            action.Add(CCMoveToAction.CreateSSAction(target[i], speed[i], isCatching));
        }
        CCSequeneActions moveSeq = CCSequeneActions.CreateSSAction(action);
        this.runAction(sourceObj, moveSeq, this);
    }

    public void HeroMove(int dir)
    {
        hero.transform.rotation = Quaternion.Euler(new Vector3(0, dir * 90, 0));
        switch (dir)
        {
            case Direction.UP:
                hero.transform.position += new Vector3(0, 0, 0.1f);
                break;
            case Direction.DOWN:
                hero.transform.position += new Vector3(0, 0, -0.1f);
                break;
            case Direction.LEFT:
                hero.transform.position += new Vector3(-0.1f, 0, 0);
                break;
            case Direction.RIGHT:
                hero.transform.position += new Vector3(0.1f, 0, 0);
                break;
        }
    }

    public int GetHeroArea()
    {
        return hero.GetComponent<HeroBehavior>().standOnArea;
    }
}
