using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private ICommandRepo _repository;
        private IMapper _mapper;

        public PlatformsController(ICommandRepo repos,IMapper mapper)
        {
            _repository = repos;
            _mapper = mapper;
        
        }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>>GetPlatforms()
    {
        Console.WriteLine("---> Getting platforms from CommandsService");
        var platformItems = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("----> Inbound POST # command service");
        return Ok("Inbound test of from Platforms Controller");
    }
}
}