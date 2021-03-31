using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void TrackerTransform(Packet _packet)
    {
        int TrackerCount = _packet.ReadInt();
        for (int i = 0; i < TrackerCount; ++i)
        {
            int TrackerID = _packet.ReadInt();
            Vector3 pos = _packet.ReadVector3();
            Quaternion rot = _packet.ReadQuaternion();
            //if (TrackerID == TrackerManager.instance.PlayerTrackerID)
            //{
            //    TrackerManager.instance.OnReceiveCameraTrackerInfo(rot, pos);
            //}
            //else if (TrackerID == TrackerManager.instance.HCOriginTrackerID)
            //{
            //    if(!GeneralManager.instance.HaveCalibrated)
            //    {
            //        GeneralManager.instance.SetHCOrigin(rot, pos);
            //    }
            //}
            //TrackerManager.instance.Trackers[TrackerID].position = pos;
            //TrackerManager.instance.Trackers[TrackerID].rotation = rot;
            DeviceManager.instance.RecieveTrackerTransform(TrackerID, pos, rot);
            Debug.Log($"recieve device {TrackerID} transform from client handle");
        }
    }

    public static void NotifyCountDown(Packet _packet)
    {
        SpaceShipManager.instance.SpawnEnemy();

    }

    public static void SendPlayerChoice(Packet _packet)
    {
        int deviceID = _packet.ReadInt();
        GeneralManager.instance.OnOtherSelect(deviceID);
    }

    public static void SendNextGameState(Packet _packet)
    { 
        int nextGameState = _packet.ReadInt();
        GeneralManager.instance.OnGameStateChange(nextGameState);

    }

    public static void SendNextStageState(Packet _packet)
    {
        int nextStageState = _packet.ReadInt();
        GeneralManager.instance.OnStageStateChange(nextStageState);

    }

    public static void SendDeviceReady(Packet _packet)
    {
        GeneralManager.instance.OnDeviceReady();

    }

    public static void SendPanelInfo(Packet _packet)
    {
        int RedBtn = _packet.ReadInt();
        int BlueBtn = _packet.ReadInt();
        int Slider1 = _packet.ReadInt();
        int Slider2 = _packet.ReadInt();
        int Slider3 = _packet.ReadInt();
        int Slider4 = _packet.ReadInt();
        int x = _packet.ReadInt();
        int y = _packet.ReadInt();
        int degree = _packet.ReadInt();
        Debug.Log($"RedBtn: {RedBtn}, BlueBtn: {BlueBtn}, Slider1: {Slider1}, Slider2: {Slider2}, Slider3: {Slider3}, Slider4: {Slider4}, x: {x}, y: {y}, degree: {degree}");

        DeviceManager.instance.RecievePanelData(new PanelData(RedBtn, BlueBtn, Slider1, Slider2, Slider3, Slider4, x, y, degree));


    }

    public static void SendPlayerScore(Packet _packet)
    {
        int playerScore = _packet.ReadInt();
        PuzzleManager.instance.RecieveCode(playerScore);

    }

    public static void SendPlayerEnterStage(Packet _packet)
    {
        GeneralManager.instance.WhenOtherEnterGame();

    }

    public static void SendGunFire(Packet _packet)
    {
        if (GeneralManager.instance.myChoiceDeviceID == (int)DeviceNum.Shield)
        {
            DefenderManager.instance.OtherPlayerShoot();
        }
        else
        {
            Shooting gunShooting = GeneralManager.instance.propManager.Gun.GetComponent<Shooting>();
            gunShooting.Shoot();
        
        }
    }

    
    //public static void PlayerDisconnected(Packet _packet)
    //{
    //    int _id = _packet.ReadInt();

    //}
}
