using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour, IUserAction {
    public Button left;
    public Button right;
    //左边对话的气泡

    public Text leftText;
    public Text rightText;

    private int fontSize = 14;

    private float time = 2;
    private int current = 0;

    private ComFactory comFactory;
    //对话工厂
    public bool op = true;

    // Use this for initialization
    void Start () {
        GameDirector gameDirector = GameDirector.getInstance();
        gameDirector.currentSceneController = this;
        comFactory = ComFactory.GetInstance();
    }

    // Update is called once per frame
    void Update () {
        if (current < comFactory.com.Count)
        {
            if (comFactory.com[current].speaker == "left")
            {
                op = true;
                //说话的人是左边的人
                Talk(leftText);
            }
            else if (comFactory.com[current].speaker == "right")
            {
                op = true;
                //说话的人是右边的人
                Talk(rightText);
            }
        }
        else
        {
            op = true;
        }
    }

    private void Talk(Text talkText)
    {
        comFactory = ComFactory.GetInstance();
        reset();
        Vector3 position = talkText.rectTransform.localPosition;
        talkText.text = comFactory.com[current].content;
        if(comFactory.com[current].speaker == "left")
        {
            Animator leftAni = left.GetComponent<Animator>();
            leftAni.SetBool("big", true);
        }
        else
        {
            Animator rightAni = right.GetComponent<Animator>();
            rightAni.SetBool("big", true);
        }
        //产生对话

        if(time <= 0 && current < comFactory.com.Count)
        {
            if(comFactory.com[current].speaker == "left")
            {
                Animator leftAni = left.GetComponent<Animator>();
                leftAni.SetBool("big", false);
            }
            else
            {
                Animator rightAni = right.GetComponent<Animator>();
                rightAni.SetBool("big", false);
            }
            talkText.text = "";
            talkText.rectTransform.localPosition = position;
            if(current < comFactory.com.Count)
            {
                if(comFactory.com[current].content.Length * fontSize <= 10)
                {
                    time = 2;
                }
                else
                {
                    time = (comFactory.com[current].content.Length * fontSize - 50) * Time.deltaTime;
                }
            }
            //设置延迟
            if(talkText.text.Length * fontSize > 10)
            {
                position.x -= 1;
                talkText.rectTransform.localPosition = position;
            }
            //看看一句话的字长是否超过对话框
            time -= Time.deltaTime;
        }
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        if (GUI.Button(new Rect(0, 100, 100, 40), "<-"))
        {
            Back();
        }
        //显示上一条对话
        if (GUI.Button(new Rect(100, 100, 100, 40), "->"))
        {
            Next();
        }
        //显示下一句对话
    }

    public void Next()
    {
        if (op)
        {
            current++;
            op = false;
            reset();
        }
    }

    public void Back()
    {
        if (op)
        {
            current--;
            op = false;
            reset();
        }
    }

    public void reset()
    {
        time = 2;
        Animator leftAni = left.GetComponent<Animator>();
        leftAni.SetBool("big", false);
        Animator rightAni = right.GetComponent<Animator>();
        rightAni.SetBool("big", false);
    }
}
