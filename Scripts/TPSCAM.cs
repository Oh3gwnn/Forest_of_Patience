using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCAM : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;

    Animator animator;
    private Rigidbody rigid;

    public static bool IsJumping;
    public static bool IsCrouch;
    public static bool IsRun;
    public static bool IsFalling;
    public static bool IsPause;

    public float speed = 3f;
    public float JumpPower = 4f;
    public float time;
    public float time2;

    public AudioSource mySfx;
    public AudioClip jumpSfx;

    Vector3 Savepoint;

    private void Start()
    {
        animator = characterBody.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        IsJumping = false;
        IsCrouch = false;
        IsRun = false;
        IsFalling = false;
        IsPause = false;
    }

    private void Update()
    {
        LookAround();
        Move();
        if (!IsPause)
        {
            Jump();
            RunCrouching();
        }
        else
        {
            Pause();
        }
            
        Save();
        CheckFalling();
        Cursor.visible = false;
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;
        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 30f);
        }
        else
        {
            x = Mathf.Clamp(x, 300f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        characterBody.forward = lookForward;

        if (!IsPause)
        {
            transform.position += moveDir * Time.deltaTime * speed;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsJumping)
            {
                IsJumping = true;
                rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            }
            else
            {
                return;
            }
        }
    }

    private void RunCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            IsCrouch = true;
            IsRun = false;
            speed = 1.5f;
            JumpPower = 5f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            IsRun = true;
            IsCrouch = false;
            speed = 5f;
            JumpPower = 4f;
        }
        else
        {
            IsCrouch = false;
            IsRun = false;
            speed = 3f;
            JumpPower = 4f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
            time = 0;
            mySfx.PlayOneShot(jumpSfx);
            if (IsFalling == true)
            {
                IsFalling = false;
                IsPause = true;
            }
        }
    }

    private void Save()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Savepoint = transform.position;
        }
        else if (Input.GetKey(KeyCode.T))
        {
            transform.position = Savepoint;
        }
    }

    private void CheckFalling()
    {
        if (!Physics.BoxCast(transform.position, transform.lossyScale / 2.0f, Vector3.down, transform.rotation, 2f))
        {
            time += Time.deltaTime;

            if (time >= 1.3)
            {
                IsFalling = true;
                IsJumping = true;
            }
        }
    }

    private void Pause()
    {
        time2 += Time.deltaTime;

        if (time2 >= 0.6)
        {
            IsPause = false;
            time2 = 0;
        }
    }
}
