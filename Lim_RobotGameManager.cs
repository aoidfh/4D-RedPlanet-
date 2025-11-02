using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.UI;
public class Lim_RobotGameManager : MonoBehaviour
{
    public static Lim_RobotGameManager Instance;

    //public bool isTest;       
    public Lim_RobotEnemy_Spawn lim_RobotEnemy_Spawn;
    public Lim_RobotAnimCtrl lim_RobotAnimCtrl;
    public Lim_RobotBoss lim_RobotBoss;   
    public Lim_RobotSoundManager lim_RobotSoundManager;    
    public List<Lim_RobotPlayerUI> lim_RobotPlayerUI;    

    public GameObject result_Obj;
    
    public float total_Time;
    public float gamePlay_Time;
    public float animMove_Time;

    public int round_Count;
    public int round_Idx;
    public int step_Idx;
    int anim_Index = 1;

    public Animator player_Move; public Animator shake_Camera;
    public List<Animator> fadeAniList = new List<Animator>();
    
    public float play_Time;

    public bool isArrival;
   
    public int die_Cnt;

    public Transform player;
    public GameObject continue_Public; public Text continue_PubTxt;
    public GameObject player1_UI; public GameObject blind_P1;
    public GameObject player2_UI; public GameObject blind_P2;
    public UI_gameover_ player1_GameOver; public UI_gameover_ player2_GameOver;


    int time;
    int coin; public Text coinCount;
    
    IEnumerator Temp_TimeContinue;

    public GameObject missile;
    public GameObject dron;

    public GameObject end_Soldier1P; public GameObject end_Soldier2P;


    public bool is_Event; public bool eventClick_Player1; public bool eventClick_Player2;
    
    public Transform hit_Effect_Pos;  public GameObject hit_Effect;
      
    public bool is_BossDie;

    public bool is_Pause;

    public GameObject title_Obj; public GameObject success_Obj; public GameObject fail_Obj;
    public GameObject holdOn_Obj;
    public int skip_Idx; public GameObject skip_Obj;

    public bool is_GameOver;

