using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PrefabType
{
    square,
    rectangle
}

[System.Serializable]
public class Category
{
    public int categoryIconID;
    public string categoryName;
    public List<SubCategory> subCategory;
}

[System.Serializable]
public class ProductDetail
{
    public string productName;
    public string topIconId;
    public int mainImageId;
    public int currencyIconId;
    public int price;
}
[System.Serializable]
public class ProductsRoot
{
    public List<Category> Category;
}
[System.Serializable]
public class SubCategory
{
    public string subCategoryName;
    public string subCategoryActivatedIconId;
    public bool square;
    public bool isColorPalete;
    public List<SubFeature> subFeatures;
    public List<ProductDetail> productDetails;

}
[System.Serializable]
public class SubFeature
{
    public string featureName;
    public List<SubFeatureProduct> subFeatureProduct;
}
[System.Serializable]
public class SubFeatureProduct
{
    public string productName;
    public string topIconId;
    public int mainImageId;
    public int currencyIconId;
    public int price;
}