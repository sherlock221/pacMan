using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDot : MonoBehaviour {


    public bool isSuperPacdot = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.gameObject.name == "Pacman"){

            if(isSuperPacdot){               
                GameManager.Instance.OnEatSupperPacDot();
            }           

            //移除pacdot
            GameManager.Instance.OnEatPacDot(gameObject);

            Destroy(gameObject);
          
        }
	}


}
