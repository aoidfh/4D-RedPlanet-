using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RayFire;
using System.Diagnostics;

public enum RobotPart
{
    HEAD,
    BODY
}

public enum RobotSpawnType
{
    UnFix,
    MoveAfterFixing,
    Fix
}

[System.Serializable]
public class Robot_Soldier
{
    public float max_Hp;
    public float damage;
    public int bodyScore;
    public int headScore;
    public int killScore;
    public Robot_Soldier(int index, RobotType robotType)
    {
        if (index == 1)
        {
            switch (robotType)
            {
                case RobotType.PrivateFirstClass:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp1_1P;
                    damage = Lim_RobotDefine.RobotSoldierDmg1_1P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody1;
                    headScore = Lim_RobotDefine.RobotSoldierHead1;
                    killScore = Lim_RobotDefine.RobotSoldier1Score1P;
                    break;
                case RobotType.Sergeant:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp2_1P;
                    damage = Lim_RobotDefine.RobotSoldierDmg2_1P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody2;
                    headScore = Lim_RobotDefine.RobotSoldierHead2;
                    killScore = Lim_RobotDefine.RobotSoldier2Score1P;
                    break;
                case RobotType.StaffSergeant:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp3_1P;
                    damage = Lim_RobotDefine.RobotSoldierDmg3_1P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody3;
                    headScore = Lim_RobotDefine.RobotSoldierHead3;
                    killScore = Lim_RobotDefine.RobotSoldier3Score1P;
                    break;
                case RobotType.SecondLieutenant:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp4_1P;
                    damage = Lim_RobotDefine.RobotSoldierDmg4_1P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody4;
                    headScore = Lim_RobotDefine.RobotSoldierHead4;
                    killScore = Lim_RobotDefine.RobotSoldier4Score1P;
                    break;
                case RobotType.Drone:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp5_1P;
                    damage = Lim_RobotDefine.RobotSoldierDmg5_1P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody5;
                    headScore = Lim_RobotDefine.RobotSoldierHead5;
                    killScore = Lim_RobotDefine.RobotSoldier5Score1P;
                    break;
            }
         
        }
        else
        {
            switch (robotType)
            {
                case RobotType.PrivateFirstClass:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp1_2P;
                    damage = Lim_RobotDefine.RobotSoldierDmg1_2P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody1;
                    headScore = Lim_RobotDefine.RobotSoldierHead1;
                    killScore = Lim_RobotDefine.RobotSoldier1Score2P;
                    break;
                case RobotType.Sergeant:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp2_2P;
                    damage = Lim_RobotDefine.RobotSoldierDmg2_2P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody2;
                    headScore = Lim_RobotDefine.RobotSoldierHead2;
                    killScore = Lim_RobotDefine.RobotSoldier2Score2P;
                    break;
                case RobotType.StaffSergeant:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp3_2P;
                    damage = Lim_RobotDefine.RobotSoldierDmg3_2P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody3;
                    headScore = Lim_RobotDefine.RobotSoldierHead3;
                    killScore = Lim_RobotDefine.RobotSoldier3Score2P;
                    break;
                case RobotType.SecondLieutenant:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp4_2P;
                    damage = Lim_RobotDefine.RobotSoldierDmg4_2P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody4;
                    headScore = Lim_RobotDefine.RobotSoldierHead4;
                    killScore = Lim_RobotDefine.RobotSoldier4Score2P;
                    break;
                case RobotType.Drone:
                    max_Hp = Lim_RobotDefine.RobotSoldierHp5_2P;
                    damage = Lim_RobotDefine.RobotSoldierDmg5_2P;
                    bodyScore = Lim_RobotDefine.RobotSoldierBody5;
                    headScore = Lim_RobotDefine.RobotSoldierHead5;
                    killScore = Lim_RobotDefine.RobotSoldier5Score2P;
                    break;
            }
        }
    }
}

public class Lim_RobotSoldier : MonoBehaviour
{
    Lim_RobotEnemy_Spawn lim_RobotEnemy_Spawn;
    Lim_FasterPossibleMotion lim_FasterPossibleMotion;
    Lim_FasterPossibleMotion temp_possible_MoveArea;
    Lim_FasterPossibleMotion possible_MoveArea;

    public Robot_Soldier robot_Soldier;
    public float Hp;

    public RobotType robot_Type;
    public RobotSpawnType spawn_Type = RobotSpawnType.UnFix;
    public SoldierState state = SoldierState.Idle;

    public GameObject parent_Obj;
    public GameObject[] ani_Bodys = new GameObject[2];
    public GameObject ragdoll;
    public Rigidbody ragdoll_rigd;

