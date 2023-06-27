using DTO.File;
using DTO.Inspection;
using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.IO;
using Contracts.Managers;
using Microsoft.AspNetCore.Hosting;
using DTO.Common;
using DTO.FBInternalReport;
using Microsoft.Extensions.Options;
using Entities;

namespace LINK_UI.FileModels
{
    public class InspectionPickingPDF:ApiCommonData, IPickingPDF
    {
        private readonly IHostingEnvironment _environnement = null;
        private Document Document = null;
        private TextFrame AddressFrame = null;
        private Table Table = null;
        private Table productTable = null;
        private Table bookingTable = null;
        private Table deliveryAddressTable = null;
        private Table commentTable = null;
        private Color TableBorder = Colors.Black;
        private Color TableBlue = Colors.DarkBlue;
        private Color TableGray = Colors.Gray;
        private Section Section = null;

        public InspectionPickingPDF(IHostingEnvironment environnement)
        {
            _environnement = environnement;
        }

        public FileResponse CreatePickingDocument(IEnumerable<QcPickingData> model, EntMasterConfigItem entMasterConfig)
        {
            // Create a new MigraDoc document
            this.Document = new Document();
            this.Document.Info.Title = QCInspectionPickingPDF.GetValueOrDefault("Title", "");
            this.Document.Info.Subject = QCInspectionPickingPDF.GetValueOrDefault("Title", "");
            this.Document.Info.Author = entMasterConfig.Entity;

            DefineStyles();
            if (model != null)
            {
                CreatePage(model, entMasterConfig);
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
                    Name = $"Picking Form_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
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
            style.ParagraphFormat.AddTabStop("9cm", TabAlignment.Center);

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

        private void CreatePage(IEnumerable<QcPickingData> items, EntMasterConfigItem entMasterConfig)
        {
            foreach (var model in items)
            {
                if (model.BookingId > 0)
                {
                    // Each MigraDoc document needs at least one section.
                    Section = this.Document.AddSection();
                    Section.PageSetup = new PageSetup() { RightMargin = "1cm", LeftMargin = "1cm" };

                    AddHeader(entMasterConfig.ImageLogo);

                    //Row row = new Row();
                    //row.Cells[0].AddParagraph("Inspection Id: " + model.BookingId);

                    this.AddressFrame = Section.AddTextFrame();
                    this.AddressFrame.Height = "5.0cm";
                    this.AddressFrame.Width = "10.0cm";
                    this.AddressFrame.Left = ShapePosition.Left;
                    this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                    this.AddressFrame.Top = "1.0cm";
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
                    tab1.AddColumn("4cm");
                    tab1.Format.Alignment = ParagraphAlignment.Left;
                    tab1.AddColumn("9cm");
                    tab1.Format.Alignment = ParagraphAlignment.Left;
                    tab1.Format.Font.Size = 9;
                    tab1.Style = "Table";

                    finalTabRow = tab1.AddRow();
                    BookingTableData(model);

                    Paragraph paragraphTop = this.AddressFrame.AddParagraph();
                    paragraphTop.AddLineBreak();
                  
                    paragraphTop.Format.SpaceBefore =120;
                    paragraphTop.Format._spaceAfter = 5;
                    DeliveryAddressTableData(model);
                    

                    Paragraph paragraphMiddle = this.AddressFrame.AddParagraph();
                    paragraphMiddle.AddLineBreak();

                    paragraphMiddle.Format.SpaceBefore = 130;
                    paragraphMiddle.Format._spaceAfter = 20;
                    if (model.Products != null)
                        ProductTableData(model, entMasterConfig.Entity);

                    CommentTable();

                    FinalAddressTable(model, entMasterConfig.Entity, entMasterConfig);

                    AddPageFoooter(Section);
                }
            }
        }

        private void AddHeader(string imageLogo)
        {
            Table tableInfo = Section.Headers.Primary.AddTable();

            tableInfo.Style = "Table";
            tableInfo.Borders.Color = TableBorder;
            tableInfo.Rows.LeftIndent = 0;
            tableInfo.Format.SpaceBefore = 10;
            tableInfo.Format.SpaceAfter = 10;
            Column columnInfoLabel = tableInfo.AddColumn("3.5cm");
            columnInfoLabel.Format.Alignment = ParagraphAlignment.Left;
            Column columnInfoValue = tableInfo.AddColumn("15.5cm");
            columnInfoValue.Format.Alignment = ParagraphAlignment.Left;

            Row rowInfoCustomer = tableInfo.AddRow();

            Image image = rowInfoCustomer.Cells[0].AddImage(Path.Combine(_environnement.ContentRootPath, imageLogo));
            image.Height = "1.5cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;

            image.Top = ShapePosition.Center;
            //image.Left = ShapePosition.Left;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.WrapFormat.Style = WrapStyle.Through;

            rowInfoCustomer.Cells[0].Column.Width = 100;
            rowInfoCustomer.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Title", ""));
            rowInfoCustomer.Cells[1].Format.Font.Bold = true;
            rowInfoCustomer.Cells[1].Format.Font.Size = 18;
            rowInfoCustomer.Cells[1].Format.Alignment = ParagraphAlignment.Center;

        }

        private void ProductTableData(QcPickingData model, string entity)
        {
            ProductHeaderTable();
            int i = 0;
            int pageNumber = 1;

            foreach (var product in model.Products)
            {
                i++;
                if ( i > 6)
                {
                    Paragraph commentParagragh = this.AddressFrame.AddParagraph();
                    commentParagragh.Format.SpaceBefore = 40;
                    commentParagragh.Format.SpaceAfter = 10;

                    CommentTable();
                    FinalAddressTable(model, entity,null);

                    this.Section.AddPageBreak();

                    this.AddressFrame = Section.AddTextFrame();
                    this.AddressFrame.Height = "5.0cm";
                    this.AddressFrame.Width = "10.0cm";
                    this.AddressFrame.Left = ShapePosition.Left;
                    this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                    this.AddressFrame.Top = "4.0cm";
                    //this.AddressFrame. = "4.0cm";
                    this.AddressFrame.RelativeVertical = RelativeVertical.Page;

                    //Paragraph bookingParagragh = this.AddressFrame.AddParagraph();
                    //bookingParagragh.Format.SpaceBefore = 60;
                    //bookingParagragh.Format.SpaceAfter = 5;

                    BookingTableData(model);

                    Paragraph addressParagragh = this.AddressFrame.AddParagraph();
                    addressParagragh.Format.SpaceBefore = 120;
                    addressParagragh.Format.SpaceAfter = 15;
                   
                    DeliveryAddressTableData(model);

                    Paragraph paragraphMiddle = this.AddressFrame.AddParagraph();
                    paragraphMiddle.AddLineBreak();
                    paragraphMiddle.Format.SpaceAfter = 15;
                    paragraphMiddle.Format.SpaceBefore = 130;
                    ProductHeaderTable();
                    i = 1;
                    pageNumber++;
                }

                var row = productTable.AddRow();
                row.Height = 20;
                if (product.PONumber?.Length > 14)
                {
                    row.Cells[0].AddParagraph($"{SplitStringWithSpace(product.PONumber, 14)}");
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                }
                else
                {
                    row.Cells[0].AddParagraph($"{product.PONumber}");
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                }

                if (product.ProductId?.Length > 16)
                {
                    row.Cells[1].AddParagraph($"{SplitStringWithSpace(product.ProductId, 16)}");
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                }
                else
                {
                    row.Cells[1].AddParagraph($"{product.ProductId}");
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                }


                if (product.FactoryReference?.Length > 16)
                {
                    row.Cells[2].AddParagraph($"{SplitStringWithSpace(product.FactoryReference, 16)}");
                    row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                }
                else
                {
                    row.Cells[2].AddParagraph($"{product.FactoryReference}");
                    row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                }

                row.Cells[3].AddParagraph($"{product.PickingQuantity}");
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                if (product.DestinationCountry != null && product.DestinationCountry?.Length > 14)
                {
                    row.Cells[4].AddParagraph($"{SplitStringWithSpace(product.DestinationCountry, 14)}");
                    row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                }
                else
                {
                    row.Cells[4].AddParagraph($"{product.DestinationCountry}");
                    row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                }

            }
        }

        private void ProductHeaderTable()
        {
            productTable = this.AddressFrame.AddTable();
            productTable.Style = "Table";
            productTable.Borders.Color = TableBorder;
            productTable.Borders.Width = 0.25;
            productTable.Borders.Left.Width = 0.5;
            productTable.Borders.Right.Width = 0.5;
            productTable.Rows.LeftIndent = 0;

            

            // Before you can add a row, you must define the columns
            Column column = productTable.AddColumn("3.25cm");

            column = productTable.AddColumn("3.25cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("3.25cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("1.25cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = productTable.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Right;


            //Column header = productTable.AddColumn("18cm");
            Row rowHeader = productTable.AddRow();
            rowHeader.HeadingFormat = true;
            rowHeader.Format.Alignment = ParagraphAlignment.Left;
            rowHeader.Format.Font.Bold = true;
            rowHeader.Shading.Color = Colors.LightGray;
            rowHeader.HeightRule = RowHeightRule.Exactly;
            rowHeader.Height = 30;

            var productHeader = rowHeader.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Samplecollection", ""));
            

            var text = rowHeader.Cells[0].AddParagraph("samples forwarded by the factory to the lab/ customer");
            text.Format.Alignment = ParagraphAlignment.Right;
            text.Format.Font.Bold = false;
            text.Format.Font.Italic = true;
            text.Format.Font.Size = 8;

            rowHeader.Cells[0].MergeRight = 8;
            rowHeader.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            productHeader.Format.Alignment = ParagraphAlignment.Justify;

            Row row = productTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            Row rowAdd = productTable.AddRow();
            rowAdd.HeadingFormat = true;
            rowAdd.Format.Alignment = ParagraphAlignment.Center;
            rowAdd.Format.Font.Bold = true;
            rowAdd.Shading.Color = Colors.LightGray;
            rowAdd.HeightRule = RowHeightRule.Exactly;
            rowAdd.Height = 20;

            var productId = row.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("PoNumber", ""));
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].MergeDown = 1;

            row.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("ProductName", ""));
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].MergeDown = 1;

            row.Cells[2].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("FactoryReference", ""));
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[2].MergeDown = 1;

