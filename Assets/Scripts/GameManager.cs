using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public List<PlayerManager> playerManagers;
	public PlayerController player1;
	public GameObject cursorActivePlayer;
	public static GameManager instance;
	public GameObject cursorAimTarget;
	//public PlayerController player2;

	// Use this for initialization
	void Start ()
	{
		instance = this;
		playerManagers = new List<PlayerManager>();
		GameObject goPM1 = new GameObject("@PM1", typeof(PlayerManager));
		PlayerManager pm1 = goPM1.GetComponent<PlayerManager>();
		//PlayerManager pm2 = new PlayerManager();
		pm1.id = "1";
		pm1.player = player1;
		//pm2.id = 2;
		playerManagers.Add (pm1);
		//playerManagers.Add (pm2);
	}

	// Update is called once per frame
	void Update ()
	{
	}
}

