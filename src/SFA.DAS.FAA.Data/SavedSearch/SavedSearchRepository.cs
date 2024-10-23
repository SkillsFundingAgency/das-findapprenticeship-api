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
        Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token);
    }

    public class SavedSearchRepository(IFindApprenticeshipsDataContext dataContext) : ISavedSearchRepository
    {
        public async Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token = default)
        {
            // Query
            var query = dataContext
                .SavedSearchEntities
                .Where(fil => fil.LastRunDate == null || fil.LastRunDate > dateFilter);

            // Count
            var count = await query.CountAsync(token);

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            
            return await PaginatedList<SavedSearchEntity?>.CreateAsync(query, count, pageNumber, pageSize);
        }
    }
}