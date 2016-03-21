using UnityEngine;
using System.Collections;

public struct DistanceData
{
	public int x;
	public int y;
	public float dis;
}
public class Hero : MonoBehaviour {

	bool moveEnable;
	float targetX;
	float targetY;
	float curruntX;
	float curruntY;
	float newX;
	float newY;
	bool moveEnd;
	public bool MoveEnd
	{
		get{return moveEnd;}
	}
	string[] moveList;
	int firstP;
	int finalP;
	FloorPoint[, ] myMap;
	string selectFloorStr;
	public string SelectFloor
	{
		set {selectFloorStr = value;}
	}
	public FloorPoint[,] Map
	{
		set {myMap = value;}
	}
	int myXInMap;
	int myYInMap;
	DistanceData[] disList;
	int disListP;
	bool stepStart;
	DistanceData[] tempList;
	int  award;
	bool awardMoveEnable;
	bool step2;
	AudioClip moveClip;

	void Awake () 
	{
		moveEnable = false;
		targetX = 0.0f;
		targetY = 0.0f;
		curruntX = this.transform.position.x;
		curruntY = this.transform.position.y;
		newX = 0.0f;
		newY = 0.0f;
		moveEnd = true;
		moveList = new string[10];
		firstP = 0;
		finalP = -1;
		myMap = new FloorPoint[6, 6];
		selectFloorStr = null;
		myXInMap = 5;
		myYInMap = 5;
		disList = new DistanceData[35];
		disListP = -1;
		stepStart = false;
		award = 0;
		awardMoveEnable = false;
		step2 = false;
		moveClip = Resources.Load("heromove") as AudioClip;
	}
	void Start()
	{
		GameObject anc = GameObject.FindGameObjectWithTag("Anchor");
		float x = this.transform.position.x - anc.transform.position.x;
		float y = this.transform.position.y - anc.transform.position.y;
		float ang = Mathf.Atan2(1, 0)/Mathf.PI * 180;
		Debug.Log(ang);
	}
	public void MyUpdate () 
	{
		if (firstP > finalP)
		{
			if (award == 0 && !awardMoveEnable)
			{
				moveEnd = true;

			}
			else if (!awardMoveEnable)
			{
				CheckAward();
			}
				
		}
			
		if (awardMoveEnable)
		{
			curruntX = this.transform.position.x;
			curruntY = this.transform.position.y;
			newX = Mathf.Lerp(curruntX, targetX, 0.1f);
			newY = Mathf.Lerp(curruntY, targetY, 0.1f);
			this.transform.position = new Vector3(newX, newY, -2);
			if (curruntX == newX && curruntY == newY)
			{
				awardMoveEnable = false;
				this.audio.Stop();
			}
		}

		if (moveEnable)
		{
			stepStart = false;
			curruntX = this.transform.position.x;
			curruntY = this.transform.position.y;
			newX = Mathf.Lerp(curruntX, targetX, 0.1f);
			newY = Mathf.Lerp(curruntY, targetY, 0.1f);
			this.transform.position = new Vector3(newX, newY, -2);
			if (curruntX == newX && curruntY == newY)
			{
				moveEnable = false;
				MoveListPop();
				stepStart = true;
				this.audio.Stop();
			}
		}

		if (firstP <= finalP  && stepStart)
		{
			if (Singleton.GetInstance().GetMyGameState() ==  GameState.GameRun)
				this.audio.PlayOneShot(moveClip);
			FindNearFloor();
		}
	}
	void MoveListAdd(string data)
	{
		for (int i = 0; i < moveList.Length; i++)
		{
			if (moveList[i] == data)
				return;
		}

		finalP += 1;
		if (finalP >= 0 && finalP <= 9)
		{
			moveList[finalP] = data;
		}
	}
	void MoveListPop()
	{
		if (firstP >= 0 && firstP <= 9)
		{
			moveList[firstP] = null;
		}
		firstP++;
		if (firstP > finalP)
		{
			MoveListClear();
		}
	}
	void MoveListClear()
	{
		for (int i = firstP; i <= finalP; i++)
		{
			moveList[i] = "";
		}
		firstP = 0;
		finalP = -1;
	}
	public void SetMoveInfo(string str)
	{
		MoveListAdd(str);
		moveEnd = false;
		stepStart = true;
	}
	void FindNearFloor()
	{
		Debug.Log ("Find Near Floor Start -----");
		DisListClear();
		selectFloorStr = moveList[firstP];
		if (selectFloorStr != null)
		{
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					if (i == myXInMap && j == myYInMap)
						continue;
					else
					{
						if (myMap[i, j].floor.GetComponent<Floor>().MyColor == selectFloorStr)
						{
							float tarx = myMap[i, j].floor.transform.position.x;
							float tary = myMap[i, j].floor.transform.position.y;
							float mx = this.transform.position.x;
							float my = this.transform.position.y;
							float f = Vector2.Distance(new Vector2(tarx, tary), new Vector2(mx, my));
							DisListAdd(f, i, j);
						}

					}
				}
			}
			if (disListP == -1 )
				return;

			int len1 = GetDisListRealLength();
			tempList = new DistanceData[len1];
			for (int i = 0 ; i < len1; i ++)
			{
				tempList[i] = disList[i];
			}
			Sort(tempList);

			int p = 0;
			int count = 1;
			while (true)
			{
				if (tempList.Length == 0)
					break;

				if (p+1 >= tempList.Length)
					break;

				if ((tempList[p+1].dis - tempList[p].dis) < 0.01f)
					count ++;
				else
					break;
				p++;
			}

