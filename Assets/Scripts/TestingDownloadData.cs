using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestingDownloadData : MonoBehaviour
{
    public ProductsRoot productsRoot;
    // Start is called before the first frame update
    void Start()
    {
        GetData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetData() => StartCoroutine(DownloadData());
    IEnumerator DownloadData()
    {
        string url = "https://lomofy.000webhostapp.com/Products.json";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("error loading data");
            }
            else
            {
                if (request.isDone)
                {
                    productsRoot = JsonUtility.FromJson<ProductsRoot>(request.downloadHandler.text);
                    if (productsRoot == null)
                    {
                        Debug.Log("null");
                    }
                    Testing();
                }
            }
        }
    }

    void Testing()
    {
        Debug.Log("called");
        foreach (var data in productsRoot.Category)
        {
            Debug.Log("categories = " + data.categoryName);
            foreach (var subdata in data.subCategory)
            {
                Debug.Log("sub categories = " + subdata.subCategoryName);
                for (int i = 0; i < subdata.productDetails.Count; i++)
                {
                    Debug.Log("products = " + subdata.productDetails[i].productName);
                }
            }
        }
    }
}
