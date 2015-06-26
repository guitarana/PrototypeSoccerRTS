using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public string id; 

	public float speed = 2f;
	public bool isMove;
	public Vector3 moveTarget;
	public NavMeshAgent agent;
	public float distance;
	public SpriteRenderer cursor;


	public bool freeze;
	public bool disableInput;

	public GameObject currentObject;
	private RaycastHit rayHit;
	private Animation _animation;
	private float timer;
	public bool haveBall;
	public bool activePlayer;

	//shoot properties
	public float maxShootPower = 5;
	public float shootPower= 0;
	public float shootTimer=0;

	//pass properties
	public float maxPassPower = 5;
	public float passPower = 20;
	public float passTimer = 0;
	public bool tooglePass;
	public Vector3 passTarget;

	public enum PlayerState{
		Idle,
		Walk,
		Shoot,
		Pass,
		None
	}

	public PlayerState playerState = PlayerState.Idle;

	public enum SubState{
		Init,
		Active,
		Deactive,
		Finish
	}

	public SubState subState = SubState.Init;

	// Use this for initialization
	void Start () {
		_animation = gameObject.GetComponent<Animation>();
		agent = gameObject.GetComponent<NavMeshAgent> ();
		GetComponent<Animation>() ["Run"].speed = 1;
		cursor.gameObject.SetActive(false);
	}

	public void Reset(){
		cursor.gameObject.SetActive(false);
		freeze = false;
		isMove = false;
		disableInput = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKeyDown (KeyCode.Escape))
				Application.Quit (); 
		}

		if(Ball.instance.currentPlayer){
			if(Ball.instance.currentPlayer != this){
				haveBall = false;
				tooglePass = false;
			}else{
				GameManager.instance.cursorAimTarget.SetActive(tooglePass);

			}
		}


		UpdateAnimation ();
		UpdateState();
		if (disableInput)return;
		UpdateInput ();

	}

	void UpdateInput(){
		if(!activePlayer) return;

		if (Application.platform == RuntimePlatform.Android){
			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit(); 
			
			int i = 0;
			while (i < Input.touchCount) {
				if (Input.GetTouch(i).phase == TouchPhase.Began) {
					Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
					RaycastHit _rayHit;
					
					if(Physics.Raycast (ray, out _rayHit) && (_rayHit.collider.tag == "Ground" || _rayHit.collider.tag == "Object")){
						rayHit = _rayHit;
						playerState = PlayerState.Walk;
						subState = SubState.Init;
					}
				}
				++i;
			}
		}else{
			if (Input.GetMouseButtonDown (1)) {
				Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit _rayHit;
				if(Physics.Raycast (ray, out _rayHit) && (_rayHit.collider.tag == "Ground" || _rayHit.collider.tag == "Object")){

					rayHit = _rayHit;
					playerState = PlayerState.Walk;
					subState = SubState.Init;
				}	
			}

			if(tooglePass){
				Ray ray2 = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit _rayHit2;
				if(Physics.Raycast (ray2, out _rayHit2) && (_rayHit2.collider.tag == "Ground")){
					passTarget = _rayHit2.point;
					GameManager.instance.cursorAimTarget.transform.position = passTarget;
				}	


			}
		
			if (Input.GetMouseButtonDown (0)) {
				if(tooglePass){
					playerState = PlayerState.Pass;
					subState = SubState.Init;
				}
			}

			if(haveBall){
				if(shootTimer >0.5f || Input.GetKeyUp(KeyCode.Space)){
					shootTimer = 0;
					playerState = PlayerState.Shoot;
					subState = SubState.Init;
					return;
				}
				
				if(Input.GetKey(KeyCode.Space)){
					shootTimer += Time.deltaTime;
					shootPower += maxShootPower/0.5f *Time.deltaTime;
					
				}
				
				if(Input.GetKeyDown(KeyCode.A)){
					tooglePass = !tooglePass;
					
				}
				
			}

		}
		

	}

	void DoIdle(){
		if(subState == SubState.Init){

		}
		if(subState == SubState.Active){

		}
		if(subState == SubState.Deactive){
			
		}
		if(subState == SubState.Finish){
			
		}
	}

	void DoWalk(){
		if(subState == SubState.Init){
			subState = SubState.Active;
			shootPower = 3;
		}
		if(subState == SubState.Active){
			Debug.Log("HIT " + rayHit.collider.tag);
			isMove = true;
			moveTarget = rayHit.point + new Vector3 ( 0.0f, 0.0f, 0.0f);
			if(rayHit.collider.tag == "Object"){

				Debug.Log("HIT OBJECT" + rayHit.collider.gameObject.name);
				currentObject = rayHit.collider.gameObject;
				playerState = PlayerState.Shoot;
				subState = SubState.Init;

			}else{
				cursor.gameObject.SetActive(true);
				cursor.gameObject.transform.position = moveTarget + new Vector3(0,0.1f,0) ;
			}

			if (isMove) {
				
				//transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(moveTarget), Time.deltaTime * 0.1f);

				if(haveBall){
					agent.speed = speed*1.5f;
					agent.destination = Ball.instance.gameObject.transform.position;
				}else{
					if(BolHelper.Distance(this.gameObject,Ball.instance.gameObject)<0.5f){
						haveBall = true;
						Ball.instance.currentPlayer = this;
					}
					agent.speed = speed;
					agent.destination = moveTarget;
				}
				distance = Vector3.Distance(transform.position,moveTarget);
				//if(agent.remainingDistance < 0.01f){
				if(distance < 0.1f){
					cursor.gameObject.SetActive(false);
					isMove = false;
					playerState = PlayerState.Idle;
					subState = SubState.Init;
				}



			}
			
			


		}
		if(subState == SubState.Deactive){
			
		}
		if(subState == SubState.Finish){
			
		}
	}

	void DoShoot(){

		if(subState == SubState.Init){
			cursor.gameObject.SetActive(false);
			subState = SubState.Active;
		}

		if(subState == SubState.Active){
			if(haveBall){
				Ball.instance.Shoot();
				shootPower = 0;
				haveBall = false;
				subState = SubState.Deactive;
			}
		}

		if(subState == SubState.Deactive){
			timer +=Time.deltaTime;
			if(timer>1){
				timer = 0;
				subState = SubState.Finish;

			}
		}

		if(subState == SubState.Finish){
//			_animation.RemoveClip("action");
			playerState = PlayerState.Idle;
			subState = SubState.Init;
		}
	}

	void DoPass(){
		
		if(subState == SubState.Init){
			cursor.gameObject.SetActive(false);
			subState = SubState.Active;
		}
		
		if(subState == SubState.Active){
			if(haveBall){
				Ball.instance.Pass();
				shootPower = 0;
				haveBall = false;
				tooglePass = false;
				subState = SubState.Deactive;
			}
		}
		
		if(subState == SubState.Deactive){
			timer +=Time.deltaTime;
			if(timer>1){
				timer = 0;
				subState = SubState.Finish;
				
			}
		}
		
		if(subState == SubState.Finish){
			//			_animation.RemoveClip("action");
			playerState = PlayerState.Idle;
			subState = SubState.Init;
		}
	}

	void UpdateAnimation(){
		if (playerState == PlayerState.Walk || (playerState == PlayerState.Shoot && subState == SubState.Active)){
			_animation.CrossFade ("Run");	
		} else if(playerState == PlayerState.Shoot && subState == SubState.Deactive) {
//			_animation.CrossFade ("action");	
		} else {
			_animation.CrossFade ("Idle");		

		}

		if (freeze) {
			_animation.CrossFade("Idle");
			playerState = PlayerState.Idle;
			subState = SubState.Init;
			agent.Stop();
			return;
		}
	}

	void UpdateState(){
		switch(playerState){
		case PlayerState.Idle:
			DoIdle();
			break;
		case PlayerState.Walk:
			DoWalk();
			break;
		case PlayerState.Shoot:
			DoShoot();
			break;
		case PlayerState.Pass:
			DoPass();
			break;
		}
	}
}
