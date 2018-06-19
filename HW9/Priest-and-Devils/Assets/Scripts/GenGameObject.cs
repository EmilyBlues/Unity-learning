using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenGameObject : MonoBehaviour {

    public GameObject[] priests_start = new GameObject[3];
    public GameObject[] priests_end = new GameObject[3];
    public GameObject[] devils_start = new GameObject[3];
    public GameObject[] devils_end = new GameObject[3];
    public GameObject[] boat = new GameObject[2];
    //用数组存储在船上的gameobject
    public GameObject boat_obj;
    //获取船的gameobject
    public Vector3 shoreStartPos = new Vector3(8, 0, 0);
    //起点的岸的坐标
    public Vector3 shoreEndPos = new Vector3(-8, 0, 0);
    //终点的岸的坐标
    public Vector3 boatStartPos = new Vector3(6, 0, 0);
    public Vector3 boatEndPos = new Vector3(-14, 0, 0);
    //记录船的两个位置
    public float gap = 1.0f;
    public int boatCapacity = 2;
    //纪录船的容量
    public int boat_position = 0;
    //纪录船的位置
    public int game = 0;
    public int find = 0;

    public Vector3 priestStartPos = new Vector3(6, 0, 0);
    public Vector3 priestEndPos = new Vector3(-6, 0, 0);
    public Vector3 devilStartPos = new Vector3(7, 0, 0);
    public Vector3 devilEndPos = new Vector3(-7, 0, 0);

    public Vector3 waterPos = new Vector3(0, 0, 0);
    public Vector3 waterPos1 = new Vector3(0, 0, 0);

    public void LoadResources()
    //载入资源
    {
        // shore  
        Instantiate(Resources.Load("Prefabs/Stone"), shoreStartPos, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Stone"), shoreEndPos, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Water"), waterPos, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Water"), waterPos1, Quaternion.identity);
        // boat  
        boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), boatStartPos, Quaternion.identity) as GameObject;
        // priests & devils  
        for (int i = 0; i < 3; ++i)
        {
            priests_start[i] = (Instantiate(Resources.Load("Prefabs/Priest")) as GameObject);
            priests_end[i] = null;
            devils_start[i] = (Instantiate(Resources.Load("Prefabs/Devil")) as GameObject);
            devils_end[i] = null;
        }
    }

    void setCharacterPositions(GameObject[] array, Vector3 pos)
    //设置人物位置
    {
        for (int i = 0; i < 3; ++i)
        {
            if (array[i] != null)
                array[i].transform.position = new Vector3(pos.x + gap * i, pos.y, pos.z);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        setCharacterPositions(priests_start, priestStartPos);
        setCharacterPositions(priests_end, priestEndPos);
        setCharacterPositions(devils_start, devilStartPos);
        setCharacterPositions(devils_end, devilEndPos);
    }
}
