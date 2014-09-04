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
	float scaleChangeSpeed;

	float oldCamDistance;
	float camDistanceChangeSpeed;

	float minScale;
	float maxScale;

	float walkSpeedMultiplier;
	float jumpHeightMultiplier;
	float inAirControlAccelerationMultiplier;
	float gravityMultiplier;
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

		transform.localScale = new Vector3 (minScale, minScale, minScale);
		
		cam.distance = minScale * camDistChangeMultiplier;
		
		playercont.walkSpeed = minScale * walkSpeedMultiplier;
		playercont.jumpHeight = minScale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = minScale * inAirControlAccelerationMultiplier;
		playercont.gravity = minScale * gravityMultiplier;

		RenderSettings.fog = false;
		RenderSettings.fogEndDistance = minScale * fogEndDistanceMultiplier;
	}

	void Update ()
	{
		handleSizeChange();

		if (transform.localScale.x >= maxScale) {
			setValues(maxScale);
		}
		if (transform.localScale.x <= minScale) {
			setValues(minScale);
		}
	}

	void setValues(float scale){
		transform.localScale = new Vector3 (scale, scale, scale);

		camDistanceChangeSpeed = Time.deltaTime * Mathf.Abs ((scale * camDistChangeMultiplier) - cam.distance);
		cam.distance = Mathf.MoveTowards (cam.distance, scale * camDistChangeMultiplier, camDistanceChangeSpeed);

		playercont.walkSpeed = scale * walkSpeedMultiplier;
		playercont.jumpHeight = scale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = scale * inAirControlAccelerationMultiplier;
		playercont.gravity = scale * gravityMultiplier;

		RenderSettings.fogEndDistance = scale * fogEndDistanceMultiplier;
	}


	void handleSizeChange(){
		if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetMouseButtonDown(1)){
			if(newScale > minScale)
				newScale = newScale / 2f;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetMouseButtonDown(0)){
			if(newScale < maxScale)
				newScale = newScale * 2f;
		}
		if(Input.GetKey(KeyCode.Alpha3)){
			//newScale = 60f;
		}


		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			scaleChange = -Input.GetAxis ("Mouse ScrollWheel");

			newScale = transform.localScale.x + (scaleChange * 5f);
		}

		scaleChangeSpeed = Time.deltaTime * Mathf.Abs (newScale - transform.localScale.x);
		currentScale = Mathf.MoveTowards (transform.localScale.x, newScale, scaleChangeSpeed);                 
		setValues(currentScale);
	}
}
