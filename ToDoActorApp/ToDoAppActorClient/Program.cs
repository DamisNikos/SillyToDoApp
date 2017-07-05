using AuthActor.Interfaces;
using CRUDActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAppActorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorId actorid = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IAuthActor>(actorid, "fabric:/ToDoActorApp");
            var CRUDActor = ActorProxy.Create<ICRUDActor>(ActorId.CreateRandom(), "fabric:/ToDoActorApp");

            //========================
            string username;
            string password;
            Boolean auth;
            do
            {
                Console.WriteLine("Dwse username: ");
                username = Console.ReadLine();
                Console.WriteLine("Dwse password: ");
                password = Console.ReadLine();
                UserEntry entry = new UserEntry(username.ToString(), password.ToString());
                auth = actor.AuthenticateUser(actorid, new UserEntry(username, password)).Result;
                Console.WriteLine(auth.ToString());
                if (auth.ToString().Equals("False"))
                {
                    Console.WriteLine("To username i to password einai la8os. Ksanaprospa8ise i dwse ws username exit gia eksodo");
                }
            } while (auth.ToString().Equals("False") && !username.Equals("exit"));
            if (auth)
            {
                //Create an entry fucntion test
                Console.WriteLine("Adding a new entry sxoli!");
                CRUDActor.Create(new TaskEntry("sxoli", username));
                CRUDActor.Read(username).Wait();
                List<TaskEntry> myEntries = CRUDActor.Read(username).Result;
                foreach (var entry in myEntries)
                {
                    Console.WriteLine(entry.taskDescr + "---" + entry.userID);
                }
                Console.ReadKey();
                //Update an entry Function test
                Console.WriteLine("Updating the previous added entry 'sxoli' to 'ergastirio'");
                CRUDActor.Update(new TaskEntry("sxoli", username), new TaskEntry("ergastirio", username)).Wait();
                myEntries = CRUDActor.Read(username).Result;
                foreach (var entry in myEntries)
                {
                    Console.WriteLine(entry.taskDescr + "---" + entry.userID);
                }
                Console.ReadKey();
                //Delete an entry function test
                Console.WriteLine("Deleting the previous entry 'ergastirio'");
                CRUDActor.Delete(new TaskEntry("ergastirio", username)).Wait();
                myEntries = CRUDActor.Read(username).Result;
                foreach (var entry in myEntries)
                {
                    Console.WriteLine(entry.taskDescr + "---" + entry.userID);
                }
                Console.WriteLine("enter a line to terminate");
                Console.ReadLine();
            }
        }

    }
}

