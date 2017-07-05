using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common
{
    public class UserEntry
    {
        public string userID { get; set; }
        public string password { get; set; }

        public UserEntry() { }
        public UserEntry(string _userID, string _password) {
            userID = _userID;
            password = _password;
        }
    }
    public class TaskEntry
    {
        public string taskDescr {get; set;}
        public string userID { get; set; }

        public TaskEntry() { }
        public TaskEntry(string _taskDescr, string _userID) {
            taskDescr = _taskDescr;
            userID = _userID;
        }
    }

    [ServiceContract]
    public interface ISillyAuthServ
    {
        [OperationContract]
        Task<bool> AuthenticateUser(UserEntry entry);
    }

    [ServiceContract]
    public interface ICRUDService
    {
        [OperationContract]
        Task<bool> Create(TaskEntry entry);
        [OperationContract]
        Task<List<TaskEntry>>Read(string user);
        [OperationContract]
        Task<bool> Update(TaskEntry entry_old, TaskEntry entry_new);
        [OperationContract]
        Task<bool> Delete(TaskEntry entry);
    }
}
