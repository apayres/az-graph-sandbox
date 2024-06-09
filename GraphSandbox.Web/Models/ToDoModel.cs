namespace GraphSandbox.Web.Models
{
    public class ToDoModel
    {
        public Dictionary<string, string> TaskLists { get; set; }

        public string SelectedTaskListID { set; get; }

        public List<ToDoItemModel> Tasks { set; get; }

        public string ToDoItem { set; get; }
    }
}
