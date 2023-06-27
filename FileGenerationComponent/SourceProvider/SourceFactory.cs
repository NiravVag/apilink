using Components.Core.entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileGenerationComponent.SourceProvider
{
    public class SourceFactory
    {
        public static ISourceProvider GetSourceProvider(SourceEnum source, FileType type, IConfiguration configuration, Action<int, string> onLog)
        {
            switch (source)
            {
                case SourceEnum.Html:
                    return HtmlProvider.GetInstance(type, onLog);
                case SourceEnum.Json:
                    return JsonProvider.GetInstance(type, configuration, onLog);
                default:
                    return HtmlProvider.GetInstance(type, onLog);
            }
        }
    }

}
