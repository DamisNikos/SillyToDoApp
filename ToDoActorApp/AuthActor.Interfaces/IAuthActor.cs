using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace AuthActor.Interfaces
{
    public class UserEntry
    {
        public string userID { get; set; }
        public string password { get; set; }

        public UserEntry() { }
        public UserEntry(string _userID, string _password)
        {
            userID = _userID;
            password = _password;
        }
    }

    public interface IAuthActor : IActor
    {
        Task<bool> AuthenticateUser(ActorId ID,UserEntry entry);
        Task<List<UserEntry>> GetUsers();
    }
}
