﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Net;
using VillaAPI_Utility;
using VillaAPI_Web.Models;
using VillaAPI_Web.Models.Dto;
using VillaAPI_Web.Models.VM;
using VillaAPI_Web.Services;
using VillaAPI_Web.Services.IServices;

namespace VillaAPI.Controllers
{
	public class VillaNumberController : Controller
	{
		private readonly IVillaNumberService _villaNumberService;
		private readonly IVillaService _villaService;
		private readonly IMapper _mapper;

		public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
		{
			_villaNumberService = villaNumberService;
			_mapper = mapper;
			_villaService = villaService;
		}
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexVillaNumber()
		{
			List<VillaNumberDTO> list = new();

			var response = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
			return View(list);
		}
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
		{
			VillaNumberCreateVM villaNumberVM = new();
			var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
					(Convert.ToString(response.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
			}

			return View(villaNumberVM);
		}
        [Authorize(Roles = "admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
		{
			if (ModelState.IsValid)
			{
				var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "VillaNumber Created";
					return RedirectToAction(nameof(IndexVillaNumber));
				}
				else
				{
					if (response.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
					}
				}
			}

			var resp = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (resp != null && resp.IsSuccess)
			{
				model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
					(Convert.ToString(resp.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
			}
			TempData["error"] = "error";

			return View(model);
		}
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int VillaID)
		{
			VillaNumberUpdateVM villaNumberVM = new();
			var response = await _villaNumberService.GetAsync<APIResponse>(VillaID, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
				villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
			}
			response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
					(Convert.ToString(response.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
				return View(villaNumberVM);
			}
			return NotFound();
		}
		[HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
		{
			if (ModelState.IsValid)
			{
				var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "VillaNumber updated";
					return RedirectToAction(nameof(IndexVillaNumber));
				}
				else
				{
					if (response.ErrorMessages.Count > 0)
					{
						ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
					}
				}
			}

			var resp = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (resp != null && resp.IsSuccess)
			{
				model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
					(Convert.ToString(resp.Result)).Select(i => new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
			}
			TempData["error"] = "error";

			return View(model);
		}
        //public async Task<IActionResult> DeleteVillaNumber(int villaID)
        //{
        //    VillaNumberDeleteVM villaNumberVM = new();
        //    var response = await _villaNumberService.GetAsync<APIResponse>(villaID);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
        //        villaNumberVM.VillaNumber = model;
        //    }
        //    response = await _villaService.GetAllAsync<APIResponse>();
        //    if (response != null && response.IsSuccess)
        //    {
        //        villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
        //            (Convert.ToString(response.Result)).Select(i => new SelectListItem
        //            {
        //                Text = i.Name,
        //                Value = i.Id.ToString()
        //            });
        //        return View(villaNumberVM);
        //    }
        //    return NotFound();
        //        //}
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]

        //        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        //        {

        //            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo);
        //            if (response != null && response.IsSuccess)
        //            {
        //                TempData["success"] = "VillaNumber deleted";
        //                return RedirectToAction(nameof(IndexVillaNumber));
        //            }
        //            TempData["error"] = "error";
        //            return View(model);
        //        }
        //    }
        //}
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaID)
		{
			VillaNumberDeleteVM villaNumberVM = new();
			var response = await _villaNumberService.GetAsync<APIResponse>(villaID, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
				villaNumberVM.VillaNumber = model;
			}
			var result = await _villaNumberService.DeleteAsync<APIResponse>(villaNumberVM.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
			TempData["success"] = "VillaNumber deleted";
			return RedirectToAction(nameof(IndexVillaNumber));
			//return View("IndexVillaNumber");
		}
	}
}
