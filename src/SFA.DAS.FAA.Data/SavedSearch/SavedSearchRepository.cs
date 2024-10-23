using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Data.SavedSearch
{
    public interface ISavedSearchRepository
    {
        Task<List<SavedSearchEntity>> GetAll(DateTime dateFilter, CancellationToken token);
    }

    public class SavedSearchRepository(IFindApprenticeshipsDataContext dataContext) : ISavedSearchRepository
    {
        public async Task<List<SavedSearchEntity>> GetAll(DateTime dateFilter, CancellationToken token = default)
        {
            var savedSearches = await dataContext
                .SavedSearchEntities
                .Where(fil => fil.LastRunDate == null || fil.LastRunDate > dateFilter)
                .ToListAsync(token);

            return savedSearches;
        }
    }
}
