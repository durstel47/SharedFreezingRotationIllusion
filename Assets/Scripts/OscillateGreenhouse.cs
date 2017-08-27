using UnityEngine;
using System.Collections;

public class OscillateGreenhouse : MonoBehaviour {

    public float amplitude = 57.3f;
    public float meanSpeed = 24f; //should be twice the rotation speed of the airplane
    private float period = 0f;
    private float rollAngle = 0;
    private float a;

    // Use this for initialization
    void Start () {
 	
	}
	
	// Update is called once per frame
	void Update () {
        if ((Mathf.Abs(meanSpeed) > 0f) && (Mathf.Abs(amplitude) > 0f))
        {
            period = 4f * amplitude / Mathf.Abs(meanSpeed);
            a = (Time.time % period) * 2f * Mathf.PI / period;
            rollAngle = amplitude * Mathf.Sin(a);
        }
        else
        {
            rollAngle = 0f;
        }
            
        transform.localRotation = Quaternion.AngleAxis(rollAngle, Vector3.forward);

    }
}
