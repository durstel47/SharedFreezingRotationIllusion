using UnityEngine;
using System.Collections;



public class RotatePropeller : MonoBehaviour {

    public float propellerRotVel = 100f;
    private Vector3 propellerAxis;
    private Transform propellerTrans;
    private float propAng = 0f;

    // Use this for initialization
    void Start()
    {
        propellerTrans = GameObject.Find("PropShaft").GetComponent<Transform>();
        propellerAxis = Vector3.up;
        propellerAxis.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        propAng += (Time.deltaTime * propellerRotVel);
        propAng = propAng % 360f;
        propellerTrans.localRotation = Quaternion.AngleAxis(propAng, propellerAxis);
    }
}
