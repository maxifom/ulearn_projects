using System;
using System.Linq.Expressions;

namespace Reflection.Differentiation
{
    public static class Algebra
    {
        /// <summary>
        /// Символьное дифференцирование
        /// тела фукнции
        /// </summary>
        /// <param name="expression">
        /// Тело функции
        /// </param>
        /// <returns>
        /// Производную функции
        /// </returns>
        private static Expression DifferentiateBody(Expression expression)
        {
            if (expression is ConstantExpression)
                return Expression.Constant(0.0);
            if (expression is ParameterExpression)
                return Expression.Constant(1.0);

            if (expression is BinaryExpression operation)
            {
                var left = operation.Left;
                var right = operation.Right;

                if (expression.NodeType == ExpressionType.Add)
                    return Expression.Add(DifferentiateBody(left), DifferentiateBody(right));

                if (expression.NodeType == ExpressionType.Multiply)
                    return Expression.Add(Expression.Multiply(DifferentiateBody(left), right),
                        Expression.Multiply(DifferentiateBody(right), left));
            }

            if (expression is MethodCallExpression methodExpression)
            {
                var newMethodExpression = expression;
                var arg = methodExpression.Arguments[0];

                if (methodExpression.Method.Name == "Sin")
                    newMethodExpression = Expression.Call(typeof(Math).GetMethod("Cos", new[] { typeof(double) }), arg);

                if (methodExpression.Method.Name == "Cos")
                    newMethodExpression = Expression.Negate(
                        Expression.Call(typeof(Math).GetMethod("Sin", new[] { typeof(double) }), arg));

                return Expression.Multiply(newMethodExpression, DifferentiateBody(arg));
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Символьное дифференцирование
        /// функции
        /// </summary>
        /// <param name="expression">
        /// Функция для дифференцирования
        /// </param>
        /// <returns>
        /// Производную функции
        /// </returns>
        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> expression)
        {
            return Expression.Lambda<Func<double, double>>(DifferentiateBody(expression.Body), expression.Parameters);
        }
    }
}