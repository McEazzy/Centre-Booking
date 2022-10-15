using DatabaseWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DatabaseWebAPI.Controllers
{
    public class AuthenticateController : ApiController
    {
        //POST request for authentication from backend server
        public IHttpActionResult PostAuth(Credential account)
        {
            if (account != null)
            {
                if (account.username == "admin" && account.password == "adminpass")
                {
                    return Ok(true);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, "Admin log-in unsuccessful! Invalid username or password");
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Empty input access");
            }
        }

        // GET request for testing
        public IHttpActionResult GetAuth()
        {
            return Ok(new Credential{
                username = "admin",
                password = "adminpass"
            });
        }
    }
}
