using DTO.CommonClass;
using DTO.OtherManday;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IOtherMandayRepository: IRepository
    {
        Task<bool> CheckIfOtherMandayAlreadyExists(SaveOtherMandayRequest request);
        Task<OmDetail> GetOtherMandayDataById(int id);
        Task<OtherMandayDataRepo> GetOtherMandayEditDataById(int id);
        IQueryable<OtherMandayDataRepo> GetOtherMandayByEfCore();
        Task<List<CommonDataSource>> GetPurposeList();
    }
}
