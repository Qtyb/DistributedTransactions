using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CoHelpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;

        public AuthController(
            IMapper mapper)
        { 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> Authenticate()
        //{
        //    return Ok();
        //}

        private void SendVerificationCode()
        {

        }
    }
}