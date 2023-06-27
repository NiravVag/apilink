using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    //public static class _ApplicationContext
    //{
    //    private static ISession _session = null; 

    //    public static void AddSession(ISession session)
    //    {
    //        _session = session; 
    //    }

    //    public static string UniqueIdentifier => $"{EntityId}_{CustomerId}_{SupplierId}_{LocationId}";
                
    //    public static int CustomerId { get { return _session.GetInt32("CustomerId") == null ? 0 : _session.GetInt32("CustomerId").Value; } set { _session.SetInt32("CustomerId", value); } }

    //    public static int SupplierId { get { return _session.GetInt32("SupplierId") == null ? 0 : _session.GetInt32("SupplierId").Value; } set { _session.SetInt32("SupplierId", value); } }

    //    public static int LocationId { get { return _session.GetInt32("LocationId") == null ? 0 : _session.GetInt32("LocationId").Value; } set { _session.SetInt32("LocationId", value); } }

    //    public static int UserId { get { return _session.GetInt32("UserId") == null ? 0 : _session.GetInt32("UserId").Value; } set { _session.SetInt32("UserId", value); }  }

    //    public static int FactoryId { get { return _session.GetInt32("FactoryId") == null ? 0 : _session.GetInt32("FactoryId").Value; } set { _session.SetInt32("FactoryId", value); } }

    //    public static int EntityId { get
    //        {
    //            return _session.GetInt32("EntityId") == null ? 0 : _session.GetInt32("EntityId").Value;
    //        }
    //        set {
    //            _session.SetInt32("EntityId", value); }
    //        }

    //    public static int StaffId {
    //        get { return _session.GetInt32("StaffId") == null ? 0 : _session.GetInt32("StaffId").Value; } set { _session.SetInt32("StaffId", value); } }

    //    public static UserTypeEnum UserType { get { return _session.GetInt32("UserType") == null ? UserTypeEnum.InternalUser : (UserTypeEnum)_session.GetInt32("UserType").Value; } set { _session.SetInt32("UserType", (int)value); } }

    //    public static IEnumerable<int> RoleList {
    //        get {

    //            if (string.IsNullOrEmpty(_session.GetString("RoleList")))
    //                return null;

    //            return _session.GetString("RoleList").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x));
    //        }

    //        set {

    //            if (value == null)
    //                _session.SetString("RoleList", "");
    //            else
    //                _session.SetString("RoleList", string.Join("|", value.ToArray()));

    //        }

    //    }

    //    public static IEnumerable<int> LocationList
    //    {
    //        get
    //        {

    //            if (string.IsNullOrEmpty(_session.GetString("LocationList")))
    //                return null;

    //            return _session.GetString("LocationList").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x));
    //        }

    //        set
    //        {

    //            if (value == null)
    //                _session.SetString("LocationList", "");
    //            else
    //                _session.SetString("LocationList", string.Join("|", value.ToArray()));

    //        }

    //    }
  
    //}
}
