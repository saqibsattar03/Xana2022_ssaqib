using System.Collections.Generic;
using UnityEngine;
using System;


public class NavigationClass : MonoBehaviour
{
    public List<GameObject> panelList;
    public GameObject startingPanel;
    Stack<GameObject> items;
    int stackCounter = 0;
    private void Start()
    {
        items = new Stack<GameObject>();
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].SetActive(false);
        }
        items.Push(startingPanel);
        // foreach (GameObject item in panelList)
        // {
        //     items.Push(item);
        // }
        Debug.Log("last item = " + items.Peek());
        //GameObject lastItem = items.Peek();
        //lastItem.SetActive(true);
    }
    void DeactivePanels()
    {
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].SetActive(false);
        }
    }
    public void PushItem(GameObject lastPushedIem, string screenName)
    {
        foreach (GameObject panelName in panelList)
        {
            if (panelName.name.Equals(screenName))
            {
                items.Push(panelName.gameObject);
                try
                {
                    DeactivePanels();
                    startingPanel.SetActive(false);
                    lastPushedIem = items.Peek();
                    lastPushedIem.SetActive(true);
                    stackCounter++;
                }
                catch (Exception e)
                {
                    print("error = " + e);
                    //throw new System.ArgumentException("Index is dfdgdjksdksd out of range", e);
                }
            }
        }
    }
    public void PopItem(GameObject lastPopedItem)
    {
        Debug.Log("items poping = " + items.Count);
        try
        {
            DeactivePanels();
            lastPopedItem = items.Peek();
            lastPopedItem.SetActive(true);
            stackCounter--;
        }
        catch (Exception e)
        {
            Debug.Log("error = " + e);
            //throw new System.ArgumentException("Index is dfdgdjksdksd out of range", e);
        }
        Debug.Log("counter = " + stackCounter);
    }
    public void NavigateToNextPage(string routeName)
    {
        //items.Push(panelList[stackCounter]);
        PushItem(items.Peek(), routeName);
    }

    public void GoBackToPreviousPage()
    {
        PopItem(items.Pop());
    }
}
