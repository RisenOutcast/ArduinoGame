using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool Moving;
    bool Aiming;

    [Range(1, 15)]
    [SerializeField]
    private float viewRadius = 11;
    [SerializeField]
    private float detectionCheckDelay = 0.1f;
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private LayerMask playerLayerMask;
    [SerializeField]
    private LayerMask visibilityLayer;

    public Transform BlastPoint;
    public GameObject projectilePrefab;
    public GameObject ShootingParticles;
    public GameObject BlastLight;

    public float turretRotationSpeed = 150;

    public int Health = 3;
    public float FireRate;
    public float Force = 10f;
    public CameraShake cameraShake;

    public float patrolDelay = 4;
    public GameObject Cannon;

    [SerializeField]
    private Vector2 randomDirection = Vector2.zero;
    [SerializeField]
    private float currentPatrolDelay;
    public float fieldOfVisionForShooting = 60;

    [field: SerializeField]
    public bool TargetVisible { get; private set; }
    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            TargetVisible = false;
        }
    }

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    private void Update()
    {
        if (Target != null)
            TargetVisible = CheckTargetVisible();
        if (TargetVisible)
        {
            PerformShootAction();
        }
        else
        {
            PerformPatrolAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            Health--;
            if(Health <= 0)
                Destroy(gameObject);
        }
    }

    private bool CheckTargetVisible()
    {
        var result = Physics2D.Raycast(transform.position, Target.position - transform.position, viewRadius, visibilityLayer);
        if (result.collider != null)
        {
            return (playerLayerMask & (1 << result.collider.gameObject.layer)) != 0;
        }
        return false;
    }

    private void DetectTarget()
    {
        if (Target == null)
            CheckIfPlayerInRange();
        else if (Target != null)
            DetectIfOutOfRange();
    }

    private void DetectIfOutOfRange()
    {
        if (Target == null || Target.gameObject.activeSelf == false || Vector2.Distance(transform.position, Target.position) > viewRadius + 1)
        {
            Target = null;
        }
    }

    private void CheckIfPlayerInRange()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, viewRadius, playerLayerMask);
        if (collision != null)
        {
            Target = collision.transform;
        }
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionCheckDelay);
        DetectTarget();
        StartCoroutine(DetectionCoroutine());

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    public void PerformPatrolAction()
    {
        float angle = Vector2.Angle(Cannon.transform.right, randomDirection);
        if (currentPatrolDelay <= 0 && (angle < 2))
        {
            randomDirection = Random.insideUnitCircle;
            currentPatrolDelay = patrolDelay;
        }
        else
        {
            if (currentPatrolDelay > 0)
                currentPatrolDelay -= Time.deltaTime;
            else
                Aim((Vector2)Cannon.transform.position + randomDirection);
        }
    }

    public void Aim(Vector2 inputPointerPosition)
    {
        var turretDirection = (Vector3)inputPointerPosition - Cannon.transform.position;

        var desiredAngle = Mathf.Atan2(turretDirection.y, turretDirection.x) * Mathf.Rad2Deg;

        var rotationStep = turretRotationSpeed * Time.deltaTime;

        Cannon.transform.rotation = Quaternion.RotateTowards(Cannon.transform.rotation, Quaternion.Euler(0, 0, desiredAngle), rotationStep);
    }

    public void PerformShootAction()
    {
        if (TargetInFOV())
        {
            //HandleMoveBody(Vector2.zero);
            if (Time.time > FireRate)
            {
                HandleShoot();
                FireRate = Time.time + 1f;
            }
        }

        Aim(Target.position);
    }

    private bool TargetInFOV()
    {
        var direction = Target.position - Cannon.transform.position;
        if (Vector2.Angle(Cannon.transform.right, direction) < fieldOfVisionForShooting / 2)
        {
            return true;
        }
        return false;
    }

    public void HandleShoot()
    {
        ShootingParticles.SetActive(true);
        BlastLight.SetActive(true);
        StartCoroutine(cameraShake.Shake(.10f, .05f));
        GameObject a_projectile = Instantiate(projectilePrefab, BlastPoint.position, BlastPoint.rotation);
        Rigidbody2D rigid = a_projectile.GetComponent<Rigidbody2D>();
        rigid.AddForce(BlastPoint.up * Force, ForceMode2D.Impulse);

    }
}
