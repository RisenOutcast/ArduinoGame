using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;// How much to smooth out the movement
    

    public float speed = 150f;
    public float RotationSpeed = 100f;
    public float PlayerRotation = 0;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    Animator anim;
    public GameObject TankSprite;

    public GameObject Cannon;
    public GameObject Lights;
    public GameObject GroundParticle_BackLeft;
    public GameObject GroundParticle_BackRight;
    public GameObject GroundParticle_FrontLeft;
    public GameObject GroundParticle_FrontRight;

    public GameObject MasterObject;
    public Master MasterCode;

    bool Rotating = false;
    bool Moving = false;


    private void Awake()
    {
        anim = TankSprite.GetComponent<Animator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        GroundParticle_BackLeft.SetActive(false);
        GroundParticle_BackRight.SetActive(false);
        GroundParticle_FrontLeft.SetActive(false);
        GroundParticle_FrontRight.SetActive(false);

        MasterCode = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();
        MasterCode.SRP_Code.port_SendData(0);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        verticalMove = Input.GetAxisRaw("Vertical") * speed;
        Cannon.transform.position = transform.position;
    }

    private void FixedUpdate()
    {
        if(MasterCode == null)
            MasterCode = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>();

        if (MasterCode.EMP)
        {
            Lights.SetActive(false);
            anim.SetBool("Move_Forward", false);
            return;
        }
        else
        {
            Lights.SetActive(true);
        }

        if (MasterCode.ArduinoInput.MoveForward)
            verticalMove = 3;
        if (MasterCode.ArduinoInput.MoveBack)
            verticalMove = -3;
        if (MasterCode.ArduinoInput.MoveLeft)
            horizontalMove = 3;
        if (MasterCode.ArduinoInput.MoveRight)
            horizontalMove = -3;

        Move(verticalMove * Time.fixedDeltaTime);
        Rotate(horizontalMove * Time.fixedDeltaTime);

        if (Input.GetButton("TurnTurretRight"))
        {
            MoveCannon(-2);
        }
        if (Input.GetButton("TurnTurretLeft"))
        {
            MoveCannon(2);
        }
        transform.eulerAngles = Vector3.forward * PlayerRotation;
        Debug.Log(verticalMove);
    }

    public void MoveCannon(float move)
    {
        Cannon.transform.eulerAngles += Vector3.forward * move;
    }

    void Move(float move)
    {
        Vector3 movement = transform.up * move * movementSmoothing;
        transform.position += movement;
        if(move < 0 && !Rotating)
        {
            Moving = true;
            anim.SetBool("Move_Forward", true);
            GroundParticle_BackLeft.SetActive(false);
            GroundParticle_BackRight.SetActive(false);
            GroundParticle_FrontLeft.SetActive(true);
            GroundParticle_FrontRight.SetActive(true);
        }
        else if(move > 0 && !Rotating)
        {
            Moving = true;
            anim.SetBool("Move_Forward", true);
            GroundParticle_BackLeft.SetActive(true);
            GroundParticle_BackRight.SetActive(true);
            GroundParticle_FrontLeft.SetActive(false);
            GroundParticle_FrontRight.SetActive(false);
        }
        else if(!Rotating)
        {
            Moving = false;
            anim.SetBool("Move_Forward", false);
            GroundParticle_BackLeft.SetActive(false);
            GroundParticle_BackRight.SetActive(false);
            GroundParticle_FrontLeft.SetActive(false);
            GroundParticle_FrontRight.SetActive(false);
        }
    }

    void Rotate(float move)
    {
        PlayerRotation -= move * RotationSpeed;
        if(move != 0 && !Moving)
        {
            Rotating = true;
            anim.SetBool("Move_Forward", true);
            GroundParticle_BackLeft.SetActive(true);
            GroundParticle_BackRight.SetActive(true);
            GroundParticle_FrontLeft.SetActive(false);
            GroundParticle_FrontRight.SetActive(false);
        }
        else if (!Moving)
        {
            Rotating = false;
            anim.SetBool("Move_Forward", false);
            GroundParticle_BackLeft.SetActive(false);
            GroundParticle_BackRight.SetActive(false);
            GroundParticle_FrontLeft.SetActive(false);
            GroundParticle_FrontRight.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyProjectile")
        {
            MasterCode.Playerhealth--;
            MasterCode.SRP_Code.port_SendData(1);
            if (MasterCode.Playerhealth <= 0)
            {
                Destroy(gameObject);
                Destroy(Cannon);
            }
        }
    }
}
