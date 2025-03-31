using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    // --------- 싱글톤 
    public static GamePlayManager Instance;
    public static bool initialized {get;private set;}
    public static bool isPlaying { get;private set; }

    // -----------------

    public GamePlaySection playingSection;


    //

    [SerializeField] DamageText prefab_damageText;

    //=============================================================
    // 싱글톤 세팅
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    // 게임 시작
    public void Start()
    {
        Init();
        StartGame();
    }


    //==========================================

    void Init()
    {
        Debug.Log("게임 초기화");
        initialized = true;
    }

    void StartGame()
    {
        Debug.Log("게임 시작");
        isPlaying = true;

        // 현재 단계 구역 생성. - 지금은 강제로 주입
        GamePlaySection section = FindObjectOfType<GamePlaySection>();
        section.Init();
        playingSection = section;


        List<BackgroundScroller> backgroundScrollers = FindObjectsOfType<BackgroundScroller>().ToList();
        foreach( BackgroundScroller bg in backgroundScrollers)  
        {
            bg.Init();
        }

        //  트럭 초기화 - 하위 박스랑 히어로도 초기화됨. 
        FindObjectOfType<Truck>().Init(playingSection,backgroundScrollers);
        

    }


    public void ClearCurrSection()
    {
        playingSection.SetCleared();
    }

    public void OnEnemyDamaged(Vector2 hitPoint, float damage)
    {
        DamageText damageText = Instantiate(prefab_damageText, hitPoint, Quaternion.identity);
        damageText.Init(damage);
    }

}
