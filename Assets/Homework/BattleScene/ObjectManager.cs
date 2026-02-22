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

    private float fBornTime = 0.0f;

    private List<Sandwich> sandwichList= new List<Sandwich>();

    private Dictionary<System.Type, List<CAssetData>> gameAssetMap = new Dictionary<System.Type, List<CAssetData>>();
    //private List<CAssetData> gameAssetList = new List<CAssetData>();

    private void Awake()
    {
        _instance = this;
        CObjectPool pool = new CObjectPool();
        fBornTime = Random.Range(1.0f, 3.0f);
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
        //每隔5~7秒會產生1~2個三明治，產生的位置會在startPoint的周圍10公尺以內的隨機位置
        if (fBornTime < 0)
        {
            CObjectPool pool = CObjectPool.Instance();

            int iBornCount = Random.Range(1, 3);
            fBornTime = Random.Range(5.0f, 7.0f);
            for (int i = 0; i < iBornCount; i++)
            {
                GameObject go = pool.LoadDataFromPool();
                if (go != null)
                {
                    Vector3 vRandomPos = new Vector3(Random.Range(-10.0f, 10.0f), 0f, Random.Range(-10.0f, 10.0f));
                    go.transform.position = startPoint.position + vRandomPos;
                    go.SetActive(true);
                    Sandwich s = go.GetComponent<Sandwich>();
                    s.ResetSandwich();
                    //sandwichList.Add(s);
                    AddSandwichToList(s);
                }
            }
        }
        else
        {
            fBornTime -= Time.deltaTime;//每幀減去經過的時間，當fBornTime小於0時就會產生三明治
        }
    }

    //把三明治放到sandwichList裡，如果有null的位子就放在null的位子，沒有的話就新增一個位子放在list裡
    private void AddSandwichToList(Sandwich s)
    {
        int i;
        int iC = sandwichList.Count;
        for (i = 0; i < iC; i++)
        {
            if (sandwichList[i] == null)
            {
                sandwichList[i] = s;
                return;
            }
        }

        sandwichList.Add(s);
    }


    public void RemoveSandwich(GameObject go)
    {
        CObjectPool pool = CObjectPool.Instance();
        for (int i = 0; i < sandwichList.Count; i++)
        {
            if (sandwichList[i] == null)
            {
                continue;
            }
            if (sandwichList[i].gameObject == go)
            {
                Debug.Log("remove sandwich " + go.name);
                pool.UnLoadDataToPool(go);
                sandwichList[i] = null;//把這個位子設為null，表示這個位子沒有三明治了
                //sandwichList.RemoveAt(i);
                break;
            }
        }
    }

    public List<Sandwich> getSandwiches()
    {
        return sandwichList;
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

        //把sandwichSrcObj放到物件池裡，並且產生25個三明治
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
