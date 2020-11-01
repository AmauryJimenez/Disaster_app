using Models.Disasters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Services
{
    public interface IDisasterInfoProvider : IDisposable
    {
        IList<Disaster> RequestDisasters(DisasterType[] requestDisasterTypes);
    }
}
