using UnityEngine;
using System.Collections;

public class LevelSelectBtn : MonoBehaviour {
	public string name;
	Texture2D open;
	Texture2D close;
	// Use this for initialization
	bool isOpen;
	void Start () 
	{
		isOpen = false;
		open = Resources.Load("levelbtnopen") as Texture2D;
		close = Resources.Load("levelbtnclose") as Texture2D;

		switch (name)
		{
		case "level1":
		{
			if (PlayerPrefs.HasKey("Level1"))
			{
				int i = PlayerPrefs.GetInt("Level1");
				if (PlayerPrefs.GetInt("Level1") == 1)
				{
					this.guiTexture.texture = open;
					isOpen = true;
				}
				else 
				{
					this.guiTexture.texture = close;
				}
			}
		}
			break;
		case "level2":
		{
			if (PlayerPrefs.HasKey("Level2"))
			{
				if (PlayerPrefs.GetInt("Level2") == 1)
				{
					this.guiTexture.texture = open;
					isOpen = true;
				}
				else 
				{
					this.guiTexture.texture = close;
				}
			}
		}
			break;
		case "level3":
		{
			if (PlayerPrefs.HasKey("Level3"))
			{
				if (PlayerPrefs.GetInt("Level3") == 1)
				{
					this.guiTexture.texture = open;
					isOpen = true;
				}
				else 
				{
					this.guiTexture.texture = close;
				}
			}
		}
			break;

		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	IEnumerator OnMouseUp()
	{
		//audio.PlayOneShot ();
		yield return new WaitForSeconds(0.35f);
		switch (name)
		{
		case "level1":
			Singleton.GetInstance().GameLevel = 1;
			break;
		case "level2":
			Singleton.GetInstance().GameLevel = 2;
			break;
		case "level3":
			Singleton.GetInstance().GameLevel = 3;
			break;
		}

		if (isOpen)
			Application.LoadLevel ("Game");
	}
}
