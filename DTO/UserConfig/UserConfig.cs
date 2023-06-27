
namespace DTO.UserAccount
{
    public class UserNameResponse
    {
        public string Name { get; set; }
        public ResponseResult Result { get; set; }
    }

    public enum ResponseResult
    {
        Success =1,
        Failure = 2,
        NotFound = 3
    }


    /*
      public class UserConfig
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public bool emailAccess { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<CustomerDataAccess> CustomerAccessList { get; set; }

    }

    public class CustomerDataAccess
    {
        public int customerId { get; set; }
        public int daUserCustomerId { get; set; }
        public int CustomerAccessId { get; set; }

        public IEnumerable<int> RoleIdAccessList { get; set; }
        public IEnumerable<int> productCategoryIdAccessList { get; set; }
        public IEnumerable<int> OfficeIdAccessList { get; set; }
        public IEnumerable<int> ServiceIdAccessList { get; set; }

        public IEnumerable<DropDown> masterCustomerList { get; set; }
        public IEnumerable<DropDown> masterRoleList { get; set; }
        public IEnumerable<DropDown> masterProductCategoryList { get; set; }
        public IEnumerable<DropDown> masterOfficeList { get; set; }
        public IEnumerable<DropDown> masterServiceList { get; set; }
    }
   
     */
}
