using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float fSpeed = 4.0f;
    public Transform[] target;
    public GameObject[] sandwichs;
    public int currentTargetIndex = 0;

    bool holyCupSpawned = false;
    Animator anim;

    public Camera cam1;
    public Transform finalCamPos;

    SandwichRotate[] sandwichRotates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        sandwichRotates = new SandwichRotate[sandwichs.Length];
        for (int i = 0; i < sandwichs.Length; i++)
        {
            sandwichRotates[i] = sandwichs[i].GetComponent<SandwichRotate>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        float fX = Input.GetAxis("Horizontal");
        float fZ = Input.GetAxis("Vertical");

        float fMoveX = fX * fSpeed * Time.deltaTime;
        float fMoveZ = fZ * fSpeed * Time.deltaTime;
        Vector3 vMoveForward = transform.forward * fMoveZ;
        Vector3 vMoveRight = transform.right * fMoveX;
        transform.position += vMoveForward + vMoveRight;
    }
}

