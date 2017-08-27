using UnityEngine;
using System.Collections;

public class RotateAircraft : MonoBehaviour {

    public float rollSpeed = 12f;
    private float rollAngle = 0f;
    

	// Use this for initialization
	void Start () {
        transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up);
	}
	
	// Update is called once per frame
	void Update () {
        rollAngle += rollSpeed * Time.deltaTime;
        rollAngle = rollAngle % 360f;
        transform.localRotation = Quaternion.AngleAxis(rollAngle, Vector3.forward);
    }
}
