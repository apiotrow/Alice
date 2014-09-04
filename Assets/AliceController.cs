using UnityEngine;
using System.Collections;

public class AliceController : MonoBehaviour
{
	public MouseOrbitImproved cam;
	public ThirdPersonController g;
	public float camDistChangeMultiplier;
	float minScale;
	float maxScale;

	void Start ()
	{
		cam = Camera.main.GetComponent<MouseOrbitImproved> ();

		camDistChangeMultiplier = 5f;
		maxScale = 10f;
		minScale = 1f;

	}

	void Update ()
	{


		handleSizeChange();


		if (transform.localScale.x >= maxScale) {
			snapCamera(maxScale);
		}
		if (transform.localScale.x <= minScale) {
			snapCamera(minScale);
		}
	}

	void snapCamera(float scale){
		transform.localScale = new Vector3 (scale, scale, scale);
		cam.distance = scale * camDistChangeMultiplier;
	}

	void handleSizeChange(){
		transform.localScale -= new Vector3 (Input.GetAxis ("Mouse ScrollWheel"), 
		                                     Input.GetAxis ("Mouse ScrollWheel"), 
		                                     Input.GetAxis ("Mouse ScrollWheel"));

		cam.distance -= Input.GetAxis ("Mouse ScrollWheel") * camDistChangeMultiplier;

		//transform
	}
}
