using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contracts.Managers;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Hosting;
namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentManager _documentManager = null;
        private readonly IHostingEnvironment _env;
        public DocumentController(IDocumentManager documentManager, IHostingEnvironment env)
        {
            _documentManager = documentManager;
            _env = env;
        }

      
    }
}