            row.Cells[3].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("QtyReq", ""));
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[3].MergeDown = 1;

            row.Cells[4].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("DestinationCountry", ""));
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[4].MergeDown = 1;

            row.Cells[5].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("QtySealed", ""));
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[5].MergeDown = 1;

            row.Cells[6].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("CartonNumbers", ""));
            row.Cells[6].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[6].MergeRight = 2;

            rowAdd.Cells[6].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("1", ""));
            rowAdd.Cells[6].VerticalAlignment = VerticalAlignment.Center;

            rowAdd.Cells[7].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("2", ""));
            rowAdd.Cells[7].VerticalAlignment = VerticalAlignment.Center;

            rowAdd.Cells[8].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("3", ""));
            rowAdd.Cells[8].VerticalAlignment = VerticalAlignment.Center;

            
        }

        private void BookingHeaderTable()
        {
            bookingTable = this.AddressFrame.AddTable();
            bookingTable.Style = "Table";
            bookingTable.Borders.Color = TableBorder;
            bookingTable.Borders.Width = 0.25;
            bookingTable.Borders.Left.Width = 0.5;
            bookingTable.Borders.Right.Width = 0.5;
            bookingTable.Rows.LeftIndent = 0;


            // Before you can add a row, you must define the columns
            Column column = bookingTable.AddColumn("2.5cm");

            column = bookingTable.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = bookingTable.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = bookingTable.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = bookingTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = bookingTable.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            Row row = bookingTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            var productId = row.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("InspectionID", ""));
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("CustomerBookingNo", ""));
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[2].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Customer", ""));
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[3].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Supplier", ""));
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[4].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("InspectionDate", ""));
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[5].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Inspector", ""));
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
        }

        private void BookingTableData(QcPickingData model)
        {
            BookingHeaderTable();
           
            //this.Section.AddPageBreak();

            this.AddressFrame = Section.AddTextFrame();
            this.AddressFrame.Height = "5.0cm";
            this.AddressFrame.Width = "10.0cm";
            this.AddressFrame.Left = ShapePosition.Left;
            this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.AddressFrame.Top = "1.0cm";
            //this.AddressFrame. = "4.0cm";
            this.AddressFrame.RelativeVertical = RelativeVertical.Page;

            
            var row = bookingTable.AddRow();
            row.Height = 20;
                        row.Cells[0].AddParagraph($"{model.BookingId}");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                        row.Cells[1].AddParagraph($"{model.CustomerBookingNo}");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;


            if (model.CustomerName?.Length > 17)
            {
                row.Cells[2].AddParagraph($"{SplitStringWithSpace(model.CustomerName, 17)}");
                row.Cells[2].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                row.Cells[2].AddParagraph($"{model.CustomerName}");
                row.Cells[2].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            }
            if (model.SupplierName?.Length > 17)
            {
                row.Cells[3].AddParagraph($"{SplitStringWithSpace(model.SupplierName, 17)}");
                row.Cells[3].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                row.Cells[3].AddParagraph($"{model.SupplierName}");
                row.Cells[3].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            }

            row.Cells[4].AddParagraph($"{model.ServiceDate}");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[5].AddParagraph($"{model.StaffName}");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

        }

        private void DeliveryAddressTableHeader()
        {
            deliveryAddressTable = this.AddressFrame.AddTable();
            deliveryAddressTable.Style = "Table";
            deliveryAddressTable.Borders.Color = TableBorder;
            deliveryAddressTable.Borders.Width = 0.25;
            deliveryAddressTable.Borders.Left.Width = 0.5;
            deliveryAddressTable.Borders.Right.Width = 0.5;
            deliveryAddressTable.Rows.LeftIndent = 0;


            // Before you can add a row, you must define the columns
            Column column = deliveryAddressTable.AddColumn("19cm");

            Row row = deliveryAddressTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Left;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            var header = row.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("DeliveryAddress", ""));
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            
        }

        private void DeliveryAddressTableData(QcPickingData model)
        {
            DeliveryAddressTableHeader();
           
            this.AddressFrame = Section.AddTextFrame();
            this.AddressFrame.Height = "10.0cm";
            this.AddressFrame.Width = "7.0cm";
            this.AddressFrame.Left = ShapePosition.Left;
            this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.AddressFrame.Top = "5.0cm";
            //this.AddressFrame. = "4.0cm";
            this.AddressFrame.RelativeVertical = RelativeVertical.Page;


            var deliveryAdd = deliveryAddressTable.AddRow();
            var table = deliveryAdd.Cells[0].AddTextFrame();
            var newTab = table.AddTable();
            newTab.Format.Font.Size = 9;

            var cQUotLabel = newTab.AddColumn("4cm");
            newTab.Format.Alignment = ParagraphAlignment.Left;
            var cQUotValue = newTab.AddColumn("15cm");
            cQUotValue.Format.Alignment = ParagraphAlignment.Left;


            Row row = newTab.AddRow();
            var Label = row.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("To", ""));
            Label.Format.Font.Bold = true;
            Label.Format.SpaceBefore = 10;

            var Value = row.Cells[1].AddParagraph($"{model.LabName}");
            Value.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            Value.Format.SpaceAfter = 5;
            Value.Format.SpaceBefore = 10;

            Row rowContact = newTab.AddRow();
            var rowContactLabel = rowContact.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Contact", ""));
            rowContactLabel.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            rowContactLabel.Format.Font.Bold = true;
            rowContactLabel.Format.SpaceAfter = 5;

            var rowContactValue = rowContact.Cells[1].AddParagraph($" {model.ContactName} - {model.Telephone} / {model.Email}");
            //rowContactValue.Format.SpaceAfter = 5;

            Row rowAddress = newTab.AddRow();
            var rowAddressLabel = rowAddress.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Address", ""));
            rowAddressLabel.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            rowAddressLabel.Format.Font.Bold = true;
            rowAddressLabel.Format.SpaceAfter = 5;

            if (model.RegionalAddress != null)
            {
                var rowAddressValue = rowAddress.Cells[1].AddParagraph($"{ model.LabAddress}" + $" ( {model.RegionalAddress} )");
                rowAddressValue.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                rowAddressValue.Format.SpaceAfter = 5;
            }
            else
            {
                var rowAddressValue = rowAddress.Cells[1].AddParagraph($"{model.LabAddress}");
                rowAddressValue.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                rowAddressValue.Format.SpaceAfter = 5;
            }

            deliveryAdd.Height = 90;
        }

        private void CommentTable()
        {
           
            this.AddressFrame = Section.AddTextFrame();
            this.AddressFrame.Height = "15.0cm";
            this.AddressFrame.Width = "10.0cm";
            this.AddressFrame.Left = ShapePosition.Left;
            this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.AddressFrame.Top = "19.5cm";
            this.AddressFrame.RelativeVertical = RelativeVertical.Page;

            commentTable = this.AddressFrame.AddTable();
            commentTable.Style = "Table";
            commentTable.Borders.Color = TableBorder;
            commentTable.Borders.Width = 0.25;
            commentTable.Borders.Left.Width = 0.5;
            commentTable.Borders.Right.Width = 0.5;
            commentTable.Rows.LeftIndent = 0;


            // Before you can add a row, you must define the columns
            Column column = commentTable.AddColumn("19cm");

            Row row = commentTable.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Left;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            var header = row.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Comments", ""));
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            var commentBox = commentTable.AddRow();
            commentBox.Height = 40;
        }

        private void FinalAddressTable(QcPickingData model, string entity, EntMasterConfigItem entMasterConfig)
        {
            this.AddressFrame = Section.AddTextFrame();
            this.AddressFrame.Height = "15.0cm";
            this.AddressFrame.Width = "10.0cm";
            this.AddressFrame.Left = ShapePosition.Left;
            this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.AddressFrame.Top = "22.5cm";
            this.AddressFrame.RelativeVertical = RelativeVertical.Page;

            var finalAddress1 = this.AddressFrame.AddTable();
            finalAddress1.Format.Font.Size = 9;

            finalAddress1.AddColumn("11.5cm");
            finalAddress1.Format.Alignment = ParagraphAlignment.Left;
            finalAddress1.AddColumn("7.5cm");
            finalAddress1.Format.Alignment = ParagraphAlignment.Left;

            Row AddressRow = finalAddress1.AddRow();
            AddressRow.Format.Font.Size = 9;
            var textFrame4 = AddressRow.Cells[0].AddTextFrame();

            Table finalAddressTable = textFrame4.AddTable();
            finalAddressTable.Style = "Table";
            finalAddressTable.Borders.Color = TableBorder;
            finalAddressTable.Rows.LeftIndent = 0;
            Column finalCol1 = finalAddressTable.AddColumn("6.33cm");
            finalCol1.Format.Alignment = ParagraphAlignment.Left;
            Column finalCol2 = finalAddressTable.AddColumn("6.33cm");
            finalCol2.Format.Alignment = ParagraphAlignment.Left;
            Column finalCol3 = finalAddressTable.AddColumn("6.33cm");
            finalCol3.Format.Alignment = ParagraphAlignment.Left;

            Row headerRow = finalAddressTable.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Format.Font.Bold = true;
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.HeightRule = RowHeightRule.Exactly;
            headerRow.Height = 20;

            var header = headerRow.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault(entity, ""));
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            headerRow.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Factory", ""));
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Center;

            headerRow.Cells[2].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Lab", ""));
            headerRow.Cells[2].VerticalAlignment = VerticalAlignment.Center;

            Row final = finalAddressTable.AddRow();
            final.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("InspChop", ""));
            var address = final.Cells[0].AddParagraph();
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.SpaceAfter =40;
            final.Cells[0].Format.SpaceBefore = 10;

            var contact = string.Concat(entMasterConfig?.Entity??"", " ", QCInspectionPickingPDF.GetValueOrDefault("APIContact", ""));
            Row final2 = finalAddressTable.AddRow();
            final2.Format.Alignment = ParagraphAlignment.Center;
            final2.Cells[0].AddParagraph(contact);
            final2.Height = 20;
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            final2.Shading.Color = Colors.LightGray;
            address.Format.SpaceAfter = 10;
            final2.Cells[0].Format.SpaceAfter = 10;
            final2.Cells[0].Format.SpaceBefore = 10;
            final2.VerticalAlignment = VerticalAlignment.Center;


            Row final3 = finalAddressTable.AddRow();
            final3.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Name", ""));
            final3.Cells[0].Format.Font.Bold = true;
            address = final3.Cells[0].AddParagraph($"{ model.CsName}");
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.SpaceAfter = 2;
            address.Format.Font.Bold = false;
            final3.Cells[0].Format.SpaceBefore = 1;


            final3.Cells[0].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Phone", ""));
            final3.Cells[0].Format.Font.Bold = true;
            address = final3.Cells[0].AddParagraph($"{ model.CsPhone}");
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.Font.Bold = false;
            address.Format.SpaceAfter = 2;
            final3.Cells[0].Format.SpaceBefore = 1;


            //Row fact1 = finalAddressTable.AddRow();
            final.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("DeiveryDateadvisedbyFactory", ""));
            address = final.Cells[1].AddParagraph();
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            final.Cells[1].Format.SpaceAfter = 10;
            final.Cells[1].Format.SpaceBefore = 10;
            address.Format.SpaceAfter = 10;
            
            final2.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("FactoryRepresentative", ""));
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            final2.Cells[1].Format.SpaceAfter = 10;
            final2.Cells[1].Format.SpaceBefore = 10;
            address.Format.SpaceAfter = 10;
            final2.VerticalAlignment = VerticalAlignment.Center;


            //Row fact3 = finalAddressTable.AddRow();
            final3.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Name", ""));
            final3.Cells[1].AddParagraph();
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.SpaceAfter = 2;
            final3.Cells[1].Format.Font.Bold = true;
            final3.Cells[1].Format.SpaceBefore = 1;


            //Row fact4 = finalAddressTable.AddRow();
            final3.Cells[1].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Phone", ""));
            final3.Cells[1].AddParagraph();
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.SpaceAfter = 2;
            final3.Cells[1].Format.Font.Bold = true;
            final3.Cells[1].Format.SpaceBefore = 1;


            final.Cells[2].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("DateReceived", ""));
            address = final.Cells[2].AddParagraph();
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.SpaceAfter = 40;
            final.Cells[2].Format.SpaceBefore = 10;


            //Row fact2 = finalAddressTable.AddRow();
            final2.Cells[2].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("LabRepresentative", ""));
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            final2.Cells[2].Format.SpaceAfter = 10;
            final2.Cells[2].Format.SpaceBefore = 10;
            address.Format.SpaceAfter = 10;
            final2.VerticalAlignment = VerticalAlignment.Center;


            //Row fact3 = finalAddressTable.AddRow();
            final3.Cells[2].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Name", ""));
            final3.Cells[2].AddParagraph();
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.SpaceAfter = 2;
            final3.Cells[2].Format.Font.Bold = true;
            final3.Cells[2].Format.SpaceBefore = 1;


            //Row fact4 = finalAddressTable.AddRow();
            final3.Cells[2].AddParagraph(QCInspectionPickingPDF.GetValueOrDefault("Phone", ""));
            final3.Cells[2].AddParagraph();
            address.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
            address.Format.SpaceAfter = 2;
            final3.Cells[2].Format.Font.Bold = true;
            final3.Cells[2].Format.SpaceBefore = 1;

            //Section.Footers.EvenPage.Add(paragraph.Clone());
        }

        /// <summary>
        /// create page footer data
        /// </summary>
        /// <param name="section"></param>
        private void AddPageFoooter(Section section)
        {
            Paragraph paragraphFooter = section.Footers.Primary.AddParagraph();
            paragraphFooter.Format.Alignment = ParagraphAlignment.Left;
           // paragraphFooter.Format.SpaceAfter = 15;


            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddTab();
            paragraph.AddPageField();
            paragraph.AddText(" of ");
            paragraph.AddNumPagesField();
            paragraph.Format.SpaceBefore = 170;

            //paragraph.AddText($"{DateTime.Now.ToString("dd/MM/yyyy")}");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            Section.Footers.EvenPage.Add(paragraph.Clone());
        }
    }
}
