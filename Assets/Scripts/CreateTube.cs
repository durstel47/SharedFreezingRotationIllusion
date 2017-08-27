using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTube : MonoBehaviour {

    private GameObject tubeUnit;
    private GameObject[] tubeUnits;
    public float ringRadius = 0.15f;
    public float ringElementMaxHeight = 0.07f;
    public float ringElementScaling = 0.8f;
    public float ringThickness = 0.04f;
    public int startRingIndex = 0;
    public int nRings = 6;
    public int nRingElements = 32;
    public float offset = 0.1f;


     

	// Use this for initialization
	void Start () {
        tubeUnit = (GameObject)Resources.Load("MyTubeUnit", typeof(GameObject));
        tubeUnits = new GameObject[nRingElements];
        //offset = nRings * ringThickness * 0.5f;
        for (int k = startRingIndex; k < (nRings + startRingIndex); k++)
        {
            for (int i = 0; i < nRingElements; i += 1)
            {

                tubeUnits[i] = Instantiate<GameObject>(tubeUnit);
                tubeUnits[i].transform.SetParent(this.transform);
                ArrrageTubeUnitMesh unit = tubeUnits[i].GetComponent<ArrrageTubeUnitMesh>();
                unit.nRingElements = nRingElements;
                unit.radius = ringRadius;
                unit.ringIndex = i;
                unit.tubeIndex = k;
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
