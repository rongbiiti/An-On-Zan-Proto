using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    private Animator animator;
    private float h;                                      // Horizontal Axis.
    private float v;                                      // Vertical Axis.
    private float speed, speedSeeker;               // Moving speed.
    private float walkSpeed = 0.3f;                 // Default walk speed.
    private float runSpeed = 1.0f;                   // Default run speed.
    private float sprintSpeed = 2.0f;                // Default sprint speed.
    private float speedDampTime = 0.01f;              // Default damp time to change the animations based on current speed.
    private FirstPersonAIO _fpsController;
    public bool isShinkuuha;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _fpsController = GetComponent<FirstPersonAIO>();
        runSpeed = _fpsController.walkSpeed;
        sprintSpeed = _fpsController.sprintSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && !animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            animator.SetBool("Attack", true);
            StartCoroutine("AttackBoolControll");
        }
        if (Input.GetButtonDown("Shinkuuha") && !animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            animator.SetBool("Attack", true);
            StartCoroutine("AttackBoolControll");
            isShinkuuha = true;
        }
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");
        //animator.SetFloat("H", h, 0.1f, Time.deltaTime);
        //animator.SetFloat("V", v, 0.1f, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //MovementManagement(animator.GetFloat("H"), animator.GetFloat("V"));
    }

    void MovementManagement(float horizontal, float vertical)
    {
        // Set proper speed.
        Vector2 dir = new Vector2(horizontal, vertical);
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
        // This is for PC only, gamepads control speed via analog stick.
        speedSeeker += Input.GetAxis("Mouse ScrollWheel");
        speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
        speed *= speedSeeker;
        if (Input.GetButton("Sprint") && ((h != 0) || (v != 0))) {
            speed = sprintSpeed;
        }

        animator.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);
    }

    private IEnumerator AttackBoolControll()
    {
        yield return new WaitForSeconds(0.9f);
        animator.SetBool("Attack", false);
    }
}
