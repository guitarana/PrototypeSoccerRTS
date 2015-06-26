using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
	List<PlayerController> players;
	public string id; 
	public PlayerController player;
	public PlayerController activePlayer;
	private RaycastHit rayHit;

	// Use this for initialization
	void Start ()
	{
		players = new List<PlayerController>();

		for(int i =0;i<11;i++){
			GameObject goPC = Instantiate(player.gameObject, new Vector3(i, 0, 0), Quaternion.identity) as GameObject;
			PlayerController pc = goPC.GetComponent<PlayerController>();
			pc.id = id;
			players.Add(pc);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit _rayHit;
			if(Physics.Raycast (ray, out _rayHit) && (_rayHit.collider.tag == "Player" || _rayHit.collider.tag == "Object")){
				if(activePlayer)
					activePlayer.activePlayer = false;
				rayHit = _rayHit;
				activePlayer = rayHit.collider.gameObject.GetComponent<PlayerController>();
				activePlayer.activePlayer = true;
				GameManager.instance.cursorActivePlayer.transform.position = activePlayer.gameObject.transform.position+ new Vector3(0,0.1f,0) ;
				
			}	
		}

		if(activePlayer)
			GameManager.instance.cursorActivePlayer.transform.position = activePlayer.gameObject.transform.position+ new Vector3(0,0.1f,0) ;
	}
}

