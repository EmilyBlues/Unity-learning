using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

public class PatrolBehavior : MonoBehaviour {
    private IAddAction addAction;
    private IGameStatusOp gameStatus;
    public int index;
    public bool isCatching;
    private float CATCH_RADIUS = 3.0f;

	// Use this for initialization
	void Start () {
        addAction = SceneController.getInstance() as IAddAction;
        gameStatus = SceneController.getInstance() as IGameStatusOp;
        index = getOwnIndex();
        isCatching = false;
    }
	
	// Update is called once per frame
	void Update () {
        checkNearByHero();
    }

    int getOwnIndex()
    {
        string name = this.gameObject.name;
        char cindex = name[name.Length - 1];
        int result = cindex - '0';
        return result;
    }

    //检测进入自己区域的hero
    void checkNearByHero()
    {
        if (gameStatus.getHeroStandOnArea() == index)
        {    //只有当走进自己的区域
            if (!isCatching)
            {
                isCatching = true;
                addAction.addDirectMovement(this.gameObject);
            }
        }
        else
        {
            if (isCatching)
            {    //刚才为捕捉状态，但此时hero已经走出所属区域
                gameStatus.heroEscapeAndScore();
                isCatching = false;
                addAction.addRandomMovement(this.gameObject, false);
            }
        }
    }

    void OnCollisionStay(Collision e)
    {
        //撞击围栏，选择下一个点移动
        if (e.gameObject.name.Contains("Patrol") || e.gameObject.name.Contains("fence")
            || e.gameObject.tag.Contains("FenceAround"))
        {
            isCatching = false;
            addAction.addRandomMovement(this.gameObject, false);
        }

        //撞击hero，游戏结束
        if (e.gameObject.name.Contains("Hero"))
        {
            gameStatus.patrolHitHeroAndGameover();
            Debug.Log("Game Over!");
        }
    }
}
