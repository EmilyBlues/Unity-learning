using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    public int life = 10;
    //每个GUI的style
    GUIStyle boldStyle = new GUIStyle();
    GUIStyle scoreStyle = new GUIStyle();
    GUIStyle textStyle = new GUIStyle();
    GUIStyle overStyle = new GUIStyle();
    private int score = 0;
    private bool gameStart = false;

    void Start ()
    {
        action = SSDirector.getInstance().currentScenceController as IUserAction;
    }
	
	void OnGUI ()
    {
        boldStyle.normal.textColor = new Color(1, 0, 0);
        boldStyle.fontSize = 20;
        textStyle.normal.textColor = new Color(0,0,0, 1);
        textStyle.fontSize = 20;
        scoreStyle.normal.textColor = new Color(1,0,1,1);
        scoreStyle.fontSize = 20;
        overStyle.normal.textColor = new Color(1, 0, 0);
        overStyle.fontSize = 25;

        if (gameStart)
        {
            //用户射击
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 pos = Input.mousePosition;
                action.Hit(pos);
            }

            GUI.Label(new Rect(10, 5, 200, 50), "Score:", textStyle);
            GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), scoreStyle);

            GUI.Label(new Rect(Screen.width - 120, 5, 50, 50), "Life:", textStyle);
            //显示当前血量
            for (int i = 0; i < life; i++)
            {
                GUI.Label(new Rect(Screen.width - 75 + 10 * i, 5, 50, 50), "X", boldStyle);
            }
            //游戏结束
            if (life == 0)
            {
                score = action.GetScore();
                GUI.Label(new Rect(Screen.width / 2 - 20, Screen.width / 2 - 250, 100, 100), "Game Over", overStyle);
                GUI.Label(new Rect(Screen.width / 2 - 10, Screen.width / 2 - 200, 50, 50), "Score:", textStyle);
                GUI.Label(new Rect(Screen.width / 2 + 50, Screen.width / 2 - 200, 50, 50), score.ToString(), textStyle);
                if (GUI.Button(new Rect(Screen.width / 2 - 20, Screen.width / 2 - 150, 100, 50), "Restart"))
                {
                    life = 10;
                    action.Restart();
                    return;
                }
                action.GameOver();
            }
        }
        else
        {
            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.width / 2 - 350, 100, 100), "Hit UFO!", overStyle);
            if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.width / 2 - 250, 100, 50), "Game Start"))
            {
                gameStart = true;
                action.BeginGame();
            }
        }
    }
    public void BloodReduce()
    {
        if(life > 0)
            life--;
    }
}
