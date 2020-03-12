using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstController : Controller
    {
        public string GetData()
        {
            return "Welcome to bootcamp phase - II";
        }
    }
}