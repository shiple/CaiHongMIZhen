using UnityEngine;
using System.Collections;
[RequireComponent (typeof (AudioSource))]

public class Floor : MonoBehaviour {

	string myColor;
	public string MyColor
	{
		set { myColor = value; }
		get { return myColor; }
	}
	int myGrade;
	public int MyGrade
	{
		set { myGrade = value; }
		get { return (myGrade==0)?0:(myGrade-3); }
	}
	GameObject plusObj;

	bool moveEnable;
	float targetX;
	float targetY;
	float curruntX;
	float curruntY;
	float newX;
	float newY;
	bool moveEnd;

	bool disapper;
	float targetA;
	float currentA;
	float newA;
	bool disapperEnd;

	bool appear;
	float targetSizeX;
	float targetSizeY;
	float currentSizeX;
	float currentSizeY;
	float newSizeX;
	float newSizeY;
	bool appearEnd;
	
	public bool MoveEnd
	{
		get { return moveEnd; }
	}
	void Awake ()
	{
		myColor = "";
		myGrade = 0;
		plusObj = null;

		moveEnable = false;
		targetX = 0.0f;
		targetY = 0.0f;
		curruntX = this.transform.position.x;
		curruntX = this.transform.position.y;
		newX = 0.0f;
		newY = 0.0f;
		moveEnd = true;

		disapper = false;
		targetA = 0.0f;;
		currentA = this.GetComponent<SpriteRenderer>().color.a;
		newA = 0.0f;
		
		appear = false;
		targetSizeX = this.transform.localScale.x;
		targetSizeY = this.transform.localScale.y;
		currentSizeX = 0.0f;
		currentSizeY = 0.0f;
		newSizeX = 0.0f;
		newSizeY = 0.0f;

		disapperEnd = true;
		appearEnd = true;
		this.transform.localScale = new Vector3(0.0f, 0.0f, 1);
		//this.transform.localScale = new Vector3(1, 1, 1);
	}

	public void MyUpdate () 
	{
		if (moveEnable)
		{
			curruntX = this.transform.position.x;
			curruntY = this.transform.position.y;
			newX = Mathf.Lerp(curruntX, targetX, 0.2f);
			newY = Mathf.Lerp(curruntY, targetY, 0.2f);
			this.transform.position = new Vector3(newX, newY, this.transform.position.z);
			if (plusObj != null)
				plusObj.transform.position = new Vector3(newX, newY, this.transform.position.z);
			if (curruntX == newX && curruntY == newY)
			{
				moveEnable = false;
				moveEnd = true;

			}
		}
		if (disapper)
		{
			currentA = this.GetComponent<SpriteRenderer>().color.a;
			newA = Mathf.Lerp(currentA, targetA, 0.1f);
			float r = this.GetComponent<SpriteRenderer>().color.r;
			float g = this.GetComponent<SpriteRenderer>().color.g;
			float b =  this.GetComponent<SpriteRenderer>().color.b;
			this.GetComponent<SpriteRenderer>().color = new Color(r, g, b, newA);
			if ((currentA - newA) <= 0.00005f)
			{
				disapper = false;
				disapperEnd = true;
				if (plusObj != null)
					Destroy(plusObj);
				Destroy(this.gameObject);
			}
		}
		if (appear)
		{
			currentSizeX = this.transform.localScale.x;
			currentSizeY = this.transform.localScale.y;
			newSizeX = Mathf.Lerp(currentSizeX, targetSizeX, 0.4f);
			newSizeY = Mathf.Lerp(currentSizeY, targetSizeY, 0.4f);
			this.transform.localScale = new Vector3(newSizeX, newSizeY, 1.0f);
			if (currentSizeX == newSizeX && currentSizeY == newSizeY)
			{
				if (myGrade != 0)
				{
					plusObj = GetPlusObj();
				}
				appear = false;
				appearEnd = true;
			}
		}
	}
	public void GetMoveInfo(bool mEnable, float x, float y)
	{
		moveEnable = mEnable;
		targetX = x;
		targetY = y;
		moveEnd = false;
	}
	public void GetAppearInfo(bool ap)
	{
		appear = ap;
		appearEnd = false;
	}
	public void GetDisappearInfo(bool disap)
	{
		disapper = disap;
		disapperEnd= false;
	}
	public bool AnimIsEnd()
	{
		return (appearEnd && disapperEnd && moveEnd);
	}
	public GameObject GetPlusObj()
	{
		GameObject obj = null;

		switch (myGrade)
		{
		case 4:
			obj = Resources.Load("plus1") as GameObject;
			break;
		case 5:
			obj = Resources.Load("plus2") as GameObject;
			break;
		case 6:
			obj = Resources.Load("plus3") as GameObject;
			break;
		default:
			break;
		}

		if (obj != null)
		{
			obj = Instantiate(obj, new Vector3(this.transform.position.x, this.transform.position.y, -1), Quaternion.identity) as GameObject;

		}
		return obj;
	}
}
