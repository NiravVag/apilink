using System;
using System.Collections.Generic;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DTO.Quotation;
using Contracts.Managers;
using DTO.File;
using DTO.InspectionCertificate;
using Entities.Enums;
using Microsoft.Extensions.Options;
using DTO.FBInternalReport;
using DTO.MasterConfig;
using DTO.Inspection;

namespace LINK_UI.FileModels
{
    public class InspectionCertificateModelPDF : IPDF
    {
        private readonly IHostingEnvironment _environnement = null;
        private Document Document = null;
        private TextFrame AddressFrame = null;
        private Section Section = null;
        private Table productTable = null;
        private Color TableBorder = Colors.Black;
        private Color TableBlue = Colors.DarkBlue;
        private Color TableGray = Colors.Gray;

        public InspectionCertificateModelPDF(IHostingEnvironment environnement)
        {
            _environnement = environnement;
        }

        public bool isRemarksNewPage = false;
        public bool isSignedNewPage = false;
        public FileResponse CreateICDocument(InspectionCertificatePDF model)
        {
            var entity = model.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            // Create a new MigraDoc document
            this.Document = new Document();
            this.Document.Info.Title = "Inspection Certificate";
            this.Document.Info.Subject = "Inspection Certificate details";
            this.Document.Info.Author = entity;

            DefineStyles();
            if (model != null)
            {
                CreatePage(model);
            }

            // FillContent();
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);

