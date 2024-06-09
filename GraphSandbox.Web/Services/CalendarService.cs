using GraphSandbox.Web.Models;
using GraphSandbox.Web.Services.Contracts;
using Microsoft.Graph;
using Microsoft.Graph.Extensions;

namespace GraphSandbox.Web.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public CalendarService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<CalendarModel> LoadCalendarEvents(string? selectedCalendarEvent)
        {
            var calendarEvents = await _graphServiceClient.Me.Events.Request().GetAsync();
            var model = new CalendarModel()
            {
                Events = calendarEvents.Select(x => new CalendarEventModel()
                {
                    EndDateTime = x.End.ToDateTime().ToLocalTime(),
                    Id = x.Id,
                    StartDateTime = x.Start.ToDateTime().ToLocalTime(),
                    TimeZone = x.OriginalStartTimeZone,
                    Title = x.Subject
                }).Where(x => x.StartDateTime >= DateTime.Today).ToList()
            };

            if (!string.IsNullOrWhiteSpace(selectedCalendarEvent))
            {
                var calendarEvent = await _graphServiceClient.Me.Events[selectedCalendarEvent].Request().GetAsync();
                model.TargetEvent = new CalendarEventModel()
                {
                    EndDateTime = calendarEvent.End.ToDateTime().ToLocalTime(),
                    Id = calendarEvent.Id,
                    StartDateTime = calendarEvent.Start.ToDateTime().ToLocalTime(),
                    TimeZone = calendarEvent.OriginalStartTimeZone,
                    Title = calendarEvent.Subject
                };
            }
            else
            {
                model.TargetEvent = new CalendarEventModel();
            }

            return model;
        }

        public async Task<CalendarModel> SaveCalendarEvent(CalendarModel model)
        {
            if (model.TargetEvent == null)
            {
                throw new Exception("Target event is null");
            }

            if (!string.IsNullOrWhiteSpace(model.TargetEvent.Id))
            {
                await _graphServiceClient.Me.Events[model.TargetEvent.Id].Request().UpdateAsync(new Event()
                {
                    Subject = model.TargetEvent.Title,
                    Start = model.TargetEvent.StartDateTime.Value.ToUniversalTime().ToDateTimeTimeZone(),
                    End = model.TargetEvent.EndDateTime.Value.ToUniversalTime().ToDateTimeTimeZone(),
                    Id = model.TargetEvent.Id
                });

                return await LoadCalendarEvents(model.TargetEvent.Id);
            }

            await _graphServiceClient.Me.Events.Request().AddAsync(new Event()
            {
                Subject = model.TargetEvent.Title,
                Start = model.TargetEvent.StartDateTime.Value.ToUniversalTime().ToDateTimeTimeZone(),
                End = model.TargetEvent.EndDateTime.Value.ToUniversalTime().ToDateTimeTimeZone()
            });

            return await LoadCalendarEvents(model.TargetEvent.Id);
        }

        public async void DeleteCalendarEvent(string eventID)
        {
            await _graphServiceClient.Me.Events[eventID].Request().DeleteAsync();
        }
    }
}
