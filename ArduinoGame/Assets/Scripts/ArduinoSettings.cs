using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArduinoSettings : MonoBehaviour
{
    GameObject MasterObject;
    public Master MasterCode;

    public TMP_Dropdown PortChoices_Dd;

    public GameObject SuccessSign;
    public GameObject FailureSign;

    // Start is called before the first frame update
    void Start()
    {
        if (MasterObject == null)
            MasterObject = GameObject.FindWithTag("Master");
        MasterCode = MasterObject.GetComponent<Master>();
    }

    void Update()
    {
        if (MasterCode.ArduinoConnectionActive)
        {
            SuccessSign.SetActive(true);
            FailureSign.SetActive(false);
        }
        else
        {
            FailureSign.SetActive(true);
            SuccessSign.SetActive(false);
        }
    }
}
