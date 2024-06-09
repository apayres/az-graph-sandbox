using GraphSandbox.Web.Models;
using GraphSandbox.Web.Services.Contracts;
using Microsoft.Graph;
using TaskStatus = Microsoft.Graph.TaskStatus;

namespace GraphSandbox.Web.Services
{
    public class ToDoService : IToDoService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public ToDoService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<ToDoModel> LoadTaskLists(string? selectedTaskListID)
        {
            var taskLists = await _graphServiceClient.Me.Todo.Lists.Request().GetAsync();

            var model = new ToDoModel()
            {
                TaskLists = taskLists.Where(x => x.WellknownListName?.ToString()?.ToLower() != "flaggedemails")
                    .ToDictionary(x => x.Id, y => y.DisplayName)
            };

            if (!string.IsNullOrWhiteSpace(selectedTaskListID))
            {
                var tasks = await _graphServiceClient.Me.Todo.Lists[selectedTaskListID].Tasks.Request().GetAsync();
                model.Tasks = tasks.Select(x => new ToDoItemModel()
                {
                    Completed = x.Status == TaskStatus.Completed,
                    Id = x.Id,
                    Title    = x.Title
                }).ToList();

                model.SelectedTaskListID = selectedTaskListID;
            }

            return model;
        }

        public async Task<ToDoModel> CreateTask(ToDoModel model)
        {
            await _graphServiceClient.Me.Todo.Lists[model.SelectedTaskListID].Tasks.Request().AddAsync(new TodoTask()
            {
                Title = model.ToDoItem
            });

            return await LoadTaskLists(model.SelectedTaskListID);
        }

        public async void UpdateTaskStatus(string taskListID, string taskID, bool complete)
        {
            await _graphServiceClient.Me.Todo.Lists[taskListID].Tasks[taskID].Request().UpdateAsync(new TodoTask()
            {
                Id = taskID,
                Status = complete ? TaskStatus.Completed : TaskStatus.NotStarted
            });
        }

    }
}
