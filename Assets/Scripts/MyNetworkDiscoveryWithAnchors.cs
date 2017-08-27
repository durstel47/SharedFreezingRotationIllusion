﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using HoloToolkit.Examples.SharingWithUNET;

/// <summary>
/// Inherits from UNet's NetworkDiscovery script. 
/// Adds automatic anchor management on discovery. //disabled by mrd
/// If the script detects that it should be the server then
/// the script starts the anchor creation and export process. /disabled by mrd
/// If the script detects that it should be a client then the 
/// script kicks off the anchor ingestion process.
/// </summary>
public class MyNetworkDiscoveryWithAnchors : NetworkDiscovery
{
    /// <summary>
    /// This flag gets set when we recieve a broadcast.
    /// if this flag is set then we should not create a server.
    /// </summary>
    public bool receivedBroadcast { get; private set; }

    /// <summary>
    /// Controls how often a broadcast should be sent to clients
    /// looking to join our session.
    /// </summary>
    public int BroadcastInterval = 1000;


    /// <summary>
    /// Keeps track of the IP address of the system that sent the 
    /// broadcast.  We will use this IP address to connect and 
    /// download anchor data.
    /// </summary>
    public string ServerIp { get; private set; }

    /// <summary>
    /// Sanity checks that our scene has everything we need to proceed.
    /// </summary>
    /// <returns>true if we have what we need, false otherwise.</returns>
    private bool CheckComponents()
    {
#if !UNITY_EDITOR
         if (GenericNetworkTransmitter.Instance == null)
        {
            Debug.Log("Need a UNetNetworkTransmitter in the sceneS for sending anchor data");
            return false;
        }
#endif
        if (NetworkManager.singleton == null)
        {
            Debug.Log("Need a NetworkManager in the scene");
            return false;
        }

        return true;
    }

    private void Start()
    {
        // Initializes NetworkDiscovery.
        Initialize();
        //checking for anchors, not needed for me
        if (!CheckComponents())
        {
            Debug.Log("Invalid configuration detected. Network Discovery disabled.");
            Destroy(this);
            return;
        }

        broadcastInterval = BroadcastInterval;

        // Start listening for broadcasts.
        StartAsClient();

        // But if we don't get a broadcast after some time we'll start broadcasting.
        // We randomize how long we wait so that we reduce the chances that everyone joins at
        // once and decides that they are the server.  
        // An alternative would be to create UI for managing who hosts.
        float InvokeWaitTime = 3 * BroadcastInterval + Random.value * 3 * BroadcastInterval;
        Invoke("MaybeInitAsServer", InvokeWaitTime * 0.001f);
    }

    /// <summary>
    /// If we haven't received a broadcast by the time this gets called
    /// we will start broadcasting and start creating an anchor.
    /// </summary>
    private void MaybeInitAsServer()
    {
        // If we Recieved a broadcast then we should not start as a server.
        if (receivedBroadcast)
        {
            return;
        }

        Debug.Log("Acting as host");
        // StopBroadcast will also 'StopListening'
        StopBroadcast();

        // Starting as a 'host' makes us both a client and a server.
        // There are nuances to this in UNet's sync system, so do make sure
        // to test behavior of your networked objects on both a host and a client 
        // device.
        NetworkManager.singleton.StartHost();

        // Start broadcasting for other clients.
        StartAsServer();
        //accees to holo menu items start only after the server has started

        //As beeing shift holoCollection box to the right side
        Transform holoCollectionBoxTrans = GameObject.Find("HoloCollectionBox").transform;
        //put some emperical values (was to lazy to do the calculations
        holoCollectionBoxTrans.localPosition = new Vector3(0.32f, 0f, 0.5f);
        holoCollectionBoxTrans.localRotation = Quaternion.AngleAxis(9f, Vector3.up);
        Transform holoCanvasTrans = GameObject.Find("HoloCanvas").transform;
        holoCanvasTrans.localPosition = new Vector3(-0.23f, 0f, 1f);
        holoCanvasTrans.localRotation = Quaternion.AngleAxis(0f, Vector3.up);
        //creating anchor not needed for me
#if !UNITY_EDITOR
        // Start creating an anchor.
        //UNetAnchorManager.Instance.CreateAnchor(); //I do not need the anchor for UWP (their is'nt any)
#else
        Debug.LogWarning("This script will need modification to work in the Unity Editor");
#endif
    }

    /// <summary>
    /// Called by UnityEngine when a broadcast is received. 
    /// </summary>
    /// <param name="fromAddress">When the broadcast came from</param>
    /// <param name="data">The data in the broad cast. Not currently used, but could
    /// be used for differntiating rooms or similar.</param>
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        // If we've already recieved a broadcast then we've already set everything up.
        if (receivedBroadcast)
        {
            return;
        }

        Debug.Log("Acting as client");

        receivedBroadcast = true;

        // Stop listening for more broadcasts.
        StopBroadcast();

        // Let the network manager know which address we want to attach to.
        NetworkManager.singleton.networkAddress = fromAddress;

        // We have to parse the server IP to make the string friendly to the windows APIs.
        ServerIp = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1);

#if !UNITY_EDITOR
        // Tell the network transmitter the IP to request anchor data from if needed.
        GenericNetworkTransmitter.Instance.SetServerIP(ServerIp);
#else
        Debug.LogWarning("This script will need modification to work in the Unity Editor");
#endif
        // And join the networked experience as a client.
        Debug.Log("Start Client");
        NetworkManager.singleton.StartClient();
        Debug.Log("Client Started");
     }
}
