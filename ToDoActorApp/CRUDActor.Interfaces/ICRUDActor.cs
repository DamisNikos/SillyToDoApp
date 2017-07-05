using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace CRUDActor.Interfaces
{
    public class TaskEntry
    {
        public string taskDescr { get; set; }
        public string userID { get; set; }

        public TaskEntry() { }
        public TaskEntry(string _taskDescr, string _userID)
        {
            taskDescr = _taskDescr;
            userID = _userID;
        }
    }
    public interface ICRUDActor : IActor
    {
        Task<bool> Create(TaskEntry entry);
        Task<List<TaskEntry>> Read(string user);
        Task<bool> Update(TaskEntry entry_old, TaskEntry entry_new);
        Task<bool> Delete(TaskEntry entry);
    }
}
