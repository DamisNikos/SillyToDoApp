using Common;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppClient
{

    public class CRUDClient : ServicePartitionClient<WcfCommunicationClient<ICRUDService>>,
ICRUDService
    {
        public CRUDClient(WcfCommunicationClientFactory<ICRUDService> clientFactory,
             Uri serviceName)
               : base(clientFactory, serviceName)
        {
        }
        public Task<bool> Create(TaskEntry entry)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.Create(entry));
        }

        public Task<bool> Delete(TaskEntry entry)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.Delete(entry));
        }

        public Task<List<TaskEntry>> Read(string user)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.Read(user));
        }
        public Task<bool> Update(TaskEntry entry_old, TaskEntry entry_new)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.Update(entry_old, entry_new));
        }
    }
}

