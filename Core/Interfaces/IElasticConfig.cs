using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace Core.Interfaces
{
    public interface IElasticConfig
    {
        ElasticClient GetClient();
    }
}