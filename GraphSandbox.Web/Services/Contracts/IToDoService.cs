using GraphSandbox.Web.Models;

namespace GraphSandbox.Web.Services.Contracts
{
    public interface IToDoService
    {
        Task<ToDoModel> LoadTaskLists(string? selectedTaskListID);

        Task<ToDoModel> CreateTask(ToDoModel model);

        void UpdateTaskStatus(string taskListID, string taskID, bool complete);
    }
}