using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct FloorPoint
{
	public int x;
	public int y;
	public GameObject floor;
	public bool isSign;
	public int isSign2;
	public bool isChanged;
	public int group;
}

public class MainGame : MonoBehaviour {

	FloorPoint[, ] myPoints ;
	int myMapWidth;
	int myMapHeight;

	FloorPoint firstFloor;
	FloorPoint secondFloor;
	bool isSelected;

	Vector2[] removeList;
	int listP;
	bool jianchewan;
	GameObject selectObj;
	FloorPoint selectFloor;

	GameObject plus1Obj;
	GameObject plus2Obj;
	GameObject plus3Obj;

	int floorGrade;
	static int floorLineCount;
	static int floorRowCount;
	System.Collections.Generic.List<FloorPoint> specialList = new  List<FloorPoint>();
	bool specialIsDone;

	GameObject myHero;

	bool GameIsOver;
	bool OverIsAppear;
	bool GameIsWin;
	bool WinIsAppear;
	bool GameIsPause;
	bool PauseIsAppear;

	FloorPoint selectFloor1;
	FloorPoint selectFloor2;

	int currentLevel;
	int MapWidth
	{
		set {myMapWidth = value;}
	}
	int MapHeight
	{
		set {myMapHeight = value;}
	}

	AudioClip floorMove;
	AudioClip floorDis;

	GameObject targetFloor;

	void Start () 
	{
		currentLevel = Singleton.GetInstance().GameLevel;
		firstFloor.floor = null;
		secondFloor.floor = null;
		isSelected = false;
		myMapWidth = 6;
		myMapHeight = 6;
		myHero = GetHeroObj();
		initMap(myMapWidth, myMapHeight);
		removeList = new Vector2[myMapWidth * myMapHeight];
		listP = -1;
		jianchewan = false;
		selectObj= null;
		selectFloor.floor = null;
		floorGrade = 0;
		specialIsDone = false;
		GameIsOver = false;
		OverIsAppear = false;
		GameIsWin = false;
		WinIsAppear = false;
		GameIsPause = false;
		PauseIsAppear = false;

		floorMove = Resources.Load("floormove") as AudioClip;
		floorDis = Resources.Load("floordisapper") as AudioClip;

		targetFloor = Instantiate(Resources.Load("target"), myPoints[myMapWidth-1, 0].floor.transform.position, Quaternion.identity) as GameObject;
	}

