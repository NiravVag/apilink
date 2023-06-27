using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerServicePickResponse
    {
        public IEnumerable<LevelPick1> LevelPickList { get; set; }
        public IEnumerable<Pick1> PickList { get; set; }
        public CustomerServicePickResult Result { get; set; }
    }

    public enum CustomerServicePickResult    {        Success = 1,        CannotGetLevelPick = 2,        CannotGetPick = 3    }
}
