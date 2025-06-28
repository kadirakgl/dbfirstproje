using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Web.Models;

namespace StudentManagement.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? searchId, string sortOrder = "asc")
        {
            var studentsQuery = _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .Where(s => s.IsActive == true);

            if (searchId.HasValue)
            {
                studentsQuery = studentsQuery.Where(s => s.StudentId == searchId.Value);
            }

            if (sortOrder == "desc")
            {
                studentsQuery = studentsQuery.OrderByDescending(s => s.FirstName);
            }
            else
            {
                studentsQuery = studentsQuery.OrderBy(s => s.FirstName);
            }

            var students = await studentsQuery.ToListAsync();
            ViewBag.TotalCount = _context.Students.Count(s => s.IsActive == true);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SearchId = searchId;
            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(m => m.StudentId == id && m.IsActive == true);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,DateOfBirth")] Student student)
        {
            if (await _context.Students.AnyAsync(s => s.Email == student.Email && s.IsActive == true))
            {
                ModelState.AddModelError("Email", "Bu email ile zaten aktif bir öğrenci var.");
                return View(student);
            }

            if (ModelState.IsValid)
            {
                student.EnrollmentDate = DateTime.Now;
                student.IsActive = true;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(m => m.StudentId == id && m.IsActive == true);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id && e.IsActive == true);
        }
    }
} 