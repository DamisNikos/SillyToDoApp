using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using System.ServiceModel;

namespace CRUDService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CRUDService : StatelessService, ICRUDService
    {
        public CRUDService(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<bool> Create(TaskEntry entry)
        {
            DatabaseHelper databaseHelper = new DatabaseHelper();
            return databaseHelper.Create(entry);
        }

        public async Task<bool> Delete(TaskEntry entry)
        {
            DatabaseHelper databaseHelper = new DatabaseHelper();
            return databaseHelper.Delete(entry);
        }

        public async Task<List<TaskEntry>> Read(string user)
        {
            DatabaseHelper databaseHelper = new DatabaseHelper();
            return databaseHelper.Read(user);
        }

        public async Task<bool> Update(TaskEntry entry_old, TaskEntry entry_new)
        {
            DatabaseHelper databaseHelper = new DatabaseHelper();
            return databaseHelper.Update(entry_old, entry_new);
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(
                        c  => new WcfCommunicationListener<ICRUDService>(
                            c,
                            this,
                            CreateListenBinding(),
                            endpointResourceName: "ServiceEndpoint"
                            )
                )
           };
        }
        private NetTcpBinding CreateListenBinding()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
            {
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.FromSeconds(5),
                CloseTimeout = TimeSpan.FromSeconds(5),
                MaxConnections = int.MaxValue,
                MaxReceivedMessageSize = 1024 * 1024
            };
            binding.MaxBufferSize = (int)binding.MaxReceivedMessageSize;
            binding.MaxBufferPoolSize = Environment.ProcessorCount * binding.MaxReceivedMessageSize;

            return binding;
        }
    }

}

