namespace ERestaurant.Infrastructure.HelperClass.Pagination
{
    public static class PagingHelper
    {
        public readonly record struct PagingResult(
            int Page, int PageSize, int Skip, int Take, int TotalCount, int TotalPages);

        private const int maxPageSize = 15;

        public static PagingResult Compute(
            int? pageNumber, int? pageSize, int totalCount,
            int defaultPageSize = 10, int minPageSize = 1, int maxPageSize = maxPageSize)
        {
            if (totalCount < 0) totalCount = 0;

            var page = pageNumber.GetValueOrDefault(1);
            if (page < 1) page = 1;

            var size = pageSize.GetValueOrDefault(defaultPageSize);
            if (size < minPageSize) size = minPageSize;
            if (size > maxPageSize) size = maxPageSize;

            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)size));
            if (page > totalPages) page = totalPages;

            var skipLong = (long)(page - 1) * size;
            var skip = skipLong > int.MaxValue ? int.MaxValue : (int)skipLong;
            var take = size;

            return new PagingResult(page, size, skip, take, totalCount, totalPages);
        }
    }

}
