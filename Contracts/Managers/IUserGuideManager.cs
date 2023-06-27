using DTO.File;
using DTO.UserGuide;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IUserGuideManager
    {
        Task<UserGuideDetailResponse> GetUserGuideDetails();

        Task<FileResponse> GetFileData(int userGuideId);
    }
}
