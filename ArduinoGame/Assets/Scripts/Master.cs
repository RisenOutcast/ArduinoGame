//Filename:         Master.cs
//Author:           RisenOutcast
//Description:      All important things that need to be consistent through scenes.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Master : MonoBehaviour
{
    public static Master instance = null;
    public int Playerhealth;
    public string SerialPortName = "";

    public bool ArduinoConnectionActive = false;
    public GameObject PortReader_Obj;
    public SerialPortReader SRP_Code;
    public ArduinoToInput ArduinoInput;

    // Start is called before the first frame update
    void Start()
    {
        Playerhealth = 100;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Playerhealth < 0)
        {
            Playerhealth = 0;
        }
    }
}
