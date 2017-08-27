using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class AirplaneHolderKeywords : MonoBehaviour //, ISpeechHandler
{

    private const float defaultShiftIncrement = 0.5f;
    private const float defaultAngleIncrement = 5f;
    private const float defaultSizeIncrement = 1.5f;
    private Vector3 defaultPosition;
    private Slider airplanePositionSlider;
    private Slider airplaneYawSlider;
    private Slider airplanePitchSlider;
    private Slider holoSizeSlider;


    [Tooltip("Sagital increment to use when moving the airplane fore and back.")]
    public float shiftIncrement = defaultShiftIncrement;

    [Tooltip("Angle increment to use when Rotation the airplane.")]
    public float angleIncrement = defaultAngleIncrement;

    void Awake()
    {
    }
    // Use this for initialization
    void Start () {
        airplanePositionSlider = GameObject.Find("AirplanePositionSlider").GetComponent<Slider>();
        airplaneYawSlider = GameObject.Find("AirplaneYawSlider").GetComponent<Slider>();
        airplanePitchSlider = GameObject.Find("AirplanePitchSlider").GetComponent<Slider>();
        holoSizeSlider = GameObject.Find("HoloSizeSlider").GetComponent<Slider>();
        defaultPosition = transform.localPosition;

    }

    /**********************
    public void ChangeTransform(string cmd)
    {
        switch (cmd.ToLower())
        {
            case "move foreward":
                OnMoveForeward();
                break;
            case "move backward":
                OnMoveBackward();
                break;
            case "move reset":
                OnMoveReset();
                break;
            case "turn clockwise":
                OnYawCW();
                break;
            case "turn counterclockwise":
                OnYawCCW();
                break;
            case "pitch up":
                OnPitchUp();
                break;
            case "pitch down":
                OnPitchDown();
                break;
            case "rotation reset":
                OnRotationReset();
                break;
            default:
                break;
        }
    }
    *******************/
    public void OnMakeBigger()
    {
        float exponens = holoSizeSlider.value + 1f;
        holoSizeSlider.value = exponens;
    }

    public void OnMakeSmaller()
    {
        float exponens = holoSizeSlider.value - 1f;
        holoSizeSlider.value = exponens;
    }

    public void OnMoveForeward()
    {
        //Vector3 position = transform.localPosition;
        //position.z += shiftIncrement;
        float position = airplanePositionSlider.value + shiftIncrement;
        airplanePositionSlider.value =position;
        //transform.localPosition = position;
    }

    public void OnMoveBackward()
    {
        //Vector3 position = transform.localPosition;
        //position.z -= shiftIncrement;
        float position = airplanePositionSlider.value - shiftIncrement;
        airplanePositionSlider.value = position;
        //transform.localPosition = position;
    }

    public void OnMoveReset()
    {
        airplanePositionSlider.value = defaultPosition.z;
        transform.localPosition = defaultPosition;
    }

    public void OnYawCW()
    {
        float angle = airplaneYawSlider.value += angleIncrement;
        airplaneYawSlider.value = angle;
    }

    public void OnYawCCW()
    {
        float angle = airplaneYawSlider.value -= angleIncrement;
        airplaneYawSlider.value = angle;

    }

    public void OnPitchDown()
    {
        float angle = airplanePitchSlider.value += angleIncrement;
        airplanePitchSlider.value = angle;
    }

    public void OnPitchUp()
    {
        float angle = airplanePitchSlider.value += angleIncrement;
        airplanePitchSlider.value = angle;
    }

    public void OnRotationReset()
    {
        airplaneYawSlider.value = 0f;
        airplanePitchSlider.value = 0f;
    }

    /*****************************
    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        ChangeTransform(eventData.RecognizedText);
    }
    **********************/

}
