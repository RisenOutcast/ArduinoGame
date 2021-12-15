using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
    public GameObject IndicatorLight;
    bool Set = true;
    bool PlayerInRange = false;
    float Delay;

    public Master MasterCode;

    // Start is called before the first frame update
    void Start()
    {
        MasterCode = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
        Delay = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > Delay)
        {
            IndicatorLight.SetActive(Set);
            Set = !Set;
            Delay = Time.time + 0.2f;
        }
    }

    public void DestroySelf()
    {
        if (PlayerInRange) { 
            MasterCode.EMP = true;
            MasterCode.SRP_Code.port_SendData(4);
        }   
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInRange = false;
    }
}
