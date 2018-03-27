using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homework1 : MonoBehaviour
{

    private int turn = 1;
    private int[,] state = new int[3, 3];
    public Texture2D img;
    public Texture2D img1;
    public Texture2D img2;
    // Use this for initialization
    void Start()
    {
        Reset();
    }

    void Reset()
    {
        //游戏初始化
        turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                state[i, j] = 0;
            }
        }
    }

    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        GUIStyle fontStyle1 = new GUIStyle();
        GUIStyle fontStyle2 = new GUIStyle();
        //fontStyle 用于设置最顶端游戏信息
        //fontStyle2 用于设置背景图片
        //fontStyle3 用于设置游戏提示信息

        fontStyle.fontSize = 40;
        fontStyle1.normal.background = img;
        fontStyle2.fontSize = 30;
        fontStyle2.normal.textColor = new Color(255, 255, 255);

        GUI.Label(new Rect(0, 0, 1280, 780), "", fontStyle1);
        GUI.Label(new Rect(500, 120, 100, 100), "Welcomt to Game", fontStyle);
        GUI.Label(new Rect(320, 150, 200, 100), img1);
        GUI.Label(new Rect(870, 150, 200, 100), img2);

        if (GUI.Button(new Rect(620, 500, 100, 50), "Reset"))
        {
            Reset();
        }

        int result = isWin();
        if (result == 1)
        {
            GUI.Label(new Rect(320, 250, 100, 50), "O wins!", fontStyle2);
        }
        else if (result == 2)
        {
            GUI.Label(new Rect(870, 250, 100, 50), "X wins!", fontStyle2);
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (state[i, j] == 1)
                    GUI.Button(new Rect(550 + i * 80, 220 + j * 80, 80, 80), img1);
                if (state[i, j] == 2)
                    GUI.Button(new Rect(550 + i * 80, 220 + j * 80, 80, 80), img2);
                if (GUI.Button(new Rect(550 + i * 80, 220 + j * 80, 80, 80), ""))
                {
                    if (result == 0)
                    {
                        if (turn == 1)
                            state[i, j] = 1;
                        else
                            state[i, j] = 2;
                        turn = -turn;
                    }
                }
            }
        }
    }

    int isWin()
    {
        //检查游戏是否有赢家
        //横向连线
        for (int i = 0; i < 3; i++)
        {
            if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
            {
                return state[i, 0];
            }
        }
        //纵向连线    
        for (int j = 0; j < 3; j++)
        {
            if (state[0, j] != 0 && state[0, j] == state[1, j] && state[1, j] == state[2, j])
            {
                return state[0, j];
            }
        }
        //斜向连线    
        if (state[1, 1] != 0 &&
            state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2] ||
            state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0])
        {
            return state[1, 1];
        }
        return 0;
    }
}
