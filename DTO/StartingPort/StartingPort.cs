using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.StartingPort
{
    public class StartingPortSearchRequest
    {
        public int? StartingPortId { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }
    public enum StartingPortResult
    {
        Success = 1,
        Failure = 2,
        NotFound = 3,
        AlreadyExists = 4,
        RequestNotCorrectFormat = 5
    }

    public class StartingPortResponse
    {
        public List<StartingPortSummaryData> Data { get; set; }
        public StartingPortResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public class StartingPortSummaryData
    {
        public int StartingPortId { get; set; }
        [Required(ErrorMessage = "START_PORT.MSG_STARTING_PORT")]
        [MaxLength(1000)]
        public string StartingPortName { get; set; }
        [Required(ErrorMessage = "START_PORT.MSG_CITY_REQUIRED")]
        public int CityId { get; set; }
        public string CityName { get; set; }
        public bool HasItRole { get; set; }
    }

    public class StartingPortRequest
    {
        public int Id { get; set; }
        public string StartPortName { get; set; }
        public int? CityId { get; set; }
    }

    public class StartingPortSaveResponse
    {
        public int Id { get; set; }
        public StartingPortResult Result { get; set; }
    }

    public class StartingPortEditResponse
    {
        public StartingPortSummaryData Data { get; set; }
        public StartingPortResult Result { get; set; }
    }
}
