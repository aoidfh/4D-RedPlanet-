using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Robot_RoundPos
{
    public List<Transform> step_Pos;
}
[System.Serializable]
public class RobotList
{
    public List<GameObject> soldier;
}

public class Lim_RobotEnemy_Spawn : MonoBehaviour
{    
    public List<GameObject> Round_Objs;

    public List<RobotList> soldierList = new List<RobotList>();

    public List<Robot_RoundPos> round_Pos;
    public GameObject moveToPoints;
    public List<Lim_RobotSoldier> soldier_List;
    public List<Lim_RobotDrone> drone_List;


    public Transform camera;
    public List<Transform> spawn_Pos;
    public List<Transform> fixSpawn_Pos;

    int rand;
    public int soldier_number;
    public int soldier_Index;
    public int drone_Index;
    public int soldier_SpawnIdx;
    public int spawn_MaxIndex;
    public int fix_Index;

    public bool spawn_Start;

    public float spawn_Time;
    float spawn_ResetTime = 2f;

    IEnumerator temp_SpawnCoroutine;

    public int spawn_Index;
    private void Awake()
    {
        for (int i = 0; i < Round_Objs.Count; i++)
        {           
            for (int j = 0; j < 5; j++)
            {
                round_Pos[i].step_Pos.Add(Round_Objs[i].transform.GetChild(j));
            }
        }
    }

    private void Start()
    {
        spawn_Time = spawn_ResetTime;

       
        for (int i = 0; i < Round_Objs.Count; i++)
        {
            Round_Objs[i].SetActive(false);
        }

        moveToPoints.SetActive(false);
    }

    private void Update()
    {
        Shortest_Distance();

        if (spawn_Start)
        {
            //3라운드, 5라운드 생성 개수 줄임.. 생성 시켜놓은 적이 있기 때문
            if (Lim_RobotGameManager.Instance.step_Idx == 0 && Lim_RobotGameManager.Instance.round_Idx + 1 != 3
                && Lim_RobotGameManager.Instance.round_Idx + 1 != 5)
            {
                if (GameManager.Instance.playerCnt == 1)
                    spawn_MaxIndex = 2;
                else
                    spawn_MaxIndex = 4;
            }
            else
                spawn_MaxIndex = 1;

            temp_SpawnCoroutine = Spawn_Start(Lim_RobotGameManager.Instance.round_Idx, Lim_RobotGameManager.Instance.step_Idx);
            StartCoroutine(temp_SpawnCoroutine);

            spawn_Start = false;
        }
        else
        {
            if (Lim_RobotGameManager.Instance.step_Idx != 0 && !Lim_RobotGameManager.Instance.is_Pause)
            {
                spawn_Time -= Time.deltaTime;

                if (spawn_Time <= 0)
                {
                    spawn_Start = true;
                    spawn_Time = spawn_ResetTime;
                }
            }
            else
            {
                spawn_Start = false;
                spawn_Time = spawn_ResetTime;
            }
        }
    }

    public IEnumerator Spawn_Start(int round_Idx, int step_Idx)
    {      
        if (round_Pos[round_Idx] == null) { yield return null; }

        if (step_Idx == 0)
            fix_Index = 0;

        spawn_Pos.Clear();
        fixSpawn_Pos.Clear();

        Transform pos = round_Pos[round_Idx].step_Pos[step_Idx];

        //고정 배치
        if (pos.GetChild(0).childCount != 0)
        {
            for (int i = 0; i < pos.GetChild(0).childCount; i++)
            {
                fixSpawn_Pos.Add(pos.GetChild(0).GetChild(i));
            }
        }

        //이동 배치
        if (pos.GetChild(1).childCount != 0)
        {
            for (int i = 0; i < pos.GetChild(1).childCount; i++)
            {
                spawn_Pos.Add(pos.GetChild(1).GetChild(i));
            }
        }

        while(soldier_List.Count != 0 && step_Idx == 0)
        {
            yield return new WaitForSeconds(1);
        }

        if(round_Idx == 5 && step_Idx == 0)
        {
            while(Lim_RobotGameManager.Instance.is_Pause)
            {
                yield return new WaitForSeconds(1);
            }
        }

        for (int i = 0; i < spawn_MaxIndex; i++)
        {
            rand = Random.Range(0, spawn_Pos.Count);

            soldier_SpawnIdx = Random.Range(0, soldierList[Lim_RobotGameManager.Instance.round_Idx].soldier.Count);

            yield return new WaitForSeconds(0.5f);
            if (i < fixSpawn_Pos.Count)
            {
                if ((round_Idx == 2 || round_Idx == 4) && step_Idx == 4) //라운드3, 라운드5, 스탭5
                {
                    Spawn(fixSpawn_Pos[fix_Index], RobotSpawnType.MoveAfterFixing, soldier_SpawnIdx);
                    fix_Index++;
                }
                else
                {
                    Spawn(fixSpawn_Pos[i], RobotSpawnType.MoveAfterFixing, soldier_SpawnIdx);
                }
            }
            else
            {
                Spawn(spawn_Pos[rand], RobotSpawnType.UnFix, soldier_SpawnIdx);
                spawn_Pos.RemoveAt(rand);
            }
            soldier_SpawnIdx++;
        }
        Lim_RobotGameManager.Instance.step_Idx++;
        if (Lim_RobotGameManager.Instance.step_Idx == 5) Lim_RobotGameManager.Instance.step_Idx = 4;
    }

