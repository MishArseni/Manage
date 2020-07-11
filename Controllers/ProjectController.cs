using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationContext _context;

        public ProjectController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectAsync()
        {
            string name = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == name);
            if (user != null)
            {
                var projectsId = _context.UserProjs.Where(u => u.UserId == user.UserId).Select(u => u.ProjectId);
                List<Project> userProjects = new List<Project>();

                foreach (var project in _context.Projects.ToList())
                {
                    if (projectsId.Contains(project.ProjectId))
                    {
                        userProjects.Add(project);
                    }
                }

                return View(userProjects);
            }

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> ProjectsForAdd()
        {
            string name = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == name);
            if (user != null)
            {
                ViewBag.Db = _context.Projects.ToList();
                var projectsId = _context.UserProjs.Where(u => u.UserId == user.UserId).Select(u => u.ProjectId);
                List<Project> userProjects = new List<Project>();

                foreach (var project in _context.Projects.ToList())
                {
                    if (projectsId.Contains(project.ProjectId))
                    {
                        userProjects.Add(project);
                    }
                }

                return View(userProjects);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> AddToMyProject(int id)
        {
            Project project = _context.Projects.FirstOrDefault(u => u.ProjectId == id);
            if (project != null)
            {
                string name = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == name);
                UserProj userProj = new UserProj { User = user, Projects = project };
                _context.UserProjs.Add(userProj);
                _context.SaveChanges();
                return RedirectToAction("ProjectsForAdd", "Project");
            }
            ModelState.AddModelError(string.Empty, "Что-то пошло не так");
            return RedirectToAction("ProjectsForAdd", "Project");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromMy(int id)
        {
            Project project = _context.Projects.FirstOrDefault(u => u.ProjectId == id);
            if (project != null)
            {
                string name = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == name);
                UserProj userProj = new UserProj { User = user, Projects = project };
                _context.UserProjs.Remove(userProj);
                _context.SaveChanges();
                return RedirectToAction("ProjectsForAdd", "Project");
            }
            ModelState.AddModelError(string.Empty, "Что-то пошло не так");
            return RedirectToAction("ProjectsForAdd", "Project");
        }

    }
}
