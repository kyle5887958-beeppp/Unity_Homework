using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float mouseSensitivity = 1.0f;
    public Transform camTransform;
    public Transform targetTransform;
    private float fRotateVertical = 0.0f;
    Vector3 vGravityVector = Vector3.zero;

    public PlayerInput pInput;
    private InputAction moveAction;
    private InputAction lookAction;

    public CharacterController cc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookAction = pInput.actions["Look"];
        moveAction = pInput.actions["Move"];
    }

    // Update is called once per frame
    void Update()
    {
        FPControl();
    }

    private void FPControl()
    {
        //Movement
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        float fH = moveVector.x;
        float fV = moveVector.y;

        Vector3 moveDirection = transform.forward * fV + transform.right * fH;

        moveDirection.y = 0;
        Vector3 moveAmount = Vector3.zero;

        if (moveDirection != Vector3.zero)
        {
            moveAmount = moveDirection * moveSpeed;
        }

        bool bIsGrounded = cc.isGrounded;
        if (bIsGrounded)
        {
            vGravityVector = Vector3.zero;
        }
        else
        {
            vGravityVector.y += Physics.gravity.y * Time.deltaTime;
        }
        moveAmount += vGravityVector;
        moveAmount *= Time.deltaTime;

        cc.Move(moveAmount);

        //Camera
        camTransform.position = targetTransform.position;
        Vector2 lookVector = lookAction.ReadValue<Vector2>();
        float fMX = lookVector.x * mouseSensitivity;
        float fMY = lookVector.y * mouseSensitivity;

        fRotateVertical = fRotateVertical - (fMY);

        if (fRotateVertical > 50.0f)
            fRotateVertical = 50.0f;
        if (fRotateVertical < -80.0f)
            fRotateVertical = -80.0f;

        transform.Rotate(0, fMX, 0);
        targetTransform.forward = transform.forward;
        targetTransform.Rotate(fRotateVertical, 0, 0, Space.Self);
        camTransform.forward = targetTransform.forward;
    }
}
