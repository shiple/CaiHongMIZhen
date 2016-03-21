using UnityEngine;
using System.Collections;
[RequireComponent (typeof (AudioSource))]

public class MainMenuBtns : MonoBehaviour {

	Texture2D normalTex;
	Texture2D rollOverTex;
	AudioClip beep;
	string levelName;
	AudioClip btnClip;
	public string LevelName
	{
		set {levelName = value;}
	}

	void Start()
	{
		//beep = Resources.Load("BtnClip") as AudioClip;
		btnClip = Resources.Load("btnDown") as AudioClip;
	}
	public void LoadingResources(string normal, string rollOver)
	{
		normalTex = Resources.Load(normal) as Texture2D;
		rollOverTex = Resources.Load(rollOver) as Texture2D;
	}

	void OnMouseEnter ()
	{
		guiTexture.texture = rollOverTex;
	}

	void OnMouseExit ()
	{
		guiTexture.texture =normalTex;
	}

	IEnumerator OnMouseDown()
	{
		if (Singleton.GetInstance().PlayMusic)
			audio.PlayOneShot(btnClip);
		yield return new WaitForSeconds(0.35f);
	}

	IEnumerator OnMouseUp()
	{
		//audio.PlayOneShot ();
		yield return new WaitForSeconds(0.35f);

		if (levelName == "LevelSelect")
		{
			Application.LoadLevel(levelName);
		}
		else if (levelName == "option")
		{
			GameObject cam = GameObject.FindGameObjectWithTag("Camera");
			cam.GetComponent<GameStart>().SetStartBtnsActive(false);
			if (!cam.GetComponent<GameStart>().OptionBtnsExist())
			{
				GameObject panel = Instantiate(Resources.Load("optionPanel")) as GameObject;
				GameObject btn1 = Instantiate(Resources.Load("help"), new Vector3(0.2f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
				btn1.gameObject.GetComponent<OptionBtns>().MyName = "help";
				GameObject btn2 = Instantiate(Resources.Load("music"), new Vector3(0.5f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
				btn2.gameObject.GetComponent<OptionBtns>().MyName = "music";
				GameObject btn3 = Instantiate(Resources.Load("sound"), new Vector3(0.8f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
				btn3.gameObject.GetComponent<OptionBtns>().MyName = "sound";
				GameObject clo = Instantiate(Resources.Load("opClose"), new Vector3(0.9f, 0.6f, 0.0f), Quaternion.identity) as GameObject;
				clo.gameObject.GetComponent<OptionBtns>().MyName = "opClose";
				cam.GetComponent<GameStart>().SetOptionBtns(panel, btn1, btn2, btn3, clo);
			}
			else 
			{
				cam.GetComponent<GameStart>().SetOptionBtnsActive(true);
			}
		}
		else if (levelName == "quit")
		{
			/*
			GameObject[] btns = GameObject.FindGameObjectsWithTag("Btns");
			foreach (GameObject obj in btns)
			{
				if (obj.activeSelf)
					obj.SetActive(false);
			}
			*/
			GameObject cam = GameObject.FindGameObjectWithTag("Camera");
			cam.GetComponent<GameStart>().SetStartBtnsActive(false);

			if (!cam.GetComponent<GameStart>().QuitBtnsExist())
			{
				GameObject panel = Instantiate(Resources.Load("quitPanel")) as GameObject;
				GameObject can = Instantiate(Resources.Load("cancel"), new Vector3(0.25f, 0.38f, 0.0f), Quaternion.identity) as GameObject;
				can.gameObject.GetComponent<QuitBtns>().MyName = "cancel";
				GameObject acc = Instantiate(Resources.Load("accept"), new Vector3(0.75f, 0.38f, 0.0f), Quaternion.identity) as GameObject;
				acc.gameObject.GetComponent<QuitBtns>().MyName = "accept";
				cam.GetComponent<GameStart>().SetQuitBtns(can, acc, panel);
			}
			else
			{
				cam.GetComponent<GameStart>().SetQuitBtnsActive(true);
			}

		}

	}

}
