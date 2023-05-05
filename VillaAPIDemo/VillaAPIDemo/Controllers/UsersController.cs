﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaApi.Models;
using VillaAPIDemo.Models.DTO;
using VillaAPIDemo.Repository.IRepository;

namespace VillaAPIDemo.Controllers
{
	[Route("api/UsersAuth")]
	[ApiController]
	public class UsersController : Controller
	{
		private readonly IUserRepository _userRepo;
		private readonly APIResponse _response;
		public UsersController(IUserRepository userRepo)
		{
			this._response = new();
			_userRepo = userRepo;
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			var LoginResponse =await _userRepo.Login(model);
			if (LoginResponse == null || string.IsNullOrEmpty(LoginResponse.Token))
			    {
				//return BadRequest(new { message = "user or password is incorrect" });
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("user or password is incorrect");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			_response.Result=LoginResponse;
			return Ok(_response);
		}
		[HttpPost("registration")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
		{
			bool ifUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
			if (!ifUserNameUnique)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("username already exists");
				return BadRequest(_response);

			}
			var user= await _userRepo.Register(model);	
			if(user == null) {
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Error while registering");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			
			return Ok(_response);
		}
	}
}