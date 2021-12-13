using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjectTime : MonoBehaviour
{
    public int Time = 2;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
