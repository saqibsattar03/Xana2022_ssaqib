using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Validator
{
    public static bool EmailValidation(string emailString)
    {
        Regex emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
        return emailRegex.IsMatch(emailString);
    }

    public static bool PhoneValidation(long phoneNumber)
    {
        Regex phoneNumberRegex = new Regex(@"^\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*$");
        return phoneNumberRegex.IsMatch(phoneNumber.ToString());

    }
    public static bool PasswordValidation(string password)
    {
        var input = password;


        if (string.IsNullOrWhiteSpace(input))
        {
            throw new System.Exception("Password should not be empty");
        }

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,15}");
        var hasLowerChar = new Regex(@"[a-z]+");
        // var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        if (!hasLowerChar.IsMatch(input))
        {

            return false;
        }
        else if (!hasUpperChar.IsMatch(input))
        {

            return false;
        }
        else if (!hasMiniMaxChars.IsMatch(input))
        {

            return false;
        }
        else if (!hasNumber.IsMatch(input))
        {

            return false;
        }

        // else if (!hasSymbols.IsMatch(input))
        // {
        //     ErrorMessage = "Password should contain at least one special case character.";
        //     return false;
        // }
        else
        {
            return true;
        }
    }
}