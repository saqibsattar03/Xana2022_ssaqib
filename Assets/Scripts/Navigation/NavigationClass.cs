using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class NavigationClass : MonoBehaviour
{

    //Input fields references

    [SerializeField] TMP_InputField emailorPhoneNumberField;
    [SerializeField] TMP_InputField passwordField;

    //Warning Text References

    [SerializeField] TMP_Text emailWarningText;
    [SerializeField] TMP_Text passwordWarningText;

    //Button References

    [SerializeField] Button loginButton;

    public List<GameObject> panelList;
    public GameObject startingPanel;
    Stack<GameObject> items;
    int stackCounter = 0;
    private void Start()
    {
        loginButton.interactable = false;
        // string email = "abc@gmail.com";
        // Debug.Log("email validator = " + Validator.EmailValidation(email));

        // long number = +923082025173;
        // Debug.Log("phone validator = " + Validator.PhoneValidation(number));

        // string password = "2K16bscs123@";
        // Debug.Log("password = " + Validator.PasswordValidation(password, "lol"));
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

    // void Push(string screenName)
    // {

    //     startingPanel.SetActive(false);
    //     Debug.Log("screen name =" + screenName);

    //     foreach (GameObject obj in panelList)
    //     {
    //         if (obj.name.Equals(screenName))
    //         {
    //             obj.SetActive(true);
    //             Debug.Log("panel found");
    //         }
    //     }
    // }
    // public void NavigateToNextPage1(string routeName)
    // {
    //     Push(routeName);
    // }


    public void EmailInputCheck()
    {
        string emailText = emailorPhoneNumberField.text;
        if (!emailorPhoneNumberField.isFocused)
        {
            bool response = Validator.EmailValidation(emailText);
            if (!response)
            {
                emailWarningText.gameObject.SetActive(true);
            }
            else
            {
                emailWarningText.gameObject.SetActive(false);
            }
        }
    }

    public void PasswordInputcheck()
    {
        string passwordText = passwordField.text;
        if (!passwordField.isFocused)
        {
            bool response = Validator.PasswordValidation(passwordText);
            Debug.Log("response put side = " + response);
            if (!response)
            {
                Debug.Log(" inside response = " + response);
                passwordWarningText.gameObject.SetActive(true);
            }
            else
            {
                passwordWarningText.gameObject.SetActive(false);
            }
        }
    }


}
