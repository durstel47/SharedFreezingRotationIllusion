using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackAndForthInDepth : MonoBehaviour {

    public bool bMove = false;
    public float meanSpeed = 0.5f;
    public float amplitude = 2f;
    private float period;
    private float depth;
    private float a;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (!bMove)
            return;
        if ((Mathf.Abs(meanSpeed) > 0f) && (Mathf.Abs(amplitude) > 0f))
        {
            period = 4f * amplitude / Mathf.Abs(meanSpeed);
            a = (Time.time % period) * 2f * Mathf.PI / period;
            depth = amplitude * Mathf.Sin(a);
        }
        else
        {
            depth = 0f;
        }

        transform.localPosition = new Vector3(0f, 0f, depth);

    }
    public void Reset()
    {
        transform.localPosition = Vector3.zero;
    }
}
