using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
    public GameObject IndicatorLight;
    bool Set = true;
    bool PlayerInRange = false;
    float Delay;
    bool Triggered = false;

    public Master MasterCode;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        MasterCode = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
        Delay = Time.time;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > Delay && Triggered)
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
            if(MasterCode.ArduinoConnectionActive)
                MasterCode.SRP_Code.port_SendData(4);
        }   
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInRange = true;
        Triggered = true;
        anim.SetBool("Triggered", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInRange = false;
    }
}
