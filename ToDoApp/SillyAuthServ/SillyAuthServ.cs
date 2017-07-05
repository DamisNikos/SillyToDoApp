using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using System.ServiceModel;

namespace SillyAuthServ
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SillyAuthServ : StatefulService, ISillyAuthServ
    {
        public SillyAuthServ(StatefulServiceContext context)
            : base(context)
        { }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            UserEntry user1 = new UserEntry("user1", "pass1");
            UserEntry user2 = new UserEntry("user2", "pass2");
            UserEntry user3 = new UserEntry("user3", "pass3");
            var credential = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UserEntry>>("userscredentials");
            using (var tx = this.StateManager.CreateTransaction())
            {

                await credential.AddOrUpdateAsync(tx, user1.userID, user1, (k, v) => user1);
                await credential.AddOrUpdateAsync(tx, user2.userID, user2, (k, v) => user2);
                await credential.AddOrUpdateAsync(tx, user3.userID, user3, (k, v) => user3);
                await tx.CommitAsync();
            }

            var result = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UserEntry>>("userscredentials");
            using (var tx = this.StateManager.CreateTransaction())
            {
                var items = await result.CreateEnumerableAsync(tx);
                using (var e = items.GetAsyncEnumerator())
                {
                    while (await e.MoveNextAsync(new CancellationToken()).ConfigureAwait(false))
                    {
                        ServiceEventSource.Current.ServiceMessage(this.Context, "User:{0} = Password:{1}",
                           e.Current.Value.userID, e.Current.Value.password);
                    }
                }

            }
        }
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {

            return new[]
            {
                new ServiceReplicaListener(
                        c  => new WcfCommunicationListener<ISillyAuthServ>(
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
        public async Task<bool> AuthenticateUser(UserEntry entry)
        {
            bool auth = false;
            var result = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, UserEntry>>("userscredentials");
            using (var tx = this.StateManager.CreateTransaction())
            {
                var items = await result.CreateEnumerableAsync(tx);
                using (var e = items.GetAsyncEnumerator())
                {
                    while (await e.MoveNextAsync(new CancellationToken()).ConfigureAwait(false))
                    {
                        if (entry.userID.Equals(e.Current.Value.userID) && entry.password.Equals(e.Current.Value.password)) {
                            auth = true;
                        }
                    }
                }

            }
            return auth; 
        }
    }
}
