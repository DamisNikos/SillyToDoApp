using Common;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string username;
            string password;
            Boolean auth;
            //comm client with Auth service
            Uri AuthServiceName = new Uri("fabric:/ToDoApp/SillyAuthServ");
            ServicePartitionResolver AuthserviceResolver = new ServicePartitionResolver(() => new FabricClient());
            NetTcpBinding Authbinding = CreateClientConnectionBinding();
            Client toDoClient = new Client(new WcfCommunicationClientFactory<ISillyAuthServ>(
                clientBinding: CreateClientConnectionBinding())
                , AuthServiceName
                );
            //comm client with CRUD service
            Uri CRUDServiceName = new Uri("fabric:/ToDoApp/CRUDService");
            ServicePartitionResolver CRUDserviceResolver = new ServicePartitionResolver(() => new FabricClient());
            NetTcpBinding CRUDbinding = CreateClientConnectionBinding();
            CRUDClient toDoCRUDClient = new CRUDClient(new WcfCommunicationClientFactory<ICRUDService>(
                clientBinding: CreateClientConnectionBinding())
                , CRUDServiceName
                );
            //apo dw kai katw diko m logic
            do
            {
                Console.WriteLine("Dwse username: ");
                username = Console.ReadLine();
                Console.WriteLine("Dwse password: ");
                password = Console.ReadLine();
                UserEntry entry = new UserEntry(username.ToString(), password.ToString());
                auth = toDoClient.AuthenticateUser(entry).Result;
                Console.WriteLine(auth.ToString());
                if (auth.ToString().Equals("False")) {
                    Console.WriteLine("To username i to password einai la8os. Ksanaprospa8ise i dwse ws username exit gia eksodo");
                }
            } while(auth.ToString().Equals("False") && !username.Equals("exit"));
            if (auth) {
                //Create an entry fucntion test
                Console.WriteLine("Adding a new entry sxoli!");
                toDoCRUDClient.Create(new TaskEntry("sxoli", username));
                toDoCRUDClient.Read(username).Wait();
                List<TaskEntry> myEntries = toDoCRUDClient.Read(username).Result;
                foreach(var entry in myEntries)
                {
                    Console.WriteLine(entry.taskDescr +"---"+ entry.userID);
                }
                Console.ReadKey();
                //Update an entry Function test
                Console.WriteLine("Updating the previous added entry 'sxoli' to 'ergastirio'");
                toDoCRUDClient.Update(new TaskEntry("sxoli", username), new TaskEntry("ergastirio", username)).Wait();
                myEntries = toDoCRUDClient.Read(username).Result;
                foreach (var entry in myEntries)
                {
                    Console.WriteLine(entry.taskDescr + "---" + entry.userID);
                }
                Console.ReadKey();
                //Delete an entry function test
                Console.WriteLine("Deleting the previous entry 'ergastirio'");
                toDoCRUDClient.Delete(new TaskEntry("ergastirio", username)).Wait();
                myEntries = toDoCRUDClient.Read(username).Result;
                foreach (var entry in myEntries)
                {
                    Console.WriteLine(entry.taskDescr + "---" + entry.userID);
                }
                Console.WriteLine("enter a line to terminate");
                Console.ReadLine();
            }
        }


        private static NetTcpBinding CreateClientConnectionBinding()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
            {
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                OpenTimeout = TimeSpan.FromSeconds(5),
                CloseTimeout = TimeSpan.FromSeconds(5),
                MaxReceivedMessageSize = 1024 * 1024
            };
            binding.MaxBufferSize = (int)binding.MaxReceivedMessageSize;
            binding.MaxBufferPoolSize = Environment.ProcessorCount * binding.MaxReceivedMessageSize;
            return binding;
        }
    }
}
