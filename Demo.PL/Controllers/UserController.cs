using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager , IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string UserSearchInput)
        {
            var users = Enumerable.Empty<UserViewModel>();

            if (string.IsNullOrEmpty(UserSearchInput))
            {
                users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result

                }).ToListAsync();
            }
            else
            {
                users = await _userManager.Users.Where(U => U.Email
                                                .ToLower()
                                                .Contains(UserSearchInput.ToLower()))
                                                .Select(U => new UserViewModel()
                                                {
                                                    Id = U.Id,
                                                    FirstName = U.FirstName,
                                                    LastName = U.LastName,
                                                    Email = U.Email,
                                                    Roles = _userManager.GetRolesAsync(U).Result
                                                }).ToListAsync();

                TempData["data"]=users;
            }

            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            //if (employee.ImageName is not null)
            //{
            //    DocumentSettings.DeleteFile(employee.ImageName, "images");
            //}
            //employee.ImageName = DocumentSettings.UploadFile(employee.Image, "images");


            if (ModelState.IsValid)
            {
                var userFromDB = await _userManager.FindByIdAsync(id);
                if(userFromDB is null)
                    return NotFound ();

                //var usesViewModel = _mapper.Map<ApplicationUser>(model);
                // update manual 

                userFromDB.FirstName = model.FirstName;
                userFromDB.LastName = model.LastName;
                userFromDB.Email = model.Email;

               var result = await _userManager.UpdateAsync(userFromDB);


                //int count = _employeeRepository.Update(employeeViewModel);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Update(string id) => await Details(id, "Update");



        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); // Error 400

           var userFromDB = await _userManager.FindByIdAsync(id);
            if (userFromDB is null)
                return NotFound();

            var user = _mapper.Map<UserViewModel>(userFromDB);

            return View(ViewName, user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            //if (employee.ImageName is not null)
            //{
            //    DocumentSettings.DeleteFile(employee.ImageName, "images");
            //}
            //employee.ImageName = DocumentSettings.UploadFile(employee.Image, "images");


            if (ModelState.IsValid)
            {
                var userFromDB = await _userManager.FindByIdAsync(id);
                if (userFromDB is null)
                    return NotFound();

                //var usesViewModel = _mapper.Map<ApplicationUser>(model);
                // update manual 

           

                var result = await _userManager.DeleteAsync(userFromDB);


                //int count = _employeeRepository.Update(employeeViewModel);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string id) => await Details(id, "Delete");
    }
}
