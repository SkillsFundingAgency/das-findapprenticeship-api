using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.SavedSearch
{
    public interface ISavedSearchRepository
    {
        Task<SavedSearchEntity> Get(Guid id, CancellationToken token);
        Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token);
        Task Save(SavedSearchEntity savedSearch, CancellationToken token);
        Task BulkSave(IEnumerable<SavedSearchEntity> savedSearchEntities, CancellationToken token);
    }

    public class SavedSearchRepository(IFindApprenticeshipsDataContext dataContext) : ISavedSearchRepository
    {
        public async Task<SavedSearchEntity> Get(Guid id, CancellationToken token)
        {
            return await dataContext.SavedSearchEntities.FirstOrDefaultAsync(fil => fil.Id == id, token);
        }

        public async Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token = default)
        {
            // Query
            var query = dataContext
                .SavedSearchEntities
                .AsNoTracking()
                .Where(fil => fil.LastRunDate == null || fil.LastRunDate > dateFilter);

            // Count
            var count = await query.CountAsync(token);

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            
            return await PaginatedList<SavedSearchEntity?>.CreateAsync(query, count, pageNumber, pageSize);
        }
        
        public async Task Save(SavedSearchEntity savedSearch, CancellationToken token = default)
        {
            dataContext.SavedSearchEntities.Update(savedSearch);
            await dataContext.SaveChangesAsync(token);
        }

        public async Task BulkSave(IEnumerable<SavedSearchEntity> savedSearchEntities, CancellationToken token)
        {
            var context = dataContext.GetContext();
            await using var transaction = await context.Database.BeginTransactionAsync(token);
            await context.BulkUpdateAsync(savedSearchEntities, cancellationToken: token);
            await transaction.CommitAsync(token);
        }
    }
}