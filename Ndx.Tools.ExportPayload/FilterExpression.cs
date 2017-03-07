using Ndx.Ingest.Trace;
using System;
using System.Collections.Generic;
using System.Linq;                             
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sprache;
namespace Ndx.Tools.ExportPayload
{
    /// <summary>
    /// This class represents filter expression. 
    /// </summary>
    /// <remarks>
    /// For supported syntax see: https://www.wireshark.org/docs/wsug_html_chunked/ChWorkBuildDisplayFilterSection.html
    /// </remarks>
    public class FilterExpression
    {
        public static FilterExpression TryParse(string filter)
        {
            var t = Lambda.Parse(filter);
            return new FilterExpression(t);
        }

        static Parser<ExpressionType> Operator(string op, ExpressionType opType) =>             
            Parse.String(op).Token().Return(opType);

        static readonly Parser<ExpressionType> AndAlso = Operator("&&", ExpressionType.AndAlso);

        static readonly Parser<ExpressionType> OrElse = Operator("||", ExpressionType.OrElse);

        static readonly Parser<ExpressionType> Not = Operator("!", ExpressionType.Not);

        static readonly Parser<ExpressionType> Equal = Operator("==", ExpressionType.Equal);

        static readonly Parser<ExpressionType> NotEqual = Operator("!=", ExpressionType.NotEqual);

        static readonly Parser<ExpressionType> GreaterThan = Operator(">", ExpressionType.GreaterThan);

        static readonly Parser<ExpressionType> LessThan = Operator("<", ExpressionType.LessThan);

        static readonly Parser<ExpressionType> GreaterThanOrEqual = Operator(">=", ExpressionType.GreaterThanOrEqual);

        static readonly Parser<ExpressionType> LessThanOrEqual = Operator("<=", ExpressionType.LessThanOrEqual);

        static readonly Parser<Expression> Constant =
           Parse.Decimal
           .Select(x => Expression.Constant(long.Parse(x)));

        static readonly Parser<Expression> Field =
            from name in Parse.Letter.AtLeastOnce().Text()
            select AccessField(name);

        static Expression AccessField(string name)
        {
            var propertyName = name;
            Expression instance = null;
            // TODO: implement access to instance of FlowKey.
            return Expression.PropertyOrField(instance, propertyName);
        }

        static readonly Parser<Expression> Expr = null;

        static readonly Parser<Expression<Func<FlowKey, bool>>> Lambda =
           Expr.End().Select(body => Expression.Lambda<Func<FlowKey, bool>>(body));



        private Expression<Func<FlowKey, bool>> filterExpression;

        public FilterExpression(Expression<Func<FlowKey, bool>> expression)
        {
            this.filterExpression = expression;
        }

        public Func<FlowKey, bool> GetFlowFilter()
        {
            return filterExpression.Compile();
        }
    }
}
