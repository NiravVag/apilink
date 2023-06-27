using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.UserGuide;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class UserGuideController : ControllerBase
    {
        private readonly IUserGuideManager _userGuideManager = null;
        private readonly IHostingEnvironment _env;
        private readonly IDocumentManager _documentManager = null;

        public UserGuideController(IUserGuideManager userGuideManager, IHostingEnvironment env, IDocumentManager documentManager)
        {
            _userGuideManager = userGuideManager;
            _env = env;
            _documentManager = documentManager;
        }

        [HttpGet]
        public async Task<UserGuideDetailResponse> GetUserGuideDetails()
        {
            return await _userGuideManager.GetUserGuideDetails();
        }

        [HttpGet("downloadFile/{userGuideId}")]
        public async Task<IActionResult> DownloadFile(int userGuideId)
        {
            var file = await _userGuideManager.GetFileData(userGuideId);

            if (file == null || file.Result == DTO.File.FileResult.NotFound || file.Content == null)
                return NotFound();

            return File(file.Content, file.MimeType); // returns a FileStreamResult
        }
    }
}