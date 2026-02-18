using System.Collections;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ResourceLoadAsync("sandwich"));
        }
    }
    IEnumerator ResourceLoadAsync(string name)
    {
        ResourceRequest rr = Resources.LoadAsync(name);
        yield return rr;

        if (rr.isDone)
        {
            Instantiate(rr.asset);
        }
    }
}
