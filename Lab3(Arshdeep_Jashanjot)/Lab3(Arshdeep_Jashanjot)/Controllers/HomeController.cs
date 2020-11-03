using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab3_Arshdeep_Jashanjot_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Lab3_Arshdeep_Jashanjot_.Service;
using System.IO;

namespace Lab3_Arshdeep_Jashanjot_.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        DynamoService dynamoService = new DynamoService();

        public async Task<IActionResult> Index()
        {
            var all = await dynamoService.GetAllAsync();
            return View(all);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Upload() 
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UploadMovie(IFormFile document)
        {
            bool isUploadSuccess = false;
            if (document != null)
            {
                
                Stream stream = document.OpenReadStream();
                DocumentService documentService = new DocumentService();
                isUploadSuccess = await documentService.UploadDocument(stream, document.FileName);
                Movie movie = new Movie
                {
                    name = document.FileName,
                    url = "https://moviesnet.s3.us-east-2.amazonaws.com/" + document.FileName.Replace(" ","+"),
                };
               
                
                dynamoService.Store(movie);
                await stream.DisposeAsync();
                
            }
            else
            {
                return BadRequest("Please select file to upload");
            }

            if (isUploadSuccess)
            {
                
            this.TempData["message"] = "Success!";
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error occurred" });
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
