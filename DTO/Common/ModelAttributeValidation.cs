using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DTO.Common
{
    public class RequiredGreaterThanZero : ValidationAttribute
    {
        //
        // Summary:
        //     Check whether the model property value greater than zero value
        //
        // Parameters:
        //   errorMessage:
        //      Condition not matched given errorMessage will return. 
        public RequiredGreaterThanZero() { }
        public Type FieldType { get; set; }
        public RequiredGreaterThanZero(string errorMessage)
        : base(errorMessage)
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                bool resultValidate = value != null;
                if (resultValidate)
                {
                    if (FieldType == typeof(double))
                    {
                        double i;
                        resultValidate = double.TryParse(value.ToString(), out i) && i > 0;
                    }
                    else
                    {
                        int i;
                        resultValidate = int.TryParse(value.ToString(), out i) && i > 0;
                    }

                }

                if (!resultValidate)
                {
                    validationResult = new ValidationResult(ErrorMessageString);
                }
            }
            catch (Exception ex)
            {
                validationResult = new ValidationResult("An error occurred while validating the property");
                throw ex;
            }
            return validationResult;
        }
    }

    public class RequiredList : ValidationAttribute
    {
        //
        // Summary:
        //     Check whether the model property number list values count atleast one.  
        //
        // Parameters:
        //   errorMessage:
        //      Condition not matched given errorMessage will return. 
        public RequiredList() { }
        public RequiredList(string errorMessage)
        : base(errorMessage)
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                var list = value as IList;
                if (list != null)
                {
                    if (list.Count <= 0)
                    {
                        validationResult = new ValidationResult(ErrorMessageString);
                    }

                }
                else
                    validationResult = new ValidationResult(ErrorMessageString);
            }
            catch (Exception ex)
            {
                validationResult = new ValidationResult("An error occurred while validating the property");
                throw ex;
            }
            return validationResult;
        }
    }

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        //
        // Summary:
        //      Compare other model property date value with should be greater than or equal to current model property date value.
        //
        // Parameters:
        //
        //   otherPropertyName:
        //      otherPropertyName type should be DateObject.
        //
        //   errorMessage:
        //      Condition not matched given errorMessage will return. 

        public string otherPropertyName;
        public DateGreaterThanAttribute() { }
        public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
        : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    return validationResult;
                }
                var datatype = extensionValue.GetType();

                if (field == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName));
                if ((field.PropertyType == typeof(DateObject) || (field.PropertyType.IsGenericType && field.PropertyType == typeof(DateObject))))
                {
                    DateTime toValidate = ((DateObject)value).ToDateTime();
                    DateTime referenceProperty = ((DateObject)field.GetValue(validationContext.ObjectInstance, null)).ToDateTime();
                    if (!toValidate.Equals(referenceProperty))
                    {

                        if (toValidate.CompareTo(referenceProperty) < 1)
                        {
                            validationResult = new ValidationResult(ErrorMessageString);
                        }

                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. From date is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                validationResult = new ValidationResult("An error occurred while validating the property");
                throw ex;
            }
            return validationResult;
        }
    }

    public class DateShouldBeGreaterInNew : ValidationAttribute
    {
        //
        // Summary:
        //      DateShould be in future date only in New New entry.
        //
        // Parameters:
        //
        //   otherPropertyName:
        //      otherPropertyName type should be int.
        //
        //   errorMessage:
        //      Condition not matched given errorMessage will return. 
        public string otherPropertyName;
        public DateShouldBeGreaterInNew() { }
        public DateShouldBeGreaterInNew(string otherPropertyName, string errorMessage)
        : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                if (value != null)
                {
                    var containerType = validationContext.ObjectInstance.GetType();
                    var field = containerType.GetProperty(this.otherPropertyName);
                    var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                    if (extensionValue == null)
                    {
                        return validationResult;
                    }
                    var datatype = extensionValue.GetType();

                    if (field == null)
                        return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName));
                    if ((field.PropertyType == typeof(int) || (field.PropertyType.IsGenericType && field.PropertyType == typeof(int))))
                    {
                        DateTime dateValidate = ((DateObject)value).ToDateTime();
                        int referenceProperty = ((int)field.GetValue(validationContext.ObjectInstance, null));
                        if (referenceProperty == 0)
                        {
                            DateTime today = DateTime.Now.Date;
                            if (!dateValidate.Equals(today))
                            {
                                if (dateValidate.CompareTo(today) < 1)
                                {
                                    validationResult = new ValidationResult(ErrorMessageString);
                                }

                            }
                        }
                    }
                    else
                    {
                        validationResult = new ValidationResult("An error occurred while validating the property. From date is not of type DateTime");
                    }
                }
            }
            catch (Exception ex)
            {
                validationResult = new ValidationResult("An error occurred while validating the property");
                throw ex;
            }
            return validationResult;
        }
    }
}
