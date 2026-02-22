using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EatSandwich : MonoBehaviour
{
    public float fEatDistance = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckEatSandwich();
    }

    public void CheckEatSandwich()
    {
        System.Collections.Generic.List<Sandwich> pList = ObjectManager.Instance().getSandwiches();
        if (pList != null && pList.Count > 0)
        {
            Vector3 v;
            int i;
            for (i = 0; i < pList.Count; i++)
            {
                Sandwich s = pList[i];
                if (s == null)
                {
                    continue;
                }
                v = transform.position - s.transform.position;
                if (v.magnitude < fEatDistance)
                {
                    s.Eat();
                    s = null;
                }
            }
        }
    }
}
