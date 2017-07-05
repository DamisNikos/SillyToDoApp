using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using CRUDActor.Interfaces;

namespace CRUDActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class CRUDActor : Actor, ICRUDActor
    {
         public CRUDActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task<bool>Create(TaskEntry entry)
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
    }
}
