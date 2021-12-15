using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform BlastPoint;
    public GameObject projectilePrefab;
    public GameObject ShootingParticles;
    public GameObject BlastLight;
    public float Force = 20f;
    public CameraShake cameraShake;

    public Master MasterCode;

    float Delay;


    // Start is called before the first frame update
    void Start()
    {
        ShootingParticles.SetActive(false);
        MasterCode = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
        Delay = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterCode.EMP)
            return;

        if (Input.GetButtonDown("Fire1") || MasterCode.ArduinoInput.Shoot)
        {
            if (Time.time > Delay)
            {
                Shoot();
                Delay = Time.time + 0.3f;
            }
        }
        
    }

    void Shoot()
    {
        ShootingParticles.SetActive(true);
        BlastLight.SetActive(true);
        StartCoroutine(cameraShake.Shake(.10f,.05f));
        GameObject a_projectile = Instantiate(projectilePrefab, BlastPoint.position, BlastPoint.rotation);
        Rigidbody2D rigid = a_projectile.GetComponent<Rigidbody2D>();
        rigid.AddForce(BlastPoint.up * Force, ForceMode2D.Impulse);
    }
}
