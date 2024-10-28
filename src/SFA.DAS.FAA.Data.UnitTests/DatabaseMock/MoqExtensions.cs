using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq.Language.Flow;

namespace SFA.DAS.FAA.Data.UnitTests.DatabaseMock

{
    public static class MoqExtensions
    {
        public static Mock<IQueryable<TEntity>> BuildMock<TEntity>(this IQueryable<TEntity> data) where TEntity : class
        {
            var mock = new Mock<IQueryable<TEntity>>();
            var enumerable = new TestAsyncEnumerableEfCore<TEntity>(data);
            mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
            mock.ConfigureQueryableCalls(enumerable, data);
            return mock;
        }

        public static Mock<DbSet<TEntity>> BuildMockDbSet<TEntity>(this IQueryable<TEntity> data) where TEntity : class
        {
            var mock = new Mock<DbSet<TEntity>>();
            var enumerable = new TestAsyncEnumerableEfCore<TEntity>(data);
            mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
            mock.As<IQueryable<TEntity>>().ConfigureQueryableCalls(enumerable, data);
            mock.ConfigureDbSetCalls(data);
            return mock;
        }

        public static DbSet<TEntity> BuildDbSet<TEntity>(this IEnumerable<TEntity> data) where TEntity : class
        {
            return BuildMockDbSet(data.AsQueryable()).Object;
        }

        public static IReturnsResult<IFindApprenticeshipsDataContext> ReturnsDbSet<TEntity>(
            this ISetup<IFindApprenticeshipsDataContext, DbSet<TEntity>> setupResult,
            IEnumerable<TEntity> entities) where TEntity : class
        {
            return setupResult.Returns(entities.BuildDbSet());
        }

        private static void ConfigureDbSetCalls<TEntity>(this Mock<DbSet<TEntity>> mock, IQueryable<TEntity> data)
            where TEntity : class
        {
            mock.Setup(m => m.AsQueryable()).Returns(mock.Object);
            mock.Setup(m => m.AsAsyncEnumerable()).Returns(CreateAsyncMock(data));
        }

        private static void ConfigureQueryableCalls<TEntity>(
            this Mock<IQueryable<TEntity>> mock,
            IQueryProvider queryProvider,
            IQueryable<TEntity> data) where TEntity : class
        {
            mock.Setup(m => m.Provider).Returns(queryProvider);
            mock.Setup(m => m.Expression).Returns(data?.Expression);
            mock.Setup(m => m.ElementType).Returns(data?.ElementType);
            mock.Setup(m => m.GetEnumerator()).Returns(() => data?.GetEnumerator());
        }

        private static void ConfigureAsyncEnumerableCalls<TEntity>(
            this Mock<IAsyncEnumerable<TEntity>> mock,
            IAsyncEnumerable<TEntity> enumerable)
        {
            mock.Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(() => enumerable.GetAsyncEnumerator());
        }

        private static async IAsyncEnumerable<TEntity> CreateAsyncMock<TEntity>(IEnumerable<TEntity> data)
            where TEntity : class
        {
            foreach (var entity in data)
            {
                yield return entity;
            }

            await Task.CompletedTask;
        }
    }
}
