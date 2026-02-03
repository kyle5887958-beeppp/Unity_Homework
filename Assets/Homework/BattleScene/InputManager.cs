using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86;

public class InputManager : MonoBehaviour
{
    public float moveSpeed = 6.0f;
    public float mouseSensitivity = 2.0f;
    public Transform camTransform;
    public Transform targetTransform;
    float fRotateVertical = 0.0f;
    public CharacterController cc;

    //old
    public Transform[] target;
    public GameObject[] sandwichs;
    public int currentTargetIndex = 0;
    SandwichRotate[] sandwichRotates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sandwichRotates = new SandwichRotate[sandwichs.Length];
        for (int i = 0; i < sandwichs.Length; i++)
        {
            sandwichRotates[i] = sandwichs[i].GetComponent<SandwichRotate>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Movement
        float fX = Input.GetAxis("Horizontal");
        float fZ = Input.GetAxis("Vertical");

        Vector3 vMoveForward = transform.forward * fZ;
        Vector3 vMoveRight = transform.right * fX;
        Vector3 vMoveDirection = vMoveForward + vMoveRight;

        vMoveDirection.y = 0;
        Vector3 vMoveAmount = Vector3.zero;

        if (vMoveDirection != Vector3.zero)
        {
            vMoveAmount = vMoveDirection * moveSpeed;
            //transform.position += vMoveAmount;
        }
        cc.SimpleMove(vMoveAmount);

        //Camera Rotation
        camTransform.position = targetTransform.position;
        float fMouseX = Input.GetAxis("Mouse X");
        float fMouseY = Input.GetAxis("Mouse Y");

        fRotateVertical = fRotateVertical - (fMouseY * mouseSensitivity);

        if (fRotateVertical > 50.0f)
            fRotateVertical = 50.0f;
        if (fRotateVertical < -80.0f)
            fRotateVertical = -80.0f;

        transform.Rotate(0, fMouseX, 0);
        targetTransform.forward = transform.forward;
        targetTransform.Rotate(fRotateVertical, 0, 0, Space.Self);

        camTransform.forward = targetTransform.forward;
    }
}

