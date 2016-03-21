using UnityEngine;
using System.Collections;
[RequireComponent (typeof (AudioSource))]

public class OptionBtns : MonoBehaviour {

	string myName;
	bool musicIsOff;
	bool soundIsOff;
	Texture2D musicOffObj1;
	Texture2D soundOffObj2;
	Texture2D yuanTexture;
	GameObject cam;
	GameObject sound;
	AudioClip btnClip;

	public bool MusicIsOff
	{
		get {return musicIsOff;}
	}
	public bool SoundIsOff
	{
		get {return soundIsOff;}
	}

	public string MyName
	{
		set {myName = value;}
	}
	void Start ()
	{
		musicIsOff = false;
		soundIsOff = false;
		musicOffObj1 = Resources.Load("musicoff") as Texture2D;
		soundOffObj2 = Resources.Load("soundoff") as Texture2D;
		yuanTexture = guiTexture.texture as Texture2D;
		cam = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
		sound = GameObject.FindWithTag("sound") as GameObject;
		btnClip =  Resources.Load("btnDown") as AudioClip;

		if (myName == "music")
		{
			if (PlayerPrefs.HasKey("PlayMusic"))
			{
				if (PlayerPrefs.GetInt("PlayMusic") == 1)
				{
					musicIsOff = false;
					guiTexture.texture = yuanTexture;
				}
				else 
				{
					musicIsOff = true;
					guiTexture.texture = musicOffObj1;
				}
			}
		}

		if (myName == "sound")
		{
			if (PlayerPrefs.HasKey("PlaySound"))
			{
				if (PlayerPrefs.GetInt("PlaySound") == 1)
				{
					soundIsOff = false;
					guiTexture.texture = yuanTexture;
				}
				else 
				{
					soundIsOff = true;
					guiTexture.texture = soundOffObj2;
				}
			}
		}

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
		
		if (myName == "GamingGoon")
		{
			Debug.Log("GoOn");
			cam.GetComponent<GameingUI>().SetOpitonbtnActive(false);
			Singleton.GetInstance().SetMyGameState(GameState.GameRun);
		}
		else if (myName == "music")
		{
			Debug.Log("Music");
			if (musicIsOff)
			{
				musicIsOff = false;
				guiTexture.texture = yuanTexture;
				Singleton.GetInstance().PlayMusic = true;
				PlayerPrefs.SetInt ("PlayMusic", 1);

			}
			else 
			{
				musicIsOff = true;
				guiTexture.texture = musicOffObj1;
				Singleton.GetInstance().PlayMusic = false;
				PlayerPrefs.SetInt ("PlayMusic", 0);
			}

		}
		else if (myName == "sound")
		{
			Debug.Log("Sound");
			if (soundIsOff)
			{
				soundIsOff = false;
				guiTexture.texture = yuanTexture;
				sound.GetComponent<AudioSource>().Play();
				PlayerPrefs.SetInt ("PlaySound", 1);
			}
			else 
			{
				soundIsOff = true;
				guiTexture.texture = soundOffObj2;
				sound.GetComponent<AudioSource>().Pause();
				PlayerPrefs.SetInt ("PlaySound", 0);
			}
		}
		else if (myName == "opClose")
		{
			GameObject cam = GameObject.FindGameObjectWithTag("Camera");
			if (cam.GetComponent<GameStart>().OptionBtnsExist())
				cam.GetComponent<GameStart>().SetOptionBtnsActive(false);
			cam.GetComponent<GameStart>().SetStartBtnsActive(true);
		}
		
	}
}
