using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nyerievents.services.iservices;
using NyeriEvents.Entities;
using NyeriEvents.Requests;
using NyeriEvents.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NyeriEvents.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IMapper mapper, IUserService Service, IConfiguration configuration)
        {
            _mapper = mapper;
            _userService = Service;
            _configuration = configuration;
        }

        [HttpPost("User")]
        //registering a user
        public async Task<ActionResult<string>> RegisterAUser(AddUser addUser)
        {
            var user = _mapper.Map<User>( addUser );
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var res = await _userService.RegisterAUser(user);
            return CreatedAtAction(nameof(RegisterAUser), res);
        }

        //booking an event
        [HttpPut("Book An Event")]
        public async Task<ActionResult<string>> BookAnEvent(BookEvent book)
        {
            try
            {
                var res = await _userService.BookAnEvent(book);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message);
            }

        }

        //get all users
        [HttpGet("All Users")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            var users = _mapper.Map<IEnumerable<UserResponse>>(response);
            return Ok(users);
        }

        //get a user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(Guid id)
        {
            var response = await _userService.GetAUserById(id);
            if (response == null)
            {
                return NotFound("User Does Not Exist");
            }



            var user = _mapper.Map<UserResponse>(response);
            return Ok(user);
        }

        //update a user
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateUser(Guid id, User User)
        {
            var response = await _userService.GetAUserById(id);
            if (response == null)
            {
                return NotFound( "User Does Not Exist");
            }
            var IsPasswordValid = BCrypt.Net.BCrypt.HashPassword(User.Password);
            var existingUser = _mapper.Map(User, response);
            var res = await _userService.UpdateUser(id,User);
            return Ok(res);

        }

        //delete a user
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUser(Guid id)
        {
            var response = await _userService.GetAUserById(id);
            if (response == null)
            {
                return NotFound("User Does Not Exist");
            }
            var role = User.Claims.FirstOrDefault(c => c.Type == "Role").Value;
            if (!string.IsNullOrWhiteSpace(role) && role == "Admin")
            {
                var res = await _userService.DeleteUser(response);
                return Ok(res);
            }
            return BadRequest("Admin Rights Required");
        }

        //create a token
        private string CreateToken(User user)
        {
            //key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("TokenSecurity:SecretKey")));
            //Signing Credentials
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //payload-data

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Names", user.Name));
            claims.Add(new Claim("Sub", user.Id.ToString()));
            claims.Add(new Claim("Role", user.Role));

            //create Token 
            var tokenGenerated = new JwtSecurityToken(
                _configuration["TokenSecurity:Issuer"],
                _configuration["TokenSecurity:Audience"],
                signingCredentials: cred,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);
            return token;
        }

    }
}
