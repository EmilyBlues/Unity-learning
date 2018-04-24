using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    public GameObject diskPrefab = null;
    private List<DiskData> used = new List<DiskData>();
    private List<DiskData> free = new List<DiskData>();

    public GameObject GetDisk(int round)
    {
        int choice = 0;
        int scope = 1, scope1 = 4, scope2 = 7;
        float startY = -10f;
        string tag;
        diskPrefab = null;

        if(round == 1)
            choice = Random.Range(0, scope);
        else if(round == 2)
            choice = Random.Range(0, scope1);
        else if(round == 3)
            choice = Random.Range(0, scope2);

        if(choice <= scope)
            tag = "disk1";
        else if(choice <= scope && choice > scope1)
            tag = "disk2";
        else
            tag = "disk3";

        for(int i = 0; i < free.Count; i++)
        {
            if(free[i].tag == tag)
            {
                diskPrefab = free[i].gameObject;
                free.Remove(free[i]);
                break;
            }
        }
        if(diskPrefab == null)
        {
            if (tag == "disk1")
                diskPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/disk1"), new Vector3(0, startY, 0), Quaternion.identity);
            else if(tag == "disk2")
                diskPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/disk2"), new Vector3(0, startY, 0), Quaternion.identity);
            else if(tag == "disk3")
                diskPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/disk3"), new Vector3(0, startY, 0), Quaternion.identity);

            float ranX = Random.Range(-1f, -1f) < 0 ? -1 : 1;
            diskPrefab.GetComponent<DiskData>().direction = new Vector3(ranX, startY, 0);
            diskPrefab.transform.localScale = diskPrefab.GetComponent<DiskData>().scale;
        }
        used.Add(diskPrefab.GetComponent<DiskData>());
        return diskPrefab;
    }

    public void FreeDisk(GameObject disk)
    {
        for (int i = 0; i < used.Count; i++)
        {
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID())
            {
                used[i].gameObject.SetActive(false);
                free.Add(used[i]);
                used.Remove(used[i]);
                break;
            }
        }
    }

}
