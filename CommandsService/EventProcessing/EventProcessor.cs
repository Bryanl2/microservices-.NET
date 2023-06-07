using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private IServiceScopeFactory _scopeFactory;
        private IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
                 Console.WriteLine("--> Determining Event");
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private void addPlatform(string platformPublishedMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedDto);
                if(!repo.ExternalPlatformExist(plat.ExternalID))
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                    Console.WriteLine("--> Platform added!");
                }
                else
                {
                    Console.WriteLine("--> Platform already exists...");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not add platform to DB {ex.Message}");
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            // var O = eventType.Event switch 
            // {
            //     "Platform_Published" =>  EventType.PlatformPublished,
            //     _ => EventType.PlatformPublished,
            // };

            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
            
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}