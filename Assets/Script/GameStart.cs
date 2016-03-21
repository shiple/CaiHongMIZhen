using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	GameObject background;
	GameObject startBtn;
	GameObject optionBtn;
	GameObject quitBtn;

	GameObject quitBtn1;
	GameObject quitBtn2;
	GameObject quitPanel;

	GameObject opPanel;
	GameObject opBtn1;
	GameObject opBtn2;
	GameObject opBtn3;
	GameObject opClose;

	//定义菜单项贴图
	public Texture start;
	public Texture exit;
	
	//定义标准屏幕分辨率
	public float m_fScreenWidth = 1920;
	public float m_fScreenHeight = 1080;
	
	//定义缩放系数
	public float m_fScaleWidth;
	public float m_fScaleHeight;

	GameObject sound;
	/*
	void Start () {
		
		//计算缩放系数
		m_fScaleWidth = (float)Screen.width / m_fScreenWidth;
		m_fScaleHeight = (float)Screen.height / m_fScreenHeight;
	}
	
	void OnGUI()
	{
		//绘制菜单
		GUI.Button(new Rect(10 * m_fScaleWidth, 10 * m_fScaleHeight, 200 * m_fScaleWidth, 50 * m_fScaleHeight), start);
		GUI.Button(new Rect(814 * m_fScaleWidth,708 * m_fScaleHeight, 200 * m_fScaleWidth, 50 * m_fScaleHeight), exit);
	}*/ 
	void Start () 
	{
		background = Resources.Load("StartBackground2") as GameObject;
		Instantiate(background, Vector3.zero, Quaternion.identity);
	
		float btnW = 0.0f;
		float btnH = 0.0f;
		float btnAnchorX = 0.0f;
		float btnAnchorY = 0.0f;
		float ratio= 0.8f;
		startBtn = GameObject.Find("start1") as GameObject;
		startBtn.GetComponent<MainMenuBtns>().LoadingResources("start2", "start3");
		startBtn.GetComponent<MainMenuBtns>().LevelName = "LevelSelect";
		btnW = startBtn.guiTexture.pixelInset.width * ratio;
		btnH = startBtn.guiTexture.pixelInset.height * ratio;
		btnAnchorX = -btnW/2;
		btnAnchorY = -btnH/2;
		startBtn.guiTexture.pixelInset = new Rect(btnAnchorX, btnAnchorY, btnW, btnH);

		optionBtn = GameObject.Find("option1") as GameObject;
		optionBtn.GetComponent<MainMenuBtns>().LoadingResources("option2", "option3");
		optionBtn.GetComponent<MainMenuBtns>().LevelName = "option";
		btnW = optionBtn.guiTexture.pixelInset.width * ratio;
		btnH = optionBtn.guiTexture.pixelInset.height * ratio;
		btnAnchorX = -btnW/2;
		btnAnchorY = -btnH/2;
		optionBtn.guiTexture.pixelInset = new Rect(btnAnchorX, btnAnchorY, btnW, btnH);

		quitBtn = GameObject.Find("quit1") as GameObject;
		quitBtn.GetComponent<MainMenuBtns>().LoadingResources("quit2", "quit3");
		quitBtn.GetComponent<MainMenuBtns>().LevelName = "quit";
		btnW = quitBtn.guiTexture.pixelInset.width * ratio;
		btnH = quitBtn.guiTexture.pixelInset.height * ratio;
		btnAnchorX = -btnW/2;
		btnAnchorY = -btnH/2;
		quitBtn.guiTexture.pixelInset = new Rect(btnAnchorX, btnAnchorY, btnW, btnH);

		if (!PlayerPrefs.HasKey("Level1"))
			PlayerPrefs.SetInt("Level1", 0);
		if (!PlayerPrefs.HasKey("Level2"))
			PlayerPrefs.SetInt("Level2", 0);
		if (!PlayerPrefs.HasKey("Level3"))
			PlayerPrefs.SetInt("Level3", 0);

		PlayerPrefs.SetInt("Level1", 1);

		if (!PlayerPrefs.HasKey("PlaySound"))
			PlayerPrefs.SetInt("PlaySound", 1);
		if (!PlayerPrefs.HasKey("PlayMusic"))
			PlayerPrefs.SetInt("PlayMusic", 1);


		sound = GameObject.FindWithTag("sound");

		if (PlayerPrefs.GetInt("PlayMusic") == 1)
			Singleton.GetInstance().PlayMusic = true;
		else
			Singleton.GetInstance().PlayMusic = false;

		if (PlayerPrefs.GetInt("PlaySound") == 1)
			sound.GetComponent<AudioSource>().Play();
		else
			sound.GetComponent<AudioSource>().Stop();


		if (Singleton.GetInstance().HasSound)
		{
			Destroy(sound);
		}
		else 
		{
			sound = GameObject.FindWithTag("sound");
			DontDestroyOnLoad(sound);
			Singleton.GetInstance().HasSound = true;
		}

		//PlayerPrefs.SetInt("Level1", 1);
		//PlayerPrefs.SetInt("Level2", 1);
		//PlayerPrefs.SetInt("Level3", 0);
	}
	void OnGUI()
	{
	
	}
	void Update () {
	
	}
	public void SetQuitBtns(GameObject btn1, GameObject btn2, GameObject panel)
	{
		quitBtn1 = btn1;
		quitBtn2 =btn2;
		quitPanel = panel;
	}
	public void SetQuitBtnsActive(bool b)
	{
		quitBtn1.SetActive(b);
		quitBtn2.SetActive(b);
		quitPanel.SetActive(b);
	}
	public void SetStartBtnsActive(bool b)
	{
		startBtn.SetActive(b);
		optionBtn.SetActive(b);
		quitBtn.SetActive(b);
	}
	public bool QuitBtnsExist()
	{
		if (quitBtn1 != null || quitBtn2 != null || quitPanel != null)
			return true;
		return false;
	}
	public void SetOptionBtns(GameObject pan, GameObject btn1, GameObject btn2, GameObject btn3, GameObject clo)
	{
		opPanel = pan;
		opBtn1 = btn1;
		opBtn2 = btn2;
		opBtn3 = btn3;
		opClose = clo;
	}
	public void SetOptionBtnsActive(bool b)
	{
		opPanel.SetActive(b);
		opBtn1.SetActive(b);
		opBtn2.SetActive(b);
		opBtn3.SetActive(b);
		opClose.SetActive(b);
	}
	public bool	 OptionBtnsExist()
	{
		if (opPanel != null || opBtn1 != null || opBtn2 != null || opBtn3 != null || opClose != null)
			return true;
		return false;
	}
}
