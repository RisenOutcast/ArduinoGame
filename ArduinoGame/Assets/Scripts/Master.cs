//Filename:         Master.cs
//Author:           RisenOutcast
//Description:      All important things that need to be consistent through scenes.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Master : MonoBehaviour
{
    public static Master instance = null;
    public int Playerhealth = 0;
    public int Collectables = 0;
    public string SerialPortName = "";

    public bool ArduinoConnectionActive = false;
    public GameObject PortReader_Obj;
    public SerialPortReader SRP_Code;
    public ArduinoToInput ArduinoInput;

    public bool EMP = false;
    bool SimulateAlreadyRunning = false;
    public bool GameWon = false;
    public bool GameLost = false;
    bool WaitForSignal;

    // Start is called before the first frame update
    void Start()
    {
        Playerhealth = 3;
        GameWon = false;
        GameLost = false;
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
        if (EMP && !SimulateAlreadyRunning)
        {
            StartCoroutine(SimulateEMP());
        }
        if(Collectables >= 3)
        {
            GameWon = true;
        }
        if(Playerhealth < 1)
        {
            GameLost = true;
        }
    }

    IEnumerator SimulateEMP()
    {
        SimulateAlreadyRunning = false;
        yield return new WaitForSeconds(3F);

        EMP = false;
        SimulateAlreadyRunning = false;
    }
}
