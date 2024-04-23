using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentInfoById.Models;
using System.Diagnostics;

namespace StudentInfoById.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            return View();
        }

        public IActionResult GetStudentInfo(string StudentId, string username, string password)
        {
            var studentInfo = new StudentInfoModel();
            string url = buildUrl(StudentId, username, password);
            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if(response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                studentInfo = JsonConvert.DeserializeObject<StudentInfoModel>(responseBody);
            }
            return View(studentInfo);
        }

        private string buildUrl(string StudentId, string username, string Password)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            return string.Format($"{baseUrl}/api/GetStudent/GetStudentInfo?StudentId={StudentId}&username={username}&password={Password}");
        }
    }
}
