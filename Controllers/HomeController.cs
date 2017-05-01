using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;

namespace dotnetsheff_2017.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GeneratePdf([FromServices] INodeServices nodeServices, [FromServices] IHostingEnvironment env)
        {
            var data = GetHtmlTicket(env);

            if(data == null)
                return NotFound();
            
            var options = new { format = "Letter" };     
            
            var result = await nodeServices.InvokeAsync<Stream>("generatePdf.js",options, data);
            return File(result, "application/pdf");
        }

        private object GetHtmlTicket(IHostingEnvironment env){
            var data = new { Film ="Logan", Value = "£10.00", Type = "VIP"};

            var reportPath = env.WebRootFileProvider.GetFileInfo("html/ticket.html");
            
            if(reportPath == null)
                return null;
            
            var html =  System.IO.File.ReadAllText(reportPath.PhysicalPath);

            html = html.Replace("{#film}",data.Film);
            html = html.Replace("{#type}",data.Type);
            html = html.Replace("{#value}",data.Value);
            
            return new {html};
        }
    }
}
