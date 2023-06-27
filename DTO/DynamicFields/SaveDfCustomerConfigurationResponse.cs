using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class SaveDfCustomerConfigurationResponse
    {
        public int Id { get; set; }
        public DfCustomerConfgigurationResult Result { get; set; }
    }

    public enum DfCustomerConfgigurationResult
    {
        Success = 1,
        ConfigurationIsNotSaved = 2,
        ConfigurationIsNotFound = 3,
        ConfigurationExists = 4,
        CascadingParentDropDownNotFound=5
    }
}
