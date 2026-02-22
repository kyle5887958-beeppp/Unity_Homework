using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CObjectPool
{
    private static CObjectPool _instance = null;
    public static CObjectPool Instance() { return _instance; }
    private List<CObjectPoolData> sandwichGameObjects;

    private Transform poolRoot;

    public CObjectPool()
    {
        _instance = this;
        sandwichGameObjects = new List<CObjectPoolData>();
        //創造一個空物件當作物件池的根節點(比較不會亂)
        GameObject root = new GameObject("ObjectPool");
        poolRoot = root.transform;
    }

    public void InitPoolData(Object src, int iCount)
    {
        if (sandwichGameObjects == null)
        {
            sandwichGameObjects = new List<CObjectPoolData>();
        }
        for (int i = 0; i < iCount; i++)
        {
            CObjectPoolData data = new CObjectPoolData();
            data.go = GameObject.Instantiate(src) as GameObject;
            data.isUsing = false;
            data.go.SetActive(false);
            data.go.transform.SetParent(poolRoot);
            sandwichGameObjects.Add(data);
        }
    }

    public GameObject LoadDataFromPool() 
    {
        if (sandwichGameObjects == null)
        {
            return null; 
        }

        int iCount = sandwichGameObjects.Count;
        for (int i = 0; i < iCount; i++)
        {
            if (sandwichGameObjects[i].isUsing == false)
            {
                sandwichGameObjects[i].isUsing = true;
                sandwichGameObjects[i].go.SetActive(true);
                return sandwichGameObjects[i].go;
            }
        }
        return null;
    }
    public void UnLoadDataToPool(GameObject go)
    {
        if (sandwichGameObjects == null)
        {
            return ;
        }

        int iCount = sandwichGameObjects.Count;
        for (int i = 0; i < iCount; i++)
        {
            if (sandwichGameObjects[i].go == go)
            {
                sandwichGameObjects[i].isUsing = false;
                sandwichGameObjects[i].go.SetActive(false);
                break;
            }
        }
        return ;
    }

}
