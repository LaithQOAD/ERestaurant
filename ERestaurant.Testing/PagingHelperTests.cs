using ERestaurant.Infrastructure.HelperClass.Pagination;

namespace ERestaurant.Testing
{
    public class PagingHelperTests
    {
        [Fact]
        public void ComputePageSizeLimit()
        {

            var totalCount = 100;


            var result = PagingHelper.Compute(
                pageNumber: 1,
                pageSize: 1000,
                totalCount: totalCount,
                defaultPageSize: 10,
                minPageSize: 1,
                maxPageSize: 15
            );


            Assert.Equal(15, result.PageSize);
            Assert.Equal(1, result.Page);
            Assert.Equal(0, result.Skip);
            Assert.Equal(15, result.Take);
            Assert.Equal(totalCount, result.TotalCount);
            Assert.Equal((int)Math.Ceiling(totalCount / 15.0), result.TotalPages);
        }

        [Fact]
        public void ComputePageNumberLessThanZero()
        {

            var totalCount = 50;


            var result = PagingHelper.Compute(
                pageNumber: -5,
                pageSize: 5,
                totalCount: totalCount,
                defaultPageSize: 10,
                minPageSize: 1,
                maxPageSize: 15
            );


            Assert.Equal(1, result.Page);
            Assert.Equal(0, result.Skip);
            Assert.Equal(5, result.Take);
            Assert.Equal(totalCount, result.TotalCount);
            Assert.Equal((int)Math.Ceiling(totalCount / 5.0), result.TotalPages);
        }
    }
}
