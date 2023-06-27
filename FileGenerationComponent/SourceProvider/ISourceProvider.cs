using System;
using System.Collections.Generic;
using System.Text;

namespace FileGenerationComponent.SourceProvider
{

    public interface  ISourceProvider
    {

        // Get source 
        FileObject GetFileObject(object source);

        // Get FuncGetTtime
        Func<string, string> FuncGetMimeType { get; set;  }

        string RootPath { get; set; }
    }
}
