using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public HomeController(IVillaService villaService, IMapper mapper)
        {
            _mapper = mapper;
            _villaService = villaService;
        }
       
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new();
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }




        //public async Task<IActionResult> Index()
        //{
        //    List<VillaDTO> list = new();

        //    // Get the session token
        //    string token = HttpContext.Session.GetString(SD.SessionToken);

        //    // Check if the token is available
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        // Handle the case where the token is not available
        //        // Redirect to the login page or show an error message
        //        return RedirectToAction("Login", "Auth");
        //    }

        //    // Make the API call with the token
        //    var response = await _villaService.GetAllAsync<APIResponse>(token);

        //    // Check if the response is successful
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        //    }
        //    else
        //    {
        //        // Handle the case where the response is not successful
        //        // Log the error or show an error message
        //        ModelState.AddModelError(string.Empty, "Error retrieving villa list.");
        //    }

        //    return View(list);
        //}
    }
}
