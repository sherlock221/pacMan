using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager> {

    public GameObject PacMan;

    public GameObject Blinky;
    public GameObject Clyde;
    public GameObject Inky;
    public GameObject Pinky;


    public GameObject StartPanel;
    public GameObject GamePanel;
    public GameObject StartCountDownPrefab;
    public GameObject GameOverPrefab;
    public GameObject WinPrefab;
    public AudioClip StartClip;




    //豆子总数
    public int dotCount;
    public int nowEat;
    public int score;


    public Text remainText;
    public Text nowText;
    public Text scoreText;

    public bool isSuperPacMan = false;


    public List<int> pathIndexList = new List<int>();
    public List<int> sourceIndexList = new List<int>(){0,1,2,3};

    private List<GameObject> pacDots = new List<GameObject>();






	protected override void Awake()
	{
        base.Awake();

        int count = sourceIndexList.Count;
        for (int i = 0; i < count; i++)
        {
            int tempIndex = Random.Range(0, sourceIndexList.Count);
            pathIndexList.Add(sourceIndexList[tempIndex]);
            sourceIndexList.RemoveAt(tempIndex);
        }



        //获得地图豆子
        foreach (Transform   item in GameObject.Find("Maze").transform)
        {
            pacDots.Add(item.gameObject);
        }


        dotCount = GameObject.Find("Maze").transform.childCount;

    }

	private void Start()
	{

        SetGameState(false);
	}


    public void OnStartButton(){

        StartCoroutine(PlayStartCountDown());
        StartPanel.SetActive(false);    
        AudioSource.PlayClipAtPoint(StartClip,new Vector3(0,0,-5));

    }

    public void OnExitButton(){
        Application.Quit();
    }


    IEnumerator  PlayStartCountDown(){
        GameObject go = Instantiate(StartCountDownPrefab);
        yield return new WaitForSeconds(4f);
        Destroy(go);
        SetGameState(true);
        //10秒 超级豆子
        Invoke("CreateSuperDot", 10f);

        GamePanel.SetActive(true);

        //播放背景音乐
        GetComponent<AudioSource>().Play();
    }
   

    //吃掉超级豆子
    public void OnEatSupperPacDot(){
        Invoke("CreateSuperDot", 10f);
        isSuperPacMan = true;
        FreezeEnemy();

        score += 200;

        //开启协程序
        StartCoroutine(RecoverEnemy());


    }

	private void Update()
	{		
        //积分面板显示
        if(GamePanel.activeInHierarchy){
            remainText.text = "Remain:\n\n" + (dotCount - nowEat);
            nowText.text = "Eaten:\n\n" + nowEat;
            scoreText.text = "Remain:\n\n" + score;
        }

        //豆子被吃完胜利
        if(nowEat == dotCount && PacMan.GetComponent<PacManMove>().enabled != false){
            GamePanel.SetActive(false);           
            StopAllCoroutines();
            SetGameState(false);
            Instantiate(WinPrefab);
        }


        if(nowEat == dotCount){
            if(Input.anyKeyDown){
                SceneManager.LoadScene(0);
            }
        }
	}



	//吃掉豆子
	public void OnEatPacDot(GameObject go){
        pacDots.Remove(go);
        nowEat++;
        score += 100;
    }

	private void CreateSuperDot(){

        //如果小于5个 不在生成超级豆子
        if(pacDots.Count < 5){
            return;
        }

        int tempIndex = Random.Range(0, pacDots.Count);
        //放大3倍
        pacDots[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacDots[tempIndex].GetComponent<PacDot>().isSuperPacdot = true;
    }




    IEnumerator RecoverEnemy(){
        yield return new WaitForSeconds(3);
        UnFreezeEnemy();
        isSuperPacMan = false;
    }

    //冻结敌人
    private void FreezeEnemy(){
        
        //update方法失效
        Blinky.GetComponent<GhostMove>().enabled = false;
        Pinky.GetComponent<GhostMove>().enabled = false;
        Inky.GetComponent<GhostMove>().enabled = false;
        Clyde.GetComponent<GhostMove>().enabled = false;

        //透明度降低
        Blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);
        Pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);


    }


    //解冻
    private void UnFreezeEnemy(){

        Blinky.GetComponent<GhostMove>().enabled = true;
        Pinky.GetComponent<GhostMove>().enabled = true;
        Inky.GetComponent<GhostMove>().enabled = true;
        Clyde.GetComponent<GhostMove>().enabled = true;
               
        Blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

    }

    private void SetGameState(bool state)
    {
           

        Blinky.GetComponent<GhostMove>().enabled = state;
        PacMan.GetComponent<PacManMove>().enabled = state;
        Pinky.GetComponent<GhostMove>().enabled = state;
        Inky.GetComponent<GhostMove>().enabled = state;
        Clyde.GetComponent<GhostMove>().enabled = state;
    }



}
