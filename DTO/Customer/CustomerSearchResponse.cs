﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerSearchResponse

        public IEnumerable<CustomerItem> Data { get; set; }

    public enum CustomerSearchResult
}