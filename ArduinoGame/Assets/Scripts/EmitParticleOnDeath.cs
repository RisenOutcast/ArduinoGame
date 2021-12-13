using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticleOnDeath : MonoBehaviour
{
    public GameObject ParticlesPrefab;

    private void OnDestroy()
    {
        GameObject a_projectile = Instantiate(ParticlesPrefab, transform.position, transform.rotation);
    }
}
