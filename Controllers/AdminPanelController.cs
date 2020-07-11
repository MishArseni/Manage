using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using ProjectManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    [Authorize(Roles = "projDirect ")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationContext _context;
        public AdminPanelController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult ProjectList()
        {
            return View(_context.Projects.ToList());
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateProject(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {

                Project project = new Project { Name = model.Name, Date = model.Date, Price = model.Price };
                var projectsName = _context.Projects.Select(u => u.Name);
                if (!projectsName.Contains(project.Name))
                {
                    await _context.Projects.AddAsync(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ProjectList", "AdminPanel");

                }

                ModelState.AddModelError(string.Empty, "Проект с таким именем уже существует!");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Введены не все значения");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(u => u.ProjectId == id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProjectList", "AdminPanel");
            }
            ModelState.AddModelError(string.Empty, "Неудачное удаление");
            return RedirectToAction("Index", "AdminPanel");
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(u => u.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            EditProjectViewModel model = new EditProjectViewModel { Name = project.Name, Date = project.Date, Price = project.Price };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(u => u.ProjectId == model.Id);
            if (project != null)
            {
                project.Name = model.Name;
                project.Price = model.Price;
                project.Date = model.Date;

                _context.Update(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> CreateUser(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (result == null)
                {
                    User user = new User { Email = model.Email, Password = model.Password,Name = model.Name};

                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "staff");
                    if (userRole != null)
                        user.Role = userRole;

                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "AdminPanel");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Пользователь с таким именем уже существует");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UserList()
        {
            return View(_context.Users.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("UserList", "AdminPanel");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Name = user.Name, Email = user.Email, Password = user.Password};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.Id);
            if (user != null)
            {
                user.Name = model.Name;
                user.Email = model.Email;
                user.Password = model.Password;

                _context.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }




    }
}
