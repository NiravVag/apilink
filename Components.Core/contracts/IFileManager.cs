using Components.Core.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Core.contracts
{
    public interface IFileManager
    {
        /// <summary>
        /// Build file
        /// </summary>
        /// <param name="html"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        FileProperties BuildFile(object source, FileType fileType, SourceEnum sourceType, Action<int, string> onLog = null);

        /// <summary>
        /// Get MimeType
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        string GetMimeType(string extension);

        /// <summary>
        /// Get extension from mime Type
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        string GetExtension(string mimeType);


        /// <summary>
        /// get mime type by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetMimeTypeByUrl(string url);
    }
}
