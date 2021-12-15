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
    public static SerialPortReader instance = null;

    public GameObject MasterObject;
    public Master MasterCode;
    public string LatestRcMsg = "";

    SerialPort SerPort;
    float Delay;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
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

        Debug.Log(PortName);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
            if (Time.time > Delay)
            {
                if (!SerPort.IsOpen)
                {
                    SerPort.Open();
                    MasterCode.ArduinoConnectionActive = false;
                }
                if (SerPort.IsOpen)
                {
                    LatestRcMsg = SerPort.ReadLine();
                    MasterCode.ArduinoConnectionActive = true;
                }
                Delay = Time.time + 0.3f;
                SerPort.DiscardOutBuffer();
                SerPort.DiscardInBuffer();
            }
        }
        catch (Exception)
        {

        }
    }

    public void port_SendData(int Data)
    {
        SerPort.Write(Data.ToString());
    }

    void TestConnection()
    {

    }
}
