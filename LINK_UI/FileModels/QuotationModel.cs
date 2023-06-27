using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System.Xml.XPath;
using MigraDoc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DTO.Quotation;
using Contracts.Managers;
using DTO.File;
using DTO.Common;
using DTO.FBInternalReport;
using Microsoft.Extensions.Options;
using System.Globalization;
using Contracts.Repositories;
using DTO.MasterConfig;

namespace LINK_UI.FileModels
{
    public class QuotationPreview :ApiCommonData, IQuotationPDF
    {
        private readonly IHostingEnvironment _environnement = null;
        private Document Document = null;
        private TextFrame AddressFrame = null;
        private Section Section = null;
        private Table Table = null;
        
        private Color TableBorder = Colors.Black;
        private Color TableBlue = Colors.DarkBlue;
        private Color TableGray = Colors.Gray;

        public QuotationPreview(IHostingEnvironment environnement)
        {
            _environnement = environnement;

        }

        public FileResponse CreateDocument(QuotationDetails model)
        {
            var entity = model.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            // Create a new MigraDoc document
            this.Document = new Document();
            this.Document.Info.Title = "Quotation";
            this.Document.Info.Subject = "Quotation details";
            this.Document.Info.Author = entity;

            DefineStyles();

            CreatePage(model);

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
                    Name = $"Quo_{model.Id}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
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

        private void CreatePage(QuotationDetails model)
        {
            var entity = model.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            var imageLogo = model.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.ImageLogo).Select(x => x.Value).FirstOrDefault();
            var headOfficeWrapAddress = model.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.API_HO_Wrap_Address).Select(x => x.Value).FirstOrDefault();
            var apiInspTitle = model.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.API_INSP_Title).Select(x => x.Value).FirstOrDefault();
            var apiAuditTitle = model.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.API_AUD_Title).Select(x => x.Value).FirstOrDefault();

            int FactoryAddressCharLimit = 24;
            bool isFooter1NewPage = false;
            bool isFooter2NewPage = false;

            // Each MigraDoc document needs at least one section.
            Section = this.Document.AddSection();
            Section.PageSetup = new PageSetup() { RightMargin = "1cm", LeftMargin = "1cm" };
            // Put a logo in the header
            Image image = Section.Headers.Primary.AddImage(Path.Combine(_environnement.ContentRootPath, imageLogo));
            image.Height = "1.5cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.LockAspectRatio = true;
            //image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Left;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.WrapFormat.Style = WrapStyle.Through;

            Paragraph paragraphheader = Section.Headers.Primary.AddParagraph();
            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
            {
                paragraphheader.AddText(""+ apiInspTitle + " \n "+ headOfficeWrapAddress.Replace("^", "\n") + "");
            }
            else
            {
                paragraphheader.AddText(""+ apiAuditTitle + " \n "+ headOfficeWrapAddress.Replace("^", "\n") + "");
            }
            paragraphheader.Format.Font.Size = 9;
            paragraphheader.Format.Alignment = ParagraphAlignment.Center;
            paragraphheader.Format.LeftIndent = 10;
            paragraphheader.Format.Font.Color = Colors.Gray;

            //section.Headers.Primary.Format.Borders.Color = Colors.Black;  

            // Create footer
            Paragraph paragraph = Section.Footers.Primary.AddParagraph();
            paragraph.AddTab();
            paragraph.AddPageField();
            paragraph.AddText(" of ");
            paragraph.AddNumPagesField();

            //paragraph.AddText($"{DateTime.Now.ToString("dd/MM/yyyy")}");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            Section.Footers.EvenPage.Add(paragraph.Clone());



            // Create the text frame for the address
            this.AddressFrame = Section.AddTextFrame();
            this.AddressFrame.Height = "3.0cm";
            this.AddressFrame.Width = "7.0cm";
            this.AddressFrame.Left = ShapePosition.Left;
            this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.AddressFrame.Top = "4.0cm";
            this.AddressFrame.RelativeVertical = RelativeVertical.Page;


            var tableInfo = this.AddressFrame.AddTable();
            tableInfo.Style = "Table";
            tableInfo.Borders.Color = TableBorder;
            tableInfo.Rows.LeftIndent = 0;
            tableInfo.Format.SpaceAfter = 20;
            Column columnInfoLabel = tableInfo.AddColumn("2cm");
            columnInfoLabel.Format.Alignment = ParagraphAlignment.Left;
            Column columnInfoValue = tableInfo.AddColumn("8cm");
            columnInfoValue.Format.Alignment = ParagraphAlignment.Left;
            Column columnInfoTitle = tableInfo.AddColumn("9cm");
            columnInfoTitle.Format.Alignment = ParagraphAlignment.Left;

            var customerContactList = model.CustomerContactList?.Where(x => x.Quotation);
            if (customerContactList == null)
                customerContactList = new List<QuotationEntityContact> { model.CustomerContactList?.FirstOrDefault() };

            Row rowInfoCustomer = tableInfo.AddRow();
            var paraCustomer = rowInfoCustomer.Cells[0].AddParagraph("Customer");
            rowInfoCustomer.Cells[0].Format.Font.Bold = true;
            rowInfoCustomer.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            paraCustomer.Format.SpaceBefore = 5;
            paraCustomer.Format.SpaceAfter = 5;

            var _customerName = model.CustomerLegalName;
            if (_customerName.Length > FactoryAddressCharLimit)
            {
                _customerName= SplitStringWithSpace(_customerName, FactoryAddressCharLimit);
            }

            var rParagraph1 = rowInfoCustomer.Cells[1].AddParagraph(_customerName);
            rowInfoCustomer.Cells[1].Format.Font.Bold = false;
            rowInfoCustomer.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            rParagraph1.AddLineBreak();
            rParagraph1.AddLineBreak();
            if (customerContactList != null && customerContactList.Any())
            {
                rParagraph1.AddText($"Attn : {string.Join(", ", customerContactList?.Select(x => x.ContactName).ToArray())}");
                rParagraph1.AddLineBreak();
                rParagraph1.AddLineBreak();
                rParagraph1.AddText($"{string.Join(", ", customerContactList?.Select(x => x.ContactEmail).ToArray())}");
            }
            rParagraph1.AddLineBreak();
            rParagraph1.Format.LineSpacing = 10;
            rParagraph1.Format.SpaceBefore = 5;
            rParagraph1.Format.SpaceAfter = 5;

            var supplierContactList = model.SupplierContactList?.Where(x => x.Quotation);

            if (supplierContactList == null)
                supplierContactList = new List<QuotationEntityContact> { model.SupplierContactList?.FirstOrDefault() };

            Row rowInfoSupplier = tableInfo.AddRow();
            var paraSupplier = rowInfoSupplier.Cells[0].AddParagraph("Supplier");
            rowInfoSupplier.Cells[0].Format.Font.Bold = true;
            rowInfoSupplier.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            paraSupplier.Format.SpaceBefore = 5;
            paraSupplier.Format.SpaceAfter = 5;

            var _supplierName = model.SupplierLegalName;
            if (_supplierName.Length > FactoryAddressCharLimit)
            {
                _supplierName = SplitStringWithSpace(_supplierName, FactoryAddressCharLimit);
            }

            var rParagraphSupp2 = rowInfoSupplier.Cells[1].AddParagraph(_supplierName);
            rowInfoSupplier.Cells[1].Format.Font.Bold = false;
            rowInfoSupplier.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            rParagraphSupp2.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            rParagraphSupp2.AddLineBreak();
            rParagraphSupp2.AddLineBreak();
            if (supplierContactList != null && supplierContactList.Any())
            {
                rParagraphSupp2.AddText($"Contact : {string.Join(", ", supplierContactList?.Select(x => x.ContactName).ToArray())}");
                rParagraphSupp2.AddLineBreak();
                rParagraphSupp2.AddLineBreak();
                rParagraphSupp2.AddText($"{string.Join(", ", supplierContactList?.Select(x => $"{x.ContactEmail} {x.ContactPhone}").ToArray())}");
                rParagraphSupp2.AddLineBreak();
            }
            rParagraphSupp2.Format.LineSpacing = 10;
            rParagraphSupp2.Format.SpaceBefore = 5;
            rParagraphSupp2.Format.SpaceAfter = 5;


            var factoryList = model.FactoryContactList?.Where(x => x.Quotation);

            if (factoryList == null)
                factoryList = new List<QuotationEntityContact> { model.FactoryContactList?.FirstOrDefault() };

            Row rowInfoFactory = tableInfo.AddRow();
            var paraFactory = rowInfoFactory.Cells[0].AddParagraph("Factory & Address");
            rowInfoFactory.Cells[0].Format.Font.Bold = true;
            rowInfoFactory.Cells[0].Format.Alignment = ParagraphAlignment.Left;

            paraFactory.Format.SpaceBefore = 5;
            paraFactory.Format.SpaceAfter = 5;

            var _factoryName = model.LegalFactoryName;
            if (_factoryName.Length > FactoryAddressCharLimit)
            {
                _factoryName = SplitStringWithSpace(_factoryName, FactoryAddressCharLimit);
            }

            var rParagraphFactory2 = rowInfoFactory.Cells[1].AddParagraph(_factoryName);
            rowInfoFactory.Cells[1].Format.Font.Bold = false;
            rowInfoFactory.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            rParagraphFactory2.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            rParagraphFactory2.AddLineBreak();
            rParagraphFactory2.AddLineBreak();
            if (factoryList != null && factoryList.Any())
            {
                rParagraphFactory2.AddText($"Contact : {string.Join(", ", factoryList?.Select(x => x.ContactName).ToArray())}");
                rParagraphFactory2.AddLineBreak();
                rParagraphFactory2.AddLineBreak();
                rParagraphFactory2.AddText($"{string.Join(", ", factoryList?.Select(x => $"{x.ContactEmail} {x.ContactPhone}").ToArray())}");
                rParagraphFactory2.AddLineBreak();
                rParagraphFactory2.AddLineBreak();
            }

            if (model.FactoryAddress != null)
            {
                //factory length exceeds the FactoryAddressCharLimit(24) we are spliting the word
                if (model.FactoryAddress.Length > FactoryAddressCharLimit)
                {
                    rParagraphFactory2.AddText($"Address : {SplitStringWithSpace(model.FactoryAddress, FactoryAddressCharLimit)}");
                }
                else
                {
                    rParagraphFactory2.AddText($"Address : {model.FactoryAddress}");
                }
                rParagraphFactory2.AddLineBreak();
                rParagraphFactory2.Format.LineSpacing = 10;
                rParagraphFactory2.Format.SpaceBefore = 5;
                rParagraphFactory2.Format.SpaceAfter = 5;
            }

            var rParagraph2 = rowInfoCustomer.Cells[2].AddParagraph("QUOTATION");
            rParagraph2.Format.Font.Bold = true;
            rParagraph2.Format.Shading.Color = Colors.LightGray;
            rParagraph2.Format.Font.Size = 13;
            rParagraph2.Format.Alignment = ParagraphAlignment.Center;
            rParagraph2.Format.SpaceAfter = 5;
            rowInfoCustomer.Cells[2].Format.Font.Bold = true;
            rowInfoCustomer.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            rowInfoCustomer.Cells[2].MergeDown = 2;

            var rParagraph3 = rowInfoCustomer.Cells[2].AddParagraph(entity + " Reference");
            rParagraph3.Format.Font.Bold = true;
            rParagraph3.Format.Font.Size = 9;
            rParagraph3.Format.Font.Underline = Underline.Single;
            rParagraph3.Format.SpaceAfter = 5;

            var rText = rowInfoCustomer.Cells[2].AddTextFrame();
            var newTabQuot = rText.AddTable();
            newTabQuot.Format.Font.Size = 9;

            var cQUotLabel = newTabQuot.AddColumn("4cm");
            newTabQuot.Format.Alignment = ParagraphAlignment.Left;
            var cQUotValue = newTabQuot.AddColumn("5cm");
            cQUotValue.Format.Alignment = ParagraphAlignment.Left;

            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
            {
                Row quotRowNo = newTabQuot.AddRow();
                var inspNoLabel = quotRowNo.Cells[0].AddParagraph("Insp No :");
                inspNoLabel.Format.Font.Bold = true;

                var inspNoValue = quotRowNo.Cells[1].AddParagraph($"#{model.BookingNo}");
                inspNoValue.Format.Shading.Color = Colors.LightSkyBlue;
                inspNoValue.Format.SpaceAfter = 5;
            }
            else if (model.Service.Id == (int)Entities.Enums.Service.AuditId)
            {
                Row quotRowNo = newTabQuot.AddRow();
                var auditNoLabel = quotRowNo.Cells[0].AddParagraph("Audit No :");
                auditNoLabel.Format.Font.Bold = true;

                var auditNoValue = quotRowNo.Cells[1].AddParagraph($"#{model.BookingNo}");
                auditNoValue.Format.Shading.Color = Colors.LightSkyBlue;
                auditNoValue.Format.SpaceAfter = 5;
            }

            Row quotDateRow = newTabQuot.AddRow();
            var quotDateLabel = quotDateRow.Cells[0].AddParagraph("Quotation Date :");
            quotDateLabel.Format.Font.Bold = true;
            var quotDateValue = quotDateRow.Cells[1].AddParagraph($"{model.CreatedDate}");
            quotDateValue.Format.SpaceAfter = 5;

            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
            {
                Row quotInspDateRow = newTabQuot.AddRow();
                var quotInspDateLabel = quotInspDateRow.Cells[0].AddParagraph("Insp Request Date:");
                quotInspDateLabel.Format.Font.Bold = true;
                var quotInspDateValue = quotInspDateRow.Cells[1].AddParagraph($"{model.InspecCreatedDate}");
                quotInspDateValue.Format.SpaceAfter = 5;
            }

            var contacts = model.InternalContactList?.Take(3).Where(x => x.Quotation);


            Row quotContactRow = newTabQuot.AddRow();
            var quotContactLabel = quotContactRow.Cells[0].AddParagraph(entity + " Contact :");
            quotContactLabel.Format.Font.Bold = true;
            var quotContactValue = quotContactRow.Cells[1].AddParagraph($"{string.Join(", ", contacts?.Select(x => x.ContactName))}");
            quotContactValue.Format.SpaceAfter = 5;

            Row quotEmailRow = newTabQuot.AddRow();
            var quotEmailLabel = quotEmailRow.Cells[0].AddParagraph("Email :");
            quotEmailLabel.Format.Font.Bold = true;
            var quotEmailValue = quotEmailRow.Cells[1].AddParagraph($"{string.Join(", ", contacts?.Select(x => x.ContactEmail))}");
            quotEmailValue.Format.SpaceAfter = 5;

            Row quotPhoneRow = newTabQuot.AddRow();
            var quotPhoneLabel = quotPhoneRow.Cells[0].AddParagraph("Phone :");
            quotPhoneLabel.Format.Font.Bold = true;
            var quotPhoneValue = quotPhoneRow.Cells[1].AddParagraph($"{string.Join(", ", contacts?.Select(x => x.ContactPhone).FirstOrDefault())}");
            quotPhoneValue.Format.SpaceAfter = 5;

            var paraSpace = this.AddressFrame.AddParagraph();
            paraSpace.Format.LineSpacingRule = LineSpacingRule.Exactly;
            paraSpace.Format.LineSpacing = Unit.FromMillimeter(0.0);
            paraSpace.Format.SpaceBefore = 10;

            // Create the item table
            this.Table = this.AddressFrame.AddTable();
            this.Table.Style = "Table";
            this.Table.Borders.Color = TableBorder;
            this.Table.Borders.Width = 0.25;
            this.Table.Borders.Left.Width = 0.5;
            this.Table.Borders.Right.Width = 0.5;
            this.Table.Rows.LeftIndent = 0;
            //this.Table.Format.SpaceBefore = 20;

            int i = 0;
            int pageNumber = 1;
            int posplit = 35;
            int pagesize = 12;
            int averagerowcount = 0;
            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
            {
                AddRowServiceType(model);
                AddTableInspection(model, out i, out pageNumber, out pagesize,out averagerowcount);
            }
            //    else
            //     AddTableAudit(model);

            if (pageNumber == 1 && ((averagerowcount > 20 && averagerowcount <= 25) || (i > 9 && i <= 13)))
            {
                isFooter2NewPage = true;
            }
            else if (pageNumber == 1 && ((averagerowcount > 25) || (i > 13)))
            {
                isFooter1NewPage = true;
            }

            if (pageNumber >=2 && ((averagerowcount > 35 && averagerowcount <= 45) || (i > 15 && i <= 19)))
            {
                isFooter2NewPage = true;
            }
            else if (pageNumber >= 2 && ((averagerowcount > 45) || (i > 19)))
            {


                isFooter1NewPage = true;
            }



            //if (i <= 5 || (i < 8 && pageNumber > 1 && pagesize>=10))
            if (!isFooter1NewPage)
            {
                paraSpace = this.AddressFrame.AddParagraph();
                paraSpace.Format.LineSpacingRule = LineSpacingRule.Exactly;
                paraSpace.Format.LineSpacing = Unit.FromMillimeter(0.0);
                paraSpace.Format.SpaceBefore = 10;
            }
            else
            {
                AddNewPageHeader();
            }

            var finalTab = this.AddressFrame.AddTable();
            finalTab.Format.Font.Size = 9;

            finalTab.AddColumn("10cm");
            finalTab.Format.Alignment = ParagraphAlignment.Left;
            finalTab.AddColumn("10cm");
            finalTab.Format.Alignment = ParagraphAlignment.Left;

            Row finalTabRow = finalTab.AddRow();
            finalTabRow.Format.Font.Size = 9;
            var textFrame1 = finalTabRow.Cells[0].AddTextFrame();
            var textFrame2 = finalTabRow.Cells[1].AddTextFrame();
            textFrame1.Height = 50;
            textFrame2.Height = 50;

            var tab1 = textFrame1.AddTable();
            tab1.AddColumn("4.5cm");
            tab1.Format.Alignment = ParagraphAlignment.Left;
            tab1.AddColumn("5.5cm");
            tab1.Format.Alignment = ParagraphAlignment.Left;
            tab1.Format.Font.Size = 9;
            tab1.Style = "Table";

            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
            {
                finalTabRow = tab1.AddRow();
                var totalProducts = finalTabRow.Cells[0].AddParagraph("Total Products ");
                totalProducts.Format.Font.Bold = true;
                totalProducts = finalTabRow.Cells[1].AddParagraph($": {model.OrderList?.Sum(x => x.ProductList.Select(y=>y.Id).Distinct().Count())}");
                totalProducts.Format.SpaceAfter = 5;
            }
            //add total containers
            finalTabRow = tab1.AddRow();
            var totalContainers = finalTabRow.Cells[0].AddParagraph("Total Containers ");
            totalContainers.Format.Font.Bold = true;
            totalContainers = finalTabRow.Cells[1].AddParagraph($": {model.TotalContainers}");
            totalContainers.Format.SpaceAfter = 5;

            //add total picking 
            finalTabRow = tab1.AddRow();
            var totalPicking = finalTabRow.Cells[0].AddParagraph("Total Picking ");
            totalPicking.Format.Font.Bold = true;
            totalPicking = finalTabRow.Cells[1].AddParagraph($": {model.OrderList?.Sum(x => x.ProductList?.Where(z => z.PickingQty.HasValue).Select(y => y.PickingQty)?.Sum())}");
            // totalPicking.Format.SpaceAfter = 5;


            if (isFooter2NewPage)
            {
                AddNewPageHeader();
            }
            
            var finalTab1 = this.AddressFrame.AddTable();
            finalTab1.Format.Font.Size = 9;

            finalTab1.AddColumn("10cm");
            finalTab1.Format.Alignment = ParagraphAlignment.Left;
            finalTab1.AddColumn("40cm");
            finalTab1.Format.Alignment = ParagraphAlignment.Left;

            Row finalTabRow1 = finalTab1.AddRow();
            finalTabRow1.Format.Font.Size = 9;
            var textFrame3 = finalTabRow1.Cells[0].AddTextFrame();
           // var textFrame4 = finalTabRow1.Cells[1].AddTextFrame();

            var tab2 = textFrame3.AddTable();
            tab2.AddColumn("4.5cm");
            tab2.Format.Alignment = ParagraphAlignment.Left;
            tab2.AddColumn("12.5cm");
            tab2.Format.Alignment = ParagraphAlignment.Left;
            tab2.Format.Font.Size = 9;
            tab2.Style = "Table";

            finalTabRow1 = tab2.AddRow();
            var servPLabel = finalTabRow1.Cells[0].AddParagraph("Service Types ");
            servPLabel.Format.Font.Bold = true;

            var serviceTypes = string.Join(", ", model.OrderList?.SelectMany(x => x.ServiceTypeList).Select(x => x.Name));

            var servPValue = finalTabRow1.Cells[1].AddParagraph($": {serviceTypes}");
            servPValue.Format.SpaceAfter = 5;

            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
            {
                finalTabRow1 = tab2.AddRow();
                servPLabel = finalTabRow1.Cells[0].AddParagraph("Product category ");
                servPLabel.Format.Font.Bold = true;
                var productCategoryList = string.Join(",", model.OrderList.Select(y => y.ProductCategory).Distinct().ToArray());
                List<string> uniqueProductCategory = productCategoryList.Split(',').Distinct().ToList();
                string productCategory = string.Join(",", uniqueProductCategory);
                servPValue = finalTabRow1.Cells[1].AddParagraph($": {productCategory}");
                servPValue.Format.SpaceAfter = 5;
            }

            if (model.Service.Id == (int)Entities.Enums.Service.AuditId)
            {
                finalTabRow1 = tab2.AddRow();
                servPLabel = finalTabRow1.Cells[0].AddParagraph("Service date ");
                servPLabel.Format.Font.Bold = true;
                servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.InspecCreatedDate}");
                servPValue.Format.SpaceAfter = 5;
            }

            finalTabRow1 = tab2.AddRow();
            servPLabel = finalTabRow1.Cells[0].AddParagraph("Estimated Mandays ");
            servPLabel.Format.Font.Bold = true;
            servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.EstimatedManday}");
            servPValue.Format.SpaceAfter = 7;

            finalTabRow1 = tab2.AddRow();
            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
                servPLabel = finalTabRow1.Cells[0].AddParagraph("Inspection Fee ");
            else
                servPLabel = finalTabRow1.Cells[0].AddParagraph("Audit Fees ");

            servPLabel.Format.Font.Bold = true;
            servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.InspectionFees}");
            servPValue.Format.SpaceAfter = 5;

            finalTabRow1 = tab2.AddRow();
            servPLabel = finalTabRow1.Cells[0].AddParagraph("Travelling Cost ");
            servPLabel.Format.Font.Bold = true;
            servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.TravelCostsAir + model.TravelCostsHotel + model.TravelCostsLand}");
            servPValue.Format.SpaceAfter = 5;

            finalTabRow1 = tab2.AddRow();
            servPLabel = finalTabRow1.Cells[0].AddParagraph("Other Costs ");
            servPLabel.Format.Font.Bold = true;
            servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.OtherCosts}");
            servPValue.Format.SpaceAfter = 5;
            if (model.Discount.HasValue && model.Discount.Value != 0)
            {
                finalTabRow1 = tab2.AddRow();
                servPLabel = finalTabRow1.Cells[0].AddParagraph("Discount ");
                servPLabel.Format.Font.Bold = true;
                servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.Discount}");
                servPValue.Format.SpaceAfter = 5;
            }
            finalTabRow1 = tab2.AddRow();
            servPLabel = finalTabRow1.Cells[0].AddParagraph("Total Cost ");
            servPLabel.Format.Font.Bold = true;
            servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.TotalCost} {model.Currency.CurrencyCode}");
            servPValue.Format.SpaceAfter = 5;

            finalTabRow1 = tab2.AddRow();
            servPLabel = finalTabRow1.Cells[0].AddParagraph("Fee Paid by ");
            servPLabel.Format.Font.Bold = true;
            servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.BillingPaidBy.Name}");
            servPValue.Format.SpaceAfter = 5;

            finalTabRow1 = tab2.AddRow();
            servPLabel = finalTabRow1.Cells[0].AddParagraph("Remarks ");
            servPLabel.Format.Font.Bold = true;
            servPValue = finalTabRow1.Cells[1].AddParagraph($": {model.ApiRemark}");
            servPValue.Format.SpaceAfter = 5;

            if (model.Service.Id == (int)Entities.Enums.Service.InspectionId)
            {
                tab1 = textFrame2.AddTable();
                tab1.AddColumn("3cm");
                tab1.Format.Alignment = ParagraphAlignment.Left;
                tab1.AddColumn("7cm");
                tab1.Format.Alignment = ParagraphAlignment.Left;
                tab1.Format.Font.Size = 9;
                tab1.Style = "Table";

                var pojoined = string.Join(",", model.OrderList.SelectMany(x => x.ProductList).Select(x => x.PoNo));
                finalTabRow = tab1.AddRow();
                servPLabel = finalTabRow.Cells[0].AddParagraph("Po NO ");
                servPLabel.Format.Font.Bold = true;
                servPValue = finalTabRow.Cells[1].AddParagraph($": { SplitStringWithSpace(string.Join(", ", pojoined.Split(',').Select(x => x.Trim()).ToList().Distinct()), posplit)}");
                servPValue.Format.SpaceAfter = 5;

                finalTabRow = tab1.AddRow();
                servPLabel = finalTabRow.Cells[0].AddParagraph("Destination ");
                servPLabel.Format.Font.Bold = true;
                servPValue = finalTabRow.Cells[1].AddParagraph($": {string.Join(", ", model.OrderList.SelectMany(x => x.ProductList).SelectMany(x => x.Destination.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).Select(x => x.Trim()).Distinct())}");
                servPValue.Format.SpaceAfter = 5;

                finalTabRow = tab1.AddRow();
                servPLabel = finalTabRow.Cells[0].AddParagraph("ETD ");
                servPLabel.Format.Font.Bold = true;
                servPValue = finalTabRow.Cells[1].AddParagraph($": {model.ETD}");
               // servPValue.Format.SpaceAfter = 5;
            }
        }
        private void AddNewPageHeader()
        {
            this.Section.AddPageBreak();
            this.AddressFrame = Section.AddTextFrame();
            this.AddressFrame.Height = "3.0cm";
            this.AddressFrame.Width = "7.0cm";
            this.AddressFrame.Left = ShapePosition.Left;
            this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.AddressFrame.Top = "4.0cm";
            this.AddressFrame.RelativeVertical = RelativeVertical.Page;
        }
        private void AddRowServiceType(QuotationDetails model)
        {
            // get level picks
            if (model.ServiceTypeList != null)
            {

                try
                {
                    Column column = this.Table.AddColumn("3cm");

                    column = this.Table.AddColumn("3cm");
                    column.Format.Alignment = ParagraphAlignment.Right;

                    column = this.Table.AddColumn("3cm");
                    column.Format.Alignment = ParagraphAlignment.Right;
                    var row = Table.AddRow();
                    Table.Borders.Width = 0;
                    Table.Borders.Left.Width = 0;
                    Table.Borders.Right.Width = 0;
                    row.Format.SpaceAfter = 10;
                    row.Format.Alignment = ParagraphAlignment.Left;

                    if (model.ServiceTypeList != null)
                    {
                        var critical = model.ServiceTypeList.CriticalPick == null ? "" : model.ServiceTypeList.CriticalPick?.ToString();
                        var major = model.ServiceTypeList.MajorTolerancePick == null ? "" : model.ServiceTypeList.MajorTolerancePick?.ToString();
                        var minor = model.ServiceTypeList.MinorTolerancePick == null ? "" : model.ServiceTypeList.MinorTolerancePick?.ToString();

                        row.Cells[0].AddParagraph("Critical : " + critical);
                        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                        row.Cells[1].AddParagraph("Major : " + major);
                        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                        row.Cells[2].AddParagraph("Minor : " + minor);
                        row.Cells[2].VerticalAlignment = VerticalAlignment.Center;


                    }
                    // }
                }
                catch (Exception ex)

                {

                    throw ex;
                }


            }
        }

        private void AddHeaderTableInspection()
        {
            this.Table = this.AddressFrame.AddTable();
            this.Table.Style = "Table";
            this.Table.Borders.Color = TableBorder;
            this.Table.Borders.Width = 0.25;
            this.Table.Borders.Left.Width = 0.5;
            this.Table.Borders.Right.Width = 0.5;
            this.Table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = this.Table.AddColumn("3cm");

            column = this.Table.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = this.Table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.Table.AddColumn("2cm");
            //column.Format.Alignment = ParagraphAlignment.Right;

            column = this.Table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.Table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.Table.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            //column = this.Table.AddColumn("2cm");
            //column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = Table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            var pbno = row.Cells[0].AddParagraph("Product ID");
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            //row.Cells[1].AddParagraph("Fty Ref");
            //row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[1].AddParagraph("Description");
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[2].AddParagraph("Picking Qty");
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[3].AddParagraph("Order Qty");
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[4].AddParagraph("Sample Level");
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[5].AddParagraph("Sample Size");
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[6].AddParagraph("Reports");
            row.Cells[6].VerticalAlignment = VerticalAlignment.Center;
        }

        private void AddTableInspection(QuotationDetails model, out int i, out int pageNumber,out int secondpagesize,out int averagerowcount)
        {
            int ProductIdCharLimit = 14;
           // int FactoryReferenceCharLimit = 14;
            AddHeaderTableInspection();
            i = 0;
            pageNumber = 1;
            int totalSampleQty = 0;
            int total = 0;
            int totalReport = 0;
            string strTotal = "Total";
            //int totalProduct = 0;
            int reportCountEachProduct = 1;
            var isfollowingmergerequired = false;
            var mergecount = 0;
            var totalmergerow = 0;
            int firstpagesize = 18;
             secondpagesize = 27;
            int secondPageSkip = 0;
            int rH = 22;
            averagerowcount = 0;

            List<string> productdDesList = new List<string>();
            foreach (var item in model.OrderList)
            {
                productdDesList.AddRange(item.ProductList.Select(x => x.ProductDescription).ToList());
            }
                foreach (var order in model.OrderList)
            {
                foreach (var product in order.ProductList)
                {
                    i++;
                    if ((pageNumber == 1 && i > firstpagesize) || (pageNumber > 1 && i > secondpagesize))
                    {
                        this.Section.AddPageBreak();

                        this.AddressFrame = Section.AddTextFrame();
                        this.AddressFrame.Height = "3.0cm";
                        this.AddressFrame.Width = "7.0cm";
                        this.AddressFrame.Left = ShapePosition.Left;
                        this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                        this.AddressFrame.Top = "4.0cm";
                        this.AddressFrame.RelativeVertical = RelativeVertical.Page;

                        AddHeaderTableInspection();
                        i = 1;
                        pageNumber++;
                    }

                    //set the page size based on product desc length
                    if(i==1)
                    {
                        int average = 0;
                        int noofrows = 0;
                        int charlength = 34;
                        var totaltextlength = 0;
                        if (pageNumber==1)
                        {
                            noofrows = 35;
                            firstpagesize = productdDesList.Count()> 18 ? 18: productdDesList.Count();
                             
                            //var productlength = productdDesList.Take(firstpagesize).Count();
                          
                            // firstpagesize =length>2000? firstpagesize-12: length > 1500 ? firstpagesize - 8 : length > 1400 ? firstpagesize - 6 : length > 1200 ? firstpagesize - 4  : firstpagesize;
                            do
                            {
                                totaltextlength = string.Join("", productdDesList.Take(firstpagesize).ToList()).Length;
                                average = totaltextlength / charlength;
                                averagerowcount = average;
                                if (average > noofrows)
                                    firstpagesize--;
                            } while (average > noofrows);
                        }
                        else
                        {
                            noofrows = 54;
                            secondPageSkip =  pageNumber==2? firstpagesize : secondPageSkip + secondpagesize;
                            secondpagesize = productdDesList.Skip(secondPageSkip).Take(secondpagesize).Count()> 27 ? 27: productdDesList.Skip(secondPageSkip).Take(secondpagesize).Count();
                            // secondpagesize = length > 2100 ? secondpagesize-10 : length > 1900?secondpagesize - 8 : length > 1700 ? secondpagesize - 6:length > 1600 ? secondpagesize - 4  : length > 1500 ? secondpagesize - 2  : secondpagesize;
                            do
                            {
                                totaltextlength = string.Join("", productdDesList.Skip(secondPageSkip).Take(secondpagesize).ToList()).Length;
                                average = totaltextlength / charlength;
                                averagerowcount = average;
                                if (average > noofrows)
                                    secondpagesize--;
                            } while (average > noofrows);
                        }
                    }

                    //set the merge for combine and if merge follow to new page
                    if ((product.IsParentProduct && product.CombineProductCount > 0) || (isfollowingmergerequired && i == 1))
                    {
                        //check is the merge follow to new page
                        if (isfollowingmergerequired)
                        {
                            mergecount = (product.CombineProductCount - totalmergerow);
                        }
                        //first page
                        if (pageNumber == 1)
                        {
                            //check product combine count + i greater than page size
                            //(i-1) for getting actual product count in page. i increment top of the page
                            if (((i-1) + product.CombineProductCount) > firstpagesize)
                            {

                                mergecount = firstpagesize - (i-1);
                                totalmergerow = mergecount;
                                isfollowingmergerequired = true;
                            }
                            else
                            {
                                mergecount = product.CombineProductCount;
                                isfollowingmergerequired = false;
                                totalmergerow = 0;
                            }
                        }
                        else
                        {
                            //check product combine count + i greater than page size
                            if (((i - 1) + (product.CombineProductCount - totalmergerow)) > secondpagesize)
                            {
                                mergecount = secondpagesize - (i-1);
                                isfollowingmergerequired = true;
                                totalmergerow = totalmergerow + mergecount;
                            }
                            else
                            {
                                mergecount = isfollowingmergerequired ? mergecount : product.CombineProductCount;
                                isfollowingmergerequired = false;
                                totalmergerow = 0;
                            }
                        }


                    }
                    else
                    {
                        mergecount = 0;
                    }

                   
                    var row = Table.AddRow();
                    row.Height = rH;
                    if (product.ProductId != null)
                    {
                        if (product.ProductId.Length > ProductIdCharLimit)
                        {
                            row.Cells[0].AddParagraph($"{SplitStringWithSpace(product.ProductId, ProductIdCharLimit)}");
                        }
                        else
                        {
                            row.Cells[0].AddParagraph($"{product.ProductId}");
                        }
                    }
                    row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                    //if (product.FactoryReference != null)
                    //{
                    //    if (product.FactoryReference.Length > FactoryReferenceCharLimit)
                    //    {
                    //        row.Cells[1].AddParagraph($"{SplitStringWithSpace(product.FactoryReference, FactoryReferenceCharLimit)}");
                    //    }
                    //    else
                    //    {
                    //        row.Cells[1].AddParagraph($"{product.FactoryReference}");
                    //    }
                    //}

                    //row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                    //row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                    TextInfo myTI = new CultureInfo(EnglishUS, false)?.TextInfo;
                    var prodesc = myTI?.ToTitleCase(myTI?.ToLower(product?.ProductDescription?.Replace("\n", "").Replace("\r", "")??"")??"");
                    row.Cells[1].AddParagraph($"{prodesc}");
                    row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

                    row.Cells[2].AddParagraph($"{product?.PickingQty}");
                    row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                    row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

                    row.Cells[3].AddParagraph($"{product?.BookingQty}");
                    row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
                    row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                    var aqlLevel = row.Cells[4];
                    string aqlLevelText = product?.AqlLevel;

                    if (!string.IsNullOrWhiteSpace(product?.AqlLevelDescription))
                        aqlLevelText += $"({product?.AqlLevelDescription})";

                    if (product.IsParentProduct || mergecount > 0)
                    {

                        aqlLevel.AddParagraph($"{(product.IsParentProduct ? aqlLevelText : "")}");
                        aqlLevel.VerticalAlignment = VerticalAlignment.Center;
                        aqlLevel.Format.Alignment = ParagraphAlignment.Center;

                        var sampleQty = row.Cells[5];
                        sampleQty.AddParagraph($"{(product.IsParentProduct ? product?.SampleQty.ToString() : "")}");
                        sampleQty.VerticalAlignment = VerticalAlignment.Center;
                        sampleQty.Format.Alignment = ParagraphAlignment.Center;

                        var reports = row.Cells[6];
                        reports.AddParagraph(Convert.ToString((product.IsParentProduct ? reportCountEachProduct.ToString() : "")));
                        reports.VerticalAlignment = VerticalAlignment.Center;
                        reports.Format.Alignment = ParagraphAlignment.Center;

                        if (mergecount > 0)
                        {
                            // start the index at 0
                            aqlLevel.MergeDown = mergecount-1 ;
                            sampleQty.MergeDown = mergecount-1;
                            reports.MergeDown = mergecount-1;
                        }

                        if (product.IsParentProduct)
                            totalReport = reportCountEachProduct + totalReport;
                    }
                    totalSampleQty = totalSampleQty + product?.SampleQty??0;
                    total = total + product?.BookingQty??0;
                }

            }

            // Table footer

            var footrow = Table.AddRow();
            footrow.Format.Alignment = ParagraphAlignment.Center;

            footrow.Cells[0].MergeRight = 1;
            footrow.Cells[0].AddParagraph($"{strTotal}");
            //footrow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            footrow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            footrow.Cells[0].Format.Font.Bold = true;

            footrow.Cells[2].AddParagraph(Convert.ToString(model.OrderList?.Sum(x => x.ProductList.Select(y => y.PickingQty).Sum())));
            footrow.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            footrow.Cells[2].Format.Font.Bold = true;

            footrow.Cells[3].AddParagraph(Convert.ToString(total));
            footrow.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            footrow.Cells[3].Format.Font.Bold = true;

            footrow.Cells[5].AddParagraph($"{totalSampleQty}");
            footrow.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            footrow.Cells[5].Format.Font.Bold = true;
            
            footrow.Cells[6].AddParagraph(Convert.ToString(totalReport));
            footrow.Cells[6].VerticalAlignment = VerticalAlignment.Center;
            footrow.Cells[6].Format.Font.Bold = true;
        }

        private void AddTableAudit(QuotationDetails model)
        {

            // Before you can add a row, you must define the columns
            Column column = this.Table.AddColumn("3.5cm");

            column = this.Table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.Table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.Table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = this.Table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;


            // Create the header of the table
            Row row = Table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            row.Cells[1].AddParagraph("Fty Ref");
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[2].AddParagraph("Description");
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[3].AddParagraph("AQL");
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[4].AddParagraph("Order Qty");
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[5].AddParagraph("Sample Size");
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

            foreach (var order in model.OrderList)
            {
                row = Table.AddRow();
                row.Format.Alignment = ParagraphAlignment.Left;
                row.HeightRule = RowHeightRule.Exactly;
                row.Height = 20;

                row.Cells[1].AddParagraph($"{order.Id}");
                row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[2].AddParagraph($"{order.ServiceTypeStr}");
                row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[3].AddParagraph($"aql");
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[4].AddParagraph("Order Qty");
                row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[5].AddParagraph("Sample Size");
                row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            }

        }

        //private void FillContent()
        //{
        //    // Fill address in address text frame

        //    // Iterate the invoice items
        //    double totalExtendedPrice = 0;

        //    //while (iter.MoveNext())
        //   // {


        //        // Each item fills two rows
        //        Row row1 = this.Table.AddRow();
        //        Row row2 = this.Table.AddRow();
        //        row1.TopPadding = 1.5;
        //        row1.Cells[0].Shading.Color = TableGray;
        //        row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        //        row1.Cells[0].MergeDown = 1;
        //        row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
        //        row1.Cells[1].MergeRight = 3;
        //        row1.Cells[5].Shading.Color = TableGray;
        //        row1.Cells[5].MergeDown = 1;

        //        row1.Cells[0].AddParagraph("1");
        //        var paragraph = row1.Cells[1].AddParagraph();
        //        paragraph.AddFormattedText("PDF Reference Version 1.6 (5th Edition)", TextFormat.Bold);
        //        paragraph.AddFormattedText(" by ", TextFormat.Italic);
        //        paragraph.AddText("Adobe Systems Inc");
        //        row2.Cells[1].AddParagraph("10");
        //        row2.Cells[2].AddParagraph(" 1500 €");
        //        row2.Cells[3].AddParagraph("0.0");
        //        row2.Cells[4].AddParagraph();
        //        row2.Cells[5].AddParagraph("5000 €");
        //        row1.Cells[5].AddParagraph("25 €");
        //        row1.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;

        //        this.Table.SetEdge(0, this.Table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
        //   // }

        //    // Add an invisible row as a space line to the table
        //    Row row = this.Table.AddRow();
        //    row.Borders.Visible = false;

        //    // Add the total price row
        //    row = this.Table.AddRow();
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].AddParagraph("Total Price");
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;
        //    row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");

        //    // Add the VAT row
        //    row = this.Table.AddRow();
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].AddParagraph("VAT (19%)");
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;
        //    row.Cells[5].AddParagraph((0.19 * totalExtendedPrice).ToString("0.00") + " €");

        //    // Add the additional fee row
        //    row = this.Table.AddRow();
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].AddParagraph("Shipping and Handling");
        //    row.Cells[5].AddParagraph(0.ToString("0.00") + " €");
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;

        //    // Add the total due row
        //    row = this.Table.AddRow();
        //    row.Cells[0].AddParagraph("Total Due");
        //    row.Cells[0].Borders.Visible = false;
        //    row.Cells[0].Format.Font.Bold = true;
        //    row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //    row.Cells[0].MergeRight = 4;
        //    totalExtendedPrice += 0.19 * totalExtendedPrice;
        //    row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");

        //    // Set the borders of the specified cell range
        //    // this.Table.SetEdge(5, this.Table.Rows.Count - 4, 1, 4, Edge.Box, BorderStyle.Single, 0.75);

        //    // Add the notes paragraph
        //    paragraph = this.Document.LastSection.AddParagraph();
        //    paragraph.Format.SpaceBefore = "1cm";
        //    paragraph.Format.Borders.Width = 0.75;
        //    paragraph.Format.Borders.Distance = 3;
        //    paragraph.Format.Borders.Color = TableBorder;
        //    paragraph.Format.Shading.Color = TableGray;
        //    paragraph.AddText("This is a sample invoice created with MigraDoc. The PDF document is rendered with PDFsharp.");
        //}

    }

    public class PdFile
    {
        public string Name { get; set; }

        public string FileModelName { get; set; }

        public byte[] Content { get; set; }

        public string MimeType { get; set; }

    }
}
