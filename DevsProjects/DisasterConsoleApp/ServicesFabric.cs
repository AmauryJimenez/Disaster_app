using Commons.Services;
using Contracts.Services;
using Models;
using ReliefwebApi.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace DisasterConsoleApp
{
    public class ServicesFabric : IServicesFabric
    {
        private Container _container;
        private IDisasterInfoPersister _disasterInfoPersister;

        public ServicesFabric(Container container, IDisasterInfoPersister disasterInfoPersister)
        {
            _disasterInfoPersister = disasterInfoPersister;
            _container = container;
        }


        public IDisasterInfoProvider CreateDisasterInfoProvider(DisasterInfoProvider provider)
        {
            IDisasterInfoProvider result;

            if (provider == DisasterInfoProvider.Reliefweb)
                result = _container.GetInstance <ReliefwebServiceProvider>();
            else if (provider == DisasterInfoProvider.FileSystem)
                result = _container.GetInstance <FileSystemServiceProvider>();
            else 
                throw new InvalidOperationException("Cannot create a provider for "+ provider.ToString());

            return result;
        }

        public IDisasterInfoPersister CreateIDisasterInfoPersister()
        {
            return _disasterInfoPersister;
        }
    }
}
