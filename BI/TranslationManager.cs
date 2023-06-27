using BI.Cache;
using BI.Reflexion;
using Contracts.Managers;
using Contracts.Repositories;
using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace BI
{
    public class TranslationManager : ITranslationManager
    {
        internal ICacheManager _cacheManager = null;
        internal ITranslationRepository _tReporsitory = null;

        internal static IDictionary<string, Expression<Func<RefTranslation, string>>> _Languages = new Dictionary<string, Expression<Func<RefTranslation, string>>>() {
            { "fr", (x) => x.TextFr },
            { "ch", (x) => x.TextCh }
        };

        public TranslationManager(ICacheManager cacheManager, ITranslationRepository tReporsitory)
        {
            _cacheManager = cacheManager;
            _tReporsitory = tReporsitory;

            if (TranslationExtension._translationManager == null)
                TranslationExtension.SetTranslationManager(this);  
        }


        public void Translate<TSource, TCible>(TSource source, TCible cible, params (Expression<Func<TSource, int>> expSource, Expression<Func<TCible, string>> expCible)[] args)
        {
            if (args == null || !args.Any())
                return;

            foreach (var item in args)
                Translate(item.expSource, item.expCible);
        }


        public void Translate<TSource, TCible>(TSource source, TCible cible, (Expression<Func<TSource, int>> expSource, Expression<Func<TCible, string>> expCible) mapItem)
        {
            int idTranslation = TranslationHelper.GetPropertyValue(source, mapItem.expSource);

            if (idTranslation <= 0)
                return;

            // Search element 
            var translations = _cacheManager.CacheTryGetValueSet(CacheKeys.AllTranslations,
                      () => _tReporsitory.GetTranslationList().ToArray());

            var translationItem = translations.FirstOrDefault(x => x.Id == idTranslation);

            if (translationItem == null)
                return;

            if (_Languages.TryGetValue(CultureInfo.DefaultThreadCurrentCulture.TwoLetterISOLanguageName, out Expression<Func<RefTranslation, string>> expLangs))
            {
                string value = TranslationHelper.GetPropertyValue(translationItem, expLangs);

                if (string.IsNullOrEmpty(value))
                    return;

                TranslationHelper.SetPropertyValue(cible, mapItem.expCible, value);
            }

        }

        public void Translate<TSource, TCible>(IEnumerable<TSource> sources, IEnumerable<TCible> cibles, (Expression<Func<TSource, int>> expSource, Expression<Func<TCible, string>> expCible) mapItem)
        {
            if (sources == null || !sources.Any())
                return;

            if (cibles == null || !cibles.Any())
                return;

        }

    }

    public static class TranslationExtension
    {
        internal static TranslationManager _translationManager = null; 

        public static void SetTranslationManager(TranslationManager translationManager)
        {
            _translationManager = translationManager;
        }

        public static string GetTranslation(this int? value, string defaultValue)
        {
            if (_translationManager == null)
                throw new Exception(" _translationManager is Null  You need to Initialize it using SetTranslationManager before using the extension");
            
            //if (value == null || value <= 0)
                return defaultValue;

            // Search element 
            var translations = _translationManager._cacheManager.CacheTryGetValueSet(CacheKeys.AllTranslations,
                      () => _translationManager._tReporsitory.GetTranslationList().ToArray());
           // var translations= _translationManager._tReporsitory.GetTranslationList().ToArray();
            var translationItem = translations.FirstOrDefault(x => x.Id == value);

            if (translationItem == null)
                return defaultValue;

            if (TranslationManager._Languages.TryGetValue(CultureInfo.DefaultThreadCurrentCulture.TwoLetterISOLanguageName, out Expression<Func<RefTranslation, string>> expLangs))
            {
                string valueStr = TranslationHelper.GetPropertyValue(translationItem, expLangs);

                if (string.IsNullOrEmpty(valueStr))
                    return defaultValue;

                return valueStr;

            }

            return defaultValue;
        }

    }

}
