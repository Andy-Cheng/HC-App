using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }
    public static void SendPlayerChoice(int deviceNumber)
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendPlayerChoice))
        {
            _packet.Write(deviceNumber);
            SendTCPData(_packet);
        }

    }


    public static void NotifyCalibrationDone()
    {
        Debug.Log("Notfiy calibration done");
        using (Packet _packet = new Packet((int)ClientPackets.notifyCalibrationDone))
        {
            SendTCPData(_packet);
        }

    }

    public static void NotifyStageStateChange(int newStage)
    {
        using (Packet _packet = new Packet((int)ClientPackets.notifyStageStateChange))
        {
            _packet.Write(newStage);
            SendTCPData(_packet);
        }

    }

    public static void SendPlayerEnterStage()
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendPlayerEnterStage))
        {
            SendTCPData(_packet);
        }

    }

    public static void SendPlayerArrived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendPlayerArrived))
        {
            SendTCPData(_packet);
        }

    }

    #endregion
}
