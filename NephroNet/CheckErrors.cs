using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NephroNet
{
    public class CheckErrors
    {
        //Validate all input to avoid special characters:
        public Boolean ContainsSpecialChars(string value, out string result)
        {
            result = "Invalid input: Special characters are not allowed.";
            Boolean itContainsSpecialCharacter = false;
            Regex RgxUrl = new Regex("[^a-zA-Z0-9]");
            itContainsSpecialCharacter = RgxUrl.IsMatch(value);
            return itContainsSpecialCharacter;
        }
        //Names can have numbers like George 2, or Georger the 2nd; therefore, I am not checking for numbers in names.
        //Validate first name:
        public bool validFirstName(string firstName, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(firstName))
            {
                correct = false;
                result = "Invalid input: Please type the first name.";
            }            
            return correct;
        }
        //Validate last name:
        public bool validLastName(string lastName, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(lastName))
            {
                correct = false;
                result = "Invalid input: Please type the last name.";
            }
            return correct;
        }
        //Validate emails:
        public bool validEmail(string emailAddress, out string result)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);
            result = "Invalid input: Please type a correct email.";
            return isValid;
        }
        static Regex ValidEmailRegex = CreateValidEmailRegex();
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        //Validate city:
        public bool validCity(string city, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(city))
            {
                correct = false;
                result = "Invalid input: Please type the city.";
            }
            return correct;
        }
        //Validate state:
        public bool validState(int state, out string result)
        {
            bool correct = true;
            result = "";
            if (state == 0)
            {
                correct = false;
                result = "Invalid input: Please select a state.";
            }
            return correct;
        }
        //Validate zip code:
        public bool validZip(string zip, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(zip))
            {
                correct = false;
                result = "Invalid input: Please type the zip code.";
            }
            else if (!zip.All(char.IsDigit))
            {
                correct = false;
                result = "Invalid input: Please type the correct zip code in numbers.";
            }
            else if (zip.Length != 5)
            {
                correct = false;
                result = "Invalid input: the zip code must be 5 digits.";
            }
            return correct;
        }
        //Validate address:
        public bool validAddress(string address, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(address))
            {
                correct = false;
                result = "Invalid input: Please type the address.";
            }
            return correct;
        }
        //Validate phone:
        public bool validPhone(string phone, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(phone))
            {
                correct = false;
                result = "Invalid input: Please type the phone.";
            }
            else if (!phone.All(char.IsDigit))
            {
                correct = false;
                result = "Invalid input: Please type the correct phone numbers using digits only.";
            }
            else if (phone.Length < 9)
            {
                correct = false;
                result = "Invalid input: the phone number must be at least 9 digits.";
            }
            else if (phone.Length > 17)
            {
                correct = false;
                result = "Invalid input: the phone number must be less than 17 digits.";
            }
            return correct;
        }
        //Validate role:
        public bool validRole(int role, out string result)
        {
            bool correct = true;
            result = "";
            if (role == 0)
            {
                correct = false;
                result = "Invalid input: Please select a role.";
            }
            return correct;
        }
    }
}