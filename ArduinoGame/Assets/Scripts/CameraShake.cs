using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float Duration, float Magnitude)
    {
        Vector3 OriginalPos = transform.localPosition;

        float Elapsed = 0.0f;

        while (Elapsed < Duration)
        {
            float x = Random.Range(-1f, 1f) * Magnitude;
            float y = Random.Range(-1f, 1f) * Magnitude;

            transform.localPosition = new Vector3(x, y, OriginalPos.z);

            Elapsed += Time.deltaTime;

            yield return null;
            
        }

        transform.localPosition = OriginalPos;
    }
}
