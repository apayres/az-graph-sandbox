namespace GraphSandbox.Web.Models
{
    public class CalendarModel
    {
        public List<CalendarEventModel> Events { get; set; }

        public CalendarEventModel TargetEvent { set; get; }
    }
}
