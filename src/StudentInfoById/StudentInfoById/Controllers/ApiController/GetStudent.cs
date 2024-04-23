using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using StudentInfoById.Models;
using Newtonsoft.Json;

namespace StudentInfoById.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetStudent : ControllerBase
    {
        [HttpGet("GetStudentInfo")]
        public async Task<IActionResult> getStudentInfo([FromQuery] int StudentId, [FromQuery] string username, [FromQuery] string password)
        {
            // URL to make the request to
            string url = $"https://sisclientweb-700031.campusnexus.cloud/ds/campusnexus/Students({StudentId})";

            // Create HttpClient instance
            using var httpClient = new HttpClient();

            // Set Basic Authentication header
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}")));
            httpClient.DefaultRequestHeaders.Authorization = authValue;

            try
            {
                // Make a GET request to the URL
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var studentInfo = JsonConvert.DeserializeObject<StudentInfoModel>(responseBody);
                    // Return the response
                    return Ok(studentInfo);
                }
                else
                {
                    // Return the error status code
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Return the error message
                return BadRequest(ex.Message);
            }
        }
    }
}
