using CsvHelper;
using CsvHelper.Configuration;
using DTO.Common;
using DTO.PurchaseOrder;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class PurchaseOrderUploadMap : ApiCommonData
    {


        public List<PurchaseOrderUpload> GetPurchaseOrderList(IFormFile file)
        {
            List<PurchaseOrderUpload> purchaseOrderUploadList = null;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.RegisterClassMap<PurchaseOrderMap>();
                    purchaseOrderUploadList = csv.GetRecords<PurchaseOrderUpload>().ToList();
                }
            }
            return purchaseOrderUploadList;
        }


        /// <summary>
        /// Get the purchase order data from the file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<POProductUploadData> GetPurchaseOrderDataFromFile(IFormFile file)
        {
            List<POProductUploadData> purchaseOrderUploadList = new List<POProductUploadData>();

            var fileExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);

            if (fileExtension == "csv")
            {
                purchaseOrderUploadList = ReadPurchaseOrderDataFromCsvFile(file);
            }
            else if (fileExtension == "xlsx")
            {
                purchaseOrderUploadList = ReadPurchaseOrderDataFromExcelFile(file);
            }

            return purchaseOrderUploadList;
        }

        /// <summary>
        /// Read the purchase order data from the csv file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private List<POProductUploadData> ReadPurchaseOrderDataFromCsvFile(IFormFile file)
        {
            List<POProductUploadData> purchaseOrderUploadList = new List<POProductUploadData>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.RegisterClassMap<POProductDataMap>();
                    purchaseOrderUploadList = csv.GetRecords<POProductUploadData>().ToList();
                }
            }

            return purchaseOrderUploadList;
        }

        /// <summary>
        /// Read the purchase order data from the xlsx file(using Ep plus)
        /// </summary>
        /// <param name="file"></param>
        private List<POProductUploadData> ReadPurchaseOrderDataFromExcelFile(IFormFile file)
        {
            List<POProductUploadData> purchaseOrderUploadList = new List<POProductUploadData>();
            var stream = file.OpenReadStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First();//package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (var row = 2; row <= rowCount; row++)
                {
                    var poProductUploadData = new POProductUploadData();

                    poProductUploadData.PoNumber = worksheet.Cells[row, 1].Value?.ToString();
                    poProductUploadData.ProductReference = worksheet.Cells[row, 2].Value?.ToString();
                    poProductUploadData.ProductDescription = worksheet.Cells[row, 3].Value?.ToString();

                    //if etd value is not null
                    if (worksheet.Cells[row, 4].Value != null)
                    {
                        //Reading the date value from excel using epplus
                        //we received the long data value when we read date value from epplus
                        //identity the received value is long data type
                        //convert the value to datetime

                        try
                        {
                            long etdLongDate;
                            //check if the value is long data type
                            if (long.TryParse(worksheet.Cells[row, 4].Value.ToString(), out etdLongDate))
                            {
                                //convert to date time
                                DateTime result = DateTime.FromOADate(etdLongDate);
                                //convert to date format(dd/MM/yyyy)
                                poProductUploadData.Etd = result.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                //split the date values using the delimiter("/")
                                string[] dateValues = worksheet.Cells[row, 4].Value.ToString().Split("/");

                                //if splitted value count equal to 3 then form the date value in (dd/mm/yyyy) format

                                if (dateValues.Any() && dateValues.Count() == 3)
                                {
                                    //concatenate day and month
                                    string etd = dateValues[0] + "/" + dateValues[1];
                                    //split the year and time
                                    string[] yearValues = dateValues[2].Split(" ");
                                    //take the year value
                                    etd = etd + "/" + yearValues[0];
                                    poProductUploadData.Etd = etd;
                                }
                                else
                                    poProductUploadData.Etd = worksheet.Cells[row, 4].Value.ToString();

                            }
                        }
                        catch (Exception)
                        {
                            poProductUploadData.Etd = worksheet.Cells[row, 4].Value.ToString();
                        }
                    }

                    poProductUploadData.DestinationCountry = worksheet.Cells[row, 5].Value?.ToString();
                    poProductUploadData.Quantity = worksheet.Cells[row, 6].Value?.ToString();
                    poProductUploadData.Barcode = worksheet.Cells[row, 7].Value?.ToString();
                    poProductUploadData.FactoryReference = worksheet.Cells[row, 8].Value?.ToString();
                    poProductUploadData.ColorCode = worksheet.Cells[row, 9].Value?.ToString();
                    poProductUploadData.ColorName = worksheet.Cells[row, 10].Value?.ToString();
                    purchaseOrderUploadList.Add(poProductUploadData);
                }
            }

            return purchaseOrderUploadList;
        }
    }

    public class PurchaseOrderMap : ClassMap<PurchaseOrderUpload>
    {
        public PurchaseOrderMap()
        {

            Map(m => m.Pono).Index(0);
            Map(m => m.Product).Index(1);
            Map(m => m.ProductBarcode).Index(2);
            Map(m => m.ProductDescription).Index(3);
            Map(m => m.FtyRef).Index(4);
            Map(m => m.Etd).Index(5);
            Map(m => m.Quantity).Index(6);
            Map(m => m.DestinationCountry).Index(7);
            Map(m => m.Customer).Index(8);
            Map(m => m.Supplier).Index(9);
            Map(m => m.CustomerContact).Index(10);
            Map(m => m.CustomerDepartment).Index(11);
            Map(m => m.AEID).Index(12);
            Map(m => m.OfficeIncharged).Index(13);
            Map(m => m.BookingDate).Index(14);
            Map(m => m.Id).ConvertUsing(row => row.Context.RawRow - 1);
        }
    }

    public class POProductDataMap : ClassMap<POProductUploadData>
    {
        public POProductDataMap()
        {

            Map(m => m.PoNumber).Index(0);
            Map(m => m.ProductReference).Index(1);
            Map(m => m.ProductDescription).Index(2);
            Map(m => m.Etd).Index(3);
            Map(m => m.DestinationCountry).Index(4);
            Map(m => m.Quantity).Index(5);
            Map(m => m.Barcode).Index(6);
            Map(m => m.FactoryReference).Index(7);
            Map(m => m.ColorCode).Index(8);
            Map(m => m.ColorName).Index(9);
        }
    }
}
