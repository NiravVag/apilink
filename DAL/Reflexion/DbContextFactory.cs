//using Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace DAL.Reflexion
//{
//    public static class DbContextFactory
//    {
//        private static List<DbContextItem> _contextList = null; 

//        public static API_DBContext GetDbContext()
//        {
//            if (_contextList == null)
//                _contextList = new List<DbContextItem>();

//            if (!_contextList.Any(x => x.EntityId == _ApplicationContext.EntityId))
//            {
//                var context = new DbContextItem
//                {
//                    EntityId = _ApplicationContext.EntityId,
//                   // DbContext = new API_DBContext()

//                };

//                _contextList.Add(context);

//                return context.DbContext;
//            }

//            return _contextList.First(x => x.EntityId == _ApplicationContext.EntityId).DbContext;
//        }
//    }

//    public class DbContextItem
//    {
//        public int EntityId { get; internal set;  }

//        public API_DBContext DbContext { get; internal set; }
//    }
//}