    public string temp_State;
    public string temp_Motion;
    public List<string> temp_Point;

    public List<Transform> movePoint_List = new List<Transform>();
    public List<Transform> barrel_JumpPoint = new List<Transform>();
    public List<Transform> sand_MovePoint = new List<Transform>();

    public Transform creation_Location;  //만들어진 위치    
    public Transform look_Player;
    public Transform step_Point;

    int rand;
    int move_Rand;

    public List<float> distance_List;

    public int spawn_Index;
    public int road = 0;

    bool can_Hide;
    bool reset;
    bool isShotCoroutine;
    bool fixIs_ShotCoroutine;
    bool look_Target;
    bool jump_Over;

    public bool isDie;
    bool next_Move = true;

    float soldier_PosX; float soldier_PosZ; float rotAnglePerSecond = 720;

    Vector3 look;

    public Animator anim;
    public NavMeshAgent agent;

    public int round_Idx;
    public int step_Idx;

    public GameObject crash_Effect1; public GameObject crash_Effect2;
    public ParticleSystem muzzle_Obj; public Transform fire_Pos;
    public GameObject nomal_Death;
    public GameObject aimingP1_Obj; public GameObject aimingP2_Obj;
    public GameObject laser;

    public GameObject real_Mesh; public GameObject destroyObj;
    public GameObject gun;
    public RayfireBomb rayfirebomb;

    public List<Transform> camera_All;  //모든 카메라 각도 구하기 위해..?
    public List<Transform> side_Camera;  //사이드만 카메라 각도 구하기 위해..?
    Coroutine aimStart;

    public ParticleSystem eff_NPC_dissapear;  //라운드 종료 시 살아있는 로봇 사라지는 이펙트
    public ParticleSystem eff_NPC_apear;  //시작 시 나타나는 이펙트
    private void Awake()
    {
        robot_Soldier = new Robot_Soldier(GameManager.Instance.playerCnt, robot_Type);
        Hp = robot_Soldier.max_Hp;

        agent = gameObject.transform.parent.GetComponent<NavMeshAgent>();

        look_Player = GameObject.Find("Camera_Center").transform;
        lim_RobotEnemy_Spawn = GameObject.Find("Enemy").GetComponent<Lim_RobotEnemy_Spawn>();

        if (spawn_Type != RobotSpawnType.Fix)
        {
            for (int i = 0; i < ani_Bodys.Length; i++)
            {
                ani_Bodys[i].SetActive(true);
            }

            ragdoll.SetActive(false);
        }          
    }

    private void OnEnable()
    {
        if (spawn_Type == RobotSpawnType.Fix)
        {
            anim.Play("CoverLo_AimR");
            fixIs_ShotCoroutine = false;
            aimingP1_Obj.SetActive(false);
            aimingP2_Obj.SetActive(false);
        }
    }
    private void Start()
    {
        if (spawn_Type != RobotSpawnType.UnFix)
        {
            if (Lim_RobotGameManager.Instance.round_Idx == 2 || Lim_RobotGameManager.Instance.round_Idx == 4)
                TypeFix_StandStart();
            else
                TypeFix_Start();

        }
        else
            anim.Play("_pistol_Idle_Relaxed");

        aimingP1_Obj.SetActive(false);
        aimingP2_Obj.SetActive(false);
        crash_Effect1.SetActive(false);
        crash_Effect2.SetActive(false);

        camera_All.Add(GameObject.Find("Camera_Center").transform);
        camera_All.Add(GameObject.Find("Camera_Left").transform);
        camera_All.Add(GameObject.Find("Camera_Right").transform);

        side_Camera.Add(GameObject.Find("Camera_Left").transform);
        side_Camera.Add(GameObject.Find("Camera_Right").transform);

        if (eff_NPC_apear != null)
            eff_NPC_apear.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Start_Regdoll();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            StopAllCoroutines();
            Hit(RobotPart.BODY, 0);
        }

        if (spawn_Type == RobotSpawnType.Fix)
        {
            if (!fixIs_ShotCoroutine)
            {
                for (int i = 0; i < camera_All.Count; i++)
                {
                    Vector3 direction_Camera = this.transform.position - camera_All[i].position;

                    float angle = Vector3.Angle(direction_Camera, camera_All[i].forward);

                    if (angle < 30)
                    {
                        if (aimStart == null)
                        {
                            aimStart = StartCoroutine(TypeFix_AimStart());
                            //Debug.Log(angle);
                            fixIs_ShotCoroutine = true;
                        }
                    }
                }
            }
        }

