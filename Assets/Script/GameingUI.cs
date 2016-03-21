using UnityEngine;
using System.Collections;

public class GameingUI : MonoBehaviour {

	GameObject pausedPanel;
	GameObject goonBtn;
	GameObject againBtn;
	GameObject backBtn;

	GameObject opPanel;
	GameObject goonBtn2;
	GameObject musicBtn;
	GameObject soundBtn3;

	GameObject succPanel;
	GameObject nextBtn;
	GameObject backBtn2;

	GameObject losePanel;
	GameObject againBtn2;
	GameObject backBtn3;
	
	void Start ()
	{
		GameObject.Find("zanting").GetComponent<GamingBtns>().MyName = "paused";
		pausedPanel = Instantiate(Resources.Load("pausedPanel")) as GameObject;
		pausedPanel.GetComponent<SpriteRenderer>().sortingOrder = 1;
		goonBtn = Instantiate(Resources.Load("goon"), new Vector3(0.2f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
		againBtn =  Instantiate(Resources.Load("again"), new Vector3(0.5f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
		backBtn = Instantiate(Resources.Load("back"), new Vector3(0.8f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
		goonBtn.GetComponent<GamingPauBtn>().MyName = "GamingGoon" ;
		againBtn.GetComponent<GamingPauBtn>().MyName = "GamingAgain" ;
		backBtn.GetComponent<GamingPauBtn>().MyName = "GamingBack";
		pausedPanel.SetActive(false);
		goonBtn.SetActive(false);
		againBtn.SetActive(false);
		backBtn.SetActive(false);

		GameObject.Find("xuanxiang").GetComponent<GamingBtns>().MyName = "option";
		opPanel = Instantiate(Resources.Load("optionPanel")) as GameObject;;
		goonBtn2 = Instantiate(Resources.Load("goon"), new Vector3(0.2f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
		musicBtn = Instantiate(Resources.Load("music"), new Vector3(0.5f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
		soundBtn3 = Instantiate(Resources.Load("sound"), new Vector3(0.8f, 0.46f, 0.0f), Quaternion.identity) as GameObject;
		goonBtn2.GetComponent<OptionBtns>().MyName = "GamingGoon" ;
		musicBtn.GetComponent<OptionBtns>().MyName = "music" ;
		soundBtn3.GetComponent<OptionBtns>().MyName = "sound" ;
		opPanel.SetActive(false);
		goonBtn2.SetActive(false);
		musicBtn.SetActive(false);
		soundBtn3.SetActive(false);

		succPanel = Instantiate(Resources.Load("succPanel")) as GameObject;
		succPanel.transform.position = Vector3.zero;
		nextBtn = Instantiate(Resources.Load("next"), new Vector3(0.3f, 0.48f, 0.0f), Quaternion.identity) as GameObject;
		backBtn2 =  Instantiate(Resources.Load("back2"), new Vector3(0.7f, 0.48f, 0.0f), Quaternion.identity) as GameObject;
		succPanel.SetActive(false);
		nextBtn.SetActive(false);
		nextBtn.GetComponent<ReStartBtn>().MyName = "succ" ;
		backBtn2.SetActive(false);

		losePanel = Instantiate(Resources.Load("losePanel")) as GameObject;
		losePanel.transform.position = Vector3.zero;
		againBtn2 = Instantiate(Resources.Load("again2"), new Vector3(0.3f, 0.48f, 0.0f), Quaternion.identity) as GameObject;
		backBtn3 = Instantiate(Resources.Load("back2"), new Vector3(0.7f, 0.48f, 0.0f), Quaternion.identity) as GameObject;
		losePanel.SetActive(false);
		againBtn2.SetActive(false);
		againBtn2.GetComponent<ReStartBtn>().MyName = "lose" ;
		backBtn3.SetActive(false);
	}
	public void GameSuccessful(bool b)
	{
		succPanel.SetActive(b);
		nextBtn.SetActive(b);
		backBtn2.SetActive(b);
	}
	public void GameLose(bool b)
	{
		losePanel.SetActive(b);
		againBtn2.SetActive(b);
		backBtn3.SetActive(b);
	}
	public void SetPausedBtnActive(bool b)
	{
		pausedPanel.SetActive(b);
		goonBtn.SetActive(b);
		againBtn.SetActive(b);
		backBtn.SetActive(b);
	}
	public void SetOpitonbtnActive(bool b)
	{
		opPanel.SetActive(b);
		goonBtn2.SetActive(b);
		musicBtn.SetActive(b);
		soundBtn3.SetActive(b);
	}

}
