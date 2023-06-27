﻿using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerServiceConfigMasterResponse
    {
        public IEnumerable<Service> ServiceList { get; set; }
        public CustomerServiceConfigMasterResult Result { get; set; }
        public IEnumerable<DpPoint> DpPointList { get; set; }
    }

    public enum CustomerServiceConfigMasterResult
}