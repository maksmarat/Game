using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    [Header("Objects")]
    public CharacterController controller;
    public GameObject camera1;
    public Transform playerBody;

    float epsilon = 0.1f;

    [Header("Transform")]
    public float speed = 12f;
    public float bonus_speed = 0f;
    public float sprint_speed = 14f;

    private float sprintCd = 0f;

    public float rotationSpeed = 5f;
    public float inversion = 1;

    public float boost_cost = 20f;
    public bool boostCd = true;
    private bool bonus_speed_minusCd = true;

    private bool timeStop = false;
    private bool isShiftPressed = false;

    [Header("Health Settings")]
    public float hp = 100;
    public float hpMax = 100;

    [Header("Energy Settings")]
    public float energy—harge = 20f;
    public float energy—hargeMax = 20f;
    private float doublePressTimeThreshold = 0.5f;

    [Header("Variables")]
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float boost_jump = 1f;
    private float timer_stop = 0f;

    [Header("Ground Wall Settings")]
    public Transform groundCheck;
    public LayerMask groundMask;

    public Transform slipperyWallCheck;
    public LayerMask slipperyWallMask;

    private int slipperyWallJump = 1;

    private bool isGrounded;
    private bool isSlipperyWall;
    private bool isSprinting;

    private Vector3 velocity;
    private Vector3 previousPosition;

    private void Start()
    {

    }
    private void FixedUpdate()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (PlayerPrefs.HasKey("mouseX_rotationSpeed")) rotationSpeed = PlayerPrefs.GetFloat("mouseX_rotationSpeed", 0f);
        if (PlayerPrefs.HasKey("mouse_inversion")) inversion = PlayerPrefs.GetInt("mouse_inversion", 0);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
            if (bonus_speed > 0.1f) bonus_speed -= 1 * Time.deltaTime;
            if (Vector3.Distance(transform.position, previousPosition) < epsilon) bonus_speed = 0;
        }
        previousPosition = transform.position;
    }

    void Update()
    {
        isGrounded = (Physics.CheckSphere(groundCheck.position, 0.4f, groundMask) || Physics.CheckSphere(groundCheck.position, 0.4f, slipperyWallMask));
        isSlipperyWall = Physics.CheckSphere(slipperyWallCheck.position, 1f, slipperyWallMask);

        if (isSlipperyWall == false) slipperyWallJump = 1;
        if (isSlipperyWall) gravity = -3;
        else gravity = -9.8f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.sqrMagnitude != 0 && !isGrounded)
        {
            if (isSprinting) energy—harge -= Time.deltaTime;
            else energy—harge -= 2 * Time.deltaTime;
        }

        if (move.sqrMagnitude == 0)
        {
            timer_stop += Time.deltaTime;
            if (timer_stop > 1f)
            {
                timer_stop = 0;
                bonus_speed = 0;
            }
        }
        else timer_stop = 0;

        if (energy—harge <= 0 && bonus_speed_minusCd)
        {
            StartCoroutine(noEnergy—harge());
            energy—harge = 0;
        }

        controller.Move(move * (speed + bonus_speed) * Time.deltaTime);

        // Á‡ÏÂ‰ÎÂÌËÂ ‚ÂÏÂÌË
        if (Input.GetKeyDown(KeyCode.F) && energy—harge > sprintCd)
        {
            timeStop = !timeStop;
        }

        if (timeStop && energy—harge > sprintCd)
        {
            sprintCd = 0f;
            Time.timeScale = 0.4f;
            energy—harge -= 8 * Time.deltaTime;
        }
        else
        {
            timeStop = false;
            if (Time.timeScale == 0.4f) Time.timeScale = 1f;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || (isSlipperyWall && slipperyWallJump > 0)))
        {
            //ÓÚÒÍÓÍ ÓÚ ÒÚÂÌ˚
            if (isSlipperyWall && slipperyWallJump > 0)
            {
                bonus_speed += 2;
                slipperyWallJump--;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.8f * boost_jump * ((15 + speed + bonus_speed) / 15));
            }
            else
            {
                //Ô˚ÊÓÍ Ò ÁÂÏÎË
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity * boost_jump * ((40 + speed + bonus_speed) / 40));
            }
            if (boost_jump > 2)
            {
                bonus_speed += 12;
                boost_jump = 1;
            }  
        }

        float mouseX = inversion * Input.GetAxis("Mouse X");
        playerBody.Rotate(Vector3.up * mouseX * rotationSpeed);
        // ¡Â„
        if (isSprinting)
        {
            sprintCd = 0f;
            energy—harge -= Time.deltaTime;

            if (energy—harge <= 0f)
            {  
                isSprinting = false;
                energy—harge = 0f;
            } else
            {
                speed = sprint_speed;
            }
        }
        else
        {
            sprintCd = energy—hargeMax / 4;
            speed = 8f;
            energy—harge += Time.deltaTime;

            if (energy—harge >= energy—hargeMax)
            {
                energy—harge = energy—hargeMax;
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && energy—harge > sprintCd + boost_cost && boostCd)
        {
            if (isShiftPressed)
            {
                isShiftPressed = false;
                StartCoroutine(Jump());
                energy—harge -= boost_cost;
            }
            else isShiftPressed = true;

            StartCoroutine(ResetLeftShiftFlag());

        }
        if (Input.GetKey(KeyCode.LeftShift) && energy—harge > sprintCd)
        {
            isSprinting = true;
        }
        else isSprinting = false;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null && !rigidbody.isKinematic)
        {
            if (hit.gameObject.tag == "mobile")
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                rigidbody.velocity = pushDirection * speed * speed / rigidbody.mass;
            }
        }
    }
    //‚Á‡ËÏÓ‰ÂÈÒÚ‚ËÂ Ò ‰Û„ËÏË Ó·˙ÂÍÚ‡ÏË
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "energyAdd")
        {
            Destroy(other.gameObject);
            energy—harge += 10f;
        }

        if (other.gameObject.tag == "boost") boost_jump = 15;

        if (other.gameObject.tag == "jumpPoint")
        {
            Destroy(other.gameObject);
            StartCoroutine(Jump());
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "boost") boost_jump = 1;
    }

    IEnumerator ResetLeftShiftFlag()
    {
        yield return new WaitForSeconds(doublePressTimeThreshold);
        isShiftPressed = false;
    }

    IEnumerator Jump()
    {
        boostCd = !boostCd;
        gravity = -1;
        for (int i = 0; i < 50; i++)
        {

            controller.Move(camera1.transform.forward * 80 * Time.fixedDeltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        bonus_speed += 40f;
        yield return new WaitForSeconds(10f);
        boostCd = !boostCd;
    }
    IEnumerator noEnergy—harge()
    {
        bonus_speed_minusCd = !bonus_speed_minusCd;
        while (bonus_speed > 1f)
        { 
            bonus_speed -= 1f;
            gravity = -9.8f;
            yield return new WaitForSeconds(0.08f);
        }
        bonus_speed_minusCd = !bonus_speed_minusCd;
    }
}

