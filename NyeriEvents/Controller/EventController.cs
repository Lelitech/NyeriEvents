using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NyeriEvents.Entities;
using NyeriEvents.Requests;
using NyeriEvents.Responses;
using nyerievents.services.iservices;
using NyeriEvents.Services.IServices;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace NyeriEvents.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventController(IMapper mapper, IEventService Service)
        {
            _mapper = mapper;
            _eventService = Service;
        }

        [HttpPost]
        //add an event
        [Authorize]
        public async Task<ActionResult<string>> AddEvent(AddEvent addEvent)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == "Role").Value;
            if (!string.IsNullOrWhiteSpace(role) && role == "Admin")
            {
                var res = await _eventService.AddEvent(_mapper.Map<Event>(addEvent));
                return CreatedAtAction(nameof(AddEvent), res);
            }

            return BadRequest("You are not allowed to do that");

        }
    

        //get an event by location
        [HttpGet("Get An Event By Location")]
        public async Task<IActionResult> GetEventsByLocation([FromQuery] string? location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Location is required.");
                }

                var events = await _eventService.GetEventInLocation(location);

                if (events == null || !events.Any())
                {
                    return NotFound("No events found for the specified location.");
                }

                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //get all events
        [HttpGet("All Events")]
        public async Task<ActionResult<IEnumerable<EventResponse>>> GetAllEvents()
        {
            var response = await _eventService.GetAllEvents();
            var events = _mapper.Map<IEnumerable<EventResponse>>(response);
            return Ok(events);
        }

        //get an event by id
        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponse>> GetOneEventbyId(Guid id)
        {
            var response = await _eventService.GetOneEventbyId(id);
            if (response == null)
            {
                return NotFound("Event Does Not Exist");
            }

            var Event = _mapper.Map<EventResponse>(response);
            return Ok(Event);
        }

        //update an event
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateEvent(Guid id, AddEvent updatedEvent)
        {
            var response = await _eventService.GetOneEventbyId(id);
            if (response == null)
            {
                return NotFound("Event Does Not Exist");
            }
            var role = User.Claims.FirstOrDefault(c => c.Type == "Role").Value;
            if (!string.IsNullOrWhiteSpace(role) && role == "Admin")
            {
                var existingEvent = _mapper.Map(updatedEvent, response);
                var res = await _eventService.UpdateEvent(existingEvent);
                return Ok(res);
            }
            return BadRequest("You are not allowed to do that");

        }

        //delete an event
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteEvent(Guid id)
        {
            var response = await _eventService.GetOneEventbyId(id);
            if (response == null)
            {
                return NotFound("Event Does Not Exist");
            }
            var role = User.Claims.FirstOrDefault(c => c.Type == "Role").Value;
            if (!string.IsNullOrWhiteSpace(role) && role == "Admin")
            {

                var res = await _eventService.DeleteEvent(response);
                return Ok(res);
            }
            return BadRequest("You are not allowed to do that");

        }


    }
}
