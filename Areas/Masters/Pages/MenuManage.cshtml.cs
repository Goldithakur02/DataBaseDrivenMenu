using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MenuExample.Data;
using MenuExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MenuExample.Areas.Masters.Pages {
    public class MenuManageModel : PageModel {

        private readonly ApplicationDbContext _context;
        public MenuManageModel (ApplicationDbContext context) {
            _context = context;
        }

        [TempData]
        public string Message { get; set; }

        public bool HasMessage => !string.IsNullOrEmpty (Message) && !string.IsNullOrWhiteSpace (Message);

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList ParentMenu { get; set; }

        public IReadOnlyList<MenuDto> Menus { get; set; }

        public bool HasMenus => Menus.Any ();

        public class InputModel {

            [Required]
            [MaxLength (150)]
            public string Name { get; set; }

            [MaxLength (150)]
            [Display (Name = "Area")]
            public string AreaPage { get; set; }

            [MaxLength (150)]
            [Display (Name = "Page")]
            public string PageUrl { get; set; }

            [Display (Name = "Parent Menu")]
            public int? ParentMenuId { get; set; }

        }

        public class MenuDto {
            public int MenuId { get; set; }

            public string Name { get; set; }

            public int? ParentMenuId { get; set; }

            public string Area { get; set; }

            public string Page { get; set; }

        }
        public IActionResult OnGetCancel (CancellationToken cancellationToken = default) {
            return RedirectToPage ();
        }

        public async Task OnGetAsync (CancellationToken cancellationToken = default) {
            await FillInitials (cancellationToken);

        }
        public async Task<IActionResult> OnPostAsync (CancellationToken cancellationToken = default) {
            if (ModelState.IsValid) {
            var entity = new MenuExample.Models.Menu {
            Name = Input.Name,
            Area = Input.AreaPage,
            Page = Input.PageUrl,
            ParentMenuId = Input.ParentMenuId
                };
                await _context.Menu.AddAsync (entity);
                var result = await _context.SaveChangesAsync ();
                if (result.Equals (1)) {
                    Message = "New Menu Added to System. Kindly add to role";
                    return RedirectToPage ();
                } else {
                    ModelState.AddModelError (string.Empty, "Error occur while processing your request");
                }
            }
            await FillInitials (cancellationToken);
            return Page ();
        }

        public async Task<IActionResult> OnPostDeleteAsync (int Id, CancellationToken cancellationToken = default) {
            var entity = await _context.Menu.FindAsync (new object[] { Id });
            if (entity != null) {
                _context.Menu.Remove (entity);
                var result = await _context.SaveChangesAsync ();
                if (result.Equals (1)) {
                    Message = "Menu deleted successfully";
                    return RedirectToPage ();
                } else {
                    ModelState.AddModelError (string.Empty, "Error occur while processing your request");
                }
            }

            await FillInitials (cancellationToken);
            return Page ();

        }

        private async Task FillInitials (CancellationToken cancellationToken) {
            ParentMenu = new SelectList (await _context.Menu.Where (w => w.Area == null && w.Page == null).Select (s => new { s.Id, s.Name }).ToListAsync (), "Id", "Name");
            Menus = await _context.Menu.Select (c => new MenuDto {
                MenuId = c.Id,
                    Name = c.Name,
                    Area = c.Area,
                    Page = c.Page,
                    ParentMenuId = c.ParentMenuId
            }).ToListAsync (cancellationToken);
        }
    }
}