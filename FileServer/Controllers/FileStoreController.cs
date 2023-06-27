using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileServer.Model;
using FileServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileStoreController : ControllerBase
    {
        private readonly IAzureBlobStorage _blobStorage = null;
        public FileStoreController(IAzureBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        [HttpPost("savefile/{container}/{entity}")]
        [DisableRequestSizeLimit]
        public async Task<FileUploadResponse> UploadAttachedFiles(List<IFormFile> files, int container, int entity)
        {
            List<FileUploadData> FileUploadDataList = new List<FileUploadData>();

            byte[] dataFiles;

            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        try
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);

                                dataFiles = ms.ToArray();

                                var blobURL = await _blobStorage.SaveFile(dataFiles, file.FileName, container, file.ContentType, entity);

                                FileUploadDataList.Add(new FileUploadData()
                                {
                                    FileName = file.FileName,
                                    FileCloudUri = blobURL,
                                    Result = FileUploadResponseResult.Sucess
                                });
                            }
                        }
                        catch (Exception)
                        {
                            FileUploadDataList.Add(new FileUploadData()
                            {
                                FileName = file.FileName,
                                FileCloudUri = string.Empty,
                                Result = FileUploadResponseResult.Failure
                            });
                        }
                    }
                    else
                    {
                        FileUploadDataList.Add(new FileUploadData()
                        {
                            FileName = file.FileName,
                            FileCloudUri = string.Empty,
                            Result = FileUploadResponseResult.Failure
                        });
                    }
                }
            }

            return new FileUploadResponse() { FileUploadDataList = FileUploadDataList };
        }

        [HttpGet("getopenlinkbyid/{id}/{container}/{entity}")]
        public string GetFileURL(string id, int container, int entity)
        {
            Uri imageUri = _blobStorage.GetResourceUri(id, container, entity);
            return imageUri != null ? imageUri.ToString() : string.Empty;
        }

        [HttpGet("getopenlinksasbyid/{id}/{container}/{entity}")]
        public string GetFileURLwithSAS(string id, int container, int entity)
        {
            Uri imageUri = _blobStorage.GetResourceUriWithSas(id, container, entity);
            return imageUri != null ? imageUri.ToString() : string.Empty;
        }

        [HttpDelete("deletefile/{id}/{container}/{entity}")]
        public async Task<bool> DeleteFile(string id, int container, int entity)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            return await _blobStorage.DeleteFile(id, container, entity);
        }

        [HttpGet("downloadfile/{id}/{container}/{entity}")]
        public async Task<IActionResult> Download(string id, int container, int entity)
        {
            var fileStreamData = await _blobStorage.Download(id, container, entity);

            return File(fileStreamData.BlobStream, fileStreamData.BlobContentType, fileStreamData.BlobName);
        }

        [HttpPost("uploadzipfile")]
        public async Task<ZipFileUploadResponse> RequestZIPFile(RequestZIPFile requestZIPFile)
        {
            var response = new ZipFileUploadResponse();
            FileUploadData fileUploadData = new FileUploadData();

            var semaphore = new SemaphoreSlim(50);

            var tasks = new List<Task>();

            var streamList = new List<FileDownloadResponse>();

            foreach (var zipFileData in requestZIPFile.ZIPFileDataList)
            {
                await semaphore.WaitAsync();

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var blobData = await _blobStorage.Download(zipFileData.UniqueId, requestZIPFile.Container, requestZIPFile.EntityId);

                        streamList.Add(blobData);
                    }

                    catch (Exception)
                    {
                       
                    }

                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);

            try
            {
                // create a working memory stream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // create a zip
                    using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {

                        // iterate through the source files
                        foreach (var streamData in streamList)
                        {
                            var fileData = requestZIPFile.ZIPFileDataList.FirstOrDefault(x => x.UniqueId == streamData.BlobName);

                            var fileName = Path.GetFileNameWithoutExtension(fileData.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName);

                            // to convert stream to byte array 
                            MemoryStream stream = new MemoryStream();
                            streamData.BlobStream.CopyTo(stream);

                            // add the item name to the zip
                            ZipArchiveEntry zipItem = zip.CreateEntry(fileName);
                            // add the item bytes to the zip entry by opening the original file and copying the bytes
                            using (MemoryStream originalFileMemoryStream = new MemoryStream(stream.ToArray()))
                            {
                                using (Stream entryStream = zipItem.Open())
                                {
                                    originalFileMemoryStream.CopyTo(entryStream);
                                }
                            }
                        }
                    }

                    //save the inspection zipped file 
                    fileUploadData = await SaveInspectionZipFile(memoryStream, requestZIPFile.Container, requestZIPFile.EntityId);

                }
            }
            catch (Exception)
            {

            }

            return new ZipFileUploadResponse() { FileUploadData = fileUploadData };
        }


        /// <summary>
        /// Save the inspection booking zip file
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="containerId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        private async Task<FileUploadData> SaveInspectionZipFile(MemoryStream memoryStream, int containerId, int entityId)
        {
            var zipName = $"BookingAttachment_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip";
            var fileUniqueId = newUniqueId();
            var blobURL = await _blobStorage.SaveFile(memoryStream.ToArray(), fileUniqueId, containerId, "application/zip", entityId);

            return new FileUploadData()
            {
                FileName = zipName,
                FileCloudUri = blobURL,
                FileUniqueId = fileUniqueId,
                Result = FileUploadResponseResult.Sucess
            };
        }

        private string newUniqueId()
        {
            Guid myuuid = Guid.NewGuid();

            return myuuid.ToString();
        }

    }
}
