using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.RoleRight
{
    public class RightTreeView
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public List<RightTreeView> Children { get; set; }
        public bool Checked { get; set; }
    }
}
