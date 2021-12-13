using UnityEngine;
using System.Collections;

namespace RO
{
    public class CameraFollow : MonoBehaviour
    {

        public float interpVelocity;
        public float minDistance;
        public float followDistance;
        public GameObject Pelaaja;
        public Vector3 offset;
        Vector3 targetPos;

        public bool followPrefab;

        // Use this for initialization
        void Start()
        {
            targetPos = transform.position;
            if (followPrefab == true)
            {
                Pelaaja = GameObject.FindWithTag("Player");
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Pelaaja)
            {
                Vector3 posNoZ = transform.position;
                posNoZ.z = Pelaaja.transform.position.z;

                Vector3 targetDirection = (Pelaaja.transform.position - posNoZ);

                interpVelocity = targetDirection.magnitude * 15f;

                targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

                transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.30f);

            }
        }
    }
}