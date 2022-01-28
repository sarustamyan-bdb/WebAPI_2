/*
=================================================================================================================================================================
PROJECT: APIWEB_1 
AUTHOR: MARK ABRAMS
DATE:   JAN 26, 2022
PURPOSE: CONTROLLER FOR WEB API INITIAL TEST 
=================================================================================================================================================================
*/

using System.Web.Http;

namespace WebAPI_2.Controllers
{
    
    public class TestController : ApiController
    {
        // GET: Test
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("{Test, Success}");
        }
    }
}