			if (count == 1)
			{
				targetX = myMap[tempList[0].x, tempList[0].y].floor.transform.position.x;
				targetY = myMap[tempList[0].x, tempList[0].y].floor.transform.position.y;
				moveEnable = true;
				SetMyPosInMap(tempList[0].x, tempList[0].y);
				Debug.Log ("Find it ----");
				return;
			}

			DistanceData[] nearList = new DistanceData[count];
			for (int i = 0; i < count; i++)
			{
				nearList[i] = tempList[i];
			}

			DisListClear();
			//DistanceData[] disToZero = new DistanceData[16];
			for (int i = 0; i < nearList.Length; i++)
			{
				if (nearList[i].dis != 0)
				{
					int nx = nearList[i].x;
					int ny = nearList[i].y;
					Vector2 v1 = new Vector2(myMap[nx, ny].floor.transform.position.x, myMap[nx, ny].floor.transform.position.y);
					Vector2 v2 = new Vector2(myMap[0, 0].floor.transform.position.x, myMap[0, 0].floor.transform.position.y);
					float f = Vector2.Distance(v1, v2);
					DisListAdd(f, nearList[i].x, nearList[i].y);
				}
			}

			int len2 = GetDisListRealLength();
			tempList = new DistanceData[len2];
			for (int i = 0 ; i < len2; i ++)
			{
				tempList[i] = disList[i];
			}
			Sort(tempList);

			targetX = myMap[tempList[0].x, tempList[0].y].floor.transform.position.x;
			targetY = myMap[tempList[0].x, tempList[0].y].floor.transform.position.y;
			moveEnable = true;
			SetMyPosInMap(tempList[0].x, tempList[0].y);
			Debug.Log ("Find it ----");
		}
		Debug.Log ("Not Find -----");
	}
	public void SetMyPosInMap(int x, int y)
	{
		myXInMap = x;
		myYInMap = y;
	}
	void 	DisListAdd(float data, int x, int y)
	{
		if (disListP < 35)
			disListP ++;
		disList[disListP].dis = data;
		disList[disListP].x = x;
		disList[disListP].y = y;
	}
	void DisListClear()
	{
		for (int i = 0; i <= disListP; i++)
		{
			disList[i].dis = 0.0f;
			disList[disListP].x = 0;
			disList[disListP].y = 0;
		}
		disListP = -1;
	}

	public static void Sort(DistanceData[] numbers)
	{
		QuickSort(numbers, 0, numbers.Length - 1);
	}
	private static void QuickSort(DistanceData[] numbers, int left, int right)
	{
		if (left < right)
		{
			float middle = numbers[(int) ((left + right) / 2)].dis;
			int i = left - 1;
			int j = right + 1;
			while (true)
			{
				while (numbers[++i].dis < middle) ;
				
				while (numbers[--j].dis > middle) ;
				
				if (i >= j)
					break;
				
				Swap(numbers, i, j);
			}
			
			QuickSort(numbers, left, i - 1);
			QuickSort(numbers, j + 1, right);
		}
	}
	
	private static void Swap(DistanceData[] numbers, int i, int j)
	{
		float number = numbers[i].dis;
		int x = numbers[i].x;
		int y = numbers[i].y;
		numbers[i].dis = numbers[j].dis;
		numbers[i].x = numbers[j].x;
		numbers[i].y = numbers[j].y;
		numbers[j].dis = number;
		numbers[j].x = x;
		numbers[j].y = y;
	}
	int GetDisListRealLength()
	{
		int count = 0;
		for (int i = 0 ; i < disList.Length; i++)
		{
			if (disList[i].dis != 0)
			{
				count += 1;
			}
		}
		return count;
	}
	public Vector2 GetHeroPos()
	{
		return new Vector2(myXInMap, myYInMap);
	}
	void CheckAward()
	{
		if (award == 0)
			return;
		if (Singleton.GetInstance().PlayMusic)
		{
			this.audio.Stop();
			if (Singleton.GetInstance().GetMyGameState() ==  GameState.GameRun)
				this.audio.PlayOneShot(moveClip);
		}
		step2 = true;
		float r = Random.Range(1.0f, 2.0f);
		Debug.Log(r);
		int intr = (r<1.5)?1:2;

		switch (intr)
		{
		case 1:
		{
			if ((myXInMap - 1) >= 0)
			{
				targetX = myMap[myXInMap - 1, myYInMap].floor.transform.position.x;
				targetY = myMap[myXInMap - 1, myYInMap].floor.transform.position.y;
				awardMoveEnable = true;
				SetMyPosInMap(myXInMap - 1, myYInMap);
			}
			else if ((myYInMap - 1) >= 0)
			{
				targetX = myMap[myXInMap, myYInMap - 1].floor.transform.position.x;
				targetY = myMap[myXInMap, myYInMap - 1].floor.transform.position.y;
				awardMoveEnable = true;
				SetMyPosInMap(myXInMap, myYInMap - 1);
			}
			
			break;
		}
		case 2:
		{
			if ((myYInMap - 1) >= 0)
			{
				targetX = myMap[myXInMap, myYInMap - 1].floor.transform.position.x;
				targetY = myMap[myXInMap, myYInMap - 1].floor.transform.position.y;
				awardMoveEnable = true;
				SetMyPosInMap(myXInMap, myYInMap - 1);
			}
			else if ((myXInMap - 1) >= 0)
			{
				targetX = myMap[myXInMap - 1, myYInMap].floor.transform.position.x;
				targetY = myMap[myXInMap - 1, myYInMap].floor.transform.position.y;
				awardMoveEnable = true;
				SetMyPosInMap(myXInMap - 1, myYInMap);
			}

			break;
		}

		}

		award -= 1;

	}
	public void SetHeroAward(int awar)
	{
		award += awar;
	}
}
