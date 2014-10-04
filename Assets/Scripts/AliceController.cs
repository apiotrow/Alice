using UnityEngine;
using System.Collections;

public class AliceController : MonoBehaviour
{
	public AlicePlanet alicePlanet;
	public Transform makeSmallerObject;

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
	float runSpeedMultiplier;
	float jumpHeightMultiplier;
	float inAirControlAccelerationMultiplier;
	float gravityMultiplier;
	float fogEndDistanceMultiplier;

	bool manualSizeChanging;

	void OnGUI(){
		string text;
		text = "Manual Size Changing: " + manualSizeChanging;
		GUI.Box (new Rect (Screen.width - 200, 0, 200, 30), text);

//		if (GUI.Button (new Rect (0, Screen.height - 100, 100, 100), "go")) {
//			playercont = gameObject.GetComponent<ThirdPersonControllerCS> ();
//			playercont._characterState = ThirdPersonControllerCS.CharacterState.Running;
//		}
	}


	void Start ()
	{

		cam = Camera.main.GetComponent<MouseOrbitImproved> ();
		playercont = gameObject.GetComponent<ThirdPersonControllerCS> ();
		lerpTime = 20f;

		camDistChangeMultiplier = 5f;

		maxScale = 2000f;
		minScale = 1f;

		scaleChange = 0f;
		newScale = 1f;
		currentScale = 1f;

		walkSpeedMultiplier = 2f;
		runSpeedMultiplier = 4f;
		jumpHeightMultiplier = 4f;
		inAirControlAccelerationMultiplier = 2f;
		fogEndDistanceMultiplier = 10f;
		gravityMultiplier = 20f;

		newScale = currentScale;

		transform.localScale = new Vector3 (minScale, minScale, minScale);
		
		cam.distance = minScale * camDistChangeMultiplier;
		
		playercont.walkSpeed = minScale * walkSpeedMultiplier;
		playercont.runSpeed = minScale * runSpeedMultiplier;
		playercont.jumpHeight = minScale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = minScale * inAirControlAccelerationMultiplier;
		playercont.gravity = minScale * gravityMultiplier;

		RenderSettings.fog = false;
		RenderSettings.fogEndDistance = minScale * fogEndDistanceMultiplier;

		manualSizeChanging = true;
	}



	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel (0); 
		}

		if(Input.GetKeyDown(KeyCode.T)){
			manualSizeChanging = !manualSizeChanging;
		}

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
		playercont.runSpeed = scale * runSpeedMultiplier;
		playercont.jumpHeight = scale * jumpHeightMultiplier;
		playercont.inAirControlAcceleration = scale * inAirControlAccelerationMultiplier;
		playercont.gravity = scale * gravityMultiplier;

		RenderSettings.fogEndDistance = scale * fogEndDistanceMultiplier;
	}

	void sizeChangeByInput(){
		if(Input.GetKeyDown(KeyCode.Alpha2)/* || Input.GetMouseButtonDown(1)*/){
			if(newScale > minScale)
				newScale = newScale / 2f;
		}
		if(Input.GetKeyDown(KeyCode.Alpha1) /*|| Input.GetMouseButtonDown(0)*/){
			if(newScale < maxScale)
				newScale = newScale * 2f;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			scaleChange = -Input.GetAxis ("Mouse ScrollWheel");

			newScale = transform.localScale.x + (scaleChange * 5f);
		}
	}

	void sizeChangeByDistanceToObject(Transform obj, float multiplier){
		float distToObject;
		//distToObject = Vector3.Distance (obj.position, transform.position);
		//newScale = distToObject * multiplier;
	}


	void handleSizeChange(){
		if (!manualSizeChanging) {
			sizeChangeByDistanceToObject (makeSmallerObject, 0.1f);
		} else {
			sizeChangeByInput ();
		}

		scaleChangeSpeed = Time.deltaTime * Mathf.Abs (newScale - transform.localScale.x);
		currentScale = Mathf.MoveTowards (transform.localScale.x, newScale, scaleChangeSpeed);                 
		setValues(currentScale);
	}
	
	void applySphericalGravity(){
		transform.up = -(alicePlanet.transform.position - transform.position);
		Debug.DrawRay (transform.position, transform.up, Color.green, 5f);
		rigidbody.AddForce (-transform.up * 30f);
		
		if(Input.GetKey(KeyCode.W)){
			rigidbody.AddForce(transform.forward * 10f);
		}
		if(Input.GetKey(KeyCode.Space)){
			rigidbody.AddForce(transform.up * 50f);
		}
	}
}
