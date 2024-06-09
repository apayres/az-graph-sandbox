using GraphSandbox.Web.Models;

namespace GraphSandbox.Web.Services.Contracts
{
    public interface ICalendarService
    {
        Task<CalendarModel> LoadCalendarEvents(string? selectedCalendarEvent);
        Task<CalendarModel> SaveCalendarEvent(CalendarModel model);

        void DeleteCalendarEvent(string eventID);
    }
}