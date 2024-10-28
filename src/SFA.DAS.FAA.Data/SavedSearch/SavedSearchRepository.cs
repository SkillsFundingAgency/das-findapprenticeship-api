using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.SavedSearch
{
    public interface ISavedSearchRepository
    {
        Task<SavedSearchEntity> GetById(Guid id, CancellationToken token);
        Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token);
        Task Update(SavedSearchEntity savedSearch, CancellationToken token);
    }

    public class SavedSearchRepository(IFindApprenticeshipsDataContext dataContext) : ISavedSearchRepository
    {
        public async Task<SavedSearchEntity> GetById(Guid id, CancellationToken token)
        {
            return await dataContext.SavedSearchEntities.FirstOrDefaultAsync(fil => fil.Id == id, token);
        }

        public async Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token = default)
        {
            // Query
            var query = dataContext
                .SavedSearchEntities
                .AsNoTracking()
                .Where(fil => fil.LastRunDate == null || fil.LastRunDate > dateFilter)
                .OrderByDescending(fil => fil.DateCreated)
                .ThenBy(fil => fil.UserRef);

            // Count
            var count = await query.CountAsync(token);

            // Pagination
            query = (IOrderedQueryable<SavedSearchEntity>)query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            
            return await PaginatedList<SavedSearchEntity?>.CreateAsync(query, count, pageNumber, pageSize);
        }
        
        public async Task Update(SavedSearchEntity savedSearch, CancellationToken token = default)
        {
            dataContext.SavedSearchEntities.Update(savedSearch);
            await dataContext.SaveChangesAsync(token);
        }
    }
}