    void FindChild(GameObject target, int enemy_Idx)
    {
        foreach (Transform child in target.transform)
        {
            if (child.GetComponent<SoldierIndex>() != null)
            {
                child.GetComponent<SoldierIndex>().index = enemy_Idx;
            }
            if (child.childCount != 0)
            {
                FindChild(child.gameObject, enemy_Idx);
            }
        }
    }
    void Spawn(Transform spawn_Pos, RobotSpawnType fix, int soldier_SpawnIdx)
    {                
        GameObject obj = Instantiate(soldierList[Lim_RobotGameManager.Instance.round_Idx].soldier[soldier_SpawnIdx],
            spawn_Pos.position, Quaternion.identity);
        
        if (obj.transform.GetChild(0).GetComponent<Lim_RobotSoldier>() != null)
        {
            Lim_RobotSoldier lim_FasterSoldier = obj.transform.GetChild(0).GetComponent<Lim_RobotSoldier>();         

            lim_FasterSoldier.spawn_Index = soldier_number;
            lim_FasterSoldier.spawn_Type = fix;
            lim_FasterSoldier.round_Idx = Lim_RobotGameManager.Instance.round_Idx;
            lim_FasterSoldier.step_Idx = Lim_RobotGameManager.Instance.step_Idx;
            lim_FasterSoldier.creation_Location = spawn_Pos;

            soldier_List.Add(lim_FasterSoldier);

            if (fix == RobotSpawnType.UnFix)
                lim_FasterSoldier.TargetPoint_Move();

            FindChild(obj, soldier_Index);
            soldier_Index++;
        }
        //else if(obj.transform.GetChild(0).GetComponent<Lim_RobotDrone>())
        //{
        //    Lim_RobotDrone lim_Fasterdrone = obj.transform.GetChild(0).GetComponent<Lim_RobotDrone>();           

        //    lim_Fasterdrone.spawn_Index = soldier_number;
        //    lim_Fasterdrone.spawn_Type = fix;
        //    lim_Fasterdrone.round_Idx = Lim_RobotGameManager.Instance.round_Idx;
        //    lim_Fasterdrone.step_Idx = Lim_RobotGameManager.Instance.step_Idx;          

        //    drone_List.Add(lim_Fasterdrone);

        //    //lim_Fasterdrone.TargetPoint_Move();            

        //    FindChild(obj, drone_Index);
        //    drone_Index++;
        //}             
                     
        soldier_number++;
    }

    public void Hit(float val, int index, int player = 0,Lim_RobotSoldier soldier = null , Lim_RobotDrone drone = null , RobotPart robotPart = RobotPart.BODY)
    {        
        if (soldier != null)
        {
            soldier.StopAllCoroutines();
            soldier.Hit(robotPart, val, player);
        }
        else if(drone != null)
        {
            drone.StopAllCoroutines();
            drone.Hit(val, player);
        }
    }
    
    void Shortest_Distance()
    {
        if(spawn_Index < 6)
        {
            float distance = (camera.transform.position - Round_Objs[spawn_Index].transform.position).magnitude;

            if (distance <= 3f && Lim_RobotGameManager.Instance.step_Idx == 0)
            {
                spawn_Start = true;
                spawn_Index++;
            }
        }     
    }

    public IEnumerator Clear_SoldierList()
    {        
        if (soldier_List.Count != 0)
        {         
            for (int i = 0; i < soldier_List.Count; i++)
            {
                if (soldier_List[i].isDie)
                {
                    Destroy(soldier_List[i].parent_Obj);
                    soldier_List.RemoveAt(i);
                    i--;
                }
                else
                {
                    soldier_List[i].Back_Postion();
                    yield return new WaitForSeconds(0.1f);
                }
            }

            soldier_List.Clear();
        }       

        soldier_Index = 0;       
        soldier_SpawnIdx = 0;            
    }

    public void Spawn_StopCourutine()
    {
        if(temp_SpawnCoroutine == null) return;
        StopCoroutine(temp_SpawnCoroutine);
        temp_SpawnCoroutine = null;
        spawn_Start = false;
        spawn_Time = spawn_ResetTime;
    }

    public void ClearSoldierList_CourutineStart()
    {
        StartCoroutine(Clear_SoldierList());
    }

    public void Skip_Round6Intro()
    {
        if (temp_SpawnCoroutine == null) 
        {
            spawn_Start = true; 
        }
    }
}