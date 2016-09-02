using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class StringJamValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string jam = value.ToString();

                if (Regex.IsMatch(jam, @"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", RegexOptions.IgnoreCase))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Jam tidak valid, contoh = 09:00 atau 9:00");
                }
            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " Harus diisi");
            }
        }
    }
}