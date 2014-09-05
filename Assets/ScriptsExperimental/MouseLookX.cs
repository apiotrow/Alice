using UnityEngine;
using System.Collections;

public class MouseLookX : MonoBehaviour {
	public float MouseSensitivityX = 1;
	void Update (){
		transform.localEulerAngles += new Vector3(  0, Input.GetAxis("Mouse X") * MouseSensitivityX, 0);
	}
}