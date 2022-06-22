using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField] List<GameObject> uIScreens = new List<GameObject>();
    [SerializeField] List<int> uiFlow1 = new List<int>();
    [SerializeField] List<int> uiFlow2 = new List<int>();
    [SerializeField] List<int> uiFlow3 = new List<int>();
    [SerializeField] List<int> uiFlow4 = new List<int>();
    
    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField emailOrPhoneField;
    [SerializeField] TMP_Text errorMessages;
    [SerializeField] TMP_InputField passwordRegistration;
    [SerializeField] TMP_InputField mobileNumberInfo;
    [SerializeField] TMP_InputField emailAddressInfo;
    [SerializeField] TMP_Dropdown  countryCode;
    [SerializeField] TMP_InputField[] verifyCode;
    [SerializeField] TMP_InputField[] passwordRegistrationinfo;
    [SerializeField] int[] code;
    [SerializeField] TextMeshProUGUI nameText;
    int uiFlow;
    int index;
    int codeIndex;
    long nummericValue;
    public const string MatchEmailPattern =
          @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
   + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
   + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
   + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
    public const string MatchPasswordPattern =
    @"((?=.*\d)+(?=.*[a-z])+(?=.*[A-Z])+(?=.*[a-zA-Z]).{8,})$";
    //public const string MatchPhonePattern = "@^(+?d{1,3}|d{1,4})$";
    public const string MatchPhonePattern = @"(\d{10,12})$";
    // public const string MatchPlusSign = @"(\+*)$";
    private void Start()
    {
        uIScreens[0].SetActive(true);
        Debug.Log(PlayerPrefs.GetString("Email"));
        Debug.Log(PlayerPrefs.GetString("Password"));
        Debug.Log(PlayerPrefs.GetString("PhoneNumber"));
        Debug.Log(PlayerPrefs.GetString("UserName"));  
       
    }
    public static bool IsEmail(string email)
    {
        if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
        else return false;
    }
    public static bool IsPassword(string password)
    {
        if (password != null) return Regex.IsMatch(password, MatchPasswordPattern);
        else return false;

    }
    public static bool isCountryCode(string phone)
    {
        if (phone != null) return Regex.IsMatch(phone, MatchPhonePattern);
        else return false;

    }
    public void numbercheck(string Text)
    {
        if (isCountryCode(Text)&&PlayerPrefs.GetString("Phone")==emailOrPhoneField.text)
        {
            errorMessages.text = "";
            if(PlayerPrefs.GetString("Password")==passwordRegistration.text)
            {passwordCheck(passwordRegistration.text);}
            else
            {
                errorMessages.text="Invalid Password";
            }
        }
        else
        {
            errorMessages.text = "Invalid Phone Number or Phone Number is not registered";
        }
    }
    public void emailCheck(string Text)
    {
        if (IsEmail(Text))
        {   
            errorMessages.text = "";
            if (uiFlow == 0)
            { passwordCheck(passwordRegistration.text); }
            if (uiFlow == 2)
            { signup(); }
            if (uiFlow == 4)
            { signupEmail(); }

        }
        else
        {
            errorMessages.text = "Invalid Email";
        }
    }
    public void passwordCheck(string Text)
    {
        if (IsPassword(Text))
        {   
            Debug.Log("Password Correct");
            errorMessages.text = "";
        }
        else
        {
            errorMessages.text = "Password must contain atleast 8 digits & one Upper Case word & one numerical";
        }
    }
    public void OnLogin()
    {
        bool isNumber = long.TryParse(emailOrPhoneField.text, out nummericValue);
        if (emailOrPhoneField.text == "")
        {
            errorMessages.text = "All data required";
            return;
        }
        else
        {
            if (isNumber)
            {   if(PlayerPrefs.GetString("PhoneNumber")==emailOrPhoneField.text&&PlayerPrefs.GetString("Password")==passwordRegistration.text)
               {//numbercheck(emailOrPhoneField.text);
                uiFlow=1;
                uIScreens[uiFlow1[index]].SetActive(false);
                index++;
                uIScreens[uiFlow1[index]].SetActive(true);
               errorMessages.text="";
               }
            }
            else
            {   if(PlayerPrefs.GetString("Email")==emailOrPhoneField.text&&PlayerPrefs.GetString("Password")==passwordRegistration.text)
                {//emailCheck(emailOrPhoneField.text);
                uiFlow=1;
                uIScreens[uiFlow1[index]].SetActive(false);
                index++;
                uIScreens[uiFlow1[index]].SetActive(true);}
                errorMessages.text="";
            }
            errorMessages.text="User Data doesn't exist";
        }

    }
    public void enterNumber()
    {
        //mobileNumberInfo.text = mobileNumberInfo.text.Remove(0, 1);
        if (isCountryCode(mobileNumberInfo.text))
        {   PlayerPrefs.SetString("PhoneNumber",mobileNumberInfo.text);
            Debug.Log(countryCode.options);
            if (uiFlow == 2)
            { signup(); }
            if (uiFlow == 4)
            { signupEmail(); }
            errorMessages.text = "";
            Debug.Log(mobileNumberInfo.text);
        }
        else
        {
            errorMessages.text = "Enter valid Phone Number";
        }
    }
    public void emailInfo()
    {   if(IsEmail(emailAddressInfo.text))
        {PlayerPrefs.SetString ("Email",emailAddressInfo.text);
        Debug.Log(PlayerPrefs.GetString("Email"));}
        emailCheck(emailAddressInfo.text);
        //signup();
    }
    public void codeVerification()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (verifyCode[j].text == "")
                {
                    errorMessages.text = "Enter Code that you recieved";
                }
                else
                {
                    errorMessages.text = "";
                }
            }
            if (int.Parse(verifyCode[i].text) == code[i])
            {
                errorMessages.text = "";
                if (i == 3)
                {
                    if (uiFlow == 2)
                    { signup(); }
                    if (uiFlow == 4)
                    { signupEmail(); }
                }
            }
            else
            {
                errorMessages.text = "Verification Code is Invalid";
            }
        }
    }
    public void passRegisterInfo()
    {

        if (passwordRegistrationinfo[0].text != passwordRegistrationinfo[1].text)
        {   
            errorMessages.text = "Passwords Do not match or Invalid Password. Password must contain atleast 8 digits & one Upper Case word & one numerical";
        }
        else
        { if(passwordRegistrationinfo[0].text==passwordRegistrationinfo[1].text&&IsPassword(passwordRegistrationinfo[0].text)&&IsPassword(passwordRegistrationinfo[1].text))
        {
            PlayerPrefs.SetString("Password",passwordRegistrationinfo[0].text);
            Debug.Log(PlayerPrefs.GetString("Password"));
             errorMessages.text = "";
            if (uiFlow == 2)
            { signup(); }
            if (uiFlow == 4)
            { signupEmail(); }
        }
           
        }
    }
    public void alreadyAccount()
    {
        uiFlow = 1;
        uIScreens[uiFlow1[index]].SetActive(false);
        index++;
        uIScreens[uiFlow1[index]].SetActive(true);
    }
    public void signup()
    {
        uiFlow = 2;
        uIScreens[uiFlow2[index]].SetActive(false);
        index++;
        uIScreens[uiFlow2[index]].SetActive(true);
    }
    public void backScreen()
    {
        if (uiFlow == 1)
        {
            uIScreens[uiFlow1[index]].SetActive(false);
            index--;
            uIScreens[uiFlow1[index]].SetActive(true);
        }
        if (uiFlow == 2)
        {
            uIScreens[uiFlow2[index]].SetActive(false);
            index--;
            uIScreens[uiFlow2[index]].SetActive(true);
        }
        if (uiFlow == 4)
        {
            uIScreens[uiFlow4[index]].SetActive(false);
            index--;
            uIScreens[uiFlow4[index]].SetActive(true);
        }


    }
    public void enterName()
    {   if(userName.text!="")
        {PlayerPrefs.SetString("UserName",userName.text);
        Debug.Log(PlayerPrefs.GetString("UserName"));
         nameText.text=PlayerPrefs.GetString("UserName");   
         errorMessages.text="";
        if (uiFlow == 2)
        { signup(); }
        if (uiFlow == 3)
        {
            walletLogin();
        }
        if (uiFlow == 4)
        { signupEmail(); }}
        else
        {
         errorMessages.text = "Empty Field cannot be accepted";
        }
    }
    public void indexReset()
    {
        Debug.Log(index);
        uIScreens[uiFlow2[index]].SetActive(false);
        index = 0;
        walletLogin();
       
    }
    public void walletLogin()
    {
        uiFlow = 3;
        uIScreens[uiFlow3[index]].SetActive(false);
        index++;
        uIScreens[uiFlow3[index]].SetActive(true);
        if (index == 1)
        {
            StartCoroutine(waitLogin());
        }
    }
    public void flowChange()
    {
        uiFlow=2;
    }
    IEnumerator waitLogin()
    {
        yield return new WaitForSeconds(2);
        uIScreens[uiFlow3[index]].SetActive(false);
        index++;
        uIScreens[uiFlow3[index]].SetActive(true);
        Debug.Log("uiflow"+uiFlow);
        if(uiFlow==3)
        {Debug.Log("uiflow"+uiFlow);
        uIScreens[uiFlow3[index]].SetActive(false);
        index++;
        uIScreens[uiFlow3[index]].SetActive(true);
        }
    }
    public void signupEmail()
    {
        uiFlow = 4;
        uIScreens[uiFlow4[index]].SetActive(false);
        index++;
        uIScreens[uiFlow4[index]].SetActive(true);

    }
    public void showPassword()
    {
        StartCoroutine(showPasswordDelay(0));
    }
    public void showrePassword()
    {
        StartCoroutine(showPasswordDelay(1));
    }
    IEnumerator showPasswordDelay(int indexfield)
    {
        passwordRegistrationinfo[indexfield].contentType = TMP_InputField.ContentType.Standard;
        passwordRegistrationinfo[indexfield].ForceLabelUpdate();
        yield return new WaitForSeconds(2);
        passwordRegistrationinfo[indexfield].contentType = TMP_InputField.ContentType.Password;
        passwordRegistrationinfo[indexfield].ForceLabelUpdate();
        StopCoroutine("showPasswordDelay()");
    }
    public void otpMove()
    {   codeIndex++;
        verifyCode[codeIndex].Select();
    }
}