	void Update () 
	{
		if (Singleton.GetInstance().GetMyGameState() == GameState.GamePause || 
		    Singleton.GetInstance().GetMyGameState() == GameState.GameOption)
		{
			return;
		}
		if (!(GameIsOver || GameIsWin))
		{
				//GetRandomFloor(1.0f ,5.0f);
			if(Input.GetMouseButtonDown(0))
			{
				if (CheckAllFloorAnimIsEnd() && myHero.GetComponent<Hero>().MoveEnd)
				{
					FloorPoint ga = GetNearFloor(Input.mousePosition);
					if (ga.floor != null)
					{
						//Debug.Log("1+" + ga.floor);
						if (ga.floor != secondFloor.floor)
						{
							firstFloor = ga;
						}
						//Debug.Log(firstFloor.floor);
					}
					
					jianchewan = false;
				}

			}
			if (Input.GetMouseButtonUp(0))
			{
				if (CheckAllFloorAnimIsEnd() && myHero.GetComponent<Hero>().MoveEnd)
				{
					FloorPoint ga = GetNearFloor(Input.mousePosition);
					if (ga.floor != null)
					{
						//Debug.Log("2+" + ga.floor);
						if (ga.floor == firstFloor.floor)
						{
							// 选中
							if (!isSelected)
							{
								isSelected = true;
								selectObj = GetSelectPrefab(ga.floor.transform.position);
								selectFloor = ga;
							}
							else
							{
								if (selectObj != null)
									Destroy(selectObj);
								secondFloor = selectFloor;
								ChangeFloor();
								isSelected = false;
								secondFloor.floor = null;
								selectFloor.floor = null;
							}
						}
						else
						{
							if (selectObj == null)
							{
								//交换
								secondFloor = ga;
								ChangeFloor();
								isSelected = false;
								secondFloor.floor = null;
							}
							else
							{
								Destroy(selectObj);
								isSelected = false;
								secondFloor.floor = null;
							}
						}
						//Debug.Log(secondFloor.floor);
						
					}
				
					//bugtext();
				}
				//CheckAndRemove();
			}
			if (!CheckAllFloorAnimIsEnd())
				FloorsUpdate();
			if (CheckAllFloorAnimIsEnd() && !specialIsDone )
				InitSpecialFloor();
			if (CheckAllFloorAnimIsEnd())
				IintNewFloor();
			if (CheckAllFloorAnimIsEnd())
				myHero.GetComponent<Hero>().MyUpdate();
			if (CheckAllFloorAnimIsEnd() && MyHeroMoveEnd())
				CheckAndRemove();
			//if (CheckAllFloorAnimIsEnd() && MyHeroMoveEnd())
			CheckGameWin();
		}

		if (CheckAllFloorAnimIsEnd() && OverIsAppear)
		{
			if (Singleton.GetInstance().GetMyGameState() == GameState.GameEnd)
			{
				//Instantiate(Resources.Load("gameover"), Vector3.zero, Quaternion.identity);
				GameIsOver = true;
				OverIsAppear = false;
			}
		}
		if (CheckAllFloorAnimIsEnd() && PauseIsAppear)
		{
			if (Singleton.GetInstance().GetMyGameState() == GameState.GamePause)
			{
				//Instantiate(Resources.Load(""), Vector3.zero, Quaternion.identity);

				GameIsWin = true;
				PauseIsAppear = false;
			}
		}
		if (CheckAllFloorAnimIsEnd() && WinIsAppear)
		{
			if (Singleton.GetInstance().GetMyGameState() == GameState.GameWin)
			{
				//Instantiate(Resources.Load("gamewin"), Vector3.zero, Quaternion.identity);
				GameIsWin = true;
				WinIsAppear = false;
			}
		}
	}
	void initMap(int width = 6, int height = 6)
	{ 
		GameObject anc = GameObject.FindGameObjectWithTag("Anchor");
		float offset = 1.6f;

		myPoints = new FloorPoint[width, height];

		for (int i = 0; i < width; i++)
		{
			while(true)
			{
				ClearOneLine(i, height);
				for(int j = 0; j < height; j++)
				{
					myPoints[i, j].x = i;
					myPoints[i, j].y = j;
					string str = GetRandomFloor(1.0f, 5.0f);
					if (Resources.Load(str))
					{
						myPoints[i, j].floor = Instantiate(Resources.Load(str), new Vector3(myPoints[i, j].x, myPoints[i, j].y, 0), Quaternion.identity) as GameObject;
						myPoints[i, j].floor.GetComponent<Floor>().MyColor = str;
						myPoints[i, j].floor.transform.position = new Vector3((anc.transform.position.x + offset * i), (anc.transform.position.y + offset * j), 0);
						myPoints[i, j].isSign = false;
						myPoints[i, j].isSign2 = 0;
						myPoints[i, j].isChanged = false;
						myPoints[i, j].floor.GetComponent<Floor>().GetAppearInfo(true);
					}
				}
				if (CheckOneLine(i, height) && CheckWithOtherLine(i, height))
					break;
			} 
			myHero.GetComponent<Hero>().SetMyPosInMap(myMapWidth-1, myMapHeight-1);
			myHero.GetComponent<Hero>().Map = myPoints;
		}
		Debug.Log("First Init Map Done ---------");
	}
	string GetRandomFloor(float min, float max)
	{
		float r = Random.Range(min, (max + currentLevel - 1 ));
		//float r = Random.Range(min, 7.0f);
		int intr = (int)(r + 0.5f);
		//Debug.Log(intr);
		switch(intr)
		{
		case 1:
			return "red";
			break;
		case 2:
			return "ora";
			break;
		case 3:
			return "yel";
			break;
		case 4:
			return "gre";
			break;
		case 5:
			return "blu";
			break;
		case 6:
			return "dia";
			break;
		case 7:
			return "pur";
			break;
		default:
			return "blu";
		}
	}
	bool CheckOneLine(int i, int height)
	{
		for(int j = 0; j < height; j++)
		{
			string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
			int haveSameColorCount = 0;
			//竖方向
			if ((j - 1) >= 0 && (j + 1) < height)
			{
				//判断是否在中心点
				for (int k = -1; k < 2; k++)
				{
					if (curCol == myPoints[i, (j + k)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
					}
					else 
						break;
				}
				if (haveSameColorCount == 3)
					return false;
				else 
					haveSameColorCount = 0;
			}
			if ((j + 2) < height)
			{
				//在三个的下边
				for (int k = 1; k <= 2; k++)
				{
					if (curCol == myPoints[i, (j + k)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
					}
					else
						break;
				}
				if (haveSameColorCount == 2)
					return false;
				else 
					haveSameColorCount = 0;
			}
			if ((j - 2) >= 0)
			{
				//在三个的上边
				for (int k = 1; k <= 2; k++)
				{
					if (curCol == myPoints[i, (j - k)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
					}
					else
						break;
				}
				if (haveSameColorCount == 2)			
					return false;
				else 
					haveSameColorCount = 0;
			}
		}
		return true;
	}
	bool CheckWithOtherLine(int i, int height)
	{
		if (i >= 2)
		{
			for (int j = 0; j < height; j++)
			{
				string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
				int haveSameColorCount = 0;
				//在三个的右边
				for (int k = 1; k <= 2; k++)
				{
					if (curCol == myPoints[(i - k), j].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
					}
					else
						break;
				}
				if (haveSameColorCount == 2)
					return false;
				else 
					haveSameColorCount = 0;
			}
		}
		return true;
	}
	void ClearOneLine(int i, int height)
	{
		for (int j = 0; j < height; j++)
		{
			if (myPoints[i, j].floor != null)
				DestroyObject(myPoints[i, j].floor);
		}
	}

	void FloorsUpdate()
	{
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				if (myPoints[i, j].floor != null)
					myPoints[i, j].floor.gameObject.GetComponent<Floor>().MyUpdate();
			}
		}
	}

	FloorPoint GetNearFloor(Vector3 pos)
	{
		Vector3 mousePos = camera.ScreenToWorldPoint(pos);
		Vector3 floorSize = Vector3.zero;
		if (myPoints[0, 0].floor != null)
			floorSize = myPoints[0, 0].floor.renderer.bounds.size;

		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				float dis = Vector2.Distance(new Vector2(mousePos.x, mousePos.y), new Vector2(myPoints[i, j].floor.transform.position.x, myPoints[i, j].floor.transform.position.y));
				if (dis < (floorSize.x / 2))
					return myPoints[i, j];
			}
		}

