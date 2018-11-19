using System.Collections.Generic;

namespace MenuExample.Models {
    public class Menu {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Area { get; set; }
        public string Page { get; set; }

        public int? ParentMenuId { get; set; }

        public virtual Menu ParentMenu { get; set; }

        public virtual ICollection<Menu> ChildMenus { get; set; }
    }
}