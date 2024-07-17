using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagerMVC.Data;
using StudentManagerMVC.Models;
using static StudentManagerMVC.Models.ViewModel;

namespace StudentManagerMVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string filterField, string filterCriteria, string filterValue)
        {
            var students = from s in _context.Students select s;

            if (!string.IsNullOrEmpty(filterField))
            {
                switch (filterField)
                {
                    case "Gender":
                        if (!string.IsNullOrEmpty(filterCriteria))
                        {
                            students = students.Where(s => s.Gender == filterCriteria);
                        }
                        break;
                    case "Oldest":
                        students = students.OrderBy(s => s.DateOfBirth).Take(1);
                        break;
                    case "FullName":
                        if (!string.IsNullOrEmpty(filterCriteria))
                        {
                            var names = filterCriteria.Split(' ');
                            if (names.Length == 2)
                            {
                                students = students.Where(s => s.FirstName == names[0] && s.LastName == names[1]);
                            }
                            else if (names.Length == 1)
                            {
                                students = students.Where(s => s.FirstName == names[0] || s.LastName == names[0]);
                            }
                        }
                        break;
                    case "BirthYear":
                        if (int.TryParse(filterValue, out int year))
                        {
                            students = students.Where(s => s.DateOfBirth.Year == year);
                        }
                        break;
                    case "PlaceOfBirth":
                        if (!string.IsNullOrEmpty(filterCriteria))
                        {
                            students = students.Where(s => s.PlaceOfBirth == filterCriteria);
                        }
                        break;
                }
            }

            var distinctPlacesOfBirth = await _context.Students
                .Select(s => s.PlaceOfBirth)
                .Distinct()
                .ToListAsync();

            ViewBag.PlacesOfBirth = distinctPlacesOfBirth;

            return View(await students.ToListAsync());
        }




        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,DateOfBirth,Gender,PlaceOfBirth,Mobile,IsGraduated")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,DateOfBirth,Gender,PlaceOfBirth,Mobile,IsGraduated")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
                .FirstOrDefaultAsync(m => m.ID == id);
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
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
