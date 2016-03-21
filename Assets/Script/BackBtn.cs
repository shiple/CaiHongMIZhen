using UnityEngine;
using System.Collections;

public class BackBtn : MonoBehaviour 
{
	string myName;
	AudioClip btnClip;

	public string MyName
	{
		set {myName = value;}
	}
	// Use this for initialization
	void Start () 
	{
		btnClip =  Resources.Load("btnDown") as AudioClip;

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
		Application.LoadLevel ("GameStart");
	}
}
