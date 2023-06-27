using Components.Core.entities;
using FileGenerationComponent.SourceProvider;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileGenerationComponent.PDF
{
    public class HtmlPdfProvider : HtmlProvider
    {
        protected override DataType GetDataType(HtmlNode node)
        {
            throw new NotImplementedException();
        }

        protected override string GetFormat(HtmlNode node)
        {
            throw new NotImplementedException();
        }

        protected override string GetFormula(HtmlNode node)
        {
            throw new NotImplementedException();
        }

        protected override string GetRefName(HtmlNode node)
        {
            throw new NotImplementedException();
        }

        protected override string GetTableBeginCell(HtmlNode nodeTable)
        {
            throw new NotImplementedException();
        }

        protected override FileObject Translate(string html)
        {
            var containerList = new List<PdfFileObject.Container>();
            var fileObject = new PdfFileObject();

            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(html);

            //title 
            var htmlTitle = pageDocument.DocumentNode.SelectNodes("//pdf-title");

            if (htmlTitle != null && htmlTitle.Any())
                fileObject.Title = htmlTitle.First().InnerText;

            //Header
            var htmlHeader = pageDocument.DocumentNode.SelectNodes("//pdf-header");

            if (htmlHeader != null && htmlHeader.Any())
            {
                var header = new PdfFileObject.Container
                {
                    Type = PdfFileObject.ContainerType.Header,
                    ContainerList = GetContainerList(htmlHeader.First())
                };

                SetProperties(header, htmlHeader.First());
                containerList.Add(header);
            }

            //Footer
            var htmlFooter = pageDocument.DocumentNode.SelectNodes("//pdf-footer");

            if (htmlFooter != null && htmlFooter.Any())
            {
                var footer = new PdfFileObject.Container
                {
                    Type = PdfFileObject.ContainerType.Footer,
                    ContainerList = GetContainerList(htmlFooter.First())
                };

                SetProperties(footer, htmlFooter.First());
                containerList.Add(footer);
            }

            // pages 
            var htmlPages = pageDocument.DocumentNode.SelectNodes("//pdf-page");

            if (htmlPages != null && htmlPages.Any())
            {
                foreach (var page in htmlPages)
                    AddPage(containerList, page);
            }

            fileObject.ContainerList = containerList;

            return fileObject;
        }

        private void AddPage(List<PdfFileObject.Container> list, HtmlNode page)
        {
            var newPage = new PdfFileObject.Container
            {
                Type = PdfFileObject.ContainerType.Page,
                Text = page.InnerText,
                ContainerList = GetContainerList(page)
            };

            SetProperties(newPage, page);
            list.Add(newPage);
        }

        private IEnumerable<PdfFileObject.Container> GetContainerList(HtmlNode node)
        {
            if(node.ChildNodes.Count >0)
            {
                foreach(var childNode in node.ChildNodes)
                {
                    if(childNode.Name == "pdf-container")
                    {
                        var container = new PdfFileObject.Container
                        {
                            Type = PdfFileObject.ContainerType.Container,
                            ContainerList = GetContainerList(childNode)
                        };
                        SetProperties(container, childNode);
                        yield return container;
                    }
                    else if(childNode.Name == "table")
                    {
                        var container = new PdfFileObject.Container
                        {
                            Type = PdfFileObject.ContainerType.Table,
                            ContainerList = GetContainerList(childNode)
                        };
                        SetProperties(container, childNode);
                        yield return container;
                    }
                }
            }

        }
    }
}
