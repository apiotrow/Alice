using UnityEngine;
using System.Collections;

public struct MathFunctions {
	// always positive
	public float Positive (float FloatToChange){
		if (FloatToChange < 0){
			return (FloatToChange * (-1) );
		}
		return FloatToChange;
	}
	// Distance
	public float Distance (Vector3 From, Vector3 To){
		float TempX;
		float TempY;
		float TempZ;
		TempX = From.x - To.x; TempX *= TempX;
		TempY = From.y - To.y; TempY *= TempY;
		TempZ = From.z - To.z; TempZ *= TempZ;
		return Mathf.Sqrt(TempX + TempY + TempZ);
	}
	public Vector3 DirectionDistancePoint (Vector3 From, Vector3 To, float Length){
		return (From + (To.normalized * Length) );
	}
	// Distance / sec
	public float Vector3SpeedPerSec (Vector3 From, Vector3 To, float InTime){
		return Positive( (Distance(From, To) ) / InTime  );
	}
	public Vector3 Direction (Vector3 From, Vector3 To){
		return (To - From).normalized;
	}
}