# Adapter模式
在上周的作业中，我使用的设计模式是MVC模式。在这种MVC的设计模式中，结构清晰，而且每个类需要完成的工作也比较一目了然。这周我们需要把这个模式改成Adapter模式。以下是我学习到的Adapter设计模式。参考链接请点击[此处](https://blog.csdn.net/carson_ho/article/details/54910430)。  
## 1 介绍  
### 1.1 概述
适配器模式，即定义一个包装类，用于包装不兼容接口的对象。    

- 包装类 = 适配器Adapter； 
- 被包装对象 = 适配者Adaptee = 被适配的类。   
  
适配器模式的主要作用是：把一个类的接口变换成客户端所期待的另一种接口，从而使原本接口不匹配而无法一起工作的两个类能够在一起工作。适配器模式的形式分为：类的适配器模式和对象的适配器模式。适配器模式中主要解决的问题是：原本由于接口不兼容而不能一起工作的那些类可以在一起工作。  

###  1.2 UML图 
适配者模式可以用以下UML图表示：
![这里写图片描述](https://img-blog.csdn.net/2018042414101556?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)    
在上图中可以看出：  

- 冲突：Target期待调用Request方法，而Adaptee并没有（这就是所谓的不兼容了）。
-  解决方案：为使Target能够使用Adaptee类里的SpecificRequest方法，故提供一个中间环节Adapter类（继承Adaptee & 实现Target接口），把Adaptee的API与Target的API衔接起来（适配）。  

Adapter与Adaptee是继承关系，这决定了这个适配器模式是类的。  
## 2 本周代码改进
在使用Adapter模式中，我参考的UML图如下：
![这里写图片描述](https://img-blog.csdn.net/20180424142104416?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
结合上周我的代码，我做出如下修改：
![这里写图片描述](https://img-blog.csdn.net/20180424133358734?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0VtaWx5Qmx1c2U=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)   
### Interface 接口类的编写：
在修改代码时，我主要是修改了接口类，将之前参考老师写的代码中的接口单独出来放在Interface中，代码如下：
```C#
public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
//场景控制类
public interface ISceneController
{
    void LoadResources();
}
//之前已有的接口类，新增击中方法在其中，配置成Disk的Adapter接口
public interface IUserAction
{
    void Restart();
    void Hit(Vector3 pos);
    void GameOver();
    int GetScore();
    void BeginGame();
}

```  
接下来是编写UFOFlyAction，代码如下：
```C#
public class UFOFlyAction : SSAction {
    public float gravity = -5; 
    //设置物体下落时的重力
    private Vector3 startVector;
    //结合后面的随机，让物体出现的位置随机
    private Vector3 gravityVector = Vector3.zero;
    //物体做平抛运动是改变的位置
    private float time;
    //记录物体运动的时间，并在Update中更新
    private Vector3 currentAngle = Vector3.zero;
    //记录物体抛出的角度

    private UFOFlyAction () { }

    public static UFOFlyAction GetSSAction(Vector3 direction, float angle, float power)
    {
        UFOFlyAction action = CreateInstance<UFOFlyAction>();
        if (direction.x == -1)
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        }
        else
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        }
        return action;
    }

    public override void Update()
    {
        time += Time.fixedDeltaTime;
        gravityVector.y = gravity * time;

        transform.position += (startVector + gravityVector) * Time.fixedDeltaTime;
        currentAngle.z = Mathf.Atan((startVector.y + gravityVector.y) / startVector.x);
        transform.eulerAngles = currentAngle;

        if(this.transform.position.y < -10)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start () { }
}
```
接下来我们要修改FirstController，让他接近参考的UML图中PhysicsActionManager，其中修改的代码如下（我这里保留了之前的命名，仍为FirstController）：
```C#
 public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public FlyActionManager actionManager;
    public DiskFactory diskFactory;
    public UserGUI userGUI;
    public ScoreRecorder scoreRecorder;
    //加载资源

    private Queue<GameObject> diskQueue = new Queue<GameObject>();
    private List<GameObject> diskNotHit = new List<GameObject>();
    private int round = 1;//当前round
    private float speed = 2f;//飞碟的速度
    private bool isPlayGame = false;
    private bool isGameOver = false;
    private bool isGameStart = false;
    //用于记录游戏的各种状态
    private int scoreRound2 = 15;
    private int scoreRound3 = 30;
    //scoreRound2与scoreRound3用于记录round的数目

    void Start ()
    {
        SSDirector director = SSDirector.getInstance();     
        director.currentScenceController = this;             
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        actionManager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
    }
	
	//根据游戏的状态，对资源进行控制
	void Update ()
    {
        if(isGameStart)
        {
            if (isGameOver)
            {
                CancelInvoke("LoadResources");
            }
            if (!isPlayGame)
            {
                InvokeRepeating("LoadResources", 1f, speed);
                isPlayGame = true;
            }
            SendDisk();
            if (scoreRecorder.score >= scoreRound2 && round == 1)
            {
                round = 2;
                speed = speed - 0.6f;
                CancelInvoke("LoadResources");
                isPlayGame = false;
            }
            else if (scoreRecorder.score >= scoreRound3 && round == 2)
            {
                round = 3;
                speed = speed - 0.5f;
                CancelInvoke("LoadResources");
                isPlayGame = false;
            }
        }
    }

    //加载资源
    public void LoadResources()
    {
        diskQueue.Enqueue(diskFactory.GetDisk(round)); 
    }

    private void SendDisk()
    {
        float position_x = 16;                       
        if (diskQueue.Count != 0)
        {
            GameObject disk = diskQueue.Dequeue();
            diskNotHit.Add(disk);
            disk.SetActive(true);
            float ran_y = Random.Range(1f, 4f);
            float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            disk.GetComponent<DiskData>().direction = new Vector3(ran_x, ran_y, 0);
            Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * position_x, ran_y, 0);
            disk.transform.position = position;
            float power = Random.Range(10f, 15f);
            float angle = Random.Range(15f, 28f);
            actionManager.UFOFly(disk,angle,power);
        }

        for (int i = 0; i < diskNotHit.Count; i++)
        {
            GameObject temp = diskNotHit[i];
            if (temp.transform.position.y < -10 && temp.gameObject.activeSelf == true)
            {
                diskFactory.FreeDisk(diskNotHit[i]);
                diskNotHit.Remove(diskNotHit[i]);
                userGUI.BloodReduce();
            }
        }
    }

    //击中飞碟
    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool not_hit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<DiskData>() != null)
            {
                for (int j = 0; j < diskNotHit.Count; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == diskNotHit[j].gameObject.GetInstanceID())
                    {
                        not_hit = true;
                    }
                }
                if(!not_hit)
                {
                    return;
                }
                diskNotHit.Remove(hit.collider.gameObject);
                scoreRecorder.Record(hit.collider.gameObject);
                Transform explode = hit.collider.gameObject.transform.GetChild(0);
                explode.GetComponent<ParticleSystem>().Play();
                StartCoroutine(WaitingParticle(0.08f, hit, diskFactory, hit.collider.gameObject));
                break;
            }
        }
    }
    
    //记录得分
    public int GetScore()
    {
        return scoreRecorder.score;
    }
    
    //重新开始游戏
    public void Restart()
    {
        isGameOver = false;
        isPlayGame = false;
        scoreRecorder.score = 0;
        round = 1;
        speed = 2f;
    }

    //游戏结束
    public void GameOver()
    {
        isGameOver = true;
    }

    //等待击中飞碟时的粒子特效
    IEnumerator WaitingParticle(float wait_time, RaycastHit hit, DiskFactory disk_factory, GameObject obj)
    {
        yield return new WaitForSeconds(wait_time); 
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        disk_factory.FreeDisk(obj);
    }
    
    //重新开始游戏
    public void BeginGame()
    {
        isGameStart = true;
    }
}
``` 
在本次作业中，主要是修改了代码的结构，对游戏的总体的效果来说，并没有什么很大的改变，这里是上周游戏的视频录像[传送门](https://github.com/EmilyBlues/Unity3d-HW4/blob/master/HitUFO.mp4)，有兴趣的小伙伴可以看看哦。
作业源代码[传送门](https://github.com/EmilyBlues/Unity-learning/tree/master/HW5/Scripts)。
