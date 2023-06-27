using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Components.Core.entities;
using Microsoft.Extensions.Configuration;

namespace FileGenerationComponent.PDF
{
    internal class PDFProvider : FileProvider
    {

        public PDFProvider(IConfiguration configuration) :base(configuration)
        {

        }

        private void Addheader(PdfFileObject.Container header)
        {

        }

        private void AddFooter(PdfFileObject.Container footer)
        {

        }

        protected override FileProperties GenerateDocument(FileObject source)
        {
            throw new NotImplementedException();
        }
    }
}
