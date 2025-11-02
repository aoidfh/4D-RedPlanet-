using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boss_Part
{
    public float left_MashineGun;
    public float right_MashineGun;
    public float left_VulcanHp;
    public float right_VulcanHp;
    public float left_MissileHp;
    public float right_MissileHp;
    public float boss_bodyHp;
    public float shield_Hp;
    public float GunDmg;
    public int bodyScore;
    public int headScore;    
    public int killScore;    

    public Boss_Part(int index)
    {
        if(index == 1)
        {
            left_MashineGun = Lim_RobotDefine.left_MashineGun_1P;
            right_MashineGun = Lim_RobotDefine.right_MashineGun_1P;
            left_VulcanHp = Lim_RobotDefine.left_VulcanHp_1P;
            right_VulcanHp = Lim_RobotDefine.right_VulcanHp_1P;
            left_MissileHp = Lim_RobotDefine.left_MissileHp_1P;
            right_MissileHp = Lim_RobotDefine.right_MissileHp_1P;
            boss_bodyHp = Lim_RobotDefine.boss_bodyHp_1P;
            shield_Hp = Lim_RobotDefine.shield_Hp_1P;
            GunDmg = Lim_RobotDefine.boss_GunDmg_1P;
            bodyScore = Lim_RobotDefine.boss_body1;           
            killScore = Lim_RobotDefine.boss_Score1P;
        }
        else
        {
            left_MashineGun = Lim_RobotDefine.left_MashineGun_2P;
            right_MashineGun = Lim_RobotDefine.right_MashineGun_2P;
            left_VulcanHp = Lim_RobotDefine.left_VulcanHp_2P;
            right_VulcanHp = Lim_RobotDefine.right_VulcanHp_2P;
            left_MissileHp = Lim_RobotDefine.left_MissileHp_2P;
            right_MissileHp = Lim_RobotDefine.right_MissileHp_2P;
            boss_bodyHp = Lim_RobotDefine.boss_bodyHp_2P;
            shield_Hp = Lim_RobotDefine.shield_Hp_2P;
            GunDmg = Lim_RobotDefine.boss_GunDmg_2P;
            bodyScore = Lim_RobotDefine.boss_body2;          
            killScore = Lim_RobotDefine.boss_Score2P;
        }
    
    }
}

public enum Boss_State
{   
    WaitAttack,
    Attack,
    LastAttack,    
    Hit,
    Die
}

public enum Boss_MoveOrIdle
{
    Idle,
    Move
}

public class Lim_RobotBoss : MonoBehaviour
{
    public RobotType type;
    public Boss_State boss_State;
    public Boss_MoveOrIdle boss_MoveOrIdle;
    public Boss_Part boss_Part;
    //public BossTrailEvent bossTrailEvent;

    //¿ÞÂÊ ¹ßÄ­, ¿À¸¥ÂÊ ¹ßÄ­, ¿ÞÂÊ ¹Ì»çÀÏ, ¿À¸¥ÂÊ ¹Ì»çÀÏ
    public List<GameObject> bossPart_Objs;
    public List<GameObject> bossPart_AttackObjs;
    public List<GameObject> bossPart_HitObjs;
    public GameObject bossBody_HitObj;
    public List<GameObject> explosion_Obj;
    public List<GameObject> smoke_Obj;          

    public List<int> breakPart_Idx = new List<int>();

    private const int timeRange_Min = 1;
    private const int timeRange_Max = 3;
    private const int boss_WaitAttTime = 4;
  
    public float timeRandom;
    int random_Att; 

    Coroutine runningCoroutine = null;
    bool iscourutine = false;
 
    public Animator anim;

    public bool is_Attack;
    public bool init;

    private void Start()
    {
        Boss_Reset();
    }

