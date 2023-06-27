using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO.Common;
using DTO.RoleRight;
using DTO.User;
using Entities;

namespace BI.Maps
{
    public class RoleRightMap: ApiCommonData
    {
        public  RightTreeView GetRightTreeView(ItRight entity, int roleId)
        {
            if (entity == null)
                return null;
            if (entity.ParentId.HasValue)
                return null;
            var result = GetRightItem(entity, roleId);
            return result;
        }

        private  RightTreeView GetRightItem(ItRight entity, int roleId)
        {
            if (entity == null)
                return null;
            RightTreeView result = new RightTreeView
            {
                Text = entity.TitleName == null ? entity.MenuName : entity.TitleName,
                Value = entity.Id,
                Checked = false
            };
            if (entity.ItRoleRights.Where(x => x.RoleId == roleId).Count() > 0)
                result.Checked = true;

            if (entity.InverseParent.Count > 0)
            {
                List<RightTreeView> lst = new List<RightTreeView>();
                foreach (var item in entity.InverseParent)
                {
                    if(item.Active.HasValue && item.Active.Value)
                    {
                        var i = GetRightItem(item, roleId);
                        lst.Add(i);
                    }
                }
                result.Children = lst;
            }
            return result;
        }
    }
}
