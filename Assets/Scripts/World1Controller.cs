using UnityEngine;
using System.Collections;

public class World1Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 start = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		Vector3 end = new Vector3 (transform.eulerAngles.x, start.y - 0.1f, transform.eulerAngles.z);
		transform.eulerAngles = end;
	}
}
