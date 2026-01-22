using UnityEngine;
using UnityEngine.VFX;

public class HolyCup : MonoBehaviour
{
    private static HolyCup _instance = null;
    public static HolyCup Instance() { return _instance; }
    public GameObject myHolyCup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _instance = this;
    }

    public void SpawnHolyCup(Vector3 pos)
    {
        GameObject HCFx = Instantiate(myHolyCup);
        HCFx.transform.position = pos;
    }
}
