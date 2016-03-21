using UnityEngine;
using System.Collections;

public class GamingOptBtn : MonoBehaviour {
	string myName;
	GameObject cam;
	void Start ()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
	}
	public string MyName
	{
		set{myName = value;}
	}
	
	IEnumerator OnMouseUp()
	{
		//audio.PlayOneShot ();
		yield return new WaitForSeconds(0.35f);


			if (myName == "GamingGoon")
			{
				cam.GetComponent<GameingUI>().SetPausedBtnActive(false);
			}
			else if (myName == "GamingAgain")
			{
				//cam.GetComponent<GameingUI>().SetOpitonbtnActive(true);
				//Application.LoadLevel("Game");
				Debug.Log("GamingMusic");
			}
			else if (myName == "GamingBack")
			{
				//Application.LoadLevel("GameStrat");
				Debug.Log("GamingBack");
			}



	}
}
