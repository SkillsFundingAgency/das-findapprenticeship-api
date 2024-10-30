using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.SavedSearch;

public interface ISavedSearchesRepository
{
    Task<SavedSearchEntity> Upsert(SavedSearchEntity savedSearchEntity);
    
    Task<int> Count(Guid userReference);
}

public class SavedSearchesRepository(IFindApprenticeshipsDataContext dataContext): ISavedSearchesRepository
{
    public async Task<SavedSearchEntity> Upsert(SavedSearchEntity savedSearchEntity)
    {
        var savedSearch = await dataContext.SavedSearchEntities.SingleOrDefaultAsync(x => x.Id == savedSearchEntity.Id);

        if (savedSearch == null)
        {
            await dataContext.SavedSearchEntities.AddAsync(savedSearchEntity);
            await dataContext.SaveChangesAsync();
            return savedSearchEntity;
        }
        
        savedSearch.DateCreated = savedSearchEntity.DateCreated;
        savedSearch.LastRunDate = savedSearchEntity.LastRunDate;
        savedSearch.EmailLastSendDate = savedSearchEntity.EmailLastSendDate;
        savedSearch.SearchParameters = savedSearchEntity.SearchParameters;
        
        await dataContext.SaveChangesAsync();
        return savedSearch;
    }

    public async Task<int> Count(Guid userReference)
    {
        return await dataContext.SavedSearchEntities.CountAsync(x => x.UserRef == userReference);
    }
}