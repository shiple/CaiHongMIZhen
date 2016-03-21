using UnityEngine;
using System.Collections;

public class QuitBtns : MonoBehaviour {
	string myName;
	AudioClip btns;
	public string MyName
	{
		set {myName = value;}
	}
	void Start ()
	{
		btns = Resources.Load("btnDown") as AudioClip;
	}

	IEnumerator OnMouseDown()
	{
		if (Singleton.GetInstance().PlayMusic)
			audio.PlayOneShot (btns);

		yield return new WaitForSeconds(0.35f);
	}
	IEnumerator OnMouseUp()
	{
		//audio.PlayOneShot ();
		yield return new WaitForSeconds(0.35f);
		
		if (myName == "cancel")
		{
			/*
			GameObject[] quitBtns = GameObject.FindGameObjectsWithTag("Quit");
			foreach (GameObject obj in quitBtns)
			{
				if (obj.activeSelf)
					obj.SetActive(false);
			}
			*/
			GameObject cam = GameObject.FindGameObjectWithTag("Camera");
			cam.GetComponent<GameStart>().SetQuitBtnsActive(false);
			cam.GetComponent<GameStart>().SetStartBtnsActive(true);
		}
		else if (myName == "accept")
		{
			Application.Quit();
		}

	}
}
