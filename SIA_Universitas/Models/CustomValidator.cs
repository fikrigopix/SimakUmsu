using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;  

namespace SIA_Universitas.Models
{
    public class EmailValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string email = value.ToString();

                if (Regex.IsMatch(email, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", RegexOptions.IgnoreCase))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Please Enter a Valid Email.");
                }
            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " is required");
            }
        }
    }

    public class CheckID : ValidationAttribute
    {
        private SIAEntities db = new SIAEntities();
        public string URLs { get; set; }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string OriUrl = System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToString();
            if (OriUrl.EndsWith(URLs))
            {
                if (value != null)
                {
                    byte values = Convert.ToByte(value);

                    switch (OriUrl)
                    {
                        case "/Country/Create":
                            {
                                if (db.Mstr_Country.Find(values) == null)
                                {
                                    return ValidationResult.Success;
                                } break;
                            }
                        case "/Province/Create":
                            {
                                if (db.Mstr_Province.Find(values) == null)
                                {
                                    return ValidationResult.Success;
                                } break;
                            }
                        case "/City/Create":
                            {
                                if (db.Mstr_City.Find(values) == null)
                                {
                                    return ValidationResult.Success;
                                } break;
                            }
                        case "/Gender/Create":
                            {
                                if (db.Mstr_Gender.Find(values) == null)
                                {
                                    return ValidationResult.Success;
                                } break;
                            }
                        case "/Religion/Create":
                            {
                                if (db.Mstr_Religion.Find(values) == null)
                                {
                                    return ValidationResult.Success;
                                } break;
                            }
                    }
                    
                    return new ValidationResult("ID sudah digunakan.");
                }
            }
            
            return ValidationResult.Success;
        }
    }

}