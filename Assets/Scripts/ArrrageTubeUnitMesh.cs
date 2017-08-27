using UnityEngine;
using System.Collections;

public class ArrrageTubeUnitMesh : MonoBehaviour {

    public int nRingElements = 36;
    public int ringIndex = 0;
    public int tubeIndex = 5;
    public float radius = 5.0f;
    public float thickness = 1f;
    public Color elementColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    public float offset = 0f;

    
    public float height = 1.0f;
    public float unitScaling = 1.8f;
    private float d;
    private float dn, df;
    private float alpha;
    private float angle;
    private float distance;
    private Quaternion rot;
 
	// Use this for initialization
	void Start () {
        //Arrange();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Arrange()
    {
        alpha = Mathf.PI / (float)nRingElements;
        d =  unitScaling * Mathf.Sin(0.5f * alpha);
        df = d * (radius + 0.5f * height);
        dn = d * (radius - 0.5f * height);
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = elementColor;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        //Debug.Log(mesh);		
        Vector3[] normals = mesh.normals;
        Vector3 v;
        //Debug.Log(vertices.Length);
        for (int i = 0; i < vertices.Length; i++)
        {
            v = vertices[i];
            if (v.y > 0f)
            {
                if (v.x < 0)
                    v.x = -df;
                else
                    v.x = df;
            }
            else
            {
                if (v.x < 0)
                    v.x = -dn;
                else

                    v.x = +dn;
            }
            v.y = v.y * height;
            v.z = v.z * thickness;
            vertices[i] = v;
            //Debug.Log(vertices[i].ToString());
        }
        mesh.vertices = vertices;

        Transform t = GetComponent<Transform>();
        distance = (float)tubeIndex * thickness;
        angle = (float)ringIndex * 2.0f * alpha;
        rot = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90f, Vector3.forward);
        t.position = new Vector3(radius * Mathf.Cos(angle),radius * Mathf.Sin(angle) , distance - offset);
        t.rotation = rot;
    }
}
