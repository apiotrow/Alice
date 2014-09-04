using UnityEngine;
using System.Collections;

public class AliceController : MonoBehaviour
{
	public MouseOrbitImproved cam;
	public ThirdPersonControllerCS playercont;
	public float camDistChangeMultiplier;
	float scaleChange;
	float oldCamDistance;
	float newCamDistance;
	float minScale;
	float maxScale;
	float walkSpeedMultiplier;
	float jumpHeightMultiplier;
	float inAirControlAccelerationMultiplier;
	float fogEndDistanceMultiplier;
	float gravityMultiplier;

	void Start ()
	{
		cam = Camera.main.GetComponent<MouseOrbitImproved> ();
		playercont = gameObject.GetComponent<ThirdPersonControllerCS> ();

		camDistChangeMultiplier = 5f;

		maxScale = 200f;
		minScale = 1f;

		scaleChange = 0f;

		walkSpeedMultiplier = 2f;
		jumpHeightMultiplier = 1f;
		inAirControlAccelerationMultiplier = 4f;
		fogEndDistanceMultiplier = 10f;
		gravityMultiplier = 10f;

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

		cam.distance = Mathf.MoveTowards (cam.distance, scale * camDistChangeMultiplier,  Time.deltaTime * 3f); ;

		playercont.walkSpeed = scale * walkSpeedMultiplier;
		playercont.jumpHeight = scale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = scale * inAirControlAccelerationMultiplier;
		playercont.gravity = scale * gravityMultiplier;

		RenderSettings.fogEndDistance = scale * 20f;
	}

	void handleSizeChange(){

		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			scaleChange = -Input.GetAxis ("Mouse ScrollWheel");

			newCamDistance = cam.distance + (scaleChange * camDistChangeMultiplier);
		}

//		transform.localScale -= new Vector3 (Input.GetAxis ("Mouse ScrollWheel"), 
//		                                     Input.GetAxis ("Mouse ScrollWheel"), 
//		                                     Input.GetAxis ("Mouse ScrollWheel"));

		transform.localScale =  new Vector3 (Mathf.MoveTowards (transform.localScale.x, scaleChange,  Time.deltaTime * 3f), 
		                                     Mathf.MoveTowards (transform.localScale.y, scaleChange,  Time.deltaTime * 3f), 
		                                     Mathf.MoveTowards (transform.localScale.z, scaleChange,  Time.deltaTime * 3f));;

		//cam.distance -= Input.GetAxis ("Mouse ScrollWheel") * camDistChangeMultiplier;

		//wheel has scrolled. get new camera distance from it.

		//if new camera distance not achieved yet, keep lerping towards it.
		//if (cam.distance != newCamDistance) {
		//cam.distance += Mathf.MoveTowards (oldCamDistance, newCamDistance,  Time.deltaTime * 0.1f);

		cam.distance = Mathf.MoveTowards (cam.distance, newCamDistance,  Time.deltaTime * 3f);
		//if it has been achieved, set current camera distance to be the one we lerp from next time.
//		} else {
//			oldCamDistance = cam.distance;
//
//		}

		if (cam.distance == newCamDistance) {
			oldCamDistance = cam.distance;
		}



		playercont.walkSpeed -= Input.GetAxis ("Mouse ScrollWheel") * walkSpeedMultiplier;
		playercont.jumpHeight -= Input.GetAxis ("Mouse ScrollWheel") * jumpHeightMultiplier;
		playercont.inAirControlAcceleration -= Input.GetAxis ("Mouse ScrollWheel") * inAirControlAccelerationMultiplier;
		playercont.gravity -= Input.GetAxis ("Mouse ScrollWheel") * gravityMultiplier;

		RenderSettings.fogEndDistance -= Input.GetAxis ("Mouse ScrollWheel") * fogEndDistanceMultiplier;
	}
}
