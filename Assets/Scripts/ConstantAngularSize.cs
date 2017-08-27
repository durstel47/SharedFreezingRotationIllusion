using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantAngularSize : MonoBehaviour {


    //taken from FixedAngularSize
    public bool bConstantSize = false;
    public float sizeRatio = 0f;
  
 
    // The ratio between the transform's local scale and its starting
    // distance from the camera.
    private float startingDistance;
    private Vector3 startingScale;

 
    //meaning constant proportion of ensemble size vs. airplane from every viewpoint

    // Use this for initialization
    void Start () {      
        startingDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
   
        startingScale = transform.localScale / 1.3025f; // correction factor for scale change at start of function

        SetSizeRatio(sizeRatio);
    }

    /// <summary>
    /// Manually update the OverrideSizeRatio during runtime or through UnityEvents in the editor
    /// </summary>
    /// <param name="ratio"> 0 - 1 : Use 0 for linear scaling</param>
    public void SetSizeRatio(float ratio)
    {
        if (ratio == 0)
        {
            if (startingDistance > 0f)
            {
                // set to a linear scale ratio
                sizeRatio = 1f / startingDistance;
            }
            else
            {
                // If the transform and the camera are both in the same
                // position (that is, the distance between them is zero),
                // disable this Behaviour so we don't get a DivideByZero
                // error later on.
                enabled = false;
#if UNITY_EDITOR
                Debug.LogWarning("The object and the camera are in the same position at Start(). The attached ConstantAngularSize Behaviour is now disabled.");
#endif // UNITY_EDITOR
            }
        }
        else
        {
            sizeRatio = ratio;
        }
    }


    // Update is called once per frame
    void Update () {
        if (!bConstantSize)
            return;
        float distanceToHologram = Vector3.Distance(Camera.main.transform.position, transform.position);
        // create an offset ratio based on the starting position. This value creates a new angle that pivots
        // on the starting position that is more or less drastic than the normal scale ratio.
        float curvedRatio = 1 - startingDistance * sizeRatio;
        transform.localScale = startingScale * (distanceToHologram * sizeRatio + curvedRatio);

    }

    public void Reset()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        bConstantSize = false;
    }


}
