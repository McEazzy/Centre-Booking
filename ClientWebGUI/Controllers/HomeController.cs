using ClientWebGUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

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
                    return View(JsonConvert.DeserializeObject<List<Centre>>(restResponse.Content));
                    
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

        @Student
        public IActionResult SelectCenre()
        {

        }
    }
}
