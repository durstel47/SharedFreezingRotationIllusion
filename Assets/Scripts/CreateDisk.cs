using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDisk : MonoBehaviour {

    private GameObject arcUnit;
    private GameObject[] arcUnits;
    public float maxRingRadius = 0.15f;
    private float ringRadius;
    private float ringElementMaxHeight = 0.07f;
    public float ringElementScaling = 0.8f;
    public float ringThickness = 0.04f;
    public int startRingIndex = 0;
    public int nRings = 3;
    public int nRingElements = 32;
    public float offset = 0.1f;
    public int tubeIndex = 0;


    // Use this for initialization
    void Start () {
        ringElementMaxHeight = maxRingRadius / ((float)nRings - 0.5f);
        arcUnit = (GameObject)Resources.Load("MyTubeUnit", typeof(GameObject));
        arcUnits = new GameObject[nRingElements];
        //offset = nRings * ringThickness * 0.5f;
        for (int k = startRingIndex; k < nRings; k++)
        {
            ringRadius = (0.5f + (float)(k)) * ringElementMaxHeight;
            for (int i = 0; i < nRingElements; i += 1)
            {
                arcUnits[i] = Instantiate<GameObject>(arcUnit);
                arcUnits[i].transform.SetParent(this.transform);
                ArrrageTubeUnitMesh unit = arcUnits[i].GetComponent<ArrrageTubeUnitMesh>();
                unit.nRingElements = nRingElements;
                unit.radius = ringRadius;
                unit.ringIndex = i;
                unit.tubeIndex = tubeIndex;
                unit.height = ringElementMaxHeight * Random.value;
                unit.thickness = ringThickness;
                unit.unitScaling = ringElementScaling * 2f;
                unit.offset = offset;
                unit.elementColor = Random.ColorHSV();
                unit.Arrange();

            }
        }


    }

    // Update is called once per frame
    void Update () {
		
	}
}
