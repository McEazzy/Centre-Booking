using ClientWebGUI.Models;
using DatabaseWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace ClientWebGUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            try
            {
                RestClient restClient = new RestClient("http://localhost:50697/");
                RestRequest restRequest = new RestRequest("api/centres/", Method.Get);
                RestResponse restResponse = restClient.Execute(restRequest);

                if (restResponse.IsSuccessStatusCode)
                {
                    if(Access.admin)
                    {
                        //Home View for admin
                        return View("IndexAdmin", JsonConvert.DeserializeObject<List<PreCentre>>(restResponse.Content));
                    }
                    else
                    {
                        //Home view for non-admin
                        return View(JsonConvert.DeserializeObject<List<PreCentre>>(restResponse.Content));
                    }
                }
                else
                {
                    ViewBag.ServerError = restResponse.Content;
                    return View();
                }
            }
            catch(HttpRequestException e)
            {
                ViewBag.ServerError = "Can't establish a connection with the backend data server. Error source: " + e.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Select(int id)
        {
            try 
            {
                RestClient restClient = new RestClient("http://localhost:50697/");
                RestRequest restRequest = new RestRequest($"api/centres/{id}", Method.Get);
                RestResponse restResponse = restClient.Execute(restRequest);

                if (restResponse.IsSuccessStatusCode)
                {
                    Centre cen = JsonConvert.DeserializeObject<Centre>(restResponse.Content);
                    DateTime endDate = Convert.ToDateTime(cen.BookedSessions.Last().EndDate);

                    //if the last end date from all booking sessions occured before, set available date to today
                    string availDate;
                    if(endDate < DateTime.Today)
                    {
                        availDate = DateTime.Today.ToString("yyyy-MM-dd");
                    }
                    //increment available date to the next day of the last booking's end date
                    else
                    {
                        availDate = endDate.AddDays(1).ToString("yyyy-MM-dd");
                    }

                    return Ok(JsonConvert.SerializeObject(availDate));
                }
                else
                {
                    return StatusCode((int)restResponse.StatusCode, restResponse.Content);
                }
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Can't establish a connection to backend server. Error source: " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult Book([FromBody] PreBooking session) 
        {
            Debug.WriteLine(JsonConvert.SerializeObject(session));
            try
            {
                RestClient restClient = new RestClient("http://localhost:50697/");
                RestRequest restRequest = new RestRequest($"api/booking", Method.Post);
                restRequest.AddJsonBody(JsonConvert.SerializeObject(session));
                RestResponse restResponse = restClient.Execute(restRequest);

                if (restResponse.IsSuccessStatusCode)
                {
                    Debug.WriteLine(restResponse.Content);
                    return Ok();
                }
                else
                {
                    return StatusCode((int)restResponse.StatusCode, restResponse.Content);
                }
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Can't establish a connection to backend server. Error source: " + e.Message);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] PreCentre centre)
        {
            Debug.WriteLine("check here" + centre.Id);

            Centre newCentre = new Centre()
            {
                Id = centre.Id,
                name = centre.Name,
                BookedSessions = new List<BookedSession>(),
            };

            try
            {
                RestClient restClient = new RestClient("http://localhost:50697/");
                RestRequest restRequest = new RestRequest($"api/centres", Method.Post);
                restRequest.AddJsonBody(JsonConvert.SerializeObject(newCentre));
                RestResponse restResponse = restClient.Execute(restRequest);

                if (restResponse.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode((int)restResponse.StatusCode, restResponse.Content);
                }
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Can't establish a connection to backend server. Error source: " + e.Message);
            }
        }
    }
}
