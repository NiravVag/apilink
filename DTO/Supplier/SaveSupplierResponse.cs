using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SaveSupplierResponse
    {
        public int Id { get; set; }

        public SaveSupplierResult Result { get; set; }

        public List<string> ToEmailList { get; set; }

        public string MissingFields { get; set; }
        public string FactoryName { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public SaveSupplierResult FactoryResult { get; set; }
        public ErrorData ErrorData { get; set; }
        /// <summary>
        /// if supplier create and if same as supplier is checked then Relation Id is factory Id
        /// if factory create and if same as factory is checked then RelationId is supplier id
        /// </summary>
        public int? ParentId { get; set; }
    }
    public enum SupplierGradeResult
    {
        Success = 1,
        NotFound = 2,
        CustomerRequired = 3,
        SupplierRequired = 4,
        BookingIdsRequired = 5
    }
    public class SupplierGradeResponse
    {
        public string Grade { get; set; }
        public SupplierGradeResult Result { get; set; }
    }
    public enum SaveSupplierResult
    {
        Success = 1,
        SupplierIsNotSaved = 2,
        SupplierIsNotFound = 3,
        SupplierExists = 4,
        FactoryCountyTownNotFound = 5,
        SupplierEntityNotRemoved = 6,
        SupplierDetailsExists = 7,
        SupplierCodeExists = 8,
    }

    public class SupplierContactEntityData
    {
        public List<SuContact> SupplierContacts { get; set; }
        public List<SupplierContactUser> SupplierContactUsers { get; set; }
        public IEnumerable<ItUserRole> SupplierContactUserRoles { get; set; }
    }
    public class SupplierContactUser
    {
        public int UserId { get; set; }
        public int? ContactId { get; set; }
    }
}
