using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Nest;

namespace Core.Interfaces
{
    public interface IAppService
    {
        Task<BulkResponse> IndexManagementAsync(IEnumerable<Mgmt> mgmt);
        Task<BulkResponse> IndexPropertyAsync(IEnumerable<Property> properties);
        Task<string> SearchAsync(SearchQuery searchQuery);
    }
}