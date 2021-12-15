using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Handler : MonoBehaviour
{
    public GameObject CollectiblesGroup;
    public GameObject HealthGroup;
    public GameObject[] HealthSlots;
    public GameObject[] DiamondHolders;

    public GameObject WinningStuff;
    public GameObject LosingStuff;
    Master MasterCode;

    public GameObject[] Enemyes;

    // Start is called before the first frame update
    void Start()
    {
        MasterCode = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
        foreach (GameObject Dimand in DiamondHolders)
        {
            Dimand.SetActive(true);
        }
        foreach (GameObject Helat in HealthSlots)
        {
            Helat.SetActive(true);
        }

        WinningStuff.SetActive(false);
        LosingStuff.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(MasterCode.Collectables == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                DiamondHolders[i].SetActive(true);
            }
        }
        if (MasterCode.Collectables == 1)
        {
            DiamondHolders[0].SetActive(false);
        }
        if (MasterCode.Collectables == 2)
        {
            DiamondHolders[0].SetActive(false);
            DiamondHolders[1].SetActive(false);
        }
        if(MasterCode.Collectables == 3)
        {
            for (int i = 0; i < 2; i++)
            {
                DiamondHolders[i].SetActive(false);
            }
        }

        if (MasterCode.Playerhealth == 3)
        {
            for (int i = 0; i < 2; i++)
            {
                HealthSlots[i].SetActive(true);
            }
        }
        if (MasterCode.Playerhealth == 2)
        {
            HealthSlots[0].SetActive(false);
        }
        if (MasterCode.Playerhealth == 1)
        {
            HealthSlots[0].SetActive(false);
            HealthSlots[1].SetActive(false);
        }
        if (MasterCode.Playerhealth <= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                HealthSlots[i].SetActive(false);
            }
        }
        if (MasterCode.GameLost)
        {
            LosingStuff.SetActive(true);
            for (int i = 0; i < Enemyes.Length; i++)
            {
                Enemyes[i].SetActive(false);
            }
        }
        if (MasterCode.GameWon)
        {
            WinningStuff.SetActive(true);
            for (int i = 0; i < Enemyes.Length; i++)
            {
                Enemyes[i].SetActive(false);
            }
        }
    }
}
