这次作业其实难度没有之前巡逻兵什么的大，但是坑比较多，需要对canvas与animator比较了解。下面是我的一张成品图：
![这里写图片描述](https://img-blog.csdn.net/20180605190240521?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

## 1 添加Asset，Canvas与动画
### （1）Asset
在场景中我们需要简单的天空盒，需要两个人物，也需要对话框。所以我在Asset Store中下载了如图中的两个资源包。
![这里写图片描述](https://img-blog.csdn.net/20180605190630156?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
![这里写图片描述](https://img-blog.csdn.net/20180605190717125?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
### （2）Canvas
添加canvas，在下面添加两个button，如下所示：
![这里写图片描述](https://img-blog.csdn.net/20180605190812649?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)  
要注意的是，我们需要给这两个bubble加入动画，那就是出现和消失的动画。就参考师兄的博客上制作就好了。  
![这里写图片描述](https://img-blog.csdn.net/20180605190931610?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)   
其实动画的制作就是气泡的出现和气泡的消失，只需要对气泡的rect.transform以及scale作处理就好了。别忘了要设置一个参数，控制状态的转化。   
![这里写图片描述](https://img-blog.csdn.net/20180605191422418?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
我在这里其实踩了几个坑的，一个是在设置button的动画的时候，一不小心把button的自带的script删了，导致button不是button。还有一个就是button中的Text无法显示，这个问题是因为canvas的像素比较低，需要调节canvas的像素就好了。    
再设计完简单的场景之后，就是脚本了。以下是代码部分。  

## 代码实现  
### 代码设计模式：  
我的设计模式是参考师兄的，主要是按照下面这个UML图来写的。  
![这里写图片描述](https://img-blog.csdn.net/20180605192901785?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)  
#### GameDirector
```C#
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
```
#### IUserAction
```C#
public interface IUserAction
{
    void Next();
    void Back();
}
```
#### Speak
```C#
public class Speak {
    public string content;
    //说话内容
    public string speaker;
    //发言者

    public Speak(string content, string speaker)
    {
        this.content = content;
        this.speaker = speaker;
    }
}
```
#### ComFactory
```C#
public class ComFactory {
    public List<Speak> com;
    //储存所有对话

    private static ComFactory instance;

	// Use this for initialization
    private ComFactory () {
        //构造函数
        com = new List<Speak>();
        com.Add(new Speak("你好呀，你喜欢看电影吗？", "left"));
        com.Add(new Speak("我喜欢呢！", "right"));
        com.Add(new Speak("你都喜欢什么电影？", "left"));
        com.Add(new Speak("《大话西游》", "right"));
        com.Add(new Speak("我也喜欢呢！", "left"));
        com.Add(new Speak("你还喜欢什么？", "right"));
        com.Add(new Speak("《2046》", "left"));
        com.Add(new Speak("都是文艺片呢~", "right"));
        com.Add(new Speak("曾经我也是文艺青年", "left"));
        com.Add(new Speak("都过去了。", "right"));
        com.Add(new Speak("你单身吗？", "left"));
        com.Add(new Speak("emmm，没有", "right"));
        com.Add(new Speak("……，再见吧！", "left"));
	}

    public static ComFactory GetInstance()
    {
    //单例模式
        if(instance == null)
        {
            instance = new ComFactory();
        }
        return instance;
    }
}
```
#### SceneController
```C#
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour, IUserAction {
    public Button left;
    public Button right;
    //对话的气泡
    public Text leftText;
    public Text rightText;
    //气泡的文字部分
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
        //开始说话时，让气泡全都下去。
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
```
这是代码的[传送门]()。

这是成品的gif图片
![这里写图片描述](https://img-blog.csdn.net/20180605195737827?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