        switch (state)
        {
            case SoldierState.Idle:
                break;
            case SoldierState.Attack:
                break;
            case SoldierState.Hit:
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol_Hit_1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol_Hit_2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol_Hit_3") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    if (spawn_Type == RobotSpawnType.Fix)
                    {
                        anim.SetTrigger("Fix_Aim");
                        fixIs_ShotCoroutine = false;
                        state = SoldierState.Aim;
                    }
                    else
                    {
                        if (temp_State == "Hide")
                        {
                            Next_RanomMotion();
                        }
                        else if (temp_State == "Move" || temp_State == "Hit" || temp_State == "Jump")
                        {
                            TargetPoint_Move();
                        }
                        else if (temp_State == "Aim")
                        {
                            int rand = Random_Index(1, 101);
                            if (rand <= 20 && can_Hide)
                            {
                                AnimOff();
                            }
                            else if (rand >= 21 && rand <= 80)
                            {
                                TargetPoint_Move();
                            }
                            else
                            {
                                state = SoldierState.Aim;
                                anim.SetTrigger("IsAim");
                            }
                        }
                    }

                }
                break;
            case SoldierState.Move:
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("_pistol_SplintLoop"))
                {
                    if (!agent.pathPending)
                    {
                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                            {
                                anim.speed = 1;
                                next_Move = true;

                                if (movePoint_List[move_Rand].GetComponent<Lim_FasterPossibleMotion>() != null)
                                {
                                    lim_FasterPossibleMotion = movePoint_List[move_Rand].GetComponent<Lim_FasterPossibleMotion>();
                                }

                                //Debug.Log("tag : " + movePoint_List[move_Rand].gameObject.tag + "  rand : " + move_Rand);

                                if (movePoint_List[move_Rand].gameObject.CompareTag("barrel"))
                                {
                                    rand = Random.Range(0, 2);

                                    if (rand == 0 && lim_FasterPossibleMotion.sit)
                                        Sit_Hide();
                                    else if (rand == 1 && lim_FasterPossibleMotion.stand)
                                        StandAim_Start();
                                }
                                else if (movePoint_List[move_Rand].gameObject.CompareTag("WOOD"))
                                {
                                    Stand_Hide();
                                }
                                else if (movePoint_List[move_Rand].gameObject.CompareTag("SAND"))
                                {

                                    sand_MovePoint.Clear();
                                    can_Hide = false;

                                    look_Target = true;
                                    anim.SetTrigger("IsAim");
                                    state = SoldierState.Aim;
                                }
                                else if (movePoint_List[move_Rand].gameObject.CompareTag("Jump"))
                                {
                                    barrel_JumpPoint.Clear();
                                    for (int i = 0; i < movePoint_List[move_Rand].childCount; i++)
                                    {
                                        barrel_JumpPoint.Add(movePoint_List[move_Rand].GetChild(i).gameObject.transform);
                                    }

                                    agent.enabled = false;
                                    Jump_Over();
                                }

                                reset = false;
                            }
                        }
                    }
                }
                break;
            case SoldierState.Hide:
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("CoverHi_CoverR") ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("CoverHi_CoverL") ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("CoverLo_CoverL") ||
                    anim.GetCurrentAnimatorStateInfo(0).IsName("CoverLo_CoverR"))
                {
                    if (!reset)
                    {
                        reset = true;
                        HideAimCoroutine_Start();
                    }
                }
                break;
            case SoldierState.Aim:
                look_Target = true;

                if (!isShotCoroutine && !Lim_RobotGameManager.Instance.is_Pause)
                {
                    Fire();
                }
                break;
            case SoldierState.Back:
                if (temp_possible_MoveArea != null)
                    temp_possible_MoveArea.possible_MoveArea = true;

                aimingP1_Obj.SetActive(false);
                aimingP2_Obj.SetActive(false);

                //if (!agent.enabled)
                //    agent.enabled = true;

                //if (roundBack_Transform != null)
                //    agent.SetDestination(roundBack_Transform.position);
                //else
                //    agent.SetDestination(creation_Location.position);
                break;
        }
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case SoldierState.Move:
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("_pistol_SplintLoop"))
                {
                    TurnToDestination(agent.steeringTarget);
                }
                break;
            case SoldierState.Jump:
                Move_Point(road);
                break;
            case SoldierState.Hit:
                break;
            case SoldierState.Back:
                TurnToDestination(agent.steeringTarget);
                break;
        }
    }

    private void LateUpdate()
    {
        if (spawn_Type != RobotSpawnType.Fix)
            look = new Vector3(look_Player.position.x, transform.position.y, look_Player.position.z);
        else
            look = new Vector3(look_Player.position.x, look_Player.position.y, look_Player.position.z);

        if (look_Target || spawn_Type == RobotSpawnType.Fix)
        {
            Look_Player();
        }
    }

    void TypeFix_Start()
    {
        if (creation_Location != null)
            lim_FasterPossibleMotion = creation_Location.GetComponent<Lim_FasterPossibleMotion>();

        if (spawn_Type == RobotSpawnType.Fix)
        {
            anim.Play("CoverLo_AimR");
        }
        else if (lim_FasterPossibleMotion.stand)
        {
            if (lim_FasterPossibleMotion.left_Aim)
            {
                anim.Play("CoverHi_CoverL");
                Stand_Hide();
            }
            else if (lim_FasterPossibleMotion.right_Aim)
            {
                anim.Play("CoverHi_CoverR");
                Stand_Hide();
            }
            else if (!lim_FasterPossibleMotion.left_Aim && !lim_FasterPossibleMotion.right_Aim)
            {
                anim.Play("CoverLo_Cover");
                StandAim_Start();
            }

        }
        else if (lim_FasterPossibleMotion.sit)
        {
            if (lim_FasterPossibleMotion.left_Aim)
                anim.Play("CoverLo_CoverL");
            else if (lim_FasterPossibleMotion.right_Aim)
                anim.Play("CoverLo_CoverR");

            Sit_Hide();
        }

        Shortest_Distance();
    }

    void TypeFix_StandStart()
    {
        if (creation_Location != null)
            lim_FasterPossibleMotion = creation_Location.GetComponent<Lim_FasterPossibleMotion>();

        if (spawn_Type == RobotSpawnType.Fix)
        {
            anim.Play("CoverLo_AimR");
        }
        else if (lim_FasterPossibleMotion.stand)
        {
            anim.Play("_pistol_Idle");
            StandAim_Start();
        }


        Shortest_Distance();
    }

    void Move_Point(int road_Index = 0)
    {
        if (state == SoldierState.Jump)
        {
            Vector3 parent = parent_Obj.transform.position;
            Vector3 barrel_LastRoad = barrel_JumpPoint[barrel_JumpPoint.Count - 1].transform.position;
            Vector3 barrel_Road = barrel_JumpPoint[road_Index].transform.position;

            if (road_Index <= 1)
                parent_Obj.transform.position = Vector3.MoveTowards(
                    parent_Obj.transform.position, barrel_JumpPoint[road_Index].transform.position, 3.5f * Time.deltaTime);
            else
                agent.SetDestination(barrel_JumpPoint[road_Index].transform.position);

            if (parent.x == barrel_LastRoad.x && parent.z == barrel_LastRoad.z)
            {
                if (road == barrel_JumpPoint.Count - 1)
                {
                    state = SoldierState.Aim;
                    anim.SetTrigger("IsAim");
                    road = 0;
                    look_Target = true;
                }
            }
            else if (parent.x == barrel_Road.x && parent.z == barrel_Road.z)
            {
                if (road_Index == 1)
                {
                    agent.enabled = true;
                    //agent.speed = 3;

                    anim.speed = 1f;
                    anim.SetTrigger("Run");
                }

                road++;
            }
        }
    }
    void TurnToDestination(Vector3 curTargetPos)
    {
        //회전할 목표 방향
        if (curTargetPos - parent_Obj.transform.position != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(curTargetPos - parent_Obj.transform.position);
            parent_Obj.transform.rotation = Quaternion.RotateTowards(parent_Obj.transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);

            if (parent_Obj.transform.rotation == lookRotation)
            {
                look_Target = false;
            }
        }
    }
    public void TargetPoint_Move()
    {
        if (next_Move)
        {
            Shortest_Distance();

            for (int i = 0; i < movePoint_List.Count; i++)
            {
                possible_MoveArea = movePoint_List[i].GetComponent<Lim_FasterPossibleMotion>();

                if (possible_MoveArea.possible_MoveArea == true)
                {
                    state = SoldierState.Move;

                    agent.enabled = true;
                    agent.SetDestination(movePoint_List[i].transform.position);

                    anim.speed = 0.8f;
                    anim.SetTrigger("Run");
                    temp_Motion = "Run";

                    temp_Point.Add(movePoint_List[i].name);

                    move_Rand = i;
                    possible_MoveArea.possible_MoveArea = false;
                    i = movePoint_List.Count;
                }
            }

            if (temp_possible_MoveArea != null)
            {
                temp_possible_MoveArea.possible_MoveArea = true;
            }

            temp_possible_MoveArea = possible_MoveArea;
        }
        else
        {
            state = SoldierState.Move;

            agent.enabled = true;
            agent.SetDestination(movePoint_List[move_Rand].transform.position);

            anim.speed = 0.8f;
            anim.SetTrigger("Run");
            temp_Motion = "Run";
        }

        parent_Obj.SetActive(true);
    }

    public void Shortest_Distance()
    {
        movePoint_List.Clear();
        distance_List.Clear();

        if (spawn_Type == RobotSpawnType.UnFix)
        {
            step_Point = lim_RobotEnemy_Spawn.transform.Find("Move to Points").transform;

            for (int i = 0; i < step_Point.childCount; i++)
            {
                float distance = (transform.position - step_Point.transform.GetChild(i).transform.position).magnitude;

                if (distance >= 0.5f)  //자신의 거리에서 0.5보다 커야 생성
                {
                    this.distance_List.Add(distance);
                    movePoint_List.Add(step_Point.transform.GetChild(i));
                }
            }

            for (int i = 0; i < movePoint_List.Count; i++)  //이동 했던 지점은 삭제
            {
                for (int j = 0; j < temp_Point.Count; j++)
                {
                    if (movePoint_List.Count == 0)
                    {
                        j = temp_Point.Count;
                    }
                    else if (movePoint_List[i].name == temp_Point[j])
                    {
                        distance_List.RemoveAt(i);
                        movePoint_List.RemoveAt(i);

                        if (i > 0)
                            i--;

                        j = 0;
                    }
                }
            }

            if (movePoint_List.Count == 0)
            {
                temp_Point.Clear();

                for (int i = 0; i < step_Point.childCount; i++)
                {
                    float distance = (transform.position - step_Point.transform.GetChild(i).transform.position).magnitude;

                    if (distance >= 0.5f)  //자신의 거리에서 0.5보다 커야 생성
                    {
                        this.distance_List.Add(distance);
                        movePoint_List.Add(step_Point.transform.GetChild(i));
                    }
                }
            }

            for (int i = 0; i < distance_List.Count; i++)  //짧은 거리순으로 순서 바꾸기
            {
                for (int j = i + 1; j < distance_List.Count; j++)
                {
                    if (distance_List[i] > distance_List[j])
                    {
                        float temp = distance_List[i];
                        distance_List[i] = distance_List[j];
                        distance_List[j] = temp;

                        Transform temp_MovePoint = movePoint_List[i];
                        movePoint_List[i] = movePoint_List[j];
                        movePoint_List[j] = temp_MovePoint;
                    }
                }
            }

            next_Move = false;
        }
        else
        {
            step_Point = lim_RobotEnemy_Spawn.round_Pos[round_Idx].step_Pos[step_Idx].GetChild(0);

            //step_Point = lim_FasterEnemy_Spawn.round_Pos[Lim_FasterGameManager.Instance.Round_Index].step_Pos[Lim_FasterGameManager.Instance.Step_Index].GetChild(2);

            for (int i = 0; i < step_Point.childCount; i++)
            {
                float distance = (transform.position - step_Point.transform.GetChild(i).transform.position).magnitude;

                if (distance <= 0.5f)  //자신의 거리에서 0.5보다 작아야 생성
                {
                    this.distance_List.Add(distance);
                    movePoint_List.Add(step_Point.transform.GetChild(i));
                    temp_Point.Add(movePoint_List[0].name);
                }
            }

            if (movePoint_List.Count != 0)
            {
                temp_possible_MoveArea = movePoint_List[0].GetComponent<Lim_FasterPossibleMotion>();
                temp_possible_MoveArea.possible_MoveArea = false;
            }
        }

    }
    void Next_RanomMotion()
    {
        if (Random_Index(0, 2) == 0 && can_Hide)
        {
            AnimOff();
        }
        if (can_Hide && movePoint_List.Count != 1)
        {
            TargetPoint_Move();
        }
        else if (!can_Hide && movePoint_List.Count == 1)
        {
            state = SoldierState.Aim;
            anim.SetTrigger("IsAim");
        }
    }
    void Stand_Hide()
    {
        state = SoldierState.Hide;
        can_Hide = true;


        if (spawn_Type == RobotSpawnType.UnFix)
        {
            if (lim_FasterPossibleMotion.left_Aim && lim_FasterPossibleMotion.right_Aim)
            {
                if (Random_Index(0, 2) == 0)
                {
                    anim.SetTrigger("Stand_Hide_L");
                    temp_Motion = "Stand_Hide_L";
                }
                else
                {
                    anim.SetTrigger("Stand_Hide_R");
                    temp_Motion = "Stand_Hide_R";
                }
            }
            else if (lim_FasterPossibleMotion.left_Aim)
            {
                anim.SetTrigger("Stand_Hide_L");
                temp_Motion = "Stand_Hide_L";

            }
            else
            {
                anim.SetTrigger("Stand_Hide_R");
                temp_Motion = "Stand_Hide_R";
            }
        }
        else
        {
            if (lim_FasterPossibleMotion.left_Aim && lim_FasterPossibleMotion.right_Aim)
            {
                if (Random_Index(0, 2) == 0)
                    temp_Motion = "Stand_Hide_L";
                else
                    temp_Motion = "Stand_Hide_R";
            }
            else if (lim_FasterPossibleMotion.left_Aim)
            {
                temp_Motion = "Stand_Hide_L";
            }
            else
            {
                temp_Motion = "Stand_Hide_R";
            }

        }

        look_Target = true;
        agent.enabled = false;
    }

    void Sit_Hide()
    {
        state = SoldierState.Hide;
        can_Hide = true;
        if (spawn_Type == RobotSpawnType.UnFix)
        {
            if (lim_FasterPossibleMotion.left_Aim && lim_FasterPossibleMotion.right_Aim)
            {
                if (Random_Index(0, 2) == 0)
                {
                    anim.SetTrigger("Sit_Hide_L");
                    temp_Motion = "Sit_Hide_L";
                }
                else
                {
                    anim.SetTrigger("Sit_Hide_R");
                    temp_Motion = "Sit_Hide_R";
                }
            }
            else if (lim_FasterPossibleMotion.left_Aim)
            {
                anim.SetTrigger("Sit_Hide_L");
                temp_Motion = "Sit_Hide_L";
            }
            else
            {
                anim.SetTrigger("Sit_Hide_R");
                temp_Motion = "Sit_Hide_R";
            }
        }
        else
        {
            if (lim_FasterPossibleMotion.left_Aim && lim_FasterPossibleMotion.right_Aim)
            {
                if (Random_Index(0, 2) == 0)
                {
                    temp_Motion = "Sit_Hide_L";
                }
                else
                {
                    temp_Motion = "Sit_Hide_R";
                }
            }
            else if (lim_FasterPossibleMotion.left_Aim)
            {
                temp_Motion = "Sit_Hide_L";
            }
            else
            {
                temp_Motion = "Sit_Hide_R";
            }
        }

        look_Target = true;
        agent.enabled = false;
    }

    void StandAim_Start()
    {
        if (spawn_Type == RobotSpawnType.UnFix)
        {
            StandAim();
        }
        else
        {
            rand = Random.Range(1, 6);

            if (Lim_RobotGameManager.Instance.round_Idx == 2 || Lim_RobotGameManager.Instance.round_Idx == 4)
                Invoke("Aim", 1);
            else
                Invoke("StandAim", rand);

        }
    }

    void StandAim()
    {
        temp_State = "Hide";
        state = SoldierState.Aim;

        can_Hide = true;
        anim.SetTrigger("IsAim");
        temp_Motion = "Stand_Aim";

        look_Target = true;
        agent.enabled = false;
    }

    void Aim()
    {
        temp_State = "Hide";
        state = SoldierState.Aim;
        can_Hide = true;
        temp_Motion = "Stand_Aim";

        look_Target = true;
        agent.enabled = false;
    }

    void HideAimCoroutine_Start()
    {
        if (spawn_Type == RobotSpawnType.UnFix)
            StartCoroutine(Hide_AimOn(Random_Index(0, 3)));
        else
            StartCoroutine(Hide_AimOn(Random_Index(1, 6)));
    }
    IEnumerator Hide_AimOn(int time)
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("IsAim");
        state = SoldierState.Aim;
        reset = false;
        AimMotion_Start();
    }

    void AnimOff()
    {
        if (temp_Motion == "Stand_Hide_L" || temp_Motion == "Stand_Hide_R" ||
            temp_Motion == "Sit_Hide_L" || temp_Motion == "Sit_Hide_R")
        {
            TargetPoint_Move();
        }
        else if (temp_Motion == "Stand_Aim")
        {
            anim.SetTrigger("Stand_Hide");

            int rand = Random.Range(1, 6);
            Invoke("StandAim", rand);

            state = SoldierState.Hide;
        }
    }

    private void AimMotion_Start()
    {
        if (lim_FasterPossibleMotion.is_Left)
        {
            soldier_PosZ = parent_Obj.transform.localPosition.z;
            StartCoroutine("AimMotion_L");
        }
        else if (lim_FasterPossibleMotion.is_Right)
        {
            soldier_PosZ = parent_Obj.transform.localPosition.z;
            StartCoroutine("AimMotion_R");
        }
        else
        {
            soldier_PosX = parent_Obj.transform.localPosition.x;
            StartCoroutine("AimMotion_C");
        }
    }

    IEnumerator AimMotion_C()
    {
        yield return new WaitForEndOfFrame();
        if (temp_Motion == "Stand_Hide_L" || temp_Motion == "Sit_Hide_L")
        {
            while (parent_Obj.transform.localPosition.x >= soldier_PosX - 0.7f)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x - Time.deltaTime,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z);
                yield return new WaitForEndOfFrame();
            }
        }
        else if (temp_Motion == "Stand_Hide_R" || temp_Motion == "Sit_Hide_R")
        {
            while (parent_Obj.transform.localPosition.x <= soldier_PosX + 0.5f)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x + Time.deltaTime,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z);
                yield return new WaitForEndOfFrame();
            }
        }
    }
    IEnumerator AimMotion_L()
    {
        yield return new WaitForEndOfFrame();
        if (temp_Motion == "Stand_Hide_L" || temp_Motion == "Sit_Hide_L")
        {
            while (parent_Obj.transform.localPosition.z >= soldier_PosZ - 0.7f)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z - Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else if (temp_Motion == "Stand_Hide_R" || temp_Motion == "Sit_Hide_R")
        {
            while (parent_Obj.transform.localPosition.z <= soldier_PosZ + 0.5f)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z + Time.deltaTime);
                yield return new WaitForEndOfFrame();


            }
        }
    }

    IEnumerator AimMotion_R()
    {
        yield return new WaitForEndOfFrame();
        if (temp_Motion == "Stand_Hide_L" || temp_Motion == "Sit_Hide_L")
        {
            while (parent_Obj.transform.localPosition.z <= soldier_PosZ + 0.7f)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z + Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else if (temp_Motion == "Stand_Hide_R" || temp_Motion == "Sit_Hide_R")
        {
            while (parent_Obj.transform.localPosition.z >= soldier_PosZ - 0.5f)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z - Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }
    void HideMotion_Start(bool isLeft)
    {
        StartCoroutine(HideMotion(isLeft));
    }
    IEnumerator HideMotion(bool isLeft)
    {
        yield return new WaitForEndOfFrame();
        if (isLeft)
        {
            while (parent_Obj.transform.localPosition.x <= soldier_PosX)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x + Time.deltaTime,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (parent_Obj.transform.localPosition.x >= soldier_PosX)
            {
                parent_Obj.transform.localPosition = new Vector3(
                            parent_Obj.transform.localPosition.x - Time.deltaTime,
                            parent_Obj.transform.localPosition.y,
                            parent_Obj.transform.localPosition.z);
                yield return new WaitForEndOfFrame();
            }
        }
    }
    void Jump_Over()
    {
        state = SoldierState.Jump;
        temp_State = "Jump";
        can_Hide = false;
        jump_Over = true;

        look_Target = true;

        anim.speed = 1.5f;
        anim.SetTrigger("Jump_Over");
    }

    public void Hit(RobotPart robotPart, float val, int player = 0)
    {
        if (spawn_Type != RobotSpawnType.Fix)
        {
            spawn_Type = RobotSpawnType.UnFix;
            anim.ResetTrigger("Run");

            agent.enabled = false;
        }

        aimingP1_Obj.SetActive(false);
        aimingP2_Obj.SetActive(false);

        Hp -= val;

        road = 0;

        UIManager.Instance.UI_BottomPlayer_Redraw(player);

        if (Hp <= 50 && !crash_Effect1.activeInHierarchy && !crash_Effect2.activeInHierarchy)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
                crash_Effect1.SetActive(true);
            else
                crash_Effect2.SetActive(true);
        }

        if (Hp <= 0 && val != 0)
        {
            isDie = true;

            Die_FindChild(transform);

            Lim_RobotGameManager.Instance.SetScore(player, robot_Type, robot_Soldier.killScore);

            StopAllCoroutines();

            switch (robotPart)
            {
                case RobotPart.HEAD:
                    StartCoroutine(HeadDie());
                    break;
                case RobotPart.BODY:
                    StartCoroutine(BodyDie());
                    break;
            }
            return;
        }

        string anim_Name = string.Format("Pistol_Hit_{0}", Random_Index(1, 4));
        anim.speed = 1;
        anim.Play(anim_Name);

        reset = false;
        isShotCoroutine = false;

        anim.ResetTrigger("IsAim");
        anim.ResetTrigger("Fire");

        temp_State = state.ToString();
        state = SoldierState.Hit;
    }
    void Fire()
    {
        isShotCoroutine = true;

        int player_FireIndex = Target_Player();

        if (player_FireIndex == 0)
            aimingP1_Obj.SetActive(true);
        else if (player_FireIndex == 1)
            aimingP2_Obj.SetActive(true);
       
        StartCoroutine(Fire_Coroutine(player_FireIndex));
    }

    IEnumerator Fire_Coroutine(int player_FireIndex, int time = 4)
    {
        WaitForSeconds seconds = new WaitForSeconds(1);

        while (time > 0)
        {
            time--;

            yield return seconds;
        }

        UIManager.Instance.AlertDamage(player_FireIndex, robot_Soldier.damage, false);

        reset = false;

        Instantiate(laser, fire_Pos.position, Quaternion.identity);


        if (player_FireIndex == 0)
            aimingP1_Obj.SetActive(false);
        else if (player_FireIndex == 1)
            aimingP2_Obj.SetActive(false);

        muzzle_Obj.Play();

        anim.SetTrigger("Fire");

        Lim_RobotGameManager.Instance.Shake_Camera();

        yield return new WaitForSeconds(1f);

        if (spawn_Type == RobotSpawnType.Fix)
            fixIs_ShotCoroutine = false;

        isShotCoroutine = false;

    }

    int Random_Index(int min, int max)
    {
        rand = Random.Range(min, max);
        return rand;
    }
    void Look_Player()
    {
        TurnToDestination(look);
    }
    void Die_FindChild(Transform target)
    {
        foreach (Transform child in target)
        {
            if (child.GetComponent<Collider>() != null)
            {
                child.gameObject.SetActive(false);
            }
            if (child.childCount != 0)
            {
                Die_FindChild(child);
            }
        }

        if (temp_possible_MoveArea != null) temp_possible_MoveArea.possible_MoveArea = true;
    }

    public void Back_Postion()
    {
        StopAllCoroutines();
        if (spawn_Type != RobotSpawnType.Fix)
        {
            state = SoldierState.Back;
            StartCoroutine(Destory_Obj());
        }
    }

    public IEnumerator Destory_Obj()
    {
        eff_NPC_dissapear.Play();
        yield return new WaitForSeconds(0.5f);
        real_Mesh.SetActive(false);
        gun.SetActive(false);
        yield return new WaitForSeconds(5f);
        Destroy(parent_Obj);
    }
    public IEnumerator HeadDie()
    {
        real_Mesh.SetActive(false);
        destroyObj.SetActive(true);
        rayfirebomb.Explode(0);

        anim.gameObject.SetActive(false);

        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    public IEnumerator BodyDie()
    {
        anim.Play("Rifle_Death_L");
        nomal_Death.SetActive(true);

        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        crash_Effect1.SetActive(false);
        crash_Effect2.SetActive(false);
    }

    public IEnumerator TypeFix_AimStart()//하이라키 생성 되어 있는 적
    {
        bool t = true;

        while (t)
        {
            yield return new WaitForSeconds(0.5f);

            float distance = (transform.position - look_Player.position).magnitude;

            if (distance <= 15)  //거리보다 작으면 조준 시작
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("CoverLo_AimR"))
                {
                    state = SoldierState.Aim;
                    anim.SetTrigger("IsAim");
                    t = false;
                    aimStart = null;
                }
            }
        }
    }

    public int Target_Player() //정면 카메라에서 보는 기준 중앙 : -1, 왼쪽 : 0, 오른쪽 : 1
    {
        int player_Idx = -1;

        for (int i = 0; i < side_Camera.Count; i++)
        {
            Vector3 direction_Camera = this.transform.position - side_Camera[i].position;

            float angle = Vector3.Angle(direction_Camera, side_Camera[i].forward);

            if (angle < 60)
            {
                player_Idx = i;
                break;
            }
        }

        if (GameManager.Instance.playerCnt == 2)
        {
            if (GameManager.Instance.Infos[0].Hp <= 0)
            {
                player_Idx = 1;
            }
            else if (GameManager.Instance.Infos[1].Hp <= 0)
            {
                player_Idx = 0;
            }
            else
            {
                if (player_Idx == -1)  //중앙이면 랜덤으로 누구를 쏠지 정함
                {
                    player_Idx = Random.Range(0, 2);
                }
            }
        }
        else
        {
            player_Idx = 0;
        }

        return player_Idx;
    }
   
    public void Start_Regdoll()
    {
        for (int i = 0; i < ani_Bodys.Length; i++)
        {
            ani_Bodys[i].SetActive(false);
        }

        ragdoll.SetActive(true);
        ragdoll_rigd.AddForce(ragdoll.transform.forward * -200, ForceMode.Impulse);
        ragdoll_rigd.useGravity = true;
    }
}
