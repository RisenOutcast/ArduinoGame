//Filename:         SerialPortReader.cs
//Author:           RisenOutcast
//Description:      Functions for receiving and sending serial port data.

using System.Collections;
using System.Collections.Generic;
using System;
using System.IO.Ports;
using UnityEngine;

public class SerialPortReader : MonoBehaviour
{
    GameObject MasterObject;
    public Master MasterCode;

    SerialPort SerPort;
    float Delay;

    // Use this for initialization
    void Start()
    {
        if (MasterObject == null)
            MasterObject = GameObject.FindWithTag("Master");
        MasterCode = MasterObject.GetComponent<Master>();

        string PortName = "";
        Delay = Time.time;

        // Try to automatically get the correct port
        foreach (string Port in SerialPort.GetPortNames())
        {
            if (Port != "COM1") {
                PortName = Port; 
                break; 
            }
        }

        SerPort = new SerialPort(PortName, 9600, Parity.None, 8, StopBits.One);
        if (!SerPort.IsOpen)
        {
            SerPort.Open();
            SerPort.ReadTimeout = 100;
            SerPort.Handshake = Handshake.None;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > Delay)
        {
            if (!SerPort.IsOpen)
            {
                SerPort.Open();
            }
            if (SerPort.IsOpen)
            {
                Debug.Log(SerPort.ReadLine());
            }
            Delay = Time.time + 0.2f;
        }
    }

    void port_SendData(int Data)
    {
        SerPort.Write(Data.ToString());
    }

    void TestConnection()
    {

    }
}
