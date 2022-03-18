using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KetabAbee.Application.Security
{
    public static class PasswordStrengthChecker
    {
        public static PasswordScore CheckStrength(string password)
        {
            int score = 0;

            // using three requirements here:  min length and two types of characters (numbers and letters)
            //bool blnMinLengthRequirementMet = false;
            //bool blnRequirement1Met = false;
            //bool blnRequirement2Met = false;

            // check for chars in password
            if (password.Length < 1)
                return PasswordScore.Blank;

            // if less than 6 chars, return as too short, else, plus one
            if (password.Length < 6)
            {
                return PasswordScore.VeryWeak;
            }
            else
            {
                score++;
                //blnMinLengthRequirementMet = true;
            }

            // if 8 or more chars, plus one
            if (password.Length >= 8)
                score++;

            // if 10 or more chars, plus one
            if (password.Length >= 10)
                score++;

            // if password has a number, plus one
            if (Regex.IsMatch(password, @"[\d]", RegexOptions.ECMAScript))
            {
                score++;
                //blnRequirement1Met = true;
            }

            // if password has lower case letter, plus one
            if (Regex.IsMatch(password, @"[a-z]", RegexOptions.ECMAScript))
            {
                score++;
                //blnRequirement2Met = true;
            }

            // if password has upper case letter, plus one
            if (Regex.IsMatch(password, @"[A-Z]", RegexOptions.ECMAScript))
            {
                score++;
                //blnRequirement2Met = true;
            }

            // if password has a special character, plus one
            if (Regex.IsMatch(password, @"[~`!@#$%\^\&\*\(\)\-_\+=\[\{\]\}\|\\;:'\""<\,>\.\?\/£]", RegexOptions.ECMAScript))
                score++;

            // if password is longer than 2 characters and has 3 repeating characters, minus one (to minimum of score of 3)
            List<char> lstPass = password.ToList();
            if (lstPass.Count >= 3)
            {
                for (int i = 2; i < lstPass.Count; i++)
                {
                    char charCurrent = lstPass[i];
                    if (charCurrent == lstPass[i - 1] && charCurrent == lstPass[i - 2] && score >= 4)
                    {
                        score++;
                    }
                }
            }

            //if (!blnMinLengthRequirementMet || !blnRequirement1Met || !blnRequirement2Met)
            //{
            //    return PasswordScore.RequirementsNotMet;
            //}
            if (score >= 5)
            {
                return PasswordScore.VeryStrong;
            }
            return (PasswordScore)score;
        }
    }
    public enum PasswordScore
    {
        [Display(Name = "نا امن")]
        Blank = 0,
        [Display(Name = "خیلی ضعیف")]
        VeryWeak = 1,
        [Display(Name = "ضعیف")]
        Weak = 2,
        [Display(Name = "متوسط")]
        Medium = 3,
        [Display(Name = "قوی")]
        Strong = 4,
        [Display(Name = "بسیار قوی")]
        VeryStrong = 5
    }
}

