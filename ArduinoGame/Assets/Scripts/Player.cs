using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;// How much to smooth out the movement
    
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    public float speed = 150f;
    public float RotationSpeed = 100f;
    public float PlayerRotation = 0;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    Animator anim;
    public GameObject TankSprite;

    public GameObject Cannon;

    public GameObject GroundParticle_BackLeft;
    public GameObject GroundParticle_BackRight;
    public GameObject GroundParticle_FrontLeft;
    public GameObject GroundParticle_FrontRight;

    bool Rotating = false;
    bool Moving = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        anim = TankSprite.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GroundParticle_BackLeft.SetActive(false);
        GroundParticle_BackRight.SetActive(false);
        GroundParticle_FrontLeft.SetActive(false);
        GroundParticle_FrontRight.SetActive(false);
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
        //MoveHorizontal(horizontalMove * Time.fixedDeltaTime);
        //MoveVertical(verticalMove * Time.fixedDeltaTime);
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
        Debug.Log(Cannon.transform.eulerAngles.z);
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
}
