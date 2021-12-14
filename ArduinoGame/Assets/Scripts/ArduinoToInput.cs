using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoToInput : MonoBehaviour
{
    public static ArduinoToInput instance = null;
    public Master MasterCode;
    public SerialPortReader SRP_Code;
    string PreviousMsg;
    public string[] Inputs;

    public bool Shoot = false;
    public bool MoveForward = false;
    public bool MoveBack = false;
    public bool MoveLeft = false;
    public bool MoveRight = false;
    public int CannonRotation = 0;

    ///Shoot;CannonRotation;MoveForward;MoveBack;MoveRight;MoveLeft/

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(SRP_Code.LatestRcMsg.StartsWith("/") && SRP_Code.LatestRcMsg.EndsWith("/")){
            string NewStr = SRP_Code.LatestRcMsg.Replace("/", "");
            Inputs = NewStr.Split(';');
            Shoot = Convert.ToBoolean(int.Parse(Inputs[0]));
            CannonRotation = int.Parse(Inputs[1]);
            MoveForward = Convert.ToBoolean(int.Parse(Inputs[2]));
            MoveBack = Convert.ToBoolean(int.Parse(Inputs[3]));
            MoveRight = Convert.ToBoolean(int.Parse(Inputs[4]));
            MoveLeft = Convert.ToBoolean(int.Parse(Inputs[5]));
        }
    }
}
