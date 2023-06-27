using FileServer.Enum;
using FileServer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileServer.Services
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        private readonly CloudBlobClient _cloudBlobClient;
        private CloudBlobContainer _container;
        private static IConfiguration _configuration = null;
        private readonly Uri _blobServiceEndpoint = null;
        public Dictionary<int, Dictionary<int, string>> containerList;


        public AzureBlobStorage(IConfiguration configuration)
        {
            _configuration = configuration;

            containerList = new Dictionary<int, Dictionary<int, string>>();

            containerList.Add((int)FileContainerList.Products,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apiinspectionproductfiles" },
                                    { (int)EntityEnum.SGT, "sgtinspectionproductfiles" },
                                    { (int)EntityEnum.AQF, "aqfinspectionproductfiles" }
                                });

            containerList.Add((int)FileContainerList.DevContainer,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apidevelopmentfiles" },
                                    { (int)EntityEnum.SGT, "sgtdevelopmentfiles" },
                                     { (int)EntityEnum.AQF, "aqfdevelopmentfiles" }
                                });

            containerList.Add((int)FileContainerList.InspectionBooking,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apiinspectionbookingfiles" },
                                    { (int)EntityEnum.SGT, "sgtinspectionbookingfiles" },
                                     { (int)EntityEnum.AQF, "aqfinspectionbookingfiles" }
                                });

            containerList.Add((int)FileContainerList.Invoice,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apiinvoicefiles" },
                                    { (int)EntityEnum.SGT, "sgtinvoicefiles" },
                                    { (int)EntityEnum.AQF, "aqfinvoicefiles" }
                                });

            containerList.Add((int)FileContainerList.Hr,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apihrfiles" },
                                    { (int)EntityEnum.SGT, "sgthrfiles" },
                                     { (int)EntityEnum.AQF, "aqfhrfiles" }
                                });

            containerList.Add((int)FileContainerList.Audit,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apiauditfiles" },
                                    { (int)EntityEnum.SGT, "sgtauditfiles" },
                                    { (int)EntityEnum.AQF, "aqfauditfiles" }
                                });

            containerList.Add((int)FileContainerList.Expense,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apiexpensefiles" },
                                    { (int)EntityEnum.SGT, "sgtexpensefiles" },
                                     { (int)EntityEnum.AQF, "aqfexpensefiles" }
                                });

            containerList.Add((int)FileContainerList.EmailSend,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apiemailsend" },
                                    { (int)EntityEnum.SGT, "sgtemailsend" },
                                     { (int)EntityEnum.AQF, "aqfemailsend" }
                                });

            containerList.Add((int)FileContainerList.Report,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apireport" },
                                    { (int)EntityEnum.SGT, "sgtreport" },
                                     { (int)EntityEnum.AQF, "aqfreport" }
                                });

            containerList.Add((int)FileContainerList.QuotationPdf,
                            new Dictionary<int, string>()
                                {
                                    { (int)EntityEnum.API, "apiquotationpdffiles" },
                                    { (int)EntityEnum.SGT, "sgtquotationpdffiles" },
                                     { (int)EntityEnum.AQF, "aqfquotationpdffiles" }
                                });
            containerList.Add((int)FileContainerList.DataManagement,
                           new Dictionary<int, string>()
                               {
                                    { (int)EntityEnum.API, "apiinspectionreportfiles" },
                                    { (int)EntityEnum.SGT, "sgtinspectionreportfiles" },
                                     { (int)EntityEnum.AQF, "aqfinspectionreportfiles" }
                               });
            containerList.Add((int)FileContainerList.Claim,
                           new Dictionary<int, string>()
                               {
                                    { (int)EntityEnum.API, "apiclaimfiles" },
                                    { (int)EntityEnum.SGT, "sgtclaimfiles" },
                                     { (int)EntityEnum.AQF, "aqfclaimfiles" }
                               });
            containerList.Add((int)FileContainerList.GapSupportingDocument,
                           new Dictionary<int, string>()
                               {
                                    { (int)EntityEnum.API, "apigapfiles" },
                                    { (int)EntityEnum.SGT, "sgtgapfiles" },
                                     { (int)EntityEnum.AQF, "aqfgapfiles" }
                               });
            containerList.Add((int)FileContainerList.InvoiceSend,
                          new Dictionary<int, string>()
                              {
                                    { (int)EntityEnum.API, "apiinvoicefiles" },
                                    { (int)EntityEnum.SGT, "sgtinvoicefiles" },
                                     { (int)EntityEnum.AQF, "aqfinvoicefiles" }
                              });
            containerList.Add((int)FileContainerList.ScheduleJob,
                         new Dictionary<int, string>()
                             {
                                    { (int)EntityEnum.API, "apischedulejobs" },
                                    { (int)EntityEnum.SGT, "sgtschedulejobs" },
                                     { (int)EntityEnum.AQF, "aqfschedulejobs" }
                             });



            var credentials = new StorageCredentials(_configuration.GetSection("BlobStorageAccount").Value, _configuration.GetSection("BlobStorageAccountKey").Value);
            _blobServiceEndpoint = new Uri(_configuration.GetSection("BlobServiceEndpoint").Value);
            _cloudBlobClient = new CloudBlobClient(_blobServiceEndpoint, credentials);
        }

        /// <summary>
        /// Get Resource URI by reference id
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>

        public Uri GetResourceUri(string resourceId, int containerId, int entityId)
        {
            var containerName = containerList[containerId][entityId];
            return new Uri(_blobServiceEndpoint, $"/{containerName}/{resourceId}");
        }

        /// <summary>
        /// Get URI with SAAS token
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public Uri GetResourceUriWithSas(string resourceId, int containerId,int entityId)
        {
            var sasPolicy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.Now.AddMinutes(-10),
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30)
            };

            _container = InitializeContainer(containerId, entityId);

            CloudBlockBlob blob = _container.GetBlockBlobReference(resourceId);

            string sasToken = blob.GetSharedAccessSignature(sasPolicy);

            return new Uri(_blobServiceEndpoint, $"/{containerId}/{resourceId}{sasToken}");
        }

        /// <summary>
        /// Save or update File in the blob storage
        /// </summary>
        /// <param name="fileInputStream"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<string> SaveFile(byte[] file, string resourceId, int containerId, string contentType,int entityId)
        {
            _container = InitializeContainer(containerId, entityId);

            // string blobID = Guid.NewGuid().ToString();

            CloudBlockBlob blob = _container.GetBlockBlobReference(resourceId);

            blob.Properties.ContentType = contentType;

            await blob.UploadFromByteArrayAsync(file, 0, file.Length);

            return blob.Uri.AbsoluteUri;
        }

        /// <summary>
        /// get blob instance from Azure 
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="containerName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public CloudBlockBlob getBlobInstance(string resourceId, int containerId, string contentType, int entityId)
        {
            _container = InitializeContainer(containerId, entityId);
            CloudBlockBlob blob = _container.GetBlockBlobReference(resourceId);
            blob.Properties.ContentType = contentType;
            return blob;
        }

        /// <summary>
        /// delete the file if exist
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFile(string resourceId, int containerId,int entityId)
        {
            _container = InitializeContainer(containerId, entityId);
            CloudBlockBlob blob = _container.GetBlockBlobReference(resourceId);
            return await blob.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Initialize Container name 
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        private CloudBlobContainer InitializeContainer(int containerId,int entityId)
        {

            var containerName = containerList[containerId][entityId];

            CloudBlobContainer _container = _cloudBlobClient.GetContainerReference(containerName);

            // Create the container and set the permission  
            if (_container.CreateIfNotExistsAsync().Result)
            {
                _container.SetPermissionsAsync(
                     new BlobContainerPermissions
                     {
                         PublicAccess = BlobContainerPublicAccessType.Container
                     }
                 );
            }

            return _container;
        }

        /// <summary>
        /// Download the file from unique name 
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public async Task<FileDownloadResponse> Download(string blobName, int containerId,int entityId)
        {
            CloudBlockBlob blob;
            _container = InitializeContainer(containerId, entityId);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                blob = _container.GetBlockBlobReference(blobName);
                if (!await blob.ExistsAsync())//Data pushed to dev container due to wrong container mapping.
                {
                    _container = InitializeContainer((int)FileContainerList.DevContainer, entityId);
                    blob = _container.GetBlockBlobReference(blobName);
                }
                await blob.DownloadToStreamAsync(memoryStream);
            }

            Stream blobStream = blob.OpenReadAsync().Result;
            return new FileDownloadResponse() { BlobStream = blobStream, BlobContentType = blob.Properties.ContentType, BlobName = blob.Name };
        }
    }
}
