using Contracts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BI.Reflexion
{
    public static class TranslationHelper
    {


        public static dynamic GetPropertyValue<T>(T obj, string propertyName)
        {
            var property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == propertyName);

            if (property == null)
                return null;

            return property.GetValue(obj);
        }

        public static void SetPropertyValue<T>(T obj, string propertyName, object value)
        {
            var property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == propertyName);

            if (property == null)
                return;

            property.SetValue(obj, value);
        }

        public static  string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var member = (MemberExpression)expression.Body;
            return member.Member.Name;
        }

        public static dynamic GetPropertyValue<T, TProperty>(T obj, Expression<Func<T, TProperty>> expression)
        {
            string propertyName = GetPropertyName(expression);

            if (string.IsNullOrEmpty(propertyName))
                return null; 

            var property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == propertyName);

            if (property == null)
                return null;

            return property.GetValue(obj);
        }

        public static void SetPropertyValue<T, TProperty>(T obj, Expression<Func<T, TProperty>> expression, object value)
        {
            string propertyName = GetPropertyName(expression);

            if (string.IsNullOrEmpty(propertyName))
                return;

            var property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == propertyName);

            if (property == null)
                return;

            property.SetValue(obj, value);
        }


    }
}
