using UnityEngine;
using System.Collections;

public enum GameState
{
	GameStart,
	GameChoose,
	GameRun,
	GamePause,
    GameEnd,
	GameWin,
	GameClose,
	GameOption
}

public class Singleton
{
	private static Singleton instance = null;
	GameState myGameState = GameState.GameStart;
	int gameLevel;
	bool hasSound;
	bool playMusic = true;
	bool soundOpen;
	bool musicOpen;

	public int GameLevel
	{
		set {gameLevel = value;}
		get {return gameLevel; }
	}
	public bool HasSound
	{
		set {hasSound = value;}
		get {return hasSound;}
	}
	public bool PlayMusic
	{
		set {playMusic = value;}
		get {return playMusic;}
	}
	public bool SoundOpen
	{
		set {soundOpen = value;}
		get {return soundOpen;}
	}
	public bool MusicOpen
	{
		set {musicOpen = value;}
		get {return musicOpen;}
	}
	public static Singleton GetInstance()
	{
		if (null == instance)
		{
			instance = new Singleton();
		}
		return instance;
	}

	public void SetMyGameState(GameState state)
	{
		myGameState = state;
	}
	public GameState GetMyGameState()
	{
		return myGameState;
	}
}


