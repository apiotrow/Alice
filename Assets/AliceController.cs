using UnityEngine;
using System.Collections;

public class AliceController : MonoBehaviour
{
	public MouseOrbitImproved cam;
	public ThirdPersonControllerCS playercont;
	float lerpTime;

	public float camDistChangeMultiplier;

	float scaleChange;
	float newScale;
	float currentScale;

	float oldCamDistance;
	float newCamDistance;

	float minScale;
	float maxScale;

	float walkSpeedMultiplier;
	float newWalkSpeed;

	float jumpHeightMultiplier;
	float newJumpHeight;

	float inAirControlAccelerationMultiplier;
	float newinAirControlAcceleration;

	float gravityMultiplier;
	float newGravity;

	float fogEndDistanceMultiplier;


	void Start ()
	{
		cam = Camera.main.GetComponent<MouseOrbitImproved> ();
		playercont = gameObject.GetComponent<ThirdPersonControllerCS> ();
		lerpTime = 20f;

		camDistChangeMultiplier = 5f;

		maxScale = 200f;
		minScale = 1f;

		scaleChange = 0f;
		newScale = 1f;
		currentScale = 1f;

		walkSpeedMultiplier = 2f;
		jumpHeightMultiplier = 1f;
		inAirControlAccelerationMultiplier = 20f;
		fogEndDistanceMultiplier = 10f;
		gravityMultiplier = 10f;

		newScale = currentScale;
		newWalkSpeed = currentScale * walkSpeedMultiplier;
		newJumpHeight = currentScale * jumpHeightMultiplier;
		newinAirControlAcceleration = currentScale * inAirControlAccelerationMultiplier;
		newGravity = currentScale * gravityMultiplier;

		transform.localScale = new Vector3 (minScale, minScale, minScale);
		
		cam.distance = minScale * camDistChangeMultiplier;
		oldCamDistance = cam.distance;
		newCamDistance = cam.distance;
		
		playercont.walkSpeed = minScale * walkSpeedMultiplier;
		playercont.jumpHeight = minScale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = minScale * inAirControlAccelerationMultiplier;
		playercont.gravity = minScale * gravityMultiplier;

		RenderSettings.fog = false;
		RenderSettings.fogEndDistance = minScale * 20f;
	}

	void Update ()
	{
		handleSizeChange();

		if (transform.localScale.x >= maxScale) {
			clampValues(maxScale);
		}
		if (transform.localScale.x <= minScale) {
			clampValues(minScale);
		}
	}

	void clampValues(float scale){
		transform.localScale = new Vector3 (scale, scale, scale);

		cam.distance = Mathf.MoveTowards (cam.distance, scale * camDistChangeMultiplier,  Time.deltaTime * lerpTime);

		playercont.walkSpeed = scale * walkSpeedMultiplier;
		playercont.jumpHeight = scale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = scale * inAirControlAccelerationMultiplier;
		playercont.gravity = scale * gravityMultiplier;

		RenderSettings.fogEndDistance = scale * 20f;
	}

	void directScaleChange(float sc){
		newCamDistance = (sc * camDistChangeMultiplier);
		
		currentScale = sc;
		newWalkSpeed = sc * walkSpeedMultiplier;
		newJumpHeight = sc * jumpHeightMultiplier;
		newinAirControlAcceleration = sc * inAirControlAccelerationMultiplier;
		newGravity = sc * gravityMultiplier;
	}

	void handleSizeChange(){
		if(Input.GetKey(KeyCode.Alpha1)){
			newScale = 1f;
			directScaleChange(newScale);
		}
		if(Input.GetKey(KeyCode.Alpha2)){
			newScale = 5f;
			directScaleChange(newScale);
		}
		if(Input.GetKey(KeyCode.Alpha3)){
			newScale = 60f;
			directScaleChange(newScale);
		}


		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			scaleChange = -Input.GetAxis ("Mouse ScrollWheel");

			newCamDistance = cam.distance + (scaleChange * camDistChangeMultiplier);

			newScale = transform.localScale.x + scaleChange;
			newWalkSpeed = playercont.walkSpeed + (scaleChange * walkSpeedMultiplier);
			newJumpHeight = playercont.jumpHeight + (scaleChange * jumpHeightMultiplier);
			newinAirControlAcceleration = playercont.inAirControlAcceleration + (scaleChange * inAirControlAccelerationMultiplier);
			newGravity = playercont.gravity + (scaleChange * gravityMultiplier);
		}

		currentScale = Mathf.MoveTowards (transform.localScale.x, newScale,  Time.deltaTime * Mathf.Abs(newScale - transform.localScale.x));

//		transform.localScale = new Vector3 (Mathf.MoveTowards (transform.localScale.x, newScale,  Time.deltaTime * Mathf.Abs(newScale - transform.localScale.x)), 
//		                                    Mathf.MoveTowards (transform.localScale.y, newScale,  Time.deltaTime * Mathf.Abs(newScale - transform.localScale.x)), 
//		                                    Mathf.MoveTowards (transform.localScale.z, newScale,  Time.deltaTime * Mathf.Abs(newScale - transform.localScale.x)));
		                                 
		transform.localScale = new Vector3 (currentScale, currentScale, currentScale);


//		cam.distance = Mathf.MoveTowards (cam.distance, newCamDistance,  Time.deltaTime * Mathf.Abs(newCamDistance - cam.distance) / 2 );


//		playercont.walkSpeed = Mathf.MoveTowards (playercont.walkSpeed, 
//		                                          newWalkSpeed,  
//		                                          Time.deltaTime * Mathf.Abs(newWalkSpeed - playercont.walkSpeed));



//		playercont.jumpHeight =  Mathf.MoveTowards (playercont.jumpHeight, 
//		                                            newJumpHeight,  
//		                                            Time.deltaTime * lerpTime);
//
//
//
//		playercont.inAirControlAcceleration = Mathf.MoveTowards (playercont.inAirControlAcceleration, 
//		                                                         newinAirControlAcceleration,  
//		                                                         Time.deltaTime * lerpTime);
//
//		playercont.gravity = Mathf.MoveTowards (playercont.gravity, 
//		                                        newGravity,  
//		                                        Time.deltaTime * lerpTime);

		cam.distance = currentScale * camDistChangeMultiplier;
		playercont.walkSpeed = currentScale * walkSpeedMultiplier;
		playercont.jumpHeight = currentScale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = currentScale * inAirControlAccelerationMultiplier;
		playercont.gravity = currentScale * gravityMultiplier;

		RenderSettings.fogEndDistance -= Input.GetAxis ("Mouse ScrollWheel") * fogEndDistanceMultiplier;
	}
}
