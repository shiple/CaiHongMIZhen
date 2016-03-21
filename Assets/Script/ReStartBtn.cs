using UnityEngine;
using System.Collections;

public class ReStartBtn : MonoBehaviour 
{
	string myName;
	AudioClip btnClip;
	GameObject cam;
	public string MyName
	{
		set {myName = value;}
	}
	// Use this for initialization
	void Start () 
	{
		btnClip =  Resources.Load("btnDown") as AudioClip;
		cam = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator OnMouseDown()
	{
		if (Singleton.GetInstance().PlayMusic)
			this.audio.PlayOneShot(btnClip);
		
		yield return new WaitForSeconds(0.35f);
	}
	IEnumerator OnMouseUp()
	{
		//audio.PlayOneShot ();
		yield return new WaitForSeconds(0.35f);
		//Application.LoadLevel ("GameStart");

		cam.GetComponent<MainGame>().ReStart();
		if (myName == "succ")
		{
			cam.GetComponent<GameingUI>().GameSuccessful(false);
		}
		else if (myName == "lose")
		{
			cam.GetComponent<GameingUI>().GameLose(false);
		}
		Singleton.GetInstance().SetMyGameState(GameState.GameRun);
	}
}
