using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenuExample.Data;
using MenuExample.Models;
using MenuExample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuExample.ViewComponents {
    public class MenuViewComponent : ViewComponent {

        private readonly ApplicationDbContext _context;
        public MenuViewComponent (ApplicationDbContext context) {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync () {
            // Get all Menu from database
            var menu = MenuFormat (await GetMenu (), null);
            return View (menu);
        }

        //Get All Menu in single request
        private Task<List<Menu>> GetMenu () {
            return _context.Menu.ToListAsync ();
        }

        // Predicate for menu
        Func<Menu, int?, bool> PredicateMenu = (e, menu_id) => e.ParentMenuId == menu_id;

        // Format the menu according to need
        private IList<MenuViewModel> MenuFormat (IList<Menu> menu, int? parentId) {
            return menu.AsQueryable ()
                .Where (w => PredicateMenu (w, parentId))
                .Select (s => new MenuViewModel {
                    Id = s.Id,
                        Name = s.Name,
                        Area = s.Area,
                        Page = s.Page,
                        ChildMenu = MenuFormat (menu, s.Id)
                })
                .ToList ();
        }
    }
}