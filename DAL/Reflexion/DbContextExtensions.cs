using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DAL.Reflexion
{
    public static class DbContextExtensions
    {
        public static IQueryable<TSource> IgnoreProperty<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty>> predicate) where TSource : class, new()
        {
            // Get property
            var expression = (MemberExpression)predicate.Body;
            string name = expression.Member.Name;

            if (!string.IsNullOrEmpty(name) && typeof(TSource).GetProperties().Any(x => x.Name == name))
                return source.Select(x => GetCopyWithIgnore(x, name));

            return source;
        }

        public static IIncludableQueryable<TEntity, TPreviousProperty> IgnorePropertyInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, TPreviousProperty> source, Expression<Func<TPreviousProperty, TProperty>> predicate)
            where TEntity : class, new()
            where TPreviousProperty : class, new()
        {
            // Get property
            var expression = (MemberExpression)predicate.Body;
            string name = expression.Member.Name;

            if (!string.IsNullOrEmpty(name) && typeof(TPreviousProperty).GetProperties().Any(x => x.Name == name))
                return (IIncludableQueryable<TEntity, TPreviousProperty>)source.Select(x => GetCopyWithIgnore(x, name));

            return new IncludableQueryable<TEntity, TPreviousProperty>(source.Cast<TEntity>());
        }

        public static IIncludableQueryable<TEntity, TProperty> IgnorePropertyThen<TEntity, TPreviousProperty, TProperty, TOldProperty>(this IIncludableQueryable<TEntity, ICollection<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> predicate, Expression<Func<TEntity, ICollection<TOldProperty>>> predicate2) 
            where TEntity : class, new()
             where TPreviousProperty : class, new()
        {  
            // Get property
            var expression = (MemberExpression)predicate.Body;
            string name = expression.Member.Name;

            var expression2 = (MemberExpression)predicate2.Body;
            string name2 = expression2.Member.Name;


            if (!string.IsNullOrEmpty(name) && typeof(TPreviousProperty).GetProperties().Any(x => x.Name == name) && typeof(TEntity).GetProperties().Any(x => x.Name == name2))
            {
                var property = typeof(TEntity).GetProperties().First(x => x.Name ==  name2);

                foreach (var item in source)
                {
                    var oldCollection = (ICollection<TOldProperty>)property.GetValue(item);

                    if (oldCollection != null)
                    {
                        foreach (var old in oldCollection)
                        {
                            var valueCollection = typeof(TOldProperty).GetProperties().FirstOrDefault(x => x.PropertyType == typeof(ICollection<TPreviousProperty>)); 


                            if(valueCollection  != null)
                            {

                            }

                        }
                    }
                        

                   // property.SetValue(item, valueCollection);
                }

            }

           // return new IncludableQueryable<TEntity, TProperty>(source.Select(x => GetCopyWithIgnore(x, name)));

            return new IncludableQueryable<TEntity, TProperty>(source.Cast<TEntity>());

        }

        public static IIncludableQueryable<TEntity, TProperty> IgnorePropertyThen<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, TPreviousProperty> source, Expression<Func<TPreviousProperty, TProperty>> predicate) where TEntity : class, new()
        {
            var expression = (MemberExpression)predicate.Body;
            string name = expression.Member.Name;


            if (!string.IsNullOrEmpty(name) && typeof(TPreviousProperty).GetProperties().Any(x => x.Name == name))
                return (IIncludableQueryable<TEntity, TProperty>)source.Select(x => GetCopyWithIgnore(x, name));

            return new IncludableQueryable<TEntity, TProperty>(source.Cast<TEntity>());

        }

        private static TEntity GetCopyWithIgnore<TEntity>(TEntity entity, string propertyName) where TEntity : class , new()
        {
            var currEntity = new TEntity();

            foreach (var prop in typeof(TEntity).GetProperties().Where(x => x.Name != propertyName))
            {
                object value = prop.GetValue(entity);
                prop.SetValue(currEntity, value);
            }

            return currEntity; 
        }
       

    }


    internal class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>
    {

        private readonly IQueryable<TEntity> _Source = null;


        public IncludableQueryable(IQueryable<TEntity> source)
        {
            _Source = source; 
        }


        public Type ElementType => _Source.ElementType;

        public Expression Expression => _Source.Expression;

        public IQueryProvider Provider => _Source.Provider;

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _Source.GetEnumerator(); 
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Source.GetEnumerator();
        }
    }
}
