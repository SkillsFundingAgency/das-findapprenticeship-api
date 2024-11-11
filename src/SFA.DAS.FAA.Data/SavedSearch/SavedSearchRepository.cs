using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.SavedSearch;

public interface ISavedSearchRepository
{
    Task<SavedSearchEntity> GetById(Guid id, CancellationToken token);
    Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token);
    Task<SavedSearchEntity> Get(Guid userReference, Guid id, CancellationToken token);
    Task<List<SavedSearchEntity>> GetByUserReference(Guid userReference, CancellationToken token);
    Task Update(SavedSearchEntity savedSearch, CancellationToken token);
    Task<SavedSearchEntity> Upsert(SavedSearchEntity savedSearchEntity, CancellationToken token);
    Task<int> Count(Guid userReference);
    Task Delete(Guid userReference, Guid id, CancellationToken token);
    Task Delete(Guid id, CancellationToken token);
    
}

public class SavedSearchRepository(IFindApprenticeshipsDataContext dataContext) : ISavedSearchRepository
{
    public async Task<SavedSearchEntity> GetById(Guid id, CancellationToken token)
    {
        return await dataContext.SavedSearchEntities.FirstOrDefaultAsync(fil => fil.Id == id, token);
    }

    public async Task<PaginatedList<SavedSearchEntity>> GetAll(DateTime dateFilter, int pageNumber, int pageSize, CancellationToken token)
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

    public async Task<SavedSearchEntity> Get(Guid userReference, Guid id, CancellationToken token)
    {
        return await dataContext.SavedSearchEntities
            .Where(x => x.UserRef == userReference && x.Id == id)
            .FirstOrDefaultAsync(token);
    }

    public async Task<List<SavedSearchEntity>> GetByUserReference(Guid userReference, CancellationToken token)
    {
        return await dataContext.SavedSearchEntities
            .Where(x => x.UserRef == userReference)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(token);
    }
        
    public async Task Update(SavedSearchEntity savedSearch, CancellationToken token = default)
    {
        dataContext.SavedSearchEntities.Update(savedSearch);
        await dataContext.SaveChangesAsync(token);
    }
        
    public async Task<SavedSearchEntity> Upsert(SavedSearchEntity savedSearchEntity, CancellationToken token)
    {
        var savedSearch = await dataContext.SavedSearchEntities.SingleOrDefaultAsync(x => x.Id == savedSearchEntity.Id, token);

        if (savedSearch == null)
        {
            savedSearchEntity.DateCreated = DateTime.UtcNow;
            await dataContext.SavedSearchEntities.AddAsync(savedSearchEntity, token);
            await dataContext.SaveChangesAsync(token);
            return savedSearchEntity;
        }
        
        savedSearch.LastRunDate = savedSearchEntity.LastRunDate;
        savedSearch.EmailLastSendDate = savedSearchEntity.EmailLastSendDate;
        savedSearch.SearchParameters = savedSearchEntity.SearchParameters;
        
        await dataContext.SaveChangesAsync(token);
        return savedSearch;
    }

    public async Task<int> Count(Guid userReference)
    {
        return await dataContext.SavedSearchEntities.CountAsync(x => x.UserRef == userReference);
    }

    public async Task Delete(Guid userReference, Guid id, CancellationToken token)
    {
        var savedSearch = await dataContext.SavedSearchEntities.SingleOrDefaultAsync(x => x.Id == id && x.UserRef == userReference, token);

        if (savedSearch == null)
        {
            return;
        }
        
        dataContext.SavedSearchEntities.Remove(savedSearch);
        await dataContext.SaveChangesAsync(token);
    }

    public async Task Delete(Guid id, CancellationToken token)
    {
        var savedSearch = await dataContext.SavedSearchEntities.SingleOrDefaultAsync(x => x.Id == id, token);

        if (savedSearch == null)
        {
            return;
        }
        
        dataContext.SavedSearchEntities.Remove(savedSearch);
        await dataContext.SaveChangesAsync(token);
    }
}