using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentCourseResults.Data;
using StudentCourseResults.Models;

namespace StudentCourseResults.Controllers
{
    public class StudentResultsController : Controller
    {
        private readonly StudentResultDbContext _context;

        public StudentResultsController(StudentResultDbContext context)
        {
            _context = context;
        }

        // GET: StudentResults
        public async Task<IActionResult> Index(string statusFilter)
        {
            var results = from r in _context.StudentResults select r;

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                if (Enum.TryParse<ResultStatusName>(statusFilter, out var parsedStatus))
                {
                    results = results.Where(r => r.Status == parsedStatus);
                }
            }
            ViewBag.StatusList = Enum.GetNames(typeof(ResultStatusName)).Prepend("All").ToList();
            ViewBag.StatusFilter = statusFilter ?? "All";

            return View(await results.ToListAsync());
        }



        // GET: StudentResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentResult = await _context.StudentResults
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentResult == null)
            {
                return NotFound();
            }

            return View(studentResult);
        }

        // GET: StudentResults/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentName,CourseTitle,TotalMarks")] StudentResult studentResult)
        {
            if (ModelState.IsValid)
            {
                if (studentResult.TotalMarks < 50)
                    studentResult.Status = ResultStatusName.NeedsImprovement;
                else if (studentResult.TotalMarks < 80)
                    studentResult.Status = ResultStatusName.Good;
                else
                    studentResult.Status = ResultStatusName.Excellent;

                _context.Add(studentResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentResult);
        }

        // GET: StudentResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentResult = await _context.StudentResults.FindAsync(id);
            if (studentResult == null)
            {
                return NotFound();
            }
            return View(studentResult);
        }

        // POST: StudentResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentName,CourseTitle,TotalMarks")] StudentResult studentResult)
        {
            if (id != studentResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (studentResult.TotalMarks < 50)
                    studentResult.Status = ResultStatusName.NeedsImprovement;
                else if (studentResult.TotalMarks < 80)
                    studentResult.Status = ResultStatusName.Good;
                else
                    studentResult.Status = ResultStatusName.Excellent;

                try
                {
                    _context.Update(studentResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentResultExists(studentResult.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentResult);
        }


        // GET: StudentResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentResult = await _context.StudentResults
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentResult == null)
            {
                return NotFound();
            }

            return View(studentResult);
        }

        // POST: StudentResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentResult = await _context.StudentResults.FindAsync(id);
            if (studentResult != null)
            {
                _context.StudentResults.Remove(studentResult);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentResultExists(int id)
        {
            return _context.StudentResults.Any(e => e.Id == id);
        }
    }
}
