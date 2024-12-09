using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChangeMenueAndLogo : ControllerBase
    {
        
        private readonly apiContext _context;

        public ChangeMenueAndLogo(apiContext context)
        {
            _context = context;
        }
    }
}