    void Update()
    {
        if(!Lim_RobotGameManager.Instance.is_Pause)
        {            
            if(!is_Attack)
            {
                is_Attack = true;
                Reset_AttackTime();               
            }
        }
        else if(Lim_RobotGameManager.Instance.is_Pause)
        {
            if (runningCoroutine == null) return;
            StopCoroutine(runningCoroutine);
            runningCoroutine = null;

            Boss_EffectReset();
        }
        
        if (timeRandom > 0 && Time.timeScale != 0)
        {
            timeRandom -= Time.deltaTime;
            if (timeRandom <= 0)
            {
                timeRandom = 0;

                if(!bossBody_HitObj.activeInHierarchy)
                    runningCoroutine = StartCoroutine(BossPart_Attack());
            }
        }

        switch (boss_State)
        {                                       
            case Boss_State.Hit:
                if (runningCoroutine != null)                
                    StopCoroutine(runningCoroutine);                      
                break;
            case Boss_State.Die:             
                if (runningCoroutine != null)                
                    StopCoroutine(runningCoroutine);

                anim.SetTrigger("Die");
                bossBody_HitObj.SetActive(false);
                break;
        }       

        if (Input.GetKeyDown(KeyCode.I))
        {
            timeRandom = Random.Range(timeRange_Min, timeRange_Max);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < bossPart_Objs.Count; i++)
            {
                if (bossPart_Objs[i].activeInHierarchy)
                    BossPart_ShieldHit(i, 25);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (bossBody_HitObj.activeInHierarchy)
            {
                BossPart_Hit(6, 25);
            }
            else
            {
                for (int i = 0; i < bossPart_HitObjs.Count; i++)
                {
                    if (bossPart_HitObjs[i].activeInHierarchy)
                        BossPart_Hit(i, 25);
                }
            }
        }
    }
    public void Reset_AttackTime()
    {        
        timeRandom = Random.Range(timeRange_Min, timeRange_Max);
        iscourutine = false;        
    }
    