		return new FloorPoint();
	}
	void ChangeFloor()
	{
		if (firstFloor.floor == null || secondFloor.floor == null)
			return;
		if (!CheckTwoFloorDistance(firstFloor, secondFloor))
			return;

		if (Singleton.GetInstance().PlayMusic)
			audio.PlayOneShot(floorMove);

		myPoints[firstFloor.x, firstFloor.y].floor = secondFloor.floor;
		myPoints[firstFloor.x, firstFloor.y].floor.GetComponent<Floor>().GetMoveInfo(true, firstFloor.floor.transform.position.x, firstFloor.floor.transform.position.y);
		myPoints[firstFloor.x, firstFloor.y].isChanged =true;
		myPoints[secondFloor.x, secondFloor.y].floor = firstFloor.floor;
		myPoints[secondFloor.x, secondFloor.y].floor.GetComponent<Floor>().GetMoveInfo(true, secondFloor.floor.transform.position.x, secondFloor.floor.transform.position.y);
		myPoints[secondFloor.x, secondFloor.y].isChanged = true;

		selectFloor1 = myPoints[firstFloor.x, firstFloor.y];
		selectFloor2 = myPoints[secondFloor.x, secondFloor.y];

			//DestroyObject(myPoints[(int)removeList[i].x, (int)removeList[i].y].floor);
			/*
			myPoints[firstFloor.x, firstFloor.y].floor.GetComponent<Floor>().GetDisappearInfo(true);
			Vector2 curPos1 = new Vector2(firstFloor.x, firstFloor.y);
			if (myHero.GetComponent<Hero>().GetHeroPos() == curPos1)
			{
				Singleton.GetInstance().SetMyGameState(GameState.GameEnd);
				OverIsAppear = true;
			}
			int grade1 = myPoints[firstFloor.x, firstFloor.y].floor.GetComponent<Floor>().MyGrade;
			if ( grade1 != 0)
			{
				myHero.GetComponent<Hero>().SetHeroAward(grade1);
			}
			myHero.GetComponent<Hero>().SetMoveInfo(myPoints[firstFloor.x, firstFloor.y].floor.GetComponent<Floor>().MyColor);

			//DestroyObject(myPoints[(int)removeList[i].x, (int)removeList[i].y].floor);
			myPoints[secondFloor.x, secondFloor.y].floor.GetComponent<Floor>().GetDisappearInfo(true);
			Vector2 curPos2 = new Vector2(secondFloor.x, secondFloor.y);
			if (myHero.GetComponent<Hero>().GetHeroPos() == curPos2)
			{
				Singleton.GetInstance().SetMyGameState(GameState.GameEnd);
				OverIsAppear = true;
			}
			int grade2 = myPoints[secondFloor.x, secondFloor.y].floor.GetComponent<Floor>().MyGrade;
			if ( grade2 != 0)
			{
				myHero.GetComponent<Hero>().SetHeroAward(grade2);
			}
			myHero.GetComponent<Hero>().SetMoveInfo(myPoints[secondFloor.x, secondFloor.y].floor.GetComponent<Floor>().MyColor);
			*/

	}
	void ChangeFloor2 ()
	{
		if (selectFloor1.floor != null && selectFloor2.floor != null)
		{
			myPoints[selectFloor1.x, selectFloor1.y].floor = selectFloor2.floor;
			myPoints[selectFloor1.x, selectFloor1.y].floor.GetComponent<Floor>().GetMoveInfo(true, selectFloor1.floor.transform.position.x, selectFloor1.floor.transform.position.y);
			myPoints[selectFloor2.x, selectFloor2.y].floor = selectFloor1.floor;
			myPoints[selectFloor2.x, selectFloor2.y].floor.GetComponent<Floor>().GetMoveInfo(true, selectFloor2.floor.transform.position.x, selectFloor2.floor.transform.position.y);
		}
		selectFloor1.floor = null;
		selectFloor2.floor = null;
	}
	void RemoveListPush(Vector2 data)
	{
		listP++;
		if (listP < (myMapWidth * myMapHeight))
		{
			removeList[listP] = data;
			myPoints[(int)removeList[listP].x, (int)removeList[listP].y].isSign = true;
		}
	}
	void RemoveListPop()
	{
		if (listP >= 0)
		{
			myPoints[(int)removeList[listP].x, (int)removeList[listP].y].isSign = false;
			removeList[listP] = Vector2.zero;
		}
		listP--;
	}
	void ClearList()
	{
		for (int i = 0; i <= listP; i++)
		{
			removeList[listP] = Vector2.zero;
		}
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				myPoints[i, j].isSign = false;
				myPoints[i, j].isSign2 = 0;
				myPoints[i, j].group = 0;
				myPoints[i, j].isChanged = false;
			}
		}
		listP = -1;
	}
	void RemoveDelete(int index)
	{
		Vector2 data = Vector2.zero;

		if (index >= 0 && index <= listP)
		{
			data = removeList[index];
			myPoints[(int)removeList[listP].x, (int)removeList[listP].y].isSign = false;
			removeList[index] = Vector2.zero;
		}
		for (int i = index; i <= listP; i++)
		{
			removeList[i] = removeList[i+1];
		}
		listP--;
		for (int i = 0; i<= listP; i++)
		{
			if (removeList[i] == data)
			{
				for (int j = i; j <= listP; j++)
				{
					removeList[j] = removeList[j+1];
				}
				listP--;
			}
		}
	}
	void CheckAndRemove()
	{
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
				int haveSameColorCount = 0;
				
				if (myPoints[i, j].isSign)
					continue;
				
				//竖下方向
				int p = 0;
				while ((j - p) >= 0)
				{
					
					if (curCol == myPoints[i, (j - p)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[i, (j - p)].x, myPoints[i, (j - p)].y));
					}
					else
						break;
					p++;
				}
				//竖上方向
				p = 1;
				while((j + p) < myMapHeight)
				{
					if (curCol == myPoints[i, (j + p)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[i, (j + p)].x, myPoints[i, (j + p)].y));
					}
					else
						break;
					p++;
				}
				if (haveSameColorCount <= 2)
				{
					while (haveSameColorCount != 0)
					{
						RemoveListPop();
						haveSameColorCount--;
					}
				}
				haveSameColorCount = 0;
				//横左方向
				p = 0;
				while ((i - p) >= 0)
				{
					if (curCol == myPoints[(i - p), j].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[(i - p), j].x, myPoints[(i - p), j].y));
					}
					else
						break;
					p++;
				}
				//横右方向
				p = 1;
				while((i + p) < myMapWidth) 
				{
					if (curCol == myPoints[(i + p), j].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[(i + p), j].x, myPoints[(i + p), j].y));
					}
					else
						break;
					p++;
				}
				if (haveSameColorCount <= 2)
				{
					while (haveSameColorCount != 0)
					{
						RemoveListPop();
						haveSameColorCount--;
					}
				}
			}
		}
		/*
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
				int haveSameColorCount = 0;

				if (myPoints[i, j].isSign)
					continue;

				//竖下方向
				int p = 0;
				while ((j - p) >= 0)
				{

					if (curCol == myPoints[i, (j - p)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[i, (j - p)].x, myPoints[i, (j - p)].y));
					}
					else
						break;
					p++;
				}
				//竖上方向
				p = 1;
				while((j + p) < (myMapHeight - 1))
				{
					if (curCol == myPoints[i, (j + p)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[i, (j + p)].x, myPoints[i, (j + p)].y));
					}
					else
						break;
					p++;
				}
				if (haveSameColorCount <= 2)
				{
					while (haveSameColorCount != 0)
					{
						RemoveListPop();
						haveSameColorCount--;
					}
				}
				haveSameColorCount = 0;
				//横左方向
				p = 0;
				while ((i - p) >= 0)
				{
					if (curCol == myPoints[(i - p), j].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[(i - p), j].x, myPoints[(i - p), j].y));
					}
					else
						break;
					p++;
				}
				//横右方向
				p = 1;
				while((i + p) < (myMapWidth - 1))
				{
					if (curCol == myPoints[(i + p), j].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[(i + p), j].x, myPoints[(i + p), j].y));
					}
					else
						break;
					p++;
				}
				if (haveSameColorCount <= 2)
				{
					while (haveSameColorCount != 0)
					{
						RemoveListPop();
						haveSameColorCount--;
					}
				}
			}
		}

		for (int i = 0; i <= listP; i++)
		{
			DestroyObject(myPoints[(int)removeList[i].x, (int)removeList[i].y].floor);
		}*/

		CheckFloorGrade();
		//对于彩色砖块的特殊处理
		if (selectFloor1.floor != null && selectFloor2.floor != null)
		{
			if (selectFloor1.floor.GetComponent<Floor>().MyColor == "col" || selectFloor2.floor.GetComponent<Floor>().MyColor == "col")
			{
				RemoveListPush(new Vector2(selectFloor1.x, selectFloor1.y));
				RemoveListPush(new Vector2(selectFloor2.x, selectFloor2.y));
			}
		}
		if (listP == -1)
		{
			ChangeFloor2();
		}
		else 
		{
			if (Singleton.GetInstance().PlayMusic)
				this.audio.PlayOneShot(floorDis);
		}
		for (int i = 0; i <= listP; i++)
		{
			//DestroyObject(myPoints[(int)removeList[i].x, (int)removeList[i].y].floor);
			myPoints[(int)removeList[i].x, (int)removeList[i].y].floor.GetComponent<Floor>().GetDisappearInfo(true);
			Vector2 curPos = new Vector2((int)removeList[i].x, (int)removeList[i].y);
			//检测英雄
			if (myHero.GetComponent<Hero>().GetHeroPos() == curPos)
			{
				Singleton.GetInstance().SetMyGameState(GameState.GameEnd);
				GameFail();
				OverIsAppear = true;
				continue;
			}
			//如果撞块当中有数字加成
			int grade = myPoints[(int)removeList[i].x, (int)removeList[i].y].floor.GetComponent<Floor>().MyGrade;
			if ( grade != 0)
			{
				myHero.GetComponent<Hero>().SetHeroAward(grade);
			}
			//彩色砖块
			if (myPoints[(int)removeList[i].x, (int)removeList[i].y].floor.GetComponent<Floor>().MyColor != "col")
				myHero.GetComponent<Hero>().SetMoveInfo(myPoints[(int)removeList[i].x, (int)removeList[i].y].floor.GetComponent<Floor>().MyColor);
		}
		specialIsDone = false;
		Debug.Log("Check Floor Done -------");
	}
	void IintNewFloor()
	{
		GameObject anc = GameObject.FindGameObjectWithTag("Anchor");
		float offset = 1.6f;
		//
		/*
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				string str = GetRandomFloor(1.0f, 5.0f);
				if (myPoints[i, j].floor == null && Resources.Load(str) )
				{
					myPoints[i, j].floor = Instantiate(Resources.Load(str), new Vector3((anc.transform.position.x + offset * i), (anc.transform.position.y + offset * j), 0), Quaternion.identity) as GameObject;
					myPoints[i, j].floor.GetComponent<Floor>().MyColor = str;
					myPoints[i, j].isSign = false;
				}
			}
		}
		*/

		if (listP >= 0)
		{
			for (int i = 0; i <= listP; i++)
			{
				string str = GetRandomFloor(1.0f, 5.0f);
				if (Resources.Load(str) )
				{
					int newx = (int)removeList[i].x;
					int newy = (int)removeList[i].y;
					myPoints[newx, newy].floor = Instantiate(Resources.Load(str), new Vector3((anc.transform.position.x + offset * newx), (anc.transform.position.y + offset * newy), 0), Quaternion.identity) as GameObject;
					myPoints[newx, newy].floor.GetComponent<Floor>().MyColor = str;
					//myPoints[newx, newy].floor.GetComponent<Floor>().MyGrade = myPoints[newx, newy].isSign2;
					myPoints[newx, newy].isSign = false;
					myPoints[newx, newy].isSign2 = 0;
					myPoints[newx, newy].group = 0;
					myPoints[newx, newy].floor.GetComponent<Floor>().GetAppearInfo(true);

				}
			}
			ClearList();
			myHero.GetComponent<Hero>().Map = myPoints;
			Debug.Log("New Floor Init Done --------");
		}
	
		if (!MoveCheck())
		{
			ClearAllFloor();
			initMap();
		}
	}
	void bugtext()
	{
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
				int haveSameColorCount = 0;
				
				if (myPoints[i, j].isSign)
					continue;
				
				//竖下方向
				int p = 0;
				while ((j - p) >= 0)
				{
					
					if (curCol == myPoints[i, (j - p)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[i, (j - p)].x, myPoints[i, (j - p)].y));
					}
					else
						break;
					p++;
				}
				//竖上方向
				p = 1;
				while((j + p) < myMapHeight)
				{
					if (curCol == myPoints[i, (j + p)].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[i, (j + p)].x, myPoints[i, (j + p)].y));
					}
					else
						break;
					p++;
				}
				if (haveSameColorCount <= 2)
				{
					while (haveSameColorCount != 0)
					{
						RemoveListPop();
						haveSameColorCount--;
					}
				}
				haveSameColorCount = 0;
				//横左方向
				p = 0;
				while ((i - p) >= 0)
				{
					if (curCol == myPoints[(i - p), j].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[(i - p), j].x, myPoints[(i - p), j].y));
					}
					else
						break;
					p++;
				}
				//横右方向
				p = 1;
				while((i + p) < myMapWidth) 
				{
					if (curCol == myPoints[(i + p), j].floor.GetComponent<Floor>().MyColor)
					{
						haveSameColorCount++;
						RemoveListPush(new Vector2(myPoints[(i + p), j].x, myPoints[(i + p), j].y));
					}
					else
						break;
					p++;
				}
				if (haveSameColorCount <= 2)
				{
					while (haveSameColorCount != 0)
					{
						RemoveListPop();
						haveSameColorCount--;
					}
				}
			}
		}
		CheckFloorGrade();
		for (int i = 0; i <=listP; i++)
		{

			DestroyObject(myPoints[(int)removeList[i].x, (int)removeList[i].y].floor);
		}
	}
	bool CheckAllFloorAnimIsEnd ()
	{
		//检测所有对象运动完
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j =0; j < myMapHeight; j++)
			{
				if (myPoints[i, j].floor == null)
					continue;
				if (!myPoints[i, j].floor.GetComponent<Floor>().AnimIsEnd())
				{
					return false;
				}
			}
		}
		return true;
	}
	GameObject GetSelectPrefab(Vector3 pos)
	{
		GameObject obj = Resources.Load("select") as GameObject;
		if (obj != null)
		{
			obj = Instantiate(obj, pos, Quaternion.identity) as GameObject;
		}
		return obj;
	}
	GameObject GetPlusPrefab(int index, Vector3 pos)
	{
		GameObject obj = null;

		switch (index)
		{
		case 1:
			obj = Resources.Load("plus1") as GameObject;
			break;
		case 2:
			obj = Resources.Load("plus2") as GameObject;
			break;
		case 3:
			obj = Resources.Load("plus3") as GameObject;
			break;
		}

		if (obj != null)
		{
			obj = Instantiate(obj, pos, Quaternion.identity) as GameObject;
		}
		return obj;
	}
	void CheckFloorGrade()
	{
		int groupCount = 0;
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j = 0; j < myMapHeight; j++)
			{
				FloorPoint curFloor = myPoints[i, j];
				groupCount += 1;
				UpDir(i, j, curFloor);
				DownDir(i, j, curFloor);

				if (floorLineCount <= 1)
				{
					floorLineCount = 0;
					groupCount = 0;
				}

				LeftDir (i, j, curFloor);
				RightDir(i, j, curFloor);

				if (floorRowCount <= 1)
				{
					floorRowCount = 0;
					groupCount = 0;
				}

				switch (floorLineCount + floorRowCount)
				{
				case 3:
					myPoints[i, j].isSign2 = 4;
					break;
				case 4:
				{
					if (floorLineCount == 2 && floorRowCount == 2)
					{
						myPoints[i, j].isSign2 = 5;
					}
					else if (floorLineCount == 4 || floorRowCount == 4)
					{
						myPoints[i, j].isSign2 = 6;
					}
				}
					break;
				case 5:
					myPoints[i, j].isSign2 = 5;
					break;
				case 6:
					myPoints[i, j].isSign2 = 6;
					break;
				default:
					break;
				}
				myPoints[i, j].group =groupCount;
				floorLineCount = 0;
				floorRowCount = 0;
			}
		}


	}
	void UpDir(int x, int y, FloorPoint flo)
	{
		int mx = x;
		int my = y + 1;
		FloorPoint curFlo = flo;

		if (my < myMapHeight)
		{
			if (curFlo.floor.GetComponent<Floor>().MyColor == myPoints[mx, my].floor.GetComponent<Floor>().MyColor)
			{
				floorLineCount++;
				//myPoints[mx, mx].isSign2 = true;
				UpDir(mx, my, flo);
			}
			else 
				return;
		}
	}
	void DownDir(int x, int y, FloorPoint flo)
	{
		int mx = x;
		int my = y - 1;
		FloorPoint curFlo = flo;
		
		if (my >= 0)
		{
			if (curFlo.floor.GetComponent<Floor>().MyColor == myPoints[mx, my].floor.GetComponent<Floor>().MyColor)
			{
				floorLineCount++;
				//myPoints[mx, mx].isSign2 = true;
				DownDir(mx, my, flo);
			}
			else 
				return;
		}
	}
	void LeftDir(int x, int y, FloorPoint flo)
	{
		int mx = x - 1;
		int my = y;
		FloorPoint curFlo = flo;
		
		if (mx >= 0)
		{
			if (curFlo.floor.GetComponent<Floor>().MyColor == myPoints[mx, my].floor.GetComponent<Floor>().MyColor)
			{
				floorRowCount++;
				//myPoints[mx, mx].isSign2 = true;
				LeftDir(mx, my, flo);
			}
			else 
				return;
		}
	}
	void RightDir(int x, int y, FloorPoint flo)
	{
		int mx = x + 1;
		int my = y;
		FloorPoint curFlo = flo;
		
		if (mx < myMapWidth)
		{
			if (curFlo.floor.GetComponent<Floor>().MyColor == myPoints[mx, my].floor.GetComponent<Floor>().MyColor)
			{
				floorRowCount++;
				//myPoints[mx, mx].isSign2 = true;
				RightDir(mx, my, flo);
			}
			else 
				return;
		}
	}
	void InitSpecialFloor()
	{
		GameObject anc = GameObject.FindGameObjectWithTag("Anchor");
		float offset = 1.6f;

		List<int>groups = new List<int>();

		if (listP >= 0)
		{	
			for (int i = 0; i <= listP; i++)
			{
				string str = GetRandomFloor(1.0f, 5.0f);
				int newx = (int)removeList[i].x;
				int newy = (int)removeList[i].y;
				if (myPoints[newx, newy].isChanged && myPoints[newx, newy].isSign2 > 3)
				{
					groups.Add(myPoints[newx, newy].group);
					/*
					if (myPoints[newx, newy].isChanged && myPoints[newx, newy].isSign2 == 6)
					{
						myPoints[newx, newy].floor = Instantiate(Resources.Load("col"), new Vector3((anc.transform.position.x + offset * newx), (anc.transform.position.y + offset * newy), 0), Quaternion.identity) as GameObject;
					}
					else 
					{
						myPoints[newx, newy].floor = Instantiate(Resources.Load(str), new Vector3((anc.transform.position.x + offset * newx), (anc.transform.position.y + offset * newy), 0), Quaternion.identity) as GameObject;
					}*/

					myPoints[newx, newy].floor = Instantiate(Resources.Load(str), new Vector3((anc.transform.position.x + offset * newx), (anc.transform.position.y + offset * newy), 0), Quaternion.identity) as GameObject;
					myPoints[newx, newy].floor.GetComponent<Floor>().MyColor = str;
					myPoints[newx, newy].floor.GetComponent<Floor>().MyGrade = myPoints[newx, newy].isSign2;
					myPoints[newx, newy].isSign = false;
					myPoints[newx, newy].isSign2 = 0;
					myPoints[newx, newy].group = 0;
					myPoints[newx, newy].floor.GetComponent<Floor>().GetAppearInfo(true);
					RemoveDelete(i);

				}
			}

			for (int i = 0; i <= listP; i++)
			{
				string str = GetRandomFloor(1.0f, 5.0f);

				if (Resources.Load(str))
				{
					int newx = (int)removeList[i].x;
					int newy = (int)removeList[i].y;
					if (myPoints[newx, newy].isSign2 > 3)
					{
						if (!groups.Contains(myPoints[newx, newy].group))
						{
							groups.Add(myPoints[newx, newy].group);
							myPoints[newx, newy].floor = Instantiate(Resources.Load(str), new Vector3((anc.transform.position.x + offset * newx), (anc.transform.position.y + offset * newy), 0), Quaternion.identity) as GameObject;
							myPoints[newx, newy].floor.GetComponent<Floor>().MyColor = str;
							myPoints[newx, newy].floor.GetComponent<Floor>().MyGrade = myPoints[newx, newy].isSign2;
							myPoints[newx, newy].isSign = false;
							myPoints[newx, newy].isSign2 = 0;
							myPoints[newx, newy].group = 0;
							myPoints[newx, newy].floor.GetComponent<Floor>().GetAppearInfo(true);
							RemoveDelete(i);
						}
	
					}
				}
			}
			specialIsDone = true;
			Debug.Log("Special Floor Done ------");
		}
	}
	GameObject GetHeroObj()
	{
		GameObject anc = GameObject.FindGameObjectWithTag("Anchor");
		float offset = 1.6f;

		GameObject obj = Resources.Load("hero") as GameObject;
		if (obj != null)
		{
			float x =  anc.transform.position.x + offset * (myMapWidth - 1);
			float y = anc.transform.position.y + offset * (myMapHeight - 1);
			obj = Instantiate(obj, new Vector3(x, y, -2.0f), Quaternion.identity) as GameObject;
			Debug.Log ("Hero Loding Successful ----");
			return obj;
		}
		Debug.Log ("Hero Loding Fail ----");
		return null;
	}
	bool MyHeroMoveEnd()
	{
		return myHero.GetComponent<Hero>().MoveEnd;
	}
	void CheckGameWin()
	{
		//Vector2 v = myHero.GetComponent<Hero>().GetHeroPos();
		Vector2 v1 = new Vector2(myHero.transform.position.x, myHero.transform.position.y);
		Vector2 v2 = new Vector2(myPoints[myMapWidth-1, 0 ].floor.transform.position.x, myPoints[0, 0].floor.transform.position.y);
		if (Vector2.Distance(v1, v2) <= 0.05f)
		{
			Singleton.GetInstance().SetMyGameState(GameState.GameWin);
			GameSuccessful();
			WinIsAppear = true;
		}
	}
	void ClearAllFloor()
	{
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j = 0; j < myMapHeight; j++)
			{
				Destroy(myPoints[i, j].floor);
				myPoints[i, j].group = 0;
				myPoints[i, j].isChanged = false;
				myPoints[i, j].isSign = false;
				myPoints[i, j].isSign2 = 0;
				myPoints[i, j].x = 0;
				myPoints[i, j].y = 0;
			}
		}
	}
	bool MoveCheck()
	{
		for (int i = 0; i < myMapWidth; i++)
		{
			for (int j = 0; j < myMapHeight; j++)
			{
				//上
				if ((j + 2) < myMapHeight )
				{
					string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
					string nextCol = myPoints[i, (j+1)].floor.GetComponent<Floor>().MyColor;
					if (curCol == nextCol)
					{
						if ((j+3) < myMapHeight)
						{
							nextCol = myPoints[i, (j+3)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((i -1) >= 0)
						{
							nextCol = myPoints[(i -1), (j+2)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((i+1) < myMapWidth)
						{
							nextCol = myPoints[(i +1), (j+2)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
					}
				}
				//下
				if ((j - 2) >= 0 )
				{
					string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
					string nextCol = myPoints[i, (j-1)].floor.GetComponent<Floor>().MyColor;
					if (curCol == nextCol)
					{
						if ((j - 3) >= 0)
						{
							nextCol = myPoints[i, (j - 3)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((i -1) >= 0)
						{
							nextCol = myPoints[(i -1), (j-2)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((i+1) < myMapWidth)
						{
							nextCol = myPoints[(i +1), (j-2)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
					}
				}
				//左
				if ((i - 2) >= 0)
				{
					string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
					string nextCol = myPoints[(i - 1) , j].floor.GetComponent<Floor>().MyColor;
					if (curCol == nextCol)
					{
						if ((i - 3) >= 0)
						{
							nextCol = myPoints[(i - 3), j].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((j - 1) >= 0)
						{
							nextCol = myPoints[(i - 2), (j - 1)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((j+1) < myMapHeight)
						{
							nextCol = myPoints[(i - 2), (j + 1)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
					}
				}
				//右
				if ((i + 2) < myMapWidth)
				{
					string curCol = myPoints[i, j].floor.GetComponent<Floor>().MyColor;
					string nextCol = myPoints[(i + 1) , j].floor.GetComponent<Floor>().MyColor;
					if (curCol == nextCol)
					{
						if ((i + 3) < myMapWidth)
						{
							nextCol = myPoints[(i + 3), j].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((j - 1) >= 0)
						{
							nextCol = myPoints[(i + 2), (j - 1)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
						if ((j + 1) < myMapHeight)
						{
							nextCol = myPoints[(i + 2), (j + 1)].floor.GetComponent<Floor>().MyColor;
							if (curCol == nextCol)
								return true;
						}
					}
				}
			}
		}
		return false;
	}
	bool CheckTwoFloorDistance(FloorPoint f1, FloorPoint f2)
	{
		if (f1.x == f2.x && Mathf.Abs(f1.y - f2.y) == 1)
			return true;
		else if (f1.y == f2.y && Mathf.Abs(f1.x - f2.x) == 1)
			return true;

		return false;
	}
	void GameSuccessful()
	{
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
		cam.GetComponent<GameingUI>().GameSuccessful(true);
		string str = "Level" + (currentLevel + 1);
		if (PlayerPrefs.HasKey(str))
			PlayerPrefs.SetInt(str, 1);

		Debug.Log(str);
	}
	void GameFail()
	{
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
		cam.GetComponent<GameingUI>().GameLose(true);
	}
	public void ReStart()
	{
		ClearAllFloor();
		if (myHero != null)
			Destroy(myHero);
		if (targetFloor != null)
			Destroy(targetFloor);

		currentLevel = Singleton.GetInstance().GameLevel;
		firstFloor.floor = null;
		secondFloor.floor = null;
		isSelected = false;
		myMapWidth = 6;
		myMapHeight = 6;
		myHero = GetHeroObj();
		initMap(myMapWidth, myMapHeight);
		removeList = new Vector2[myMapWidth * myMapHeight];
		listP = -1;
		jianchewan = false;
		selectObj= null;
		selectFloor.floor = null;
		floorGrade = 0;
		specialIsDone = false;
		GameIsOver = false;
		OverIsAppear = false;
		GameIsWin = false;
		WinIsAppear = false;
		GameIsPause = false;
		PauseIsAppear = false;
	}
}

