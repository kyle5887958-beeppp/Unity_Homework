using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Sandwich : MonoBehaviour
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
        eatAudio = GetComponentInChildren<AudioSource>();
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    // Update is called once per frame
    void Update()
    {
        RotateSandwich();
    }
    Vector3 GetChildrenCenter()
    {
        Vector3 targetRoot = gameObject.GetComponentInChildren<Renderer>().bounds.center;
        return targetRoot;
    }

    public void RotateSandwich()
    {
        transform.RotateAround(center, Vector3.up, 20 * Time.deltaTime);
    }

    public void ResetSandwich()
    {
        eaten = false;
        renderers = GetComponentsInChildren<Renderer>(true);
        center = GetChildrenCenter();
        gameObject.SetActive(true);
        SetVisible(true);
    }
    public void Eat()
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
        ObjectManager.Instance().RemoveSandwich(gameObject);
    }
    void SetVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}
