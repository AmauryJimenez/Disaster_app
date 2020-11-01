using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace Contracts.Services
{
    public interface IServicesFabric
    {
        public IDisasterInfoPersister CreateIDisasterInfoPersister();

        public IDisasterInfoProvider CreateDisasterInfoProvider(DisasterInfoProvider provider);


    }
}