            renderer.Document = Document;
            renderer.RenderDocument();
            using (var memory = new MemoryStream())
            {
                renderer.PdfDocument.Save(memory);
                renderer.PdfDocument.Close();
                return new FileResponse
                {
                    Name = $"IC_{model.Id}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
                    Content = memory.ToArray(),
                    MimeType = "application/pdf",
                    Result = FileResult.Success
                };
            }
        }
        private void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.Document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = this.Document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style.ParagraphFormat.LeftIndent = 30;
            style = this.Document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.Document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.Document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        private void CreatePage(InspectionCertificatePDF model)
        {
            if (model != null)
            {
                var imageLogo = model?.EntityMasterConfigs?.Where(x => x.Type == (int)EntityConfigMaster.ImageLogo).Select(x => x.Value).FirstOrDefault();
                var apiInspTitle = model?.EntityMasterConfigs?.Where(x => x.Type == (int)EntityConfigMaster.API_INSP_Title).Select(x => x.Value).FirstOrDefault();
                var apiHOAddress = model?.EntityMasterConfigs?.Where(x => x.Type == (int)EntityConfigMaster.API_HO_Address).Select(x => x.Value).FirstOrDefault();
                var icRemarks = model?.EntityMasterConfigs?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.ICRemarks)?.Value;
                var icFooter = model?.EntityMasterConfigs?.FirstOrDefault(x => x.Type == (int)EntityConfigMaster.ICFooter)?.Value;

                string imageICChop;
                string imageICSign;
                if (model.FactoryCountryId.HasValue)
                {
                    imageICChop = model?.EntityMasterConfigs?.Where(x => x.CountryId == model.FactoryCountryId && x.Type == (int)EntityConfigMaster.ImageICChop).Select(x => x.Value).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(imageICChop))
                        imageICChop = model?.EntityMasterConfigs?.Where(x => x.CountryId == null && x.Type == (int)EntityConfigMaster.ImageICChop).Select(x => x.Value).FirstOrDefault();

                    imageICSign = model?.EntityMasterConfigs?.Where(x => x.CountryId == model.FactoryCountryId && x.Type == (int)EntityConfigMaster.ImageICSign).Select(x => x.Value).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(imageICSign))
                        imageICSign = model?.EntityMasterConfigs?.Where(x => x.CountryId == null && x.Type == (int)EntityConfigMaster.ImageICSign).Select(x => x.Value).FirstOrDefault();
                }
                else
                {
                    imageICChop = model?.EntityMasterConfigs?.Where(x => x.CountryId == null && x.Type == (int)EntityConfigMaster.ImageICChop).Select(x => x.Value).FirstOrDefault();
                    imageICSign = model?.EntityMasterConfigs?.Where(x => x.CountryId == null && x.Type == (int)EntityConfigMaster.ImageICSign).Select(x => x.Value).FirstOrDefault();
                }



                //get the ic remarks and ic footer based on the entity master configuration

                // Each MigraDoc document needs at least one section.
                Section = this.Document.AddSection();
                Section.PageSetup = new PageSetup() { RightMargin = "1cm", LeftMargin = "1cm" };

                // Create the text frame for the address
                Table tableInfo = Section.Headers.Primary.AddTable();

                tableInfo.Style = "Table";
                tableInfo.Borders.Color = TableBorder;
                tableInfo.Rows.LeftIndent = 0;
                tableInfo.Format.SpaceBefore = 10;
                tableInfo.Format.SpaceAfter = 10;
                Column columnInfoLabel = tableInfo.AddColumn("3cm");
                columnInfoLabel.Format.Alignment = ParagraphAlignment.Left;
                Column columnInfoValue = tableInfo.AddColumn("10cm");
                columnInfoValue.Format.Alignment = ParagraphAlignment.Left;
                Column columnInfoTitle = tableInfo.AddColumn("5cm");
                columnInfoTitle.Format.Alignment = ParagraphAlignment.Left;

                if (model.ICTitle != null)
                {
                    Row rowInfoTitle = tableInfo.AddRow();
                    rowInfoTitle.Height = "1.0cm";
                    var rParagraph = rowInfoTitle.Cells[0].AddParagraph(model.ICTitle);
                    rowInfoTitle.Cells[0].Format.Font.Bold = true;
                    rowInfoTitle.Cells[0].Format.Font.Size = 18;
                    rowInfoTitle.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    rowInfoTitle.Cells[0].MergeRight = 2;
                }
                Row rowInfoCustomer = tableInfo.AddRow();

                // Put a logo in the header
                Image image = rowInfoCustomer.Cells[0].AddImage(Path.Combine(_environnement.ContentRootPath, imageLogo));
                image.Height = "1.5cm";
                image.LockAspectRatio = true;
                image.RelativeVertical = RelativeVertical.Line;

                image.Top = ShapePosition.Center;
                image.RelativeHorizontal = RelativeHorizontal.Margin;
                image.WrapFormat.Style = WrapStyle.Through;

                rowInfoCustomer.Cells[0].Column.Width = 100;

                var rParagraphCompanyName = rowInfoCustomer.Cells[1].AddParagraph(apiInspTitle);

                var rParagraphAddress = rowInfoCustomer.Cells[1].AddParagraph(apiHOAddress);
                rowInfoCustomer.Cells[1].Format.Font.Bold = false;
                rowInfoCustomer.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                rParagraphAddress.AddLineBreak();
                rowInfoCustomer.Cells[1].Column.Width = 340;

                if (model.ICNo != null)
                {
                    if (model.ICTitleId == (int)InspectionCertificateTitle.InspectionCertificate)
                        rowInfoCustomer.Cells[2].AddParagraph("IC Number : " + model.ICNo);
                    else
                        rowInfoCustomer.Cells[2].AddParagraph(model.ICNo);
                }
                rowInfoCustomer.Cells[2].Format.Font.Bold = false;
                rowInfoCustomer.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                rowInfoCustomer.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                rowInfoCustomer.Cells[2].Column.Width = 100;

                this.AddressFrame = Section.AddTextFrame();
                this.AddressFrame.Height = "15.0cm";
                this.AddressFrame.Width = "7.0cm";
                this.AddressFrame.Left = ShapePosition.Left;
                this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                this.AddressFrame.Top = "5.0cm";
                this.AddressFrame.RelativeVertical = RelativeVertical.Page;

                var bookingInfo = this.AddressFrame.AddTable();
                bookingInfo.Format.Font.Size = 9;

                bookingInfo.AddColumn("11.5cm");
                bookingInfo.Format.Alignment = ParagraphAlignment.Left;
                bookingInfo.AddColumn("7.5cm");
                bookingInfo.Format.Alignment = ParagraphAlignment.Left;

                Row finalTabRow = bookingInfo.AddRow();
                finalTabRow.Format.Font.Size = 9;
                var textFrame1 = finalTabRow.Cells[0].AddTextFrame();
                var textFrame2 = finalTabRow.Cells[1].AddTextFrame();

                var tab1 = textFrame1.AddTable();
                tab1.AddColumn("2.5cm");
                tab1.Format.Alignment = ParagraphAlignment.Left;
                tab1.AddColumn("9cm");
                tab1.Format.Alignment = ParagraphAlignment.Left;
                tab1.Format.Font.Size = 9;
                tab1.Style = "Table";


                finalTabRow = tab1.AddRow();

                var BuyerName = finalTabRow.Cells[0].AddParagraph("Buyer ");
                BuyerName.Format.SpaceBefore = 15;
                BuyerName.Format.Font.Bold = true;
                if (string.IsNullOrEmpty(model.BuyerName))
                {
                    BuyerName = finalTabRow.Cells[1].AddParagraph($": {model.CustomerName}");
                }
                else
                {
                    BuyerName = finalTabRow.Cells[1].AddParagraph($": {model.BuyerName}");
                }
                BuyerName.Format.SpaceBefore = 15;
                BuyerName.Format.SpaceAfter = 5;

                finalTabRow = tab1.AddRow();

                var supplierName = finalTabRow.Cells[0].AddParagraph("Supplier ");
                supplierName.Format.Font.Bold = true;
                var benefname = finalTabRow.Cells[1].AddParagraph($": {model.BeneficiaryName}");
                benefname.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                benefname.Format.SpaceAfter = 5;

                finalTabRow = tab1.AddRow();

                var supplierAddress = finalTabRow.Cells[0].AddParagraph("Address ");
                supplierAddress.Format.Font.Bold = true;
                var supplierAddressvalue = finalTabRow.Cells[1].AddParagraph($": {model.SupplierAddress}");
                supplierAddressvalue.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                supplierAddressvalue.Format.SpaceAfter = 15;


                finalTabRow = tab1.AddRow();

                var serviceDate = finalTabRow.Cells[0].AddParagraph("Inspection Date ");
                serviceDate.Format.Font.Bold = true;

                serviceDate.Format.SpaceAfter = 5;
                serviceDate.AddLineBreak();
                serviceDate.AddLineBreak();
                serviceDate.AddLineBreak();

                var bookingNumber = model.ProductDetails.Select(y => y.BookingNumber).Distinct().ToList();
                var stringdate = string.Empty;

                for (var i = 0; i < bookingNumber.Count(); i++)
                {
                    var serviceFromDate = model.ProductDetails.Where(y => bookingNumber[i] == y.BookingNumber).Select(x => x.serviceDateFrom).FirstOrDefault();
                    var serviceToDate = model.ProductDetails.Where(y => bookingNumber[i] == y.BookingNumber).Select(x => x.serviceDateTo).FirstOrDefault();
                    if (i != 0 && i < bookingNumber.Count())
                    {
                        stringdate += ", ";
                    }
                    if (serviceFromDate == serviceToDate)
                    {
                        stringdate += serviceFromDate.ToString("MMMM d, yyyy");
                    }
                    else
                    {
                        stringdate += serviceFromDate.ToString("MMMM d, yyyy") + " to " + serviceToDate.ToString("MMMM d, yyyy");

                    }
                }
                serviceDate = finalTabRow.Cells[1].AddParagraph($": {stringdate} ");

                tab1 = textFrame2.AddTable();
                tab1.AddColumn("2cm");
                tab1.Format.Alignment = ParagraphAlignment.Left;
                tab1.AddColumn("5.5cm");
                tab1.Format.Alignment = ParagraphAlignment.Left;
                tab1.Format.Font.Size = 9;
                tab1.Style = "Table";

                finalTabRow = tab1.AddRow();

                //remove the destination table for softline, only show for the hardline
                if (model.BusinessLine != (int)BusinessLine.SoftLine)
                {
                    var destinationCountry = finalTabRow.Cells[0].AddParagraph("Destination ");
                    destinationCountry.Format.SpaceBefore = 15;
                    destinationCountry.Format.Font.Bold = true;
                    destinationCountry = finalTabRow.Cells[1].AddParagraph($": {string.Join(", ", model.ProductDetails.Select(x => x.DestinationCountry).Distinct().ToArray())}");
                    destinationCountry.Format.SpaceAfter = 5;
                    destinationCountry.Format.SpaceBefore = 15;
                }

                if (model?.Comment != null && model?.Comment != "")
                {
                    finalTabRow = tab1.AddRow();
                    var commentrow = finalTabRow.Cells[0].AddParagraph("Comment ");
                    commentrow.Format.Font.Bold = true;
                    commentrow = finalTabRow.Cells[1].AddParagraph($": {model.Comment}");
                    commentrow.Format.SpaceAfter = 5;
                }

                finalTabRow = tab1.AddRow();
                //remove the destination table for softline, only show for the hardline
                if (model.BusinessLine != (int)BusinessLine.SoftLine)
                {
                    AqlTableData(finalTabRow.Cells[0].AddTextFrame().AddTable(), model);
                }

                var bookingCount = model.ProductDetails.Select(y => y.BookingNumber).Distinct().Count();

                Paragraph paragraphMiddle = this.AddressFrame.AddParagraph("Description of goods inspected");

                int bookingSpaceBefore = 30;
                int totalCount = 0;
                if (bookingCount >= 1)
                {
                    totalCount = bookingSpaceBefore + (30 * bookingCount);
                }
                paragraphMiddle.Format.SpaceBefore = totalCount;
                paragraphMiddle.AddLineBreak();
                paragraphMiddle.AddLineBreak();

                ProductTableData(model);

                if (model != null && model.ApprovalDate != null)
                    FooterDetails(model.ApprovalDate, model.IsDraft, model.ICTitleId, icRemarks, imageICChop, imageICSign, model.BusinessLine);
                else
                    FooterDetails(null, model.IsDraft, model.ICTitleId, icRemarks, imageICChop, imageICSign, model.BusinessLine);

                // Create footer
                Table tableFooterInfo = Section.Footers.Primary.AddTable();
                tableFooterInfo.Style = "Table";
                tableFooterInfo.Rows.LeftIndent = 0;
                tableFooterInfo.Format.SpaceBefore = 10;
                tableFooterInfo.Format.SpaceAfter = 10;
                Column columnFooterInfoLabel = tableFooterInfo.AddColumn("3cm");
                columnInfoLabel.Format.Alignment = ParagraphAlignment.Left;
                Column columnFooterInfoValue = tableFooterInfo.AddColumn("13cm");
                columnInfoValue.Format.Alignment = ParagraphAlignment.Center;
                Column columnFooterInfoTitle = tableFooterInfo.AddColumn("3cm");
                columnInfoTitle.Format.Alignment = ParagraphAlignment.Right;

                Row rowFooter = tableFooterInfo.AddRow();
                rowFooter.Cells[0].AddParagraph("v1");
                rowFooter.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                if (!string.IsNullOrWhiteSpace(icFooter))
                {
                    rowFooter.Cells[1].AddParagraph(icFooter);
                    rowFooter.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                }
                else
                {
                    rowFooter.Cells[1].AddParagraph();
                }


                var paragraphPage = rowFooter.Cells[2].AddParagraph();
                paragraphPage.AddPageField();
                paragraphPage.AddText(" of ");
                paragraphPage.AddNumPagesField();
                paragraphPage.Format.Font.Size = 9;
                paragraphPage.Format.Alignment = ParagraphAlignment.Right;
                Section.Footers.EvenPage.Add(tableFooterInfo.Clone());
            }
        }
        private void aqlTable(Table table)
        {
            table.Style = "Table";
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("2cm");

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("1.5cm");
            //column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            var productId = row.Cells[0].AddParagraph("AQL");
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[1].AddParagraph("Cri");
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[2].AddParagraph("Maj");
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[3].AddParagraph("Min");
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
        }
        private void AqlTableData(Table table, InspectionCertificatePDF model)
        {
            aqlTable(table);
            var aqlData = model != null && model.ProductDetails != null ? model.ProductDetails.FirstOrDefault() : null;

            var row = table.AddRow();
            if (aqlData != null)
            {
                row.Cells[0].AddParagraph($"{aqlData.AQL}");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[1].AddParagraph($"{aqlData.Critical}");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[2].AddParagraph($"{aqlData.Major}");
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                row.Cells[3].AddParagraph($"{aqlData.Minor}");
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
            }
        }
        private void ProductHeaderTable(int businessLine)
        {
            productTable = this.AddressFrame.AddTable();
            productTable.Style = "Table";
            productTable.Borders.Color = TableBorder;
            productTable.Borders.Width = 0.25;
            productTable.Borders.Left.Width = 0.5;
            productTable.Borders.Right.Width = 0.5;
            productTable.Rows.LeftIndent = 0;


            // Before you can add a row, you must define the columns
            Column column = productTable.AddColumn("3cm");

            //for the hardline column width 
            if (businessLine == (int)BusinessLine.HardLine)
            {
                column = productTable.AddColumn("5cm");
                column.Format.Alignment = ParagraphAlignment.Right;

                column = productTable.AddColumn("5cm");
                //column.Format.Alignment = ParagraphAlignment.Right;
            }
            else if (businessLine == (int)BusinessLine.SoftLine) //for the softline column width  
            {
                column = productTable.AddColumn("3cm");
                column.Format.Alignment = ParagraphAlignment.Right;

                column = productTable.AddColumn("3cm");

                //for color columns add
                column = productTable.AddColumn("2cm");
                column.Format.Alignment = ParagraphAlignment.Right;

                //for color code columns add
                column = productTable.AddColumn("2cm");
                column.Format.Alignment = ParagraphAlignment.Right;
            }


            column = productTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            Row row = productTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            var productId = row.Cells[0].AddParagraph("INS No.");
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[1].AddParagraph("PO");
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[2].AddParagraph("Item Ref.");
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            //for softline add this columns
            if (businessLine == (int)BusinessLine.SoftLine)
            {

                row.Cells[3].AddParagraph("Color");
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[4].AddParagraph("Code");
                row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[5].AddParagraph("Quantity");
                row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[6].AddParagraph("Unit");
                row.Cells[6].VerticalAlignment = VerticalAlignment.Center;

            }
            else if (businessLine == (int)BusinessLine.HardLine) //for hardline show this columns
            {
                row.Cells[3].AddParagraph("Quantity");
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[4].AddParagraph("Unit");
                row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private void ProductTableData(InspectionCertificatePDF model)
        {
            ProductHeaderTable(model.BusinessLine.GetValueOrDefault());
            int i = 0;
            int pageNumber = 1;
            int firstPageSize = 9;
            int morePageSize = 14;
            foreach (var product in model.ProductDetails)
            {
                i++;

                if ((pageNumber == 1 && i > firstPageSize) || (pageNumber > 1 && i > morePageSize))
                {
                    this.Section.AddPageBreak();

                    this.AddressFrame = Section.AddTextFrame();
                    this.AddressFrame.Height = "3.0cm";
                    this.AddressFrame.Width = "7.0cm";
                    this.AddressFrame.Left = ShapePosition.Left;
                    this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                    this.AddressFrame.Top = "5.0cm";
                    //this.AddressFrame. = "4.0cm";
                    this.AddressFrame.RelativeVertical = RelativeVertical.Page;

                    ProductHeaderTable(model.BusinessLine.GetValueOrDefault());
                    i = 1;
                    pageNumber++;
                }


                var row = productTable.AddRow();
                row.Height = 20;
                row.Cells[0].AddParagraph($"{product.BookingNumber}");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[1].AddParagraph($"{product.PONo}");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[2].AddParagraph($"{product.ProductCode}");
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

                if (model.BusinessLine == (int)BusinessLine.SoftLine)
                {
                    row.Cells[3].AddParagraph($"{product.Color}");
                    row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                    row.Cells[4].AddParagraph($"{product.ColorCode}");
                    row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

                    row.Cells[5].AddParagraph($"{product.Quantity}");
                    row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

                    row.Cells[6].AddParagraph($"{product.Unit}");
                    row.Cells[6].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[6].VerticalAlignment = VerticalAlignment.Center;
                }
                else if (model.BusinessLine == (int)BusinessLine.HardLine)
                {
                    row.Cells[3].AddParagraph($"{product.Quantity}");
                    row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                    row.Cells[4].AddParagraph($"{product.Unit}");
                    row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                }


                if (model.BusinessLine != (int)BusinessLine.SoftLine)
                {
                    var row1 = productTable.AddRow();
                    row1.Height = 20;
                    row1.Cells[0].AddParagraph("Product Desc.");
                    row1.Cells[0].Format.Font.Bold = true;
                    row1.Cells[0].Shading.Color = Colors.LightGray;
                    row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                    row1.Cells[0].MergeRight = 1;
                    row1.Cells[0].Format.Alignment = ParagraphAlignment.Right;

                    row1.Cells[2].AddParagraph($"{product.ProductDescription}");
                    row1.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                    row1.Cells[2].MergeRight = 2;
                    row1.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                }
            }
            if (pageNumber == 1 && (i == 5 || i == 6))
            {
                isRemarksNewPage = true;
            }
            else if (pageNumber == 1 && i >= 7)
            {
                isSignedNewPage = true;
            }

            if (pageNumber > 1 && (i == 9 || i == 10))
            {
                isRemarksNewPage = true;
            }
            else if (pageNumber > 1 && i >= 11)
            {
                isSignedNewPage = true;
            }

        }

        private void FooterDetails(string ApprovalDate, bool isDraft, int ICTitleId, string icRemarks, string imageICChop, string imageICSign, int? businessLine)
        {
            string approvalDate = "Hong Kong " + ApprovalDate;
            string footerDetails = "To Whom It May Concern,";
            string footerData = "We hereby confirmed that the goods described " +
                                    "above have been inspected and allowed to be released under Buyer instruction dated on " + ApprovalDate;
            string signData = "For and on behalf of";
            string ceoSignName = "Marie-Annabelle Mermaz (CEO)";
            string signName = "Authorized Signature(s)";

            this.AddressFrame.Width = "19cm";
            if (isSignedNewPage)
            {
                this.Section.AddPageBreak();
                this.AddressFrame = Section.AddTextFrame();
                this.AddressFrame.Height = "3.0cm";
                this.AddressFrame.Width = "19cm";
                this.AddressFrame.Left = ShapePosition.Left;
                this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                this.AddressFrame.Top = "5.0cm";
                this.AddressFrame.RelativeVertical = RelativeVertical.Page;
            }
            var paragraphFooter = this.AddressFrame.AddParagraph();

            paragraphFooter.Format.Font.Size = 10;
            paragraphFooter.Format.Alignment = ParagraphAlignment.Left;
            paragraphFooter.Format.Font.Color = Colors.Black;
            paragraphFooter.Format.SpaceAfter = 10;
            paragraphFooter.Format.SpaceBefore = 10;
            paragraphFooter.AddText(approvalDate);
            paragraphFooter.AddLineBreak();

            var paragraphFooter1 = this.AddressFrame.AddParagraph();

            paragraphFooter1.Format.LeftIndent = 20;

            paragraphFooter1.AddText(footerDetails);
            paragraphFooter1.AddLineBreak();
            paragraphFooter1.Format.SpaceAfter = 10;

            var paragraphFooter2 = this.AddressFrame.AddParagraph();

            paragraphFooter2.Format.LeftIndent = 0;

            paragraphFooter2.AddText(footerData);
            paragraphFooter2.AddLineBreak();

            if (!isDraft)//draft version use for hard copy. so no need chop & signature and on behalf.. of wordings 
            {
                paragraphFooter2.Format.SpaceAfter = 10;
                Paragraph paragraphFooter3 = this.AddressFrame.AddParagraph();

                paragraphFooter3.Format.Alignment = ParagraphAlignment.Center;
                paragraphFooter3.Format.SpaceAfter = 10;
                paragraphFooter3.AddText(signData);
                paragraphFooter3.AddLineBreak();

                paragraphFooter3.AddText(ceoSignName);
                paragraphFooter3.AddLineBreak();

                Paragraph paragraphFooter4 = this.AddressFrame.AddParagraph();

                if (ICTitleId == (int)InspectionCertificateTitle.InspectionCertificate && imageICChop != null)
                {
                    Image image1 = paragraphFooter4.AddImage(Path.Combine(_environnement.ContentRootPath, imageICChop));
                    image1.Height = "1.5cm";
                    image1.LockAspectRatio = true;
                    image1.RelativeVertical = RelativeVertical.Line;

                    image1.Top = ShapePosition.Center;
                    image1.Left = ShapePosition.Center;
                    image1.RelativeHorizontal = RelativeHorizontal.Margin;
                    image1.WrapFormat.Style = WrapStyle.Through;
                }

                if (imageICSign != null)
                {
                    Image image = paragraphFooter4.AddImage(Path.Combine(_environnement.ContentRootPath, imageICSign));
                    image.Height = "1.5cm";
                    image.LockAspectRatio = true;
                    image.RelativeVertical = RelativeVertical.Line;

                    image.Top = ShapePosition.Center;
                    image.Left = ShapePosition.Center;
                    image.RelativeHorizontal = RelativeHorizontal.Margin;
                    image.WrapFormat.Style = WrapStyle.Through;
                }
                //if the image is not configure then show empty space
                if (imageICSign == null && imageICChop == null)
                {
                    paragraphFooter4.Format.SpaceAfter = 80;
                }

                paragraphFooter4.Format.Alignment = ParagraphAlignment.Center;

                Paragraph paragraphFooter5 = this.AddressFrame.AddParagraph();

                paragraphFooter5.Format.TabStops.ClearAll();
                paragraphFooter5.Format.TabStops.AddTabStop("8cm", TabAlignment.Right, TabLeader.Lines);
                paragraphFooter5.AddTab();

                paragraphFooter5.AddLineBreak();

                paragraphFooter5.AddText(signName);

                paragraphFooter5.Format.SpaceBefore = 5;
                paragraphFooter5.Format.Alignment = ParagraphAlignment.Center;
                paragraphFooter5.Format.SpaceAfter = 10;
            }
            else
            {
                //for draft mode if businessline is softline then add sign data, ceo sign name
                if (businessLine == (int)BusinessLine.SoftLine)
                {
                    paragraphFooter2.Format.SpaceAfter = 10;
                    Paragraph paragraphFooter3 = this.AddressFrame.AddParagraph();

                    paragraphFooter3.Format.Alignment = ParagraphAlignment.Center;
                    paragraphFooter3.Format.SpaceAfter = 10;
                    paragraphFooter3.AddText(signData);
                    paragraphFooter3.AddLineBreak();

                    paragraphFooter3.AddText(ceoSignName);
                    paragraphFooter3.AddLineBreak();

                    paragraphFooter3.Format.SpaceAfter = 60;

                    Paragraph paragraphFooter5 = this.AddressFrame.AddParagraph();

                    paragraphFooter5.Format.TabStops.ClearAll();
                    paragraphFooter5.Format.TabStops.AddTabStop("8cm", TabAlignment.Right, TabLeader.Lines);
                    paragraphFooter5.AddTab();

                    paragraphFooter5.AddLineBreak();

                    paragraphFooter5.AddText(signName);

                    paragraphFooter5.Format.SpaceBefore = 5;
                    paragraphFooter5.Format.Alignment = ParagraphAlignment.Center;
                    paragraphFooter5.Format.SpaceAfter = 10;
                }
                else
                {
                    paragraphFooter2.Format.SpaceAfter = 80;
                }
            }
            if (isRemarksNewPage)
            {
                this.Section.AddPageBreak();
                this.AddressFrame = Section.AddTextFrame();
                this.AddressFrame.Height = "3.0cm";
                this.AddressFrame.Width = "19cm";
                this.AddressFrame.Left = ShapePosition.Left;
                this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                this.AddressFrame.Top = "5.0cm";
                this.AddressFrame.RelativeVertical = RelativeVertical.Page;
            }

            //configurable remarks
            if (icRemarks != null)
            {
                Paragraph paragraphfooterRemarks = this.AddressFrame.AddParagraph();

                paragraphfooterRemarks.Format.Font.Size = 9;
                paragraphfooterRemarks.Format.SpaceAfter = 10;
                paragraphfooterRemarks.Format.Alignment = ParagraphAlignment.Left;
                paragraphfooterRemarks.Format.Font.Color = Colors.Black;

                paragraphfooterRemarks.AddText(icRemarks);
                paragraphfooterRemarks.AddLineBreak();
            }
        }
    }
}
