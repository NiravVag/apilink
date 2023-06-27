using Components.Core.entities;
using DocumentFormat.OpenXml.Packaging;
using FileGenerationComponent.Excel;
using FileGenerationComponent.PDF;
using FileGenerationComponent.PPT;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileGenerationComponent
{
    internal abstract class FileProvider : IDisposable
    {
        public FileProvider(IConfiguration configuration)
        {
            Stream = new MemoryStream();
            _configuration = configuration;
        }

        internal FileType Type { get; set;  }

        internal string MimeType { get; private set; }

        protected MemoryStream Stream { get; set;  }

        protected abstract FileProperties GenerateDocument(FileObject source);

        protected readonly IConfiguration _configuration = null; 

        protected static Action<int, string>  _onLog = null; 

      //  public abstract FileObject ParseDocument(ConfigurationSourceModel model);


        public FileProperties BuildFile(FileObject  fileObject)
        {
            return GenerateDocument(fileObject);

        }


        internal static FileProvider GetProvider(FileType fileType, IConfiguration configuration, Action<int, string> onLog)
        {
            _onLog = onLog;

            switch (fileType)
            {
                case FileType.Excel:
                    return new ExcelProvider(configuration);
                case FileType.PDF:
                    return new PDFProvider(configuration);
                case FileType.PPT:
                    return new PPTProvider(configuration);
                default:
                    throw new Exception($"{fileType} is not supported by system ");
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                    Stream.Dispose(); 
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~FileProvider() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
