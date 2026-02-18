using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SandwichRotate : MonoBehaviour
{
    public GameObject target;
    Vector3 center;
    public AudioSource eatAudio;
    bool eaten = false;
    private Renderer[] renderers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        center = GetChildrenCenter();
        eatAudio = GetComponent<AudioSource>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center, Vector3.up, 30 * Time.deltaTime);
    }
    Vector3 GetChildrenCenter()
    {
        Vector3 targetRoot = target.GetComponentInChildren<Renderer>().bounds.center;
        return targetRoot;
    }
    public void EatSound()
    {
        if (eaten) return;
        eaten = true;
        eatAudio.Play();
        SetVisible(false);
        StartCoroutine(DisableAfterSound());
    }
    private IEnumerator DisableAfterSound()
    {
        yield return new WaitForSeconds(eatAudio.clip.length);
        gameObject.SetActive(false);
    }
    void SetVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}
