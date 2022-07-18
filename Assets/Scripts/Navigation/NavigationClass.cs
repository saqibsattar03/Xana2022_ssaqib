using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class NavigationClass : MonoBehaviour
{
    //Input fields references
    [SerializeField] TMP_InputField login_emailorPhoneNumberField;
    [SerializeField] TMP_InputField signup_emailorPhoneNumberField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField phoneNumerField;
    [SerializeField] TMP_InputField verificationField1;
    [SerializeField] TMP_InputField verificationField2;
    [SerializeField] TMP_InputField verificationField3;
    [SerializeField] TMP_InputField verificationField4;
    [SerializeField] TMP_InputField enterPasswordField;
    [SerializeField] TMP_InputField reenterPasswordField;
    [SerializeField] TMP_InputField userNameField;


    //Warning Text References
    [SerializeField] TMP_Text emailWarningText;
    [SerializeField] TMP_Text passwordWarningText;
    [SerializeField] TMP_Text phoneNumberWarningText;
    [SerializeField] TMP_Text passwordMismatchWarningText;
    [SerializeField] TMP_Text welcoomeScreenText;
    [SerializeField] TMP_Text timerText;

    //Button References
    [SerializeField] Button loginButton;
    [SerializeField] Button mobilePanelNextButton;
    [SerializeField] Button emailPanelNextButton;
    [SerializeField] Button verifyPanelNextButton;
    [SerializeField] Button passwordDoneButton;
    [SerializeField] Button userNameNextButton;
    [SerializeField] Button walletLoginButton;
    [SerializeField] Button sendOTPButton;


    //Drop down reference
    public TMP_Dropdown dropdown;

    //Panels References
    public List<GameObject> panelList;
    public GameObject startingPanel;

    // Verification code Input Field References
    public int verificationCode1;
    public int verificationCode2;
    public int verificationCode3;
    public int verificationCode4;

    // private variables
    Stack<GameObject> items;
    int stackCounter = 0;
    bool response;
    bool isEmailFilled;
    bool isPasswordFilled;
    string signup_emailText;
    string phoneNumberText;
    string userNameText;
    string password;
    string password2;
    string deviceId;
    int timerValue;
    string dropdownItemValue;
    bool timerButtonControl = false;

    private void Start()
    {
        items = new Stack<GameObject>();
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].SetActive(false);
        }
        items.Push(startingPanel);
        passwordDoneButton.interactable = false;
    }
    private void DeactivePanels()
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

    #region work in progress
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
    #endregion

    public void ShowPassword(int num)
    {
        switch (num)
        {
            case 1:
                if (passwordField.contentType == TMP_InputField.ContentType.Password)
                {
                    passwordField.contentType = TMP_InputField.ContentType.Standard;
                }
                else
                {
                    passwordField.contentType = TMP_InputField.ContentType.Password;
                }
                break;
            case 2:
                if (enterPasswordField.contentType == TMP_InputField.ContentType.Password)
                {
                    enterPasswordField.contentType = TMP_InputField.ContentType.Standard;
                }
                else
                {
                    enterPasswordField.contentType = TMP_InputField.ContentType.Password;
                }
                break;
            case 3:
                if (reenterPasswordField.contentType == TMP_InputField.ContentType.Password)
                {
                    reenterPasswordField.contentType = TMP_InputField.ContentType.Standard;
                }
                else
                {
                    reenterPasswordField.contentType = TMP_InputField.ContentType.Password;
                }
                break;
        }
        passwordField.ForceLabelUpdate();
        enterPasswordField.ForceLabelUpdate();
        reenterPasswordField.ForceLabelUpdate();
    }
    public void LoginCredentials()
    {
        string password = passwordField.text;
        if (login_emailorPhoneNumberField.text == "" || passwordField.text == "")
        {
            Debug.Log("fill the missing field");
        }
        else
        {
            string email = login_emailorPhoneNumberField.text;

            if (email != PlayerPrefs.GetString("Email") && email != PlayerPrefs.GetString("Phone Number"))
            {
                Debug.Log("incorrect email");
            }
            else if (password != PlayerPrefs.GetString("Password"))
            {
                Debug.Log("password is incorrect");
            }
            else
            {
                NavigateToNextPage("Welcome panel");
                WelcomeScreenLoadData();
            }
        }
    }
    public void WalletSignUp()
    {
        deviceId = SystemInfo.deviceName;
        welcoomeScreenText.text = deviceId + " Welcome to Xana";

        NavigateToNextPage("Welcome panel");

    }
    public void SignupEmailInputCheck()
    {
        signup_emailText = signup_emailorPhoneNumberField.text;
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.EmailAddress);

        if (!signup_emailorPhoneNumberField.isFocused)
        {
            response = Validator.EmailValidation(signup_emailText);
            if (!response || signup_emailText == "")
            {
                emailWarningText.gameObject.SetActive(true);
            }
            else
            {
                emailWarningText.gameObject.SetActive(false);
                isEmailFilled = true;
                if (items.Contains(panelList[2].gameObject))
                {
                    emailPanelNextButton.onClick.AddListener(() =>
                    {
                        NavigateToNextPage("Code Panel");
                        GenerateOTP();
                    });
                }
                else
                {
                    emailPanelNextButton.onClick.AddListener(() =>
                    {
                        NavigateToNextPage("Mobile login panel");
                    });
                }
            }
        }
    }

    #region unwanted code
    // public void LoginEmailInputCheck()
    // {
    //     string login_emailText = login_emailorPhoneNumberField.text;
    //     string password = passwordField.text;
    //     if (login_emailText == PlayerPrefs.GetString("Email") && password == PlayerPrefs.GetString("Password"))
    //     {
    //         Debug.Log("welcome");
    //     }
    //     else
    //     {
    //         emailWarningText.gameObject.SetActive(true);
    //     }

    // if (!login_emailorPhoneNumberField.isFocused)
    // {
    //     response = Validator.EmailValidation(login_emailText);
    //     if (!response)
    //     {
    //         emailWarningText.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         emailWarningText.gameObject.SetActive(false);
    //         isEmailFilled = true;
    //         // emailPanelNextButton.onClick.AddListener(() =>
    //         // {
    //         //     NavigateToNextPage("Code Panel");
    //         // });
    //     }
    // }
    // }

    // public void PasswordInputcheck()
    // {
    //     string passwordText = passwordField.text;

    //     if (!passwordField.isFocused)
    //     {
    //         response = Validator.PasswordValidation(passwordText);
    //         if (!response)
    //         {
    //             passwordWarningText.gameObject.SetActive(true);
    //         }
    //         else
    //         {
    //             passwordWarningText.gameObject.SetActive(false);
    //             isPasswordFilled = true;
    //         }
    //     }

    //     if (isEmailFilled && isPasswordFilled)
    //     {
    //         Debug.Log("button activated");
    //         loginButton.interactable = true;
    //     }
    // }
    #endregion
    public void DronDownSelectedValue()
    {
        dropdownItemValue = dropdown.options[dropdown.value].text;
        Debug.Log("value = " + dropdownItemValue.GetType());
    }
    public void PhoneNumberValidation()
    {
        phoneNumberText = dropdownItemValue + phoneNumerField.text;
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        if (!phoneNumerField.isFocused)
        {
            response = Validator.PhoneValidation(phoneNumberText);
            if (!response || passwordField.text == null)
            {
                phoneNumberWarningText.gameObject.SetActive(true);
            }
            else
            {
                phoneNumberWarningText.gameObject.SetActive(false);
                if (items.Contains(panelList[3].gameObject))
                {
                    mobilePanelNextButton.onClick.AddListener(() =>
                    {
                        NavigateToNextPage("Code Panel");
                    });
                }
                else
                {
                    mobilePanelNextButton.onClick.AddListener(() =>
                    {
                        NavigateToNextPage("Email login panel");
                    });
                }
            }
        }
        if (phoneNumberText == "")
        {
            Debug.Log("phone number can't be empty");
        }
    }
    public void VerifyCode()
    {
        int v1 = Int32.Parse(verificationField1.text);
        int v2 = Int32.Parse(verificationField2.text);
        int v3 = Int32.Parse(verificationField3.text);
        int v4 = Int32.Parse(verificationField4.text);
        if (v1 == verificationCode1 && v2 == verificationCode2 && v3 == verificationCode3 && v4 == verificationCode4)
        {
            NavigateToNextPage("Set Password Panel");
            timerValue = 0;
        }
        else
        {
            Debug.Log("code is incorrect");
        }
    }

    public void ConfirmPassword()
    {
        password2 = enterPasswordField.text;
        Debug.Log("Password 2 = " + password2);
        if (!enterPasswordField.isFocused)
        {
            response = Validator.PasswordValidation(password2);
            if (!response)
            {
                passwordWarningText.gameObject.SetActive(true);
            }
            else
            {
                passwordWarningText.gameObject.SetActive(false);
            }
        }
    }

    public void ReenterPassword()
    {
        string reenterPassword = reenterPasswordField.text;
        if (!reenterPasswordField.isFocused)
        {
            if (String.Equals(password2, reenterPassword))
            {
                passwordMismatchWarningText.gameObject.SetActive(false);
                passwordDoneButton.interactable = true;
                passwordDoneButton.onClick.AddListener(() =>
                {
                    NavigateToNextPage("UserName panel");

                });
            }
            else
            {
                passwordMismatchWarningText.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("here in other else part");
            passwordMismatchWarningText.gameObject.SetActive(false);
        }

    }

    public void UserName()
    {
        userNameText = userNameField.text;
        userNameNextButton.onClick.AddListener(() =>
        {
            NavigateToNextPage("Welcome panel");
            SaveData();
            WelcomeScreenLoadData();
        });

    }

    void SaveData()
    {
        PlayerPrefs.SetString("User Name", userNameText);
        PlayerPrefs.SetString("Email", signup_emailText);
        PlayerPrefs.SetString("Phone Number", phoneNumberText);
        PlayerPrefs.SetString("Password", password2);
        PlayerPrefs.SetString("Device Id", deviceId);
        PlayerPrefs.Save();
    }
    public void WelcomeScreenLoadData()
    {
        string userName = PlayerPrefs.GetString("User Name");
        welcoomeScreenText.text = userName + " Welcome to Xana";
    }
    public void Timer()
    {
        if (timerValue >= 0)
        {
            sendOTPButton.interactable = false;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timerValue);
            float minutes = timeSpan.Minutes;
            float seconds = timeSpan.Seconds;
            timerValue--;
            timerText.text = minutes + " : " + seconds;
            Debug.Log("minutes = " + minutes + "seconds = " + seconds);
            Invoke("Timer", 1.0f);
        }
        else
        {
            sendOTPButton.interactable = true;
        }

    }
    public void GenerateOTP()
    {
        verificationCode1 = UnityEngine.Random.Range(0, 9);
        verificationCode2 = UnityEngine.Random.Range(0, 9);
        verificationCode3 = UnityEngine.Random.Range(0, 9);
        verificationCode4 = UnityEngine.Random.Range(0, 9);
        timerValue = 600;
        Timer();
    }
}
