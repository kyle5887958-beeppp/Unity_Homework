using System.Collections;
using UnityEngine;

public class padoru : MonoBehaviour
{
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = target.position - transform.position;

        if (distance.magnitude < 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position,target.position,1.0f*Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, target.forward, 1.0f * Time.deltaTime);
        }
        else
        {
        transform.forward = distance;
            transform.position += transform.forward * 1.5f * Time.deltaTime;
        }
    }
}
