using UnityEngine;
using UnityEngine.InputSystem;

public class TPController : MonoBehaviour
{
    public TPCamera tpCamera;
    public float moveSpeed;
    public float rotateSpeed;
    Vector3 vGravityVector = Vector3.zero;

    private Animator _animator;
    private CharacterController _cc;

    public PlayerInput pInput;
    private InputAction moveAction;
    private InputAction lookAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = pInput.actions["Move"];
        lookAction = pInput.actions["Look"];
        _cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        TPControl();
        tpCamera.UpdateFollowPt();
    }

    private void TPControl()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        float fH = moveVector.x;
        float fV = moveVector.y;

        Transform camTransform = tpCamera.transform;

        Vector3 moveDirection = camTransform.forward * fV + camTransform.right * fH;
        moveDirection.y = 0;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            //TODO: Idle Animation
        }
        Vector3 moveAmount = Vector3.zero;
        if(moveDirection != Vector3.zero)
        {
            moveAmount = moveDirection.normalized * moveSpeed;
        }
        bool bIsGrounded = _cc.isGrounded;
        if (bIsGrounded)
        {
            vGravityVector = Vector3.zero;
        }
        else
        {
            vGravityVector += Physics.gravity * Time.deltaTime;
        }
        moveAmount += vGravityVector;
        moveAmount *= Time.deltaTime;
        _cc.Move(moveAmount);
    }
}
