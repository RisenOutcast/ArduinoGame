using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    Master MasterCode;

    // Start is called before the first frame update
    void Start()
    {
        MasterCode = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MasterCode.Collectables++;
            if(MasterCode.ArduinoConnectionActive)
                MasterCode.SRP_Code.port_SendData(3);
            Destroy(gameObject);
        }
    }
}
