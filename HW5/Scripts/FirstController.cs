using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public FlyActionManager actionManager;
    public DiskFactory diskFactory;
    public UserGUI userGUI;
    public ScoreRecorder scoreRecorder;

    private Queue<GameObject> diskQueue = new Queue<GameObject>();
    private List<GameObject> diskNotHit = new List<GameObject>();
    private int round = 1;
    private float speed = 2f;
    private bool isPlayGame = false;
    private bool isGameOver = false;
    private bool isGameStart = false;
    private int scoreRound2 = 15;
    private int scoreRound3 = 30;

    void Start ()
    {
        SSDirector director = SSDirector.getInstance();     
        director.currentScenceController = this;             
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        actionManager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
    }
	
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

    public int GetScore()
    {
        return scoreRecorder.score;
    }

    public void Restart()
    {
        isGameOver = false;
        isPlayGame = false;
        scoreRecorder.score = 0;
        round = 1;
        speed = 2f;
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    IEnumerator WaitingParticle(float wait_time, RaycastHit hit, DiskFactory disk_factory, GameObject obj)
    {
        yield return new WaitForSeconds(wait_time); 
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        disk_factory.FreeDisk(obj);
    }

    public void BeginGame()
    {
        isGameStart = true;
    }
}