    public GameObject rayFireObjstage1;
    public GameObject rayFireObjstage5;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        player_Move.enabled = false;
    }
    private void Start()
    {
#if UNITY_EDITOR
        //isTest = true;
#endif                
        gamePlay_Time = (total_Time - animMove_Time) / round_Count;  //round_Count 총6 라운드에서 3,5라운드 뺀 나머지

        play_Time = gamePlay_Time;
                     
        CoinDetect();

        end_Soldier1P.SetActive(false);
        end_Soldier2P.SetActive(false);

        title_Obj.SetActive(true);
    }

    void GunBoardOnOff(bool active)
    {
        if(GameManager.Instance.Infos.Count == 1)
        {
            if (GameManager.Instance.Infos[0].Hp > 0)
                MouseAttackRobot.Instance.BS_Board(active, 0);
        }
        else if (GameManager.Instance.Infos.Count == 2)
        {
            if (GameManager.Instance.Infos[0].Hp > 0)
                MouseAttackRobot.Instance.BS_Board(active, 0);
            if (GameManager.Instance.Infos[1].Hp > 0)
                MouseAttackRobot.Instance.BS_Board(active, 1);
        }

    }
    void Update()
    {

        if (!is_Pause)
        {
            if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 1 M"))
            {
                lim_RobotSoundManager.IntroRound1_Start(); is_Pause = true; title_Obj.SetActive(false); skip_Idx = 0; /*holdOn_Obj.SetActive(true)*/;
                GunBoardOnOff(false);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 2 M"))
            {
                lim_RobotSoundManager.IntroRound2_Start(); is_Pause = true; title_Obj.SetActive(false); skip_Idx = 1; /*holdOn_Obj.SetActive(true)*/;
                GunBoardOnOff(false);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 3 M"))
            {
                lim_RobotSoundManager.IntroRound3_Start(); is_Pause = true; title_Obj.SetActive(false); skip_Idx = 2; holdOn_Obj.SetActive(true);
                GunBoardOnOff(false);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 4 M"))
            {
                lim_RobotSoundManager.IntroRound4_Start(); is_Pause = true; title_Obj.SetActive(false); skip_Idx = 3; holdOn_Obj.SetActive(true);
                GunBoardOnOff(false);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 5 M"))
            {
                lim_RobotSoundManager.IntroRound5_Start(); is_Pause = true; title_Obj.SetActive(false); skip_Idx = 4; holdOn_Obj.SetActive(true);
                GunBoardOnOff(false);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 6 M"))
            {
                lim_RobotSoundManager.IntroRound6_Start(); is_Pause = true; title_Obj.SetActive(false); skip_Idx = 5; holdOn_Obj.SetActive(true);
                GunBoardOnOff(false);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("GameOver Clear"))
            {
                is_Pause = true; ;
                GunBoardOnOff(false);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("GameOver Fail"))
            {
                is_Pause = true; ;
                GunBoardOnOff(false);
                //rayFireObjstage5.SetActive(true);
            }

        }
        else
        {
            if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 1 Play")){ 
                lim_RobotSoundManager.BattleRound1_Start(); is_Pause = false; skip_Obj.SetActive(false); holdOn_Obj.SetActive(false);
                GunBoardOnOff(true);
                //Destroy(rayFireObjstage1);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 2 Play")) { 
                lim_RobotSoundManager.BattleRound2_Start(); is_Pause = false; skip_Obj.SetActive(false); holdOn_Obj.SetActive(false);
                GunBoardOnOff(true);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 3 Play")) { 
                lim_RobotSoundManager.BattleRound3_Start(); is_Pause = false; skip_Obj.SetActive(false); holdOn_Obj.SetActive(false);
                GunBoardOnOff(true);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 4 Play")) {
                lim_RobotSoundManager.BattleRound4_Start(); is_Pause = false; skip_Obj.SetActive(false); holdOn_Obj.SetActive(false);
                GunBoardOnOff(true);
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 5 Play")) {
                lim_RobotSoundManager.BattleRound5_Start(); is_Pause = false; skip_Obj.SetActive(false); holdOn_Obj.SetActive(false);
                GunBoardOnOff(true);
                
            }
            else if (player_Move.GetCurrentAnimatorStateInfo(0).IsName("Round 6 Play")) {
                lim_RobotSoundManager.BattleRound6_Start(); is_Pause = false; skip_Obj.SetActive(false); holdOn_Obj.SetActive(false);
                GunBoardOnOff(true);
            }         
        }

      
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Skip_Intro(0, true);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Skip_Intro(1, true);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Skip_Intro(2, true);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Skip_Intro(3, true);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Skip_Intro(4, true);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Skip_Intro(5, true);
        }

        if(Input.GetKeyDown(KeyCode.Space) && skip_Obj.activeInHierarchy)
        {
            Skip_Intro(skip_Idx);                    
        }
         
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                for (int i = 0; i < lim_RobotEnemy_Spawn.soldier_List.Count; i++)
                {
                    Lim_RobotDefine.Set_TestDmgDown();
                    lim_RobotEnemy_Spawn.soldier_List[i].robot_Soldier.damage = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                for (int i = 0; i < lim_RobotEnemy_Spawn.soldier_List.Count; i++)
                {
                    lim_RobotEnemy_Spawn.soldier_List[i].robot_Soldier.damage = 5;
                    Lim_RobotDefine.Set_TestDmgUp();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Fail_End();
            }
        }
      
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            string date = string.Empty;
            // 설치하고 첫 개시일자
            if (PlayerPrefs.GetString("StartMoney") == "")
            {
                date = System.DateTime.Now.ToString("yyyy-MM-dd");
                PlayerPrefs.SetString("StartMoney", date);
            }
            // 현재 coins
            int coins = PlayerPrefs.GetInt("coins");
            ++coins;
            PlayerPrefs.SetInt("coins", coins);
            // 1일 매출에 등록
            date = System.DateTime.Now.ToString("yyyy-MM-dd");
            int totalCoins = PlayerPrefs.GetInt(date);
            totalCoins += 1;
            PlayerPrefs.SetInt(date, totalCoins);

            //CoinCheckResurrection();
        }
       
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Round_Clear();
        }

        if (isArrival && !is_Pause)  //도착시 시간 흐름
            play_Time -= Time.deltaTime;

        if (play_Time <= 0)  //초기화
        {
            Round_Clear();
        }
    }    
    public void CoinCheckResurrection()
    {
        CoinDetect();
        if (GameManager.Instance.Infos.Count == 1)
        {
            if (!GameManager.Instance.Infos[0].isBoardOn)
            {
                //MouseAttackRobot.Instance.BS_Board(true, 0);               
            }
        }
        else if (GameManager.Instance.Infos.Count == 2)
        {
            if (!GameManager.Instance.Infos[0].isBoardOn)
            {
                //MouseAttackRobot.Instance.BS_Board(true, 0);               
            }
            if (!GameManager.Instance.Infos[1].isBoardOn)
            {
                //MouseAttackRobot.Instance.BS_Board(true, 1);             
            }
        }
    }
    public void PlayerDeathCoinCheckResurrection(int pidx)
    {
        CoinDetect();
        if(coin > 0)
        {
            if (!GameManager.Instance.Infos[pidx].isBoardOn)
            {
                //MouseAttackRobot.Instance.BS_Board(true, pidx);
            }
        }
    }
    public void Round_Clear()
    {                                     
        play_Time = 0;

        Instance.round_Idx++;

        step_Idx = 0;
        
        isArrival = false;

        lim_RobotEnemy_Spawn.Spawn_StopCourutine();
        lim_RobotEnemy_Spawn.ClearSoldierList_CourutineStart();

        play_Time = gamePlay_Time;

        if (round_Idx == 6 && is_BossDie)
        {
            player_Move.Play("GameOver Clear");          

            if (GameManager.Instance.playerCnt == 1)
            {
                end_Soldier1P.SetActive(true);
            }
            else
            {
                end_Soldier1P.SetActive(true);
                end_Soldier2P.SetActive(true);
            }

            GameEnd_Start();
            lim_RobotSoundManager.EndSuccess_Start();            
        }
        else if (round_Idx == 6 && !is_BossDie)
        {
            player_Move.Play("GameOver Fail");           
            GameEnd_Start();
            lim_RobotSoundManager.EndFail_Start();            
        }
        else player_Move.Play(string.Format("Round {0} M", (round_Idx + 1)));
    }

    public void SetContinue_Obj(int player_Index)
    {
        lim_RobotPlayerUI[player_Index].life[GameManager.Instance.Infos[player_Index].Life].gameObject.SetActive(false);
        GameManager.Instance.Infos[player_Index].Life--;

        if (GameManager.Instance.Infos[player_Index].Life >= 0)
        {
            lim_RobotPlayerUI[player_Index].RecoveryOn(true, player_Index, 5f);

        }
       
        for (int i = 0; i < GameManager.Instance.Infos.Count; i++)
        {
            if (GameManager.Instance.Infos[i].Life >= 0)
                return;           
        }
        result_Obj.SetActive(true);
        FadeInOut("FadeOut");
        GunBoardOnOff(false);
        is_Pause = true;
        lim_RobotEnemy_Spawn.spawn_Start = false;
        lim_RobotEnemy_Spawn.spawn_Time = 999;
        Fail_End();
    }

    //부활
    //public void Resurrection(int player_Index)
    //{
    //    if (die_Cnt > 0)
    //        die_Cnt--;

    //    if (!is_GameOver)
    //    {
    //        // 현재 coins
    //        int coins = PlayerPrefs.GetInt("coins");

    //        if (coins > 0)
    //        {
    //            --coins;
    //            PlayerPrefs.SetInt("coins", coins);

    //            if (player_Index == 0)
    //            {               
    //                player1_GameOver.UI_gameover_onoff();                   
    //                player1_UI.SetActive(true);

    //                player1_GameOver.is_Die = false;
    //            }
    //            else if (player_Index == 1)
    //            {                 
    //                player2_GameOver.UI_gameover_onoff();
    //                player2_UI.SetActive(true);

    //                player2_GameOver.is_Die = false;
    //            }

    //            lim_RobotPlayerUI[player_Index].RecoveryOn(true, player_Index, 1f);             

    //            Set_TimeScale(1);

    //            GameManager.Instance.Infos[player_Index].BulletCnt = 30;
    //            GameManager.Instance.Infos[player_Index].MissileCnt = 3;
    //            UIManager.Instance.UI_BottomPlayer_Redraw(player_Index);
    //        }
    //        CoinDetect();

    //    }

    //}


    //public IEnumerator ContinuePublic_Time()  //모두 죽었을때 시간
    //{
    //      is_Pause = true;
    //    time = 9;
    //    while (time >= 0)
    //    {
    //        continue_PubTxt.text = time.ToString();

    //        if (time >= 0)
    //            time--;           
    //        yield return new WaitForSecondsRealtime(1);
    //    }

    //    if (time < 0)
    //    {
    //        Set_TimeScale(1);          
    //        continue_Public.SetActive(false);
    //        blind_P1.SetActive(false);
    //        blind_P2.SetActive(false);

    //        player_Move.Play("GameOver Fail");
    //        fail_Obj.SetActive(true);
    //        lim_RobotSoundManager.EndFail_Start();
    //        lim_RobotEnemy_Spawn.ClearSoldierList_CourutineStart();
    //    }
        
    //        //GameEnd_Start();
    //}

    //public IEnumerator ContinuePubliPlayer_Time(Text PIdx_Txt,GameObject continue_Obj, GameObject blind_Obj)  //한 명 죽었을때 시간
    //{
    //    time = 9;

    //    blind_Obj.SetActive(true);

    //    while (time >= 0)
    //    {
    //        PIdx_Txt.text = time.ToString();

    //        if (time >= 0)
    //            time--;
    //        yield return new WaitForSecondsRealtime(1);
    //    }

    //    if (time < 0)
    //    {
    //        continue_Obj.SetActive(false);
    //        blind_Obj.SetActive(false);
    //    }
    //}

    public void Stop_TempTimeContinue()
    {
        if (Temp_TimeContinue == null) return;
        StopCoroutine(Temp_TimeContinue);
    }
    public void GameEnd_Start()
    {
        result_Obj.SetActive(true);
        holdOn_Obj.SetActive(false);        
        Set_TimeScale(1);
        lim_RobotSoundManager.Result_Start();
    }  

    void CoinDetect()
    {
        if(coinCount)
        {
            coin = PlayerPrefs.GetInt("coins");

            if(coin > 0)
            {
                coinCount.text = "Coin " + coin * 1000;
            }
            else
            {
                coinCount.text = "Insert Coin";
            }
        }
    }    

    public void PlayerHit_Effect()
    {
        Instantiate(hit_Effect, hit_Effect_Pos.position, Quaternion.identity);
    }

    
    public IEnumerator Delay_HotKey(string playAnimName)
    {
        bool istrue = false;

        while (!istrue)
        {
            if (lim_RobotEnemy_Spawn.soldier_List.Count == 0 && lim_RobotEnemy_Spawn.drone_List.Count == 0)
            {
                istrue = true;
                player_Move.Play(playAnimName);
            }
               yield return new WaitForSeconds(1);
        }
    }

    public void SetScore(int index, RobotType type, int score)
    {
        if (GameManager.Instance == null) { return; }
        if (index < 0 || 3 < index) { return; }

        // if(hpui == null){return;}    // hpui 없어도 damage
        // if(pIdx != -1)   // 캐릭터 주변 범위내에 다가올때 죽는 방식

        if (index != -1)
        {
            if (type == RobotType.Boss)
            {
                GameManager.Instance.Infos[index].Bossscore += score;
            }
            else
            {
                GameManager.Instance.Infos[index].Bossscore += score;
            }         
        }

        if (index != -1) UIManager.Instance.UI_BottomPlayer_Redraw(index);
    }

    public void Shake_Camera(bool is_Big = false)
    {
        if(is_Big)
            shake_Camera.Play("Shake Big");
        else
            shake_Camera.Play("Shake Small");
    }

    public void Set_TimeScale(int value)
    {
        if (round_Idx > 5)
            round_Idx = 5;

        if (value == 0)
        {            
            lim_RobotBoss.Set_BossSound(true);
            
            lim_RobotSoundManager.battleRound_Sound[round_Idx].battle1.Pause();
            lim_RobotSoundManager.battleRound_Sound[round_Idx].battle2.Pause();
        }
        else
        {
            lim_RobotBoss.Set_BossSound();
            lim_RobotSoundManager.battleRound_Sound[round_Idx].battle1.Play();
            lim_RobotSoundManager.battleRound_Sound[round_Idx].battle2.Play();
        }

        Time.timeScale = value;
    }


    public void Skip_Intro(int round_Idx, bool is_Hotkey = false)
    {
        this.round_Idx = round_Idx; step_Idx = 0; lim_RobotEnemy_Spawn.spawn_Index = round_Idx;
        skip_Idx = round_Idx;
        isArrival = false; play_Time = gamePlay_Time;

        lim_RobotEnemy_Spawn.Spawn_StopCourutine();
        lim_RobotEnemy_Spawn.ClearSoldierList_CourutineStart();

        anim_Index = round_Idx + 1;
              
        if (is_Hotkey)
        {
            lim_RobotBoss.Boss_Reset(round_Idx);
            StartCoroutine(Delay_HotKey("Round " + anim_Index + " M"));
        }            
        else
        {
            switch (round_Idx)
            {
                case 0:
                    UIManager.Instance.timer = 279;
                    break;
                case 1:
                    UIManager.Instance.timer = 257;
                    break;
                case 2:
                    UIManager.Instance.timer = 180;
                    break;

                case 3:
                    UIManager.Instance.timer = 130;
                    break;
                case 4:
                    UIManager.Instance.timer = 79;
                    break;
                case 5:
                    UIManager.Instance.timer = 41;
                    lim_RobotEnemy_Spawn.Skip_Round6Intro();
                    break;
            }

            StartCoroutine(Delay_HotKey("Round " + anim_Index + " Play"));
            skip_Idx++;
            
            if(skip_Obj.activeInHierarchy) skip_Obj.SetActive(false);
            if (title_Obj.activeInHierarchy) title_Obj.SetActive(false);
        }           
    }

    public void Fail_End()
    {
        GameEnd_Start();
        player_Move.speed = 0;
        lim_RobotSoundManager.EndFail_Start();
        lim_RobotEnemy_Spawn.ClearSoldierList_CourutineStart();
        FadeInOut("FadeOut");
    }
    private void FadeInOut(string aniname)
    {
        for (int i = 0; i < fadeAniList.Count; i++)
        {
            fadeAniList[i].gameObject.SetActive(true);
            fadeAniList[i].Play(aniname);
        }
    }
}
