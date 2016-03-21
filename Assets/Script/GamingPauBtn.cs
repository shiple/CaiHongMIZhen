using UnityEngine;
using System.Collections;
[RequireComponent (typeof (AudioSource))]

public class GamingPauBtn : MonoBehaviour {
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
			Singleton.GetInstance().SetMyGameState(GameState.GameRun);
		}
		else if (myName == "GamingAgain")
		{
			cam.GetComponent<MainGame>().ReStart();
			cam.GetComponent<GameingUI>().SetPausedBtnActive(false);
			Singleton.GetInstance().SetMyGameState(GameState.GameRun);
			Debug.Log("GamingAgain");
		}
		else if (myName == "GamingBack")
		{
			Application.LoadLevel ("GameStart");
			Debug.Log("GamingBack");
		}




	}
}
