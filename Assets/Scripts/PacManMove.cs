using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMove : MonoBehaviour {

    //移动速度
    public float speed;
    //目标位置
    private Vector2 dest = Vector2.zero;

        	
	void Start () {
        //默认当前位置
        dest = transform.position;       
	}
	

	private void FixedUpdate()
	{


        if((Vector2)transform.position ==  dest){

            Debug.Log(dest);

            //判断移动位置
            if (Input.GetKey(KeyCode.UpArrow) && IsValid(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
            }

            if (Input.GetKey(KeyCode.DownArrow) && IsValid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }

            if (Input.GetKey(KeyCode.LeftArrow) && IsValid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }

            if (Input.GetKey(KeyCode.RightArrow) && IsValid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }


            Vector2 dir = dest - (Vector2)transform.position;

            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);




        }



        Vector2 temp =   Vector2.MoveTowards((Vector2)transform.position, dest, speed);
        transform.GetComponent<Rigidbody2D>().MovePosition(temp);

      
	}


    private bool  IsValid(Vector2 dir){
        Vector2 pos = transform.position;
        RaycastHit2D raycast = Physics2D.Linecast(pos + dir, pos);
        return (raycast.collider == GetComponent<Collider2D>());
    }
}
