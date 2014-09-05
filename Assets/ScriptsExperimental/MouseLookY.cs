using UnityEngine;
using System.Collections;

public class MouseLookY : MonoBehaviour {
	public float MouseSensitivityY = 1;
	void Update(){
		transform.localEulerAngles += new Vector3 (Input.GetAxis("Mouse Y") * MouseSensitivityY, 0, 0);
	}
}