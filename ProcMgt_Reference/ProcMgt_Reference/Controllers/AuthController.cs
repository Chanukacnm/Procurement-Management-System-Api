using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProcMgt_Reference.Extensions;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private IAuthServices _authService;
        private readonly IMapper _mapper; 

        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async  Task<IActionResult> Authenticate([FromBody]UserResource resource)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());


            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, resource.Password);
                resource.Password = hash;
            }


            //var User = _mapper.Map<UserResource, User>(resource);
            //var result = await _authService.SaveUserAsync(User);
            var result = await _authService.Authenticate(resource.UserName, resource.Password);

            

            if (result == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                //issuer: "http://localhost:5000",
                //audience: "http://localhost:5000",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            //return Ok(new { Token = tokenString });
            result.Token = tokenString;

            return Ok(result);
        }



        static string GetMd5Hash(MD5 md5Hash, string password)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }




        [HttpGet, Route("GetAllUser")]
        public async Task<IEnumerable<UserResource>> GetAllUser()
        {
            var user = await _authService.GetAllAsync();
            var resources = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(user);

            return resources;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _authService.GetAll();
            return Ok(users);
        }


        //[HttpPost, Route("Login")]
        //public IActionResult Login([FromBody]User user)
        //{

        //    if (user == null)
        //    {
        //        return BadRequest("Invalid client request");
        //    }



        //    if (user.UserName == "johndoe" && user.Password == "def@123")
        //    {
        //        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
        //        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        //        var tokeOptions = new JwtSecurityToken(
        //            issuer: "http://localhost:5000",
        //            audience: "http://localhost:5000",
        //            claims: new List<Claim>(),
        //            expires: DateTime.Now.AddMinutes(5),
        //            signingCredentials: signinCredentials
        //        );

        //        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        //        return Ok(new { Token = tokenString });
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}
    }
}
