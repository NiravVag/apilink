using FileServer.Model;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static FileServer.Services.AzureBlobStorage;

namespace FileServer.Services
{
    public interface IAzureBlobStorage
    {
        Uri GetResourceUri(string resourceId, int containerId, int entityId);
        Uri GetResourceUriWithSas(string resourceId, int containerId, int entityId);
        Task<string> SaveFile(byte[] file, string resourceId, int containerId, string contentType, int entityId);
        Task<bool> DeleteFile(string resourceId, int containerId, int entityId);
        Task<FileDownloadResponse> Download(string blobName, int containerId, int entityId);
        CloudBlockBlob getBlobInstance(string resourceId, int containerId, string contentType, int entityId);
    }
}
