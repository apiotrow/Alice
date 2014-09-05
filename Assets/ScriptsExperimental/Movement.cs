using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	MathFunctions MF;
	
	public float Speed = 10;
	public Transform Planet;
	public Transform LocalOrientation;
	

	
	// I need to learn how to implement it
	public AnimationCurve slopeSpeedMultiplier = new AnimationCurve (new Keyframe(-90, 1), new Keyframe(0, 1), new Keyframe(90, 0));
	
	// character controller from now on
	CharacterController Controller;
	public Transform Platform;
	
	
	public float PlanetDistance;
	public float LastPlanetDistance;
	Vector3 LookAtPlanet = Vector3.zero;
	
	// Done something in the work
	public float LastHitTime = 0;
	//  public float Distance = 0;
	public bool Grounded = false;
	public Vector3 LastSlopeTestPosition = Vector3.zero;
	public Vector3 LastGroundTestPosition;
	//  public Vector3 Velocity = Vector3.zero;
	public Vector3 MovementVelocity = Vector3.zero;
	public float MovingStartTime = 0;
	public float MovingStartStopTime = 0;
	public float MovingTime = 0;
	//  public float MovingToStopTime = 0;
	public Vector3 TempInputVelocity = Vector3.zero;
	public Vector3 JumpVelocity = Vector3.zero;
	public Vector3 GravityVelocity = Vector3.zero;
	public Vector3 Gravity = Vector3.zero;
	public float GravityStrength = 20;
	public float JumpStartDistance = 0;
	public bool StillJumping = false;
	public bool StillInMovement = false;
	public bool StoppingMovement = false;
	public float JumpStrength = 10;
	public float MovementSpeed = 1;
	public float AccelerationTime = 1;
	public float JumpHeight = 20;
	
	public bool AccelerationMovement = false;
	
	void Start(){
		Controller = GetComponent("CharacterController") as CharacterController;
		// so we don't end up teleporting at x0y0z0 if something unexpected happenes
		LastSlopeTestPosition = transform.position;
	}
	
	
	// why fixed update? had something in my mind that I need it forgot it why, ... seems to be working well with normal update too less calculations to be made with it
	// if I'm wrong something might not be sinhronized, ... don't know, ...
	void Update() {
		// Align to planet
		// only south pole doesn't seem to be working
		transform.rotation = Quaternion.FromToRotation (Vector3.up, -(Planet.position - transform.position) );
		LookAtPlanet = -transform.up;
		
		if (Grounded){
			// 1/2 Grounded false if not colliding
			IsStillGrounded();
		}
		// we check if we are still grounded.
		if (Grounded){
			InputMoveMotion();
			// inside 2/2 Grounded false if press a key
			InputJumpMotion();
		}
		// if we jump we are still not using gravity
		else if (! StillJumping){
			GravityMotion();
		}
		if (StillJumping){
			ContinueJumpMotion();
		}
		MoveChar();
	}
	void IsStillGrounded (){
		// we are not moving OR we are in air
		if (LastHitTime < (Time.time - Time.deltaTime * 2) ){
			// we are moving in mid air
			if (LastGroundTestPosition != transform.position){
				Grounded = false;
			}
			//else {we aren't in motion}
		}
		LastGroundTestPosition = transform.position;
	}
	
	public void InputMoveMotion(){
		Vector3 ImputVelocity = Vector3.zero;
		//      if (/*AI*/){
		//          ImputVelocity +=    /*all the moves*/
		//          ImputVelovity.Normalize();
		//      }
		//      else {
		if (Input.GetKey(KeyCode.UpArrow)){
			ImputVelocity += ( LocalOrientation.transform.forward);
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			ImputVelocity += ( -LocalOrientation.transform.forward);
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)){
			ImputVelocity += ( -LocalOrientation.transform.right);
		}
		if (Input.GetKey(KeyCode.RightArrow)){
			ImputVelocity += ( LocalOrientation.transform.right);
		}
		// direction length to 1 unit
		ContinueMoveMotion(ImputVelocity.normalized);
		//      }
	}
	void ContinueMoveMotion(Vector3 InputVelocity){
		if (! AccelerationMovement){
			MovementVelocity = InputVelocity;
			return ;
		}
		
		
		// seems to be working more tests need to be done, ...
		
		if (InputVelocity != Vector3.zero){
			TempInputVelocity = InputVelocity;
			// we didn't ended the stop
			if (StoppingMovement){
				StoppingMovement = false;
				MovingStartTime = Time.time - MovingTime;
			}
			// we are just starting to move
			else if (LastGroundTestPosition == transform.position  != StillInMovement){
				MovingStartTime = Time.time;
			}
			StillInMovement = true;
			if (AccelerationTime > (Time.time - MovingStartTime)){
				MovingTime = (Time.time - MovingStartTime);
			}
			else {
				MovingTime = AccelerationTime;
			}
			// where we will move
			MovementVelocity = InputVelocity * MovingTime;
		}else {
			if (StillInMovement != StoppingMovement){
				StoppingMovement = true;
				MovingStartStopTime = Time.time + MovingTime;
			}
			// I don't need this but so it doesn't calculate all the time but just veryfy the bool
			if (StoppingMovement){
				MovingTime = MovingStartStopTime - Time.time;
			}
			// if we are inside stopping time
			if (MovingTime >= 0){
				MovementVelocity = TempInputVelocity * MovingTime;
			}
			// if we stopped totally
			else {
				MovementVelocity = Vector3.zero;
				StillInMovement = false;
				StoppingMovement = false;
			}
		}
		
	}
	void InputJumpMotion(){
		// 2/2 if we jump we know we aren't grounded
		if (Input.GetMouseButton(2)){
			JumpVelocity = transform.up * JumpStrength;
			Grounded = false;
			StillJumping = true;
		}
		JumpStartDistance = MF.Distance(transform.position, Planet.position);
	}
	void ContinueJumpMotion(){
		PlanetDistance = MF.Distance(transform.position, Planet.position);
		// if we stop pressing jump we stop getting in air
		if (Input.GetMouseButtonUp(2)){
			StillJumping = false;
			JumpVelocity = Vector3.zero;
		}
		// if we reached our jump height we stop getting more in height
		else if (JumpStartDistance + JumpHeight < PlanetDistance){
			StillJumping = false;
			JumpVelocity = Vector3.zero;
		}
		else if (StillJumping){
			JumpVelocity -= (-transform.up * 0.3f * Time.deltaTime);
		}
	}
	void GravityMotion (){
		GravityVelocity = MF.Direction(transform.position, Planet.position).normalized * GravityStrength;
	}
	void MoveChar(){
		if (StillJumping){
			GravityVelocity = Vector3.zero;
		}
		if (Grounded){
			Controller.Move( (MovementVelocity * MovementSpeed) * Time.deltaTime);
		}
		else {
			Controller.Move(  ( (MovementVelocity * MovementSpeed) + JumpVelocity + GravityVelocity) * Time.deltaTime);
		}
	}
	void OnControllerColliderHit (ControllerColliderHit hit){
		// the only indicator we are grounded
		// if it's less than 45° we are grounded
		float CurrentSlope = (Vector3.Angle(LookAtPlanet, hit.normal) -180 )*-1;
		if (Controller.slopeLimit > CurrentSlope){
			Grounded = true;
			LastHitTime = Time.time;
			LastSlopeTestPosition = transform.position;
		}
		// if we are still grounded means we are on 45° + slope
		else if (Grounded && Controller.slopeLimit < CurrentSlope){
			// we need to teleport back to slope below 45°
			transform.position = LastSlopeTestPosition;
		}
		
		/****** START DEBUG Hit COMPONENTS ******
        // world point
//      Debug.Log("Point: " + hit.point);
        // oposite of direction
//      Debug.Log("Normal: " + hit.normal); // World Direction
        // direction witch was forced back
//      Debug.Log("MoveDirection: " + hit.moveDirection); // Local Direction  something strange unexplained about it
        // more length that's stopped bigger number it'll return
//      Debug.Log("moveLength: " + hit.moveLength);
//      Platform = hit.transform;
        /****** END DEBUG Hit COMPONENTS ******/
	}
}