using System.Collections.Generic;
using System.Linq;

namespace MenuExample.ViewModels {
    public class MenuViewModel {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Page { get; set; }
        public IList<MenuViewModel> ChildMenu { get; set; }
        public bool HasChildMenu => ChildMenu?.Any () ?? false;
    }
}