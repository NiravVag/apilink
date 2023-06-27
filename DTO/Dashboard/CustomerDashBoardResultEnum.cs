using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{
    public enum CustomerResult
    {
        Pass = 1,
        Fail = 2,
        Pending = 3,
        Derogated = 4
    }

    public enum ProductCategoryEnum
    {
        ElectronicAndElectrical = 1,
        Furniture = 2,
        Toys = 3,
        BabyCare = 4,
        Tools = 5,
        SportsFitnessandCamping = 6,
        PersonalCare = 7,
        HomeProducts = 8,
        Stationery = 9,
        LuggageAndBags = 10,
        CarAccessories = 11,
        NFR = 12,
        TextileandFootware = 13,
        FUR = 14,
        Garment= 20,
        Fabric=21,
        HomeAccessories=22,
        HomeTextile=23,
        Jewelry=24,
        LeatherHides=25,
        Shoes=26,
        Sports=27,
        TextileAccessories=28
    }

    public enum FBReportResultEnum
    {
        Pass=1,
        Fail=2,
        Pending=3
    }

}
