using UnityEngine;
using System.Collections;

public class GamingBtns : MonoBehaviour {

	string myName;
	public string MyName
	{
		set {myName = value;}
	}
	GameObject cam;
	void Start ()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
	}
	IEnumerator OnMouseUp()
	{

		//audio.PlayOneShot ();
		yield return new WaitForSeconds(0.35f);

		if (myName == "paused")
		{
			if (Singleton.GetInstance().GetMyGameState() != GameState.GameOption)
			{
				cam.GetComponent<GameingUI>().SetPausedBtnActive(true);
				Singleton.GetInstance().SetMyGameState(GameState.GamePause);
			}

		}
		else if (myName == "option")
		{
			if (Singleton.GetInstance().GetMyGameState() != GameState.GamePause);
			{
				cam.GetComponent<GameingUI>().SetOpitonbtnActive(true);
				Singleton.GetInstance().SetMyGameState(GameState.GameOption);
			}

		}

	}
}