    IEnumerator BossPart_Attack() //º¸½º ±âº» °ø°Ý
    {
        bool check = false;
        if(GameManager.Instance.Infos.Count == 1)
        {
            boss_Part.shield_Hp = Lim_RobotDefine.shield_Hp_1P;

        }
        else
        {
            boss_Part.shield_Hp = Lim_RobotDefine.shield_Hp_2P;

        }

        if (boss_MoveOrIdle == Boss_MoveOrIdle.Idle)
        {
            int rand = Random.Range(1, 11);
            if (rand >= 9)
            {
                rand = Random.Range(0, 2);

                if (rand == 0)
                    anim.SetTrigger("Left");
                else if (rand == 1)
                    anim.SetTrigger("Right");

                StartCoroutine(Attack_False());
                yield break;
            }
        }
        
        boss_State = Boss_State.WaitAttack;       

        while (!check)
        {
            //anim.SetTrigger("Wait_Att");
            if (Lim_RobotGameManager.Instance.round_Idx+1 == 3)
            {
                if (boss_Part.left_MashineGun > 0 || boss_Part.right_MashineGun > 0)
                {
                    random_Att = Random.Range(0, 2);

                    while (!check)
                    {
                        if (breakPart_Idx.Contains(random_Att) && breakPart_Idx.Count < 2)
                            random_Att = Random.Range(0, 2);
                        else
                            check = true;
                    }
                }
                else
                {
                    is_Attack = false;
                    yield break;
                }
            }
            else if (Lim_RobotGameManager.Instance.round_Idx+1 == 5)
            {
                if(boss_Part.left_MashineGun > 0 || boss_Part.right_MashineGun > 0 ||
                   boss_Part.left_VulcanHp > 0 || boss_Part.right_VulcanHp > 0)
                {
                    random_Att = Random.Range(0, 4);
                    while (!check)
                    {
                        if (breakPart_Idx.Contains(random_Att) && breakPart_Idx.Count < 4)
                            random_Att = Random.Range(0, 4);
                        else
                            check = true;
                    }
                }
                else
                {
                    is_Attack = false;
                    yield break;
                }
            }
            else if (Lim_RobotGameManager.Instance.round_Idx+1 == 6)
            {
                if(boss_Part.left_MashineGun > 0 || boss_Part.right_MashineGun > 0 ||
                   boss_Part.left_VulcanHp > 0 || boss_Part.right_VulcanHp > 0 ||
                   boss_Part.left_MissileHp > 0 || boss_Part.right_MissileHp > 0)
                {
                    random_Att = Random.Range(0, 6);

                    while (!check)
                    {
                        if (breakPart_Idx.Contains(random_Att) && breakPart_Idx.Count < 6)
                            random_Att = Random.Range(0, 6);
                        else
                            check = true;
                    }
                }
                else
                {
                    is_Attack = false;
                    yield break;
                }
            }
            else
            {
                is_Attack = false;
                yield break;
            }
                       
            switch (random_Att)
            {
                case 0:
                    if (boss_Part.left_MashineGun > 0)
                    {
                        bossPart_Objs[random_Att].SetActive(true);                     
                    }                 
                    break;
                case 1:
                    if (boss_Part.right_MashineGun > 0)
                    {
                        bossPart_Objs[random_Att].SetActive(true);                      
                    }
                
                    break;
                case 2:
                    if (boss_Part.left_VulcanHp > 0)
                    {
                        bossPart_Objs[random_Att].SetActive(true);                       
                    }                   
                    break;
                case 3:
                    if (boss_Part.right_VulcanHp > 0)
                    {
                        bossPart_Objs[random_Att].SetActive(true);                       
                    }                   
                    break;
                case 4:
                    if (boss_Part.left_MissileHp > 0)
                    {
                        bossPart_Objs[random_Att].SetActive(true);                        
                    }                   
                    break;
                case 5:
                    if (boss_Part.right_MissileHp > 0)
                    {
                        bossPart_Objs[random_Att].SetActive(true);                       
                    }                   
                    break;
            }
        }
               
        yield return new WaitForSeconds(boss_WaitAttTime);

        if(random_Att != -1)
        {
            boss_State = Boss_State.Attack;

            if (bossPart_Objs[random_Att].activeInHierarchy)
            {
                Debug.Log(bossPart_Objs[random_Att].name + " ¹ß»ç");

                bossPart_AttackObjs[random_Att].SetActive(true);
                bossPart_Objs[random_Att].SetActive(false);
                Lim_RobotGameManager.Instance.Shake_Camera(true);

                for (int i = 0; i < GameManager.Instance.playerCnt; i++)
                {
                    UIManager.Instance.AlertDamage(i, boss_Part.GunDmg, true);
                }

                yield return new WaitForSeconds(1f);
              //  bossPart_AttackObjs[random_Att].SetActive(false);
                is_Attack = false;               
            }
        }
        else
        {
            is_Attack = false;
        }
    }     
    public void BossPart_ShieldHit(int bossPart_idx, int val) //½¯µå ÇÇ ±ïÀ½
    {
       if(boss_State == Boss_State.WaitAttack)
       {
            boss_Part.shield_Hp -= val;
            if (boss_Part.shield_Hp <= 0)
            {
                boss_State = Boss_State.Hit;

                bossPart_Objs[bossPart_idx].SetActive(false);

                bossPart_HitObjs[bossPart_idx].SetActive(true);

                if (!iscourutine)
                    StartCoroutine(HitObj_Reset());
            }
       }
    }  
    public void BossPart_Hit(int bossPart_idx, int val) //°¢ ÆÄÃ÷ ÇÇ ±ïÀ½
    {
        switch (bossPart_idx)
        {
            case 0:
                boss_Part.left_MashineGun -= val;
                if (boss_Part.left_MashineGun <= 0)
                {
                    bossPart_HitObjs[bossPart_idx].SetActive(false);                                                           
                    smoke_Obj[bossPart_idx].SetActive(true);
                    explosion_Obj[bossPart_idx].SetActive(true);

                    breakPart_Idx.Add(bossPart_idx);
                }
                break;
            case 1:
                boss_Part.right_MashineGun -= val;
                if (boss_Part.right_MashineGun <= 0)
                {
                    bossPart_HitObjs[bossPart_idx].SetActive(false);
                    smoke_Obj[bossPart_idx].SetActive(true);
                    explosion_Obj[bossPart_idx].SetActive(true);

                    breakPart_Idx.Add(bossPart_idx);
                }
                break;
            case 2:
                boss_Part.left_VulcanHp -= val;
                if (boss_Part.left_VulcanHp <= 0)
                {
                    bossPart_HitObjs[bossPart_idx].SetActive(false);
                    smoke_Obj[bossPart_idx].SetActive(true);
                    explosion_Obj[bossPart_idx].SetActive(true);

                    breakPart_Idx.Add(bossPart_idx);
                }
                break;
            case 3:
                boss_Part.right_VulcanHp -= val;
                if (boss_Part.right_VulcanHp <= 0)
                {
                    bossPart_HitObjs[bossPart_idx].SetActive(false);
                    smoke_Obj[bossPart_idx].SetActive(true);
                    explosion_Obj[bossPart_idx].SetActive(true);

                    breakPart_Idx.Add(bossPart_idx);
                }
                break;
            case 4:
                boss_Part.left_MissileHp -= val;
                if (boss_Part.left_MissileHp <= 0)
                {
                    bossPart_HitObjs[bossPart_idx].SetActive(false);
                    smoke_Obj[bossPart_idx].SetActive(true);
                    explosion_Obj[bossPart_idx].SetActive(true);
                    breakPart_Idx.Add(bossPart_idx);
                }
                break;
            case 5:
                boss_Part.right_MissileHp -= val;
                if (boss_Part.right_MissileHp <= 0)
                {
                    bossPart_HitObjs[bossPart_idx].SetActive(false);
                    smoke_Obj[bossPart_idx].SetActive(true);
                    explosion_Obj[bossPart_idx].SetActive(true);

                    breakPart_Idx.Add(bossPart_idx);
                }
                break;
            case 6:      
                if(bossBody_HitObj.activeInHierarchy)
                    boss_Part.boss_bodyHp -= val;
                if (boss_Part.boss_bodyHp <= 0)
                {                   
                    smoke_Obj[bossPart_idx].SetActive(true);
                    explosion_Obj[bossPart_idx].SetActive(true);                  
                    Lim_RobotGameManager.Instance.is_BossDie = true;                    
                }
                break;
        }

        for (int i = 0; i < bossPart_Objs.Count; i++)
        {
            bossPart_Objs[i].SetActive(false);
        }

        BossPartHp_Check();
    }  
    public void BossPartHp_Check() //ºÎ¼­Áø ÆÄÃ÷ Ã¼Å©
    {                       
        if (boss_Part.boss_bodyHp <= 0)
        {
            boss_State = Boss_State.Die;

            for (int i = 0; i < GameManager.Instance.playerCnt; i++)
            {
                Lim_RobotGameManager.Instance.SetScore(i, type, boss_Part.killScore);
            }            
        }
        else if (breakPart_Idx.Count == 6)
        {
            bossBody_HitObj.SetActive(true);            
        }
    }        
    public void Boss_Reset(int round_Idx =0)
    {      
        boss_Part = new Boss_Part(GameManager.Instance.Infos.Count);
        
        breakPart_Idx.Clear();

        for (int i = 0; i < bossPart_Objs.Count; i++)
        {
            bossPart_Objs[i].SetActive(false);
            bossPart_HitObjs[i].SetActive(false);
            explosion_Obj[i].SetActive(false);
            smoke_Obj[i].SetActive(false);
        }

        bossBody_HitObj.SetActive(false);

        is_Attack = false;
        
        if(round_Idx+1 != 6 && round_Idx != 0)
            Boss_StateMove();

        if(!init)
            init = true;
    }
    public void Boss_EffectReset()
    {       
        for (int i = 0; i < bossPart_Objs.Count; i++)
        {
            bossPart_Objs[i].SetActive(false);
            bossPart_HitObjs[i].SetActive(false);
            bossPart_AttackObjs[i].SetActive(false);
        }
        
        is_Attack = false;
    }
    public IEnumerator HitObj_Reset()
    {
        iscourutine = true;
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < bossPart_HitObjs.Count; i++)
        {
            bossPart_HitObjs[i].SetActive(false);
        }       

