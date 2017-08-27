using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HoloMenu : NetworkBehaviour
{
    private OscillateGreenhouse oscillateTube;
    private MoveBackAndForthInDepth moveSurround;
    private MoveBackAndForthInDepth moveAircraft;
    private RotateAircraft rotateAircraft;
    private ConstantAngularSize scaleSurround;
    private ConstantAngularSize scaleAircraft;
    private RotatePropeller rotatePropeller;

    //private TapToPlace tapToPlace;
    private HandDraggable handDraggable;
    private Transform holderTrans;
    private Text clientServerLabel;
    private Text holoSizeLabel;
    private Text airplaneSpeedLabel;
    private Text tubeSpeedLabel;
    private Text tubeBackForthLabel;
    private Text airplanePositionLabel;
    private Text airplaneBackForthLabel;
    private Text airplaneSagittalAmpLabel;
    private Text airplaneYawLabel;
    private Text airplanePitchLabel;
    private Text propellerSpeedLabel;
    private float holoSizeExponens = 0f;
    public float holoSize = 1.5f;
    public float meanTubeSpeed = 36f; //changed from 24 MRD
    public float tubeDepthAmp = 1f;
    public float tubeDepthSpeed = 0.2f;
    public float airplaneSpeed = 12f;
    public float airplanePosition = 0f;
    public float airplaneDepthAmp = 2f;
    public float airplaneDepthSpeed = 1.5f; 
    public float airplaneYaw = 0f;
    public float airplanePitch = 0f;
    public float propellerSpeed = 540f;
    private Slider holoSizeSlider;
    private Slider tubeSpeedSlider;
    private Slider tubeBackForthSlider;
    private Slider propellerSpeedSlider;
    private Slider airplaneSpeedSlider;
    private Slider airplanePositionSlider;
    private Slider airplaneBackForthSlider;
    private Slider airplaneSagittalAmpSlider;
    private Slider airplaneYawSlider;
    private Slider airplanePitchSlider;
    private Button resetButton;
    private Toggle tubeToggle;
    private Toggle diskToggle;
    private Toggle skyboxToggle;
    private Toggle gazePlacingToggle;
    private Toggle uprightToggle;
    private Toggle orientToggle;
    private Toggle moveTubeBackForthToggle;
    private Toggle constRetSizeToggle;
    private Toggle moveAirplaneBackForthToggle;
    //private Toggle airplaneConstantSizeToggle;
    private Toggle soundToggle;
    private Toggle meshToggle;


    private GameObject airplaneTube;
    private GameObject backgroundDisk;
    private GameObject holoCollectionBox;
    private Renderer skyBoxRenderer;
    private AudioSource audioSource;
    private SpatialMappingManager spatialMappingManager;
    private Vector3 holoBoxStartScale;


    void Awake()
    {
        airplaneTube = GameObject.Find("AirplaneTube");
        backgroundDisk = GameObject.Find("BackgroundDisk");
        holoCollectionBox = GameObject.Find("HoloCollectionBox");
    }

    // Use this for initialization
    void Start () {

        /**************************************************
        if (base.isServer)
        {
            Canvas canv = GetComponent<Canvas>();
            canv.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canv.GetComponent<CanvasScaler>();
            scaler.referenceResolution = new Vector2(1200f, 800f);
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }
        *************************************************/

        Canvas canv = GetComponent<Canvas>();
        canv.renderMode = RenderMode.WorldSpace;
        transform.localPosition = new Vector3(-0.4f, 0f, 1f);
        transform.localRotation = Quaternion.AngleAxis(-30f, Vector3.up);

        //tapToPlace = GameObject.Find("HoloCollectionBox").GetComponent<TapToPlace>();
        handDraggable = GameObject.Find("HoloCollectionBox").GetComponent<HandDraggable>();
        holoBoxStartScale = holoCollectionBox.transform.localScale;

        GameObject tubeGo = GameObject.Find("SurroundEnsemble");
        oscillateTube = tubeGo.GetComponent<OscillateGreenhouse>();
        moveSurround = tubeGo.GetComponent<MoveBackAndForthInDepth>();
        moveSurround.bMove = false;
        moveSurround.amplitude = tubeDepthAmp;
        scaleSurround = tubeGo.GetComponent<ConstantAngularSize>();
        scaleSurround.bConstantSize = false;

        GameObject planeGo = GameObject.Find("AirplaneEnsemble");
        rotateAircraft = planeGo.GetComponent<RotateAircraft>();
        moveAircraft = planeGo.GetComponent<MoveBackAndForthInDepth>();
        moveAircraft.bMove = false;
        moveAircraft.amplitude = airplaneDepthAmp;
        scaleAircraft = planeGo.GetComponent<ConstantAngularSize>();
        scaleAircraft.bConstantSize = false;

        rotatePropeller = GameObject.Find("Airplane").GetComponent<RotatePropeller>();

  
        holderTrans = GameObject.Find("AirplaneHolder").transform;
        skyBoxRenderer = GameObject.Find("SkySphere").GetComponent<Renderer>();
        audioSource = GameObject.Find("Airplane").GetComponent<AudioSource>();
        spatialMappingManager = GameObject.Find("SpatialMapping").GetComponent<SpatialMappingManager>();

        holoSizeLabel = GameObject.Find("HoloSizeLabel").GetComponent<Text>();
        float scale = Mathf.Pow(holoSize, holoSizeExponens);
        holoSizeLabel.text = "Hologram Ensemble Scale: " + scale.ToString("F2");
        holoSizeSlider = GameObject.Find("HoloSizeSlider").GetComponent<Slider>();
        holoSizeSlider.onValueChanged.AddListener((value) => { OnChangedHoloSize(value); });
        holoSizeSlider.onValueChanged.AddListener((value) => { CmdOnChangedHoloSize(value); });
  

        propellerSpeedLabel = GameObject.Find("PropellerSpeedLabel").GetComponent<Text>();
        propellerSpeedLabel.text = "Propeller's Rotational Speed: " + propellerSpeed.ToString("F1") + "°/s";
        propellerSpeedSlider = GameObject.Find("PropellerSpeedSlider").GetComponent<Slider>();
        propellerSpeedSlider.onValueChanged.AddListener((value) => { OnChangedPropellerSpeed(value); });

        airplaneSpeedLabel = GameObject.Find("AirSpeedLabel").GetComponent<Text>();
        airplaneSpeedLabel.text = "Rotational Speed: " + airplaneSpeed.ToString("F1") + "°/sec";
        airplaneSpeedSlider = GameObject.Find("AirSpeedSlider").GetComponent<Slider>();
        airplaneSpeedSlider.onValueChanged.AddListener((value) => { OnChangedAirplaneSpeed(value); });

        tubeSpeedLabel = GameObject.Find("TubeSpeedLabel").GetComponent<Text>();
        tubeSpeedLabel.text = "Mean Rotational Speed: " + meanTubeSpeed.ToString("F1") + "°/sec";
        tubeSpeedSlider = GameObject.Find("TubeSpeedSlider").GetComponent<Slider>();
        tubeSpeedSlider.onValueChanged.AddListener((value) => { OnChangedTubeSpeed(value); });

        tubeBackForthLabel = GameObject.Find("TubeBackForthLabel").GetComponent<Text>();
        tubeBackForthLabel.text = "Mean Sagittal Speed: " + moveAircraft.meanSpeed.ToString("F1") + "m/s";
        tubeBackForthSlider = GameObject.Find("TubeBackForthSlider").GetComponent<Slider>();
        tubeBackForthSlider.onValueChanged.AddListener((value) => { OnChangedTubeBackAndForthSpeed(value); });
        tubeBackForthLabel.gameObject.SetActive(false);
        tubeBackForthSlider.gameObject.SetActive(false);

        airplanePositionLabel = GameObject.Find("AirplanePositionLabel").GetComponent<Text>();
        airplanePositionLabel.text = "Position Along Sagittal Axis: " + airplanePosition.ToString("F1") + "m";
        airplanePositionSlider = GameObject.Find("AirplanePositionSlider").GetComponent<Slider>();
        airplanePositionSlider.onValueChanged.AddListener((value) => { OnChangedAirplanePosition(value); });
     
        airplaneBackForthLabel = GameObject.Find("AirplaneBackForthLabel").GetComponent<Text>();
        airplaneBackForthLabel.text = "Mean Sagittal Speed: " + moveAircraft.meanSpeed.ToString("F1") + "m/s";
        airplaneBackForthLabel.gameObject.SetActive(false);
        airplaneBackForthSlider = GameObject.Find("AirplaneBackForthSlider").GetComponent<Slider>();
        airplaneBackForthSlider.onValueChanged.AddListener((value) => { OnChangedAirplaneBackForth(value); });
        airplaneBackForthSlider.gameObject.SetActive(false);

        airplaneSagittalAmpLabel = GameObject.Find("AirplaneSagittalAmpLabel").GetComponent<Text>();
        airplaneSagittalAmpLabel.gameObject.SetActive(false);
        airplaneSagittalAmpLabel.text = "Sagittal Motion Amplitude" + airplaneDepthAmp.ToString("F1") + "m";
        airplaneSagittalAmpSlider = GameObject.Find("AirplaneSagittalAmpSlider").GetComponent<Slider>();
        airplaneSagittalAmpSlider.onValueChanged.AddListener((value) => { OnChangedAirplaneSagittalAmp(value); });
        airplaneSagittalAmpSlider.gameObject.SetActive(false);


        airplaneYawLabel = GameObject.Find("AirplaneYawLabel").GetComponent<Text>();
        airplaneYawLabel.text = "Yaw Angle: " + airplaneYaw.ToString("F1") + "°";
        airplaneYawSlider = GameObject.Find("AirplaneYawSlider").GetComponent<Slider>();
        airplaneYawSlider.onValueChanged.AddListener((value) => { OnChangedAirplaneYaw(value); });

        airplanePitchLabel = GameObject.Find("AirplanePitchLabel").GetComponent<Text>();
        airplanePitchLabel.text = "Pitch Angle: " + airplanePitch.ToString("F1") + "°";
        airplanePitchSlider = GameObject.Find("AirplanePitchSlider").GetComponent<Slider>();
        airplanePitchSlider.onValueChanged.AddListener((value) => { OnChangedAirplanePitch(value); });

        tubeToggle = GameObject.Find("TubeToggle").GetComponent<Toggle>();
        tubeToggle.onValueChanged.AddListener((value) => { OnChangedTubeToggle(value); });

        diskToggle = GameObject.Find("DiskToggle").GetComponent<Toggle>();
        diskToggle.onValueChanged.AddListener((value) => { OnChangedDiskToggle(value); });

        skyboxToggle = GameObject.Find("SkyboxToggle").GetComponent<Toggle>();
        skyboxToggle.onValueChanged.AddListener((value) => { OnChangedSkyboxToggle(value); });

        //I could not return from gaze placing to hand dragging
        //gazePlacingToggle = GameObject.Find("GazePlacingToggle").GetComponent<Toggle>();
        //gazePlacingToggle.onValueChanged.AddListener((value) => { OnChangedGazePlacingToogle(value); });

        uprightToggle = GameObject.Find("UprightToggle").GetComponent<Toggle>();
        uprightToggle.onValueChanged.AddListener((value) => { OnChangedIsUprightToggle(value); });

        orientToggle = GameObject.Find("OrientToggle").GetComponent<Toggle>();
        orientToggle.onValueChanged.AddListener((value) => { OnChangedOrientToggle(value); });

        moveTubeBackForthToggle = GameObject.Find("MoveTubeBackForthToggle").GetComponent<Toggle>();
        moveTubeBackForthToggle.onValueChanged.AddListener((value) => { OnChangedMoveSurroundBackForthToggle(value); });

        constRetSizeToggle = GameObject.Find("ConstRetSizeToggle").GetComponent<Toggle>();
        constRetSizeToggle.onValueChanged.AddListener((value) => { OnChangedConstantRetinalSizeToggle(value); });

        moveAirplaneBackForthToggle = GameObject.Find("MoveAirplaneBackForthToggle").GetComponent<Toggle>();
        moveAirplaneBackForthToggle.onValueChanged.AddListener((value) => { OnChangedMoveAirplaneBackForthToggle(value); });

        //airplaneConstantSizeToggle = GameObject.Find("AirplaneConstSizeToggle").GetComponent<Toggle>();
        //airplaneConstantSizeToggle.onValueChanged.AddListener((value) => { OnChangedAirplaneConstantSizeToggle(value); });

        soundToggle = GameObject.Find("SoundToggle").GetComponent<Toggle>();
        soundToggle.onValueChanged.AddListener((value) => { OnChangedSoundToggle(value); });

        meshToggle = GameObject.Find("MeshToggle").GetComponent<Toggle>();
        meshToggle.onValueChanged.AddListener((value) => { OnChangedMeshToggle(value); });
      

        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        resetButton.onClick.AddListener(() => { OnClickedReset(); });
    }


    // called when Monobehaviour class is destroyed
    private void OnDestroy()
    {
        if (holoSizeSlider != null)
        {
            holoSizeSlider.onValueChanged.RemoveAllListeners();
            tubeToggle.onValueChanged.RemoveAllListeners();
            diskToggle.onValueChanged.RemoveAllListeners();
            skyboxToggle.onValueChanged.RemoveAllListeners();
            tubeSpeedSlider.onValueChanged.RemoveAllListeners();
            tubeBackForthSlider.onValueChanged.RemoveAllListeners();
            airplaneSpeedSlider.onValueChanged.RemoveAllListeners();
            propellerSpeedSlider.onValueChanged.RemoveAllListeners();
            airplanePositionSlider.onValueChanged.RemoveAllListeners();
            airplaneBackForthSlider.onValueChanged.RemoveAllListeners();
            airplaneSagittalAmpSlider.onValueChanged.RemoveAllListeners();
            airplaneYawSlider.onValueChanged.RemoveAllListeners();
            airplanePitchSlider.onValueChanged.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
            uprightToggle.onValueChanged.RemoveAllListeners();
            orientToggle.onValueChanged.RemoveAllListeners();
            moveTubeBackForthToggle.onValueChanged.RemoveAllListeners();
            constRetSizeToggle.onValueChanged.RemoveAllListeners();
            moveAirplaneBackForthToggle.onValueChanged.RemoveAllListeners();
            //airplaneConstantSizeToggle.onValueChanged.RemoveAllListeners();
            soundToggle.onValueChanged.RemoveAllListeners();
        }
    }

    [ServerCallback]
    public void OnChangedHoloSize(float exponens)
    {
        RpcOnChangedHoloSize(exponens);
    }
    
    [ClientRpc]
    public void RpcOnChangedHoloSize(float exponens)
    {
        float scale = Mathf.Pow(holoSize, exponens);
        holoSizeLabel.text = "Hologram Ensemble Scale: " + scale.ToString("F2");
        holoSizeSlider.value = exponens; //one has to set the local canvas slider value
        holoCollectionBox.transform.localScale = holoBoxStartScale * scale;
    }
    [Command]
    public void CmdOnChangedHoloSize(float exponens)
    {
        holoSizeSlider.value = exponens; //one has to set the local canvas slider value
    }

    [ServerCallback]
    public void OnChangedAirplaneSpeed(float vel)
    {
        RpcOnChangedAirplaneSpeed(vel);
    }
    [ClientRpc]
    public void RpcOnChangedAirplaneSpeed(float vel)
    {
        airplaneSpeedLabel.text = "Rotational Speed: " + vel.ToString("F1") + "°/s";
        airplaneSpeedSlider.value = vel;
        rotateAircraft.rollSpeed = vel;
    }

    [ServerCallback]
    public void OnChangedTubeSpeed(float vel)
    {
        RpcOnChangedTubeSpeed(vel);
    }
    [ClientRpc]
    public void RpcOnChangedTubeSpeed(float vel)
    {
        tubeSpeedLabel.text = "Mean Rotational Speed: " + vel.ToString("F1") + "°/s";
        tubeSpeedSlider.value = vel;
        oscillateTube.meanSpeed = vel;
    }

    [ServerCallback]
    public void OnChangedTubeBackAndForthSpeed(float vel)
    {
        RpcOnChangedTubeBackAndForthSpeed(vel);
    }

    [ClientRpc]
    public void RpcOnChangedTubeBackAndForthSpeed(float vel)
    {
        tubeBackForthLabel.text = "Mean Sagittal Speed: " + vel.ToString("F1") + "m/s";
        tubeBackForthSlider.value = vel;
        moveSurround.meanSpeed = vel;
    }

    [ServerCallback]
    public void OnChangedPropellerSpeed(float vel)
    {
        RpcOnChangedPropellerSpeed(vel);
    }
    [ClientRpc]
    public void RpcOnChangedPropellerSpeed(float vel)
    {
        propellerSpeedLabel.text = "Propeller Speed: " + vel.ToString("F0") + "°/s";
        propellerSpeedSlider.value = vel;
        rotatePropeller.propellerRotVel = vel;
    }

    [ServerCallback]
    public void OnChangedAirplanePosition(float z)
    {
        RpcOnChangedAirplanePosition(z);
    } 
    [ClientRpc]
    public void RpcOnChangedAirplanePosition(float z)
    {
        airplanePositionLabel.text = "Sagittal Position: " + z.ToString("F1") + "m";
        airplanePositionSlider.value = z;
        Vector3 pos = holderTrans.localPosition;
        pos.z = z;
        holderTrans.localPosition = pos;
    }

    [ServerCallback]
    public void OnChangedAirplaneBackForth(float s)
    {
        RpcOnChangedAirplaneBackForth(s);
    }

    [ClientRpc]
    public void RpcOnChangedAirplaneBackForth(float s)
    {
        airplaneBackForthLabel.text = "Mean Sagittal Speed: " + s.ToString("F1") + "m/s";
        airplaneBackForthSlider.value = s;
        moveAircraft.meanSpeed = s;
    } 
    [ServerCallback]
    public void OnChangedAirplaneSagittalAmp(float a)
    {
        RpcOnChangedAirplaneSagittalAmp(a);
    }

    [ClientRpc]
    public void RpcOnChangedAirplaneSagittalAmp(float a)
    {
        airplaneSagittalAmpLabel.text = "Sagittal Motion Amplitude: " + a.ToString("F1") + "m";
        airplaneSagittalAmpSlider.value = a;
        moveAircraft.amplitude = a;
    }

    [ServerCallback]
    public void OnChangedAirplaneYaw(float a)
    {
        RpcOnChangedAirplaneYaw(a);
    }
    [ClientRpc]
    public void RpcOnChangedAirplaneYaw(float a)
    {
        airplaneYawLabel.text = "Yaw Angle: " + a.ToString("F1") + "°";
        airplaneYawSlider.value = a;
        float p = airplanePitchSlider.value; // get current pitch value
        holderTrans.localRotation = Quaternion.AngleAxis(p, Vector3.right) * Quaternion.AngleAxis(a, Vector3.up);
    }

    [ServerCallback]
    public void OnChangedAirplanePitch(float p)
    {
        RpcOnChangedAirplanePitch(p);
    }
    [ClientRpc]
    public void RpcOnChangedAirplanePitch(float p)
    {
        airplanePitchLabel.text = "Pitch Angle: " + p.ToString("F1") + "°";
        airplanePitchSlider.value = p;
        float a = airplaneYawSlider.value;
        holderTrans.localRotation = Quaternion.AngleAxis(p, Vector3.right) * Quaternion.AngleAxis(a, Vector3.up);
    }

    [ServerCallback]
    public void OnChangedTubeToggle(bool isOn)
    {
        RpcOnChangedTubeToggle(isOn);
    }
    [ClientRpc]
    public void RpcOnChangedTubeToggle(bool isOn)
    {
        airplaneTube.SetActive(isOn);
        tubeToggle.isOn = isOn;
    }

    [ServerCallback]
    public void OnChangedDiskToggle(bool isOn)
    {
        RpcOnChangedDiskToggle(isOn);
    }
    [ClientRpc]
    public void RpcOnChangedDiskToggle(bool isOn)
    {
        backgroundDisk.SetActive(isOn);
        diskToggle.isOn = isOn;

    }

    [ServerCallback]
    public void OnChangedSkyboxToggle(bool isOn)
    {
        RpcOnChangedSkyboxToggle(isOn);
    }
    [ClientRpc]
    public void RpcOnChangedSkyboxToggle(bool isOn)
    {
        skyBoxRenderer.enabled = isOn;
        skyboxToggle.isOn = isOn;
    }

    /*****************works from hand draging ro gaze dragging, but not from gaze to hand dragging
    public void OnChangedGazePlacingToogle(bool isOn)
    {
        tapToPlace.enabled = isOn;
        handDraggable.enabled = !isOn;
        uprightToggle.enabled = !isOn;
        orientToggle.enabled = !isOn;
    }
    **************************************/
    [ServerCallback]
    public void OnChangedIsUprightToggle(bool isOn)
    {
        RpcOnChangedIsUprightToggle(isOn);
    }
    [ClientRpc]
    public void RpcOnChangedIsUprightToggle(bool isOn)
    {
        uprightToggle.isOn = isOn;
        SetHandDraggableFlags();
    }

    private void SetHandDraggableFlags()
    {
        HandDraggable.RotationModeEnum rotMode = HandDraggable.RotationModeEnum.Default;
        if (orientToggle.isOn && !uprightToggle.isOn)
        {
            rotMode = HandDraggable.RotationModeEnum.OrientTowardUser;
        }
        else if (uprightToggle.isOn)
        {
            orientToggle.isOn = true;
            rotMode = HandDraggable.RotationModeEnum.OrientTowardUserAndKeepUpright;
        }
        handDraggable.RotationMode = rotMode;
    }

    [ServerCallback]
    public void OnChangedOrientToggle(bool isOn)
    {
        RpcOnChangedOrientToggle(isOn);
    }

    [ClientRpc]
    public void RpcOnChangedOrientToggle(bool isOn)
    {
        orientToggle.isOn = isOn;
        SetHandDraggableFlags();
    }

    [ServerCallback]
    public void OnChangedMoveSurroundBackForthToggle(bool isOn)
    {
        RpcOnChangedMoveSurroundBackForthToggle(isOn);
    }

    [ClientRpc]
    public void RpcOnChangedMoveSurroundBackForthToggle(bool isOn)
    {
        moveSurround.bMove = isOn;
        if (isOn)
        {
            //tubeSpeedLabel.gameObject.SetActive(false);
            //tubeSpeedSlider.gameObject.SetActive(false);
            tubeBackForthLabel.gameObject.SetActive(true);
            tubeBackForthSlider.gameObject.SetActive(true);
            moveSurround.bMove = true;
            OnChangedTubeBackAndForthSpeed(tubeBackForthSlider.value);
        }
        else
        {
            moveSurround.bMove = false;
            moveSurround.Reset();
            tubeBackForthLabel.gameObject.SetActive(false);
            tubeBackForthSlider.gameObject.SetActive(false);
            //tubeSpeedLabel.gameObject.SetActive(true);
            //tubeSpeedSlider.gameObject.SetActive(true);
        }
    }

    [ServerCallback]
    public void OnChangedConstantRetinalSizeToggle(bool isOn)
    {
        RpcOnChangedConstantRetinalSizeToggle(isOn);
    }

    [ClientRpc]
    public void RpcOnChangedConstantRetinalSizeToggle(bool isOn)
    {
        scaleSurround.bConstantSize = isOn;
        scaleAircraft.bConstantSize = isOn;
        constRetSizeToggle.isOn = isOn;
    }

    [ServerCallback]
    public void OnChangedMoveAirplaneBackForthToggle(bool isOn)
    {
        RpcOnChangedMoveAirplaneBackForthToggle(isOn);
    }

    [ClientRpc]
    public void RpcOnChangedMoveAirplaneBackForthToggle(bool isOn)
    {
        moveAirplaneBackForthToggle.isOn = isOn;
        if (isOn)
        {
            //airplanePositionLabel.gameObject.SetActive(false);
            //airplanePositionSlider.gameObject.SetActive(false);
            airplaneBackForthLabel.gameObject.SetActive(true);
            airplaneBackForthSlider.gameObject.SetActive(true);
            airplaneSagittalAmpLabel.gameObject.SetActive(true);
            airplaneSagittalAmpSlider.gameObject.SetActive(true);
            moveAircraft.bMove = true;
            OnChangedAirplaneBackForth(airplaneBackForthSlider.value);
        }
        else
        {
            moveAircraft.bMove = false;
            moveAircraft.Reset();
            airplaneBackForthLabel.gameObject.SetActive(false);
            airplaneBackForthSlider.gameObject.SetActive(false);
            //airplanePositionLabel.gameObject.SetActive(true);
            //airplanePositionSlider.gameObject.SetActive(true);
            airplaneSagittalAmpLabel.gameObject.SetActive(false);
            airplaneSagittalAmpSlider.gameObject.SetActive(false);
            OnChangedAirplanePosition(airplanePositionSlider.value);
        }

    }

    /****************************************
    [ServerCallback]
    public void OnChangedAirplaneConstantSizeToggle(bool isOn)
    {
        RpcOnChangedAirplaneConstantSizeToggle(isOn);
    }

    [ClientRpc]
    public void RpcOnChangedAirplaneConstantSizeToggle(bool isOn)
    {
        scaleAircraft.bConstantSize = isOn;
        airplaneConstantSizeToggle.isOn = isOn;
    }
    ********************************/
    [ServerCallback]
    public void OnChangedSoundToggle(bool isOn)
    {
        RpcOnChangedSoundToggle(isOn);
    }
    [ClientRpc]
    public void RpcOnChangedSoundToggle(bool isOn)
    {
        audioSource.mute = !isOn;
        soundToggle.isOn = isOn;
    }
   
    [ServerCallback]
    public void OnChangedMeshToggle(bool isOn)
    {
        RpcOnChangedMeshToggle(isOn);
    }
    [ClientRpc]
    public void RpcOnChangedMeshToggle(bool isOn)
    {
        spatialMappingManager.DrawVisualMeshes = isOn;
        meshToggle.isOn = isOn;
    }

    [ServerCallback]
    public void OnClickedReset()
    {
        RpcOnClickedReset();
    }

    [ClientRpc]
    public void RpcOnClickedReset()
    {
        constRetSizeToggle.isOn = false;
        moveTubeBackForthToggle.isOn = false;
        moveSurround.Reset();
        scaleSurround.Reset();
        tubeSpeedSlider.value = meanTubeSpeed;
        tubeBackForthSlider.value = tubeDepthSpeed;
        //airplaneConstantSizeToggle.isOn = false;
        moveAirplaneBackForthToggle.isOn = false;
        moveAircraft.Reset();
        scaleAircraft.Reset();
        holoSizeSlider.value = 0f;
        airplaneSpeedSlider.value = airplaneSpeed;
        airplaneBackForthSlider.value = airplaneDepthSpeed;
        airplanePositionSlider.value = airplanePosition;
        airplaneSagittalAmpSlider.value = airplaneDepthAmp;
        airplaneYawSlider.value = airplaneYaw;
        airplanePitchSlider.value = airplanePitch;
        propellerSpeedSlider.value = propellerSpeed;
        soundToggle.isOn = false;

    }

}
