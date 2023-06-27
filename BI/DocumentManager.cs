using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Documents;
using DTO.File;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class DocumentManager : IDocumentManager
    {
        private readonly IFileManager _fileManager = null;
        private readonly IUserConfigRepository _userConfigRepo = null;
        public DocumentManager( IFileManager fileManager, IUserConfigRepository userConfigRepo)
        {
            _fileManager = fileManager;
            _userConfigRepo = userConfigRepo;
        }

        public FileResponse GetFileData(string filepath, string inspBookTerms)
        {
            var filename =string.Concat(filepath, inspBookTerms);
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            var filecontent = FileParser.ReadFiletoByteArray(filename);

            if (filecontent == null)
                return null;

                    return new FileResponse
                    {
                        Content = filecontent,
                        MimeType = _fileManager.GetMimeType(Path.GetExtension(filename)),
                        Result = FileResult.Success
                    };

        }
    }
}
