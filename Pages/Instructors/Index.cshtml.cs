using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;

        public IndexModel(SchoolContext context)    
        {
            _context = context;
        }

        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id, int? courseID)
        {
            InstructorData = new InstructorIndexData();

            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Enrollments)
                            .ThenInclude(i => i.Student)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if (id != null)
            {
                InstructorID = id.Value;
                //This calls the where method separately
                //Instructor instructor = InstructorData.Instructors
                //    .Where(i => i.ID == id.Value).Single();

                //This Single method passes in the where condition, instead of calling it separately
                Instructor instructor = InstructorData.Instructors.Single(
                    i => i.ID == id.Value);
                InstructorData.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                CourseID = courseID.Value;
                //This calls the where method separately
                //var selectedCourse = InstructorData.Courses
                //    .Where(x => x.CourseID == courseID).Single();
                //InstructorData.Enrollments = selectedCourse.Enrollments;


                //This Single method passes in the where condition, instead of calling it separately
                InstructorData.Enrollments = InstructorData.Courses.Single(
                    x => x.CourseID == courseID).Enrollments;
            }
        }
    }
}
