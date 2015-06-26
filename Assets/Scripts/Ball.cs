using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	public PlayerController currentPlayer;
	public static Ball instance;
	// Use this for initialization
	void Start ()
	{
		instance = this;
	}

	// Update is called once per frame
	void Update ()
	{
		if(BolHelper.Distance(currentPlayer.gameObject,this.gameObject)<0.3f){
			this.gameObject.rigidbody.AddForce((currentPlayer.moveTarget - this.gameObject.transform.position).normalized * 15);
		}
	}

	public void Shoot(){
		//this.gameObject.rigidbody.AddRelativeForce((currentPlayer.moveTarget - this.gameObject.transform.position).normalized * currentPlayer.shootPower);
		this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(currentPlayer.transform.rotation.eulerAngles.x,currentPlayer.transform.rotation.eulerAngles.y,currentPlayer.transform.rotation.eulerAngles.z));
		this.gameObject.rigidbody.AddRelativeForce (Vector3.forward * currentPlayer.shootPower, ForceMode.Impulse); //NOW THE BALL IS FACING THE DIRECTION OF SWIPE, ADD FORWARD FORCE SIMPLY
		this.gameObject.rigidbody.AddRelativeForce (Vector3.up * currentPlayer.shootPower*0.5f, ForceMode.Impulse);
	}

	public void Pass(){
		this.gameObject.rigidbody.AddRelativeForce (BolHelper.CalculateBestThrowSpeed(this.gameObject.transform.position,currentPlayer.passTarget,2) * currentPlayer.passPower, ForceMode.Impulse); //NOW THE BALL IS FACING THE DIRECTION OF SWIPE, ADD FORWARD FORCE SIMPLY

	}


	public void ResetVelocity(){
		gameObject.rigidbody.isKinematic = true;
		Debug.Log("reset");
		//gameObject.tag ="OffBall";
		//gameObject.rigidbody.angularVelocity = Vector3.zero;
		//gameObject.rigidbody.velocity = Vector3.zero;
		gameObject.rigidbody.isKinematic = false;
	}

}

