﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace NephroNet
{
    public class CheckErrors
    {
        public bool isDigit(string value)
        {
            bool correct = true;
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return correct;
        }
        //Validate all input to avoid special characters:
        public Boolean ContainsSpecialChars(string value)
        {
            Boolean itContainsSpecialCharacter = false;
            Regex RgxUrl = new Regex("[^a-zA-Z0-9]");
            itContainsSpecialCharacter = RgxUrl.IsMatch(value);
            return itContainsSpecialCharacter;
        }
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
            bool correct = true;
            result = "";
            //ValidEmailRegex.IsMatch(emailAddress);
            if (string.IsNullOrEmpty(emailAddress))
            {
                correct = false;
                result = "Invalid input: Please type an email.";
            }
            //This a built-in class that validates emails. Note: a@a is a valid email according to .NET
            if (correct)
            {
                try
                {
                    emailAddress = new MailAddress(emailAddress).Address;
                }
                catch (FormatException)
                {
                    //address is invalid
                    correct = false;
                    result = "Invalid input: Please type a correct email.";
                }
            }
            return correct;
        }
        //static Regex ValidEmailRegex = CreateValidEmailRegex();
        //private static Regex CreateValidEmailRegex()
        //{
        //    string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
        //        + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
        //        + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        //    return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        //}
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
            else if (phone.Length < 10)
            {
                correct = false;
                result = "Invalid input: the phone number must be 10 digits.";
            }
            else if (phone.Length > 10)
            {
                correct = false;
                result = "Invalid input: the phone number must be 10 digits.";
            }
            return correct;
        }
        //Validate international phone:
        public bool validInternationalPhone(string phone, out string result)
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
            else if (phone.Length < 10)
            {
                correct = false;
                result = "Invalid input: the phone number cannot be less than 10 digits.";
            }
            else if (phone.Length > 20)
            {
                correct = false;
                result = "Invalid input: the phone number cannot be more than 20 digits.";
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
        //Validate patient ID:
        public bool validPatientId(string patientId, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(patientId))
            {
                correct = false;
                result = "Invalid input: Please type the patient ID.";
            }
            return correct;
        }
        //Validate question
        public bool validQuestion(string question, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(question))
            {
                correct = false;
                result = "Invalid input: Please type the question.";
            }
            return correct;
        }
        //Validate answer
        public bool validAnswer(string answer, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(answer))
            {
                correct = false;
                result = "Invalid input: Please type the answer.";
            }
            return correct;
        }
        //Validate password
        public bool validPassword(string password, out string result)
        {
            bool correct = true;
            result = "";
            if (string.IsNullOrEmpty(password))
            {
                correct = false;
                result = "Invalid input: Please type the password.";
            }
            else if (password.Contains("'"))
            {
                correct = false;
                result = "Invalid input: The character (') is not allowed for passwords.";
            }
            else if (!passwordRequirements(password))
            {
                correct = false;
                result = "Invalid input: Please follow the password requirements.";                
            }
            return correct;
        }
        public bool passwordRequirements(string password)
        {
            bool correct = true;
            if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower)
                || !password.Any(char.IsDigit) || password.IndexOfAny(";,.!@#$%^&*()".ToCharArray()) == -1)
            {
                correct = false;
            }
            
            return correct;
        }
        public bool passwordsMatch(string p1, string p2, out string result)
        {
            bool correct = true;
            result = "";
            if (!p1.Equals(p2))
            {
                correct = false;
                result = "Value error: The two passwords do not match.";
            }
            return correct;
        }
    }
}