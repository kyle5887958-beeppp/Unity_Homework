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
        Run();
    }
    void Run()
    {
        anim.SetBool("Run", fSpeed > 0);
        if (target == null || target.Length == 0)
        {
            return;
        }
        if (currentTargetIndex >= target.Length)
        {
            if (!holyCupSpawned)
            {
                holyCupSpawned = true;
                HolyCup.Instance().SpawnHolyCup(transform.position);
            }
            MoveToFinalCamPos();
            fSpeed = 0;
            return;
        }
        Transform currentTarget = target[currentTargetIndex];
        if (currentTarget == null)
        {
            return;
        }
        Vector3 targetVector = currentTarget.position - transform.position;
        targetVector.y = 0;//忽略y軸
        float fMoveAmount = fSpeed * Time.deltaTime;
        float fDistanceToTarget = targetVector.magnitude;
        transform.forward = targetVector;//forward為單位向量
        if (currentTargetIndex <= target.Length - 1)
        {
            if (fDistanceToTarget < fMoveAmount)
            {
                Vector3 newPosition = currentTarget.position;
                newPosition.y = transform.position.y;
                transform.position = newPosition;
                currentTargetIndex++;
            }
            if (fDistanceToTarget >= fMoveAmount)
            {
                transform.position += transform.forward * fMoveAmount;
            }
        }
        for (int i = 0; i < sandwichs.Length; i++)
        {
            var s = sandwichRotates[i];
            float distance = Vector3.Distance(s.transform.position, transform.position);
            if (distance < 0.6f)
            {
                s.EatSound();
                //s.SetActive(false);
            }
        }
    }
    public void MoveToFinalCamPos()
    {
        cam1.transform.position = Vector3.Lerp(cam1.transform.position, finalCamPos.position, 2.0f*Time.deltaTime);
        cam1.transform.forward = Vector3.Lerp(cam1.transform.forward, finalCamPos.forward, 2.0f * Time.deltaTime);
    }
}

