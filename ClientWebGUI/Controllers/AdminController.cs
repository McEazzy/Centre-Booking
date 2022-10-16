using ClientWebGUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;

namespace ClientWebGUI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Admin";
            if(Access.admin)
            {
                return View("IndexAdmin");
            }

            return View();
        }
        //Request.Cookies.ContainsKey("admin-key")
        //Expire = DateTime.Now.AddDays(-1)

        [HttpPost]
        public IActionResult Login([FromBody] Credential access)
        {
            Debug.WriteLine(access.Username);
            try
            {
                //prepare a POST request to backend for validation of admin
                RestClient restClient = new RestClient("http://localhost:50697/");
                RestRequest restRequest = new RestRequest("api/authenticate/");
                restRequest.AddJsonBody(JsonConvert.SerializeObject(access));
                RestResponse restResponse = restClient.Post(restRequest);

                //for successful return response from backend server
                if (restResponse.IsSuccessStatusCode)
                {
                    //Create a cookie to memorise admin registration for an hour
                    //Response.Cookies.Append("AKey", "AVal", new CookieOptions { Expires = DateTime.Now.AddHours(1) });
                    Access.admin = true;
                    return Ok(restResponse.Content);
                }
                //for other failed responses
                else
                {
                    return StatusCode((int)restResponse.StatusCode, restResponse.Content);
                }
            }
            catch(HttpRequestException)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Can't establish a connection to backend server");
            }
        }
    }
}
