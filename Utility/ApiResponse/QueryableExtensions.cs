using System.Linq.Expressions;

namespace ITKANSys_api.Utility.ApiResponse
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Enum de gestion des ordres de tri
        /// </summary>
        public enum Order
        {
            /// <summary>
            /// Tri ascendant
            /// </summary>
            Asc,

            /// <summary>
            /// Tri descendant
            /// </summary>
            Desc
        }

        /// <summary>
        /// fonction pour le tri dynamique
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderByMember"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderByMember,
            Order direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));
            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == Order.Asc ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }
    }
}