        is_Attack = false;
    }  

    public void Boss_Hit(int bossPart_idx, int val)
    {
        if (bossPart_idx == 6)
        {
            BossPart_Hit(bossPart_idx, val);
        }
        else if (bossPart_Objs[bossPart_idx].activeInHierarchy)
        {
            BossPart_ShieldHit(bossPart_idx, val);         
        }
        else if (bossPart_HitObjs[bossPart_idx].activeInHierarchy)
        {
            BossPart_Hit(bossPart_idx, val);                   
        }        
    }     

    public void Boss_StateMove()
    {
        if (boss_MoveOrIdle == Boss_MoveOrIdle.Move) return;

        boss_MoveOrIdle = Boss_MoveOrIdle.Move;
        anim.SetTrigger("Move");       
    }

    public void Boss_StateIdle()
    {
        if (boss_MoveOrIdle == Boss_MoveOrIdle.Idle) return;

        boss_MoveOrIdle = Boss_MoveOrIdle.Idle;
        anim.SetTrigger("Idle");
    }   

    public IEnumerator Attack_False()
    {
        yield return new WaitForSeconds(4);
        is_Attack = false;        
    }

    public void Set_BossSound(bool is_Pause = false)
    {
        if (is_Pause)
        {
            for (int i = 0; i < bossPart_AttackObjs.Count; i++)
            {
                if (bossPart_AttackObjs[i].activeInHierarchy)
                    bossPart_AttackObjs[i].GetComponent<AudioSource>().Pause();
            }
        }
        else
        {
            for (int i = 0; i < bossPart_AttackObjs.Count; i++)
            {
                if (bossPart_AttackObjs[i].activeInHierarchy)
                    bossPart_AttackObjs[i].GetComponent<AudioSource>().Play();
            }
        }

       
    }

}
