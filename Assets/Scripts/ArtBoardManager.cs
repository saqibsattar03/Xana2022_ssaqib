using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ArtBoardManager : MonoBehaviour
{
    //Transform references
    public Transform subCategoriesTransfrom;
    public Transform productTransform;
    public Transform subFeatureTransform;
    public Transform subFeatureScrollViewTransfrom;
    public Transform productScrollViewTransform;
    public GameObject subFeatureGameObject;
    public GameObject colorPalete;
    public GameObject colorPaleteButton;
    public GameObject productPanel;
    public GameObject emptyPanel;

    //Button references
    public List<Button> categoryButton;

    // Data class references
    Category categoryData;
    SubCategory subCategory;

    //Prefabs
    public Button subCategoriesButtonPrefab;
    public GameObject productSquarePrefab;
    public GameObject productRectanglePrefab;
    public Button subFeatureButtonPrefab;

    //Root Class reference
    public ProductsRoot productsRoot;

    Button previousButton;

    // Start is called before the first frame update
    void Start()
    {
        GetData();
        // using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(Path.Combine(Application.streamingAssetsPath + "\\" + "Active Icons 1")))
        // {
        //     Debug.Log("Data fetching");
        // Debug.Log("loaded data = " + DownloadHandlerTexture.GetContent(uwr));
        // };
        // var loadedData = UnityWebRequestTexture.GetTexture(Path.Combine(Application.streamingAssetsPath + "\\" + "Active Icons 1"));

        // var laodedData = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "Active Icons 1"));
        // Debug.Log("loaded data = " + laodedData.downloadHandler.);
        // Debug.Log("streaming assets path" + Application.streamingAssetsPath + "/Active Icons 1/");
    }
    void GetData() => StartCoroutine(DownloadData());
    IEnumerator DownloadData()
    {
        string url = "https://unityprojectdfgh.000webhostapp.com/Products.json";
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
                    FillSingleCategoryData();
                }
            }
        }
    }
    void FillSingleCategoryData()
    {
        for (int i = 0; i < productsRoot.Category.Count; i++)
        {
            categoryButton[i].GetComponent<SingleCategoryData>().categoryData = productsRoot.Category[i];
        }
    }
    public void FetchSingleCategoryData(int index)
    {
        categoryData = categoryButton[index].gameObject.GetComponent<SingleCategoryData>().categoryData;
        FectingSubcategories();
    }
    void FectingSubcategories()
    {
        RemoveContent(subCategoriesTransfrom);
        foreach (var subcategoryData in categoryData.subCategory)
        {
            // var icons = LoadIcon<Sprite>("SubCategoryButtonIcons/", subcategoryData.subCategoryActivatedIconId);
            var icons = LoadIcon<Sprite>("SubCategoryButtonIcons/", subcategoryData.subCategoryActivatedIconId);
            Button subCatBtn = Instantiate(subCategoriesButtonPrefab, subCategoriesTransfrom);
            subCatBtn.name = subcategoryData.subCategoryName;
            subCatBtn.GetComponent<SingleSubcategory>().subCategory = subcategoryData;
            subCatBtn.GetComponent<Image>().sprite = icons;
            subCatBtn.onClick.AddListener(() =>
            {
                ButtonSelection(subCatBtn);
                FetchingFeatureProductsList(subCatBtn.GetComponent<SingleSubcategory>().subCategory.subFeatures, subCatBtn.GetComponent<SingleSubcategory>().subCategory.productDetails, subcategoryData);
                colorPalete.SetActive(false);
                emptyPanel.SetActive(false);
                ActivateColorPalete(subcategoryData);
            });
            var previousButton = subCatBtn;
            Debug.Log("previous button name = " + previousButton.name);
        }
    }
    void ButtonSelection(Button currentButton)
    {
        if (previousButton != null)
        {
            previousButton.gameObject.GetComponent<Image>().sprite = LoadIcon<Sprite>("SubCategoryButtonIcons/", previousButton.gameObject.GetComponent<Image>().sprite.name);
        }

        previousButton = currentButton;
        var name = currentButton.gameObject.GetComponent<Image>().sprite.name;
        currentButton.gameObject.GetComponent<Image>().sprite = LoadIcon<Sprite>("Active Icons/", name);
    }

    void FetchingFeatureProductsList(List<SubFeature> subFeatures, List<ProductDetail> products, SubCategory subcategoryData)
    {
        RemoveContent(subFeatureScrollViewTransfrom);
        if (subcategoryData.subFeatures.Count > 0)
        {
            subFeatureGameObject.SetActive(true);
            productScrollViewTransform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 860);
            RemoveContent(productTransform);
            foreach (var data in subFeatures)
            {
                if (data.featureName != null)
                {
                    Button btn = Instantiate(subFeatureButtonPrefab, subFeatureScrollViewTransfrom);
                    btn.transform.GetChild(0).GetComponent<TMP_Text>().text = data.featureName;
                    btn.onClick.AddListener(() =>
                    {
                        FetchingFeatureProducts(data.subFeatureProduct, subCategory);
                    });
                }
            }
        }
        else
        {
            subFeatureGameObject.SetActive(false);
            productScrollViewTransform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 990);
            FetchingProducts(products, subcategoryData);
        }
    }
    void FetchingFeatureProducts(List<SubFeatureProduct> subFeatures, SubCategory subCategory)
    {
        RemoveContent(productTransform);
        foreach (var subFeatureProducts in subFeatures)
        {
            productScrollViewTransform.GetChild(0).GetChild(0).gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);
            productScrollViewTransform.GetChild(0).GetChild(0).gameObject.GetComponent<GridLayoutGroup>().padding.top = 60;
            Instantiate(productSquarePrefab, productTransform);
        }
    }
    void FetchingProducts(List<ProductDetail> products, SubCategory subcategoryData)
    {
        RemoveContent(productTransform);
        foreach (var product in products)
        {
            if (subcategoryData.square == false)
            {
                productScrollViewTransform.GetChild(0).GetChild(0).gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 235);
                productScrollViewTransform.GetChild(0).GetChild(0).gameObject.GetComponent<GridLayoutGroup>().padding.top = 70;
                Instantiate(productRectanglePrefab, productTransform);
            }
            else
            {
                productScrollViewTransform.GetChild(0).GetChild(0).gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);
                productScrollViewTransform.GetChild(0).GetChild(0).gameObject.GetComponent<GridLayoutGroup>().padding.top = 60;
                Instantiate(productSquarePrefab, productTransform);
            }
        }
    }
    void ActivateColorPalete(SubCategory subcategoryData)
    {
        if (subcategoryData.isColorPalete)
        {
            colorPaleteButton.SetActive(true);
        }
        else
        {
            colorPaleteButton.SetActive(false);
        }
    }

    bool isActive;
    public void SetColorPaleteState()
    {
        if (!isActive)
        {
            // var color = productScrollViewTransform.GetChild(0).GetComponent<Image>().color;
            // color.a = 0;
            emptyPanel.SetActive(true);
            colorPalete.SetActive(true);
            //Debug.Log("alpha in active condotion = " + color.a);
            //  productPanel.SetActive(false);
        }
        else
        {
            colorPalete.SetActive(false);
            emptyPanel.SetActive(false);
            productPanel.SetActive(true);
        }
        isActive = !isActive;
    }

    // Sprite ChangeActiveButtonIcon(string name)
    // {
    //     Sprite sprite = Resources.Load<Sprite>("Active Icons/" + name);
    //     return sprite;
    // }
    public void RemoveContent(Transform contentTransform)
    {
        foreach (Transform content in contentTransform.transform)
        {
            Destroy(content.gameObject);
        }
    }
    //Resources load Method 
    T LoadIcon<T>(string reourcesFolder, string path) where T : Object
    {
        return Resources.Load<T>(reourcesFolder + path);
    }
    public void ReloadData()
    {
        Debug.Log("downloading data...");
        GetData();
    }
}