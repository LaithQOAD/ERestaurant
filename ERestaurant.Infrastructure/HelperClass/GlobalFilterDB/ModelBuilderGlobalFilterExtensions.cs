using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERestaurant.Infrastructure.HelperClass.GlobalFilterDB
{
    public static class ModelBuilderGlobalFilterExtensions
    {
        public static void ApplyGlobalFilter<TBase>(
            this ModelBuilder modelBuilder,
            Expression<Func<TBase, bool>> filter)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clr = entityType.ClrType;
                if (!typeof(TBase).IsAssignableFrom(clr)) continue;

                var param = Expression.Parameter(clr, "e");
                var body = new ReplaceParamVisitor(filter.Parameters[0], param).Visit(filter.Body)!;
                var lambda = Expression.Lambda(body, param);

                modelBuilder.Entity(clr).HasQueryFilter(lambda);
            }
        }

        private sealed class ReplaceParamVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter, _newParameter;
            public ReplaceParamVisitor(ParameterExpression oldP, ParameterExpression newP)
                => (_oldParameter, _newParameter) = (oldP, newP);
            protected override Expression VisitParameter(ParameterExpression node)
                => node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }

}
