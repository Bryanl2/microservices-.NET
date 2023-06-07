using AutoMapper;
using Data;
using Dtos;
using Microsoft.AspNetCore.Mvc;
using Models;
using PlatformService.AsyncDataServices;
using PlatformService.Dtos;
using SyncDataServices;
using System;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Platformscontroller : ControllerBase
    {
        private IPlatformRepo _repository;
        private IMapper _mapper;
        private ICommandDataClient _commandDataClient;
        private IMessageBusClient _messageBusClient;

        public Platformscontroller(ICommandDataClient commandDataClient,IPlatformRepo repository,IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>>GetPlatforms()
        {
            System.Console.WriteLine("-->Getting platforms...");
            var platformItem = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}",Name="GetPlatformById")]
        public ActionResult<PlatformReadDto>GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>>CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            //Send sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"------> Could not send Synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"------> Could not send Asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById),new {Id=platformReadDto.Id},platformReadDto);
        }
    }
}