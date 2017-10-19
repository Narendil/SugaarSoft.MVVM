using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace SugaarSoft.MVVM.Helpers
{
    public static class PropertyNameHelper
    {
        public static PropertyChangedEventArgs CreateArgs<T>(Expression<Func<T, Object>> propertyExpression)
        {
            return new PropertyChangedEventArgs(GetPropertyName(propertyExpression));
        }

        public static string GetPropertyName<T>(Expression<Func<T, Object>> propertyExpression)
        {
            var lambda = propertyExpression as LambdaExpression;
            return ((MemberExpression)(lambda.Body is UnaryExpression
                                            ? ((UnaryExpression)lambda.Body).Operand
                                            : lambda.Body)).Member.Name;
        }

        public static string GetPropertyName<T>(this Object obj, Expression<Func<T, Object>> propertyExpression)
        {
            return GetPropertyName(propertyExpression);
        }

        public static string GetPropertyName<T>(this Object obj, Expression<Func<T>> propertyExpression)
        {
            return GetPropertyName(propertyExpression);
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = null;
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = propertyExpression.Body as MemberExpression;
                propertyName = memberExpr.Member.Name;
            }
            return propertyName;
        }
    }
}
