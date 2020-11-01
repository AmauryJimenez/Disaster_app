using Models.Disasters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Services
{
    public interface IDisasterInfoPersister
    {
        void SaveDisasters(IList<Disaster> disasters);
    }
}
