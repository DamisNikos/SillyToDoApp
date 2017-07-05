using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using AuthActor.Interfaces;
using System.Runtime.Serialization;

namespace AuthActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class AuthActor : Actor, IAuthActor
    {

        [DataContract]
        internal sealed class ActorState

        {
            [DataMember]
            public List<UserEntry> Users;

        }

        public AuthActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            List<UserEntry> entries = new List<UserEntry>();
            entries.Add(new UserEntry("user1", "pass1"));
            entries.Add(new UserEntry("user2", "pass2"));
            entries.Add(new UserEntry("user3", "pass3"));
            return this.StateManager.TryAddStateAsync("mystate", new ActorState
            {
                Users = new List<UserEntry>(entries)
            });
        }


        public async Task<bool> AuthenticateUser(ActorId ID,UserEntry entry)
        {
            bool auth = false;
            ActorState state = await this.StateManager.GetStateAsync<ActorState>("mystate");
            foreach (var user in state.Users) {
                if (user.userID.Equals(entry.userID) && user.password.Equals(entry.password)) {
                    auth = true;
                }
            }
            return auth;
        }

        public async Task<List<UserEntry>> GetUsers()
        {
            List<UserEntry> entries = new List<UserEntry>();
            ActorState state = await this.StateManager.GetStateAsync<ActorState>("mystate");

            foreach (var user in state.Users)
            {
                ActorEventSource.Current.ActorMessage(this, "User:{0}---Pass:{1}", user.userID, user.password);
                entries.Add(user);
            }
            return entries;
        }
    }
}
