using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour 
{
	public bool DontDestroyOnLoad = true;
	public static Global Instance = null;
	void Awake()
	{
		if(Instance != null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}
		Instance = this;
		if(this.DontDestroyOnLoad)
			GameObject.DontDestroyOnLoad(this);
	}
}
