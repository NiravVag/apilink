using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Contracts.Managers
{
    public interface ITranslationManager
    {
        /// <summary>
        /// Set Translation for many properties
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TCible"></typeparam>
        /// <param name="source"></param>
        /// <param name="cible"></param>
        /// <param name="args"></param>
        void Translate<TSource, TCible>(TSource source, TCible cible, params (Expression<Func<TSource, int>> expSource, Expression<Func<TCible, string>> expCible)[] args);

        /// <summary>
        /// Set Translation for one property
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TCible"></typeparam>
        /// <param name="source"></param>
        /// <param name="cible"></param>
        /// <param name="mapItem"></param>
        void Translate<TSource, TCible>(TSource source, TCible cible, (Expression<Func<TSource, int>> expSource, Expression<Func<TCible, string>> expCible) mapItem);
    }
}
