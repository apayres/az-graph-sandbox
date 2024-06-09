namespace GraphSandbox.Web.Models
{
    public class CalendarEventModel
    {
        public string Id { set; get; }

        public string Title { set; get; }

        public DateTime? StartDateTime { set; get; }

        public DateTime? EndDateTime { set; get; }

        public string TimeZone { set; get; }
    }
}
