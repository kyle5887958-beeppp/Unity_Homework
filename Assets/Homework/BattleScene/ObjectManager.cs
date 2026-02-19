using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance = null;
    public static ObjectManager Instance() { return _instance; }

    private Object sandwichSrcObj = null;
    public Transform startPoint;

    private Dictionary<System.Type, List<CAssetData>> gameAssetMap = new Dictionary<System.Type, List<CAssetData>>();
    //private List<CAssetData> gameAssetList = new List<CAssetData>();

    private void Awake()
    {
        _instance = this;
        CObjectPool pool = new CObjectPool();
    }
    void Start()
    {
        System.Type gType = typeof(GameObject);
        List<CAssetData> assetList = new List<CAssetData>();
        gameAssetMap.Add(gType, assetList);

        StartCoroutine(InitializeSandwiches("sandwich"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ResourcesLoadSandwiches(string sName)
    {
        System.Type t = typeof(GameObject);
        if (gameAssetMap.ContainsKey(t))
        {
            foreach (CAssetData c in gameAssetMap[t])
            {
                if (c.name == sName)
                {
                    sandwichSrcObj = c.asset;
                    yield break;
                }
            }
        }
        else 
        {
            gameAssetMap.Add(t, new List<CAssetData>());
        }

        ResourceRequest rr = Resources.LoadAsync(sName);
        yield return rr;
        if (rr.isDone)
        {
            CAssetData data = new CAssetData();
            data.name = sName;
            data.asset = rr.asset;
            gameAssetMap[typeof(GameObject)].Add(data);

            sandwichSrcObj = rr.asset;
        }
    }

    //會先跑完ResourcesLoadSandwiches，等到sandwichSrcObj有值後才會繼續往下跑
    IEnumerator InitializeSandwiches(string name)
    {
        yield return ResourcesLoadSandwiches(name);

        CObjectPool.Instance().InitPoolData(sandwichSrcObj, 25);
        //產生25個三明治，擺成5x5的陣列
        //int i, j;
        //for (i=0; i<5; i++)
        //{
        //    for (j=0; j<5; j++)
        //    { 
        //        Vector3 pos = startPoint.position + new Vector3(i*2.0f, 0, j*2.0f);
        //        GameObject go = Instantiate(sandwichSrcObj, pos, Quaternion.Euler(-90, 0, 0)) as GameObject;
        //    }
        //}
    }
}
