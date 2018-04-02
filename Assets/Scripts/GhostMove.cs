using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostMove : MonoBehaviour {


    public GameObject[] pathArray;
    private int index;
    private int indexPath;
    private List<Vector3> pointList = new List<Vector3>();
    private float speed = 0.08f;

    private Vector3 startPos;


    private void GetPath()
    {

        pointList.Clear();

        foreach (Transform tran in pathArray[indexPath].transform)
        {
            pointList.Add(tran.position);
        }

        pointList.Insert(0,startPos);
        pointList.Add(startPos);

        Debug.Log("pointList COUNT -->" + pointList.Count);
    }


	private void Start()
	{
        startPos = transform.position + new Vector3(0, 3, 0);
        //获得随机的路径 对应的索引

        indexPath = GameManager.Instance.pathIndexList[GetComponent<SpriteRenderer>().sortingOrder - 2];

        Debug.Log(gameObject.name+" 选择的随机路径-->" + pathArray[indexPath].name);
                
        GetPath();
	}

	private void FixedUpdate()
	{   
        if(pointList[index] != transform.position){
            Vector2 temp = Vector2.MoveTowards(transform.position, pointList[index], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else{
            
            index++;

            if(index >= pointList.Count){
                index = 0;
                GetPath();
            }


        }


        Vector2 dir = pointList[index] - transform.position;

        //动画状态机
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.gameObject.name == "Pacman"){


            if(GameManager.Instance.isSuperPacMan){
                //碰到超级吃豆人 送回老家
                gameObject.transform.position = startPos - new Vector3(0, 3, 0);
                index = 0;
                //鬼+100分
                GameManager.Instance.score += 500;

            }else{

                //死亡逻辑
                collision.gameObject.SetActive(false);
                GameManager.Instance.GamePanel.SetActive(false);
                Instantiate(GameManager.Instance.GameOverPrefab);
                Invoke("ReStart", 3f);
            }


        }
	}


    private void ReStart(){

        SceneManager.LoadScene(0);
    }






}
