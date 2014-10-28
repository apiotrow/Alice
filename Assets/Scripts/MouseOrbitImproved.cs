using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{
	
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	float x = 0.0f;
	float y = 0.0f;
	public float minDistance = 0.5f;
	public float maxDistance = 15f;
	public float smooth = 10.0f;
	Vector3 dollyDir;
	
	// Use this for initialization
	void Start ()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;

		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}
	
	void LateUpdate ()
	{
		if (target) {
			x += Input.GetAxis ("Mouse X") * xSpeed /** distance */* 0.02f * 5f;
			y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle (y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler (y, x, 0);
			
			//distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*165, distanceMin, distanceMax);

			Vector3 desiredCameraPos = transform.TransformPoint (dollyDir * maxDistance);
			
			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit)) {
//				distance -=  hit.distance;
			}
			if (Physics.Linecast (transform.position, desiredCameraPos, out hit)) {
//				distance = Mathf.Clamp( hit.distance, minDistance, maxDistance );
//				
//				distance = Mathf.MoveTowards (hit.distance, hit.distance - 1, Time.deltaTime * 5f); 
			} else {
//				distance = maxDistance;
			}
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;
			
			transform.rotation = rotation;
			transform.position = position;
			
		}
		
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
	
	
}
