using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexSelectModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexSelectModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<CourseViewModel> CourseVM { get; set; }

        public async Task OnGetAsync()
        {
            CourseVM = await _context.Courses
            .Select(p => new CourseViewModel
            {
                CourseID = p.CourseID,
                Title = p.Title,
                Credits = p.Credits,
                DepartmentName = p.Department.Name
            }).ToListAsync();
        }

        
}
}

