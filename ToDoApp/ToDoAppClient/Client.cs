using Common;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppClient
{
    public class Client : ServicePartitionClient<WcfCommunicationClient<ISillyAuthServ>>, ISillyAuthServ
    {
        public Client(WcfCommunicationClientFactory<ISillyAuthServ> clientFactory,
             Uri serviceName)
               : base(clientFactory, serviceName, new ServicePartitionKey(1))
        {
        }

        public Task<bool> AuthenticateUser(UserEntry entry)
        {
            return this.InvokeWithRetryAsync(client => client.Channel.AuthenticateUser(entry));
        }
    }

}

