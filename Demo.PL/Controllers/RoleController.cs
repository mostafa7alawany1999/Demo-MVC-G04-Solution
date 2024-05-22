using AutoMapper;
using Demo.DAL.Models;
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
    [Authorize(Roles ="Admin")]

    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RoleController( RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _roleManger = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string RoleSearchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(RoleSearchInput))
            {
                roles = await _roleManger.Roles.Select(R => new RoleViewModel()
                {
                    ID = R.Id,
                    RoleName = R.Name,
                }).ToListAsync();
            }
            else
            {
                roles = await _roleManger.Roles.Where(R => R.Name
                                                .ToLower()
                                                .Contains(RoleSearchInput.ToLower()))
                                                .Select(R => new RoleViewModel()
                                                {
                                                    ID = R.Id,
                                                    RoleName = R.Name,
                                                }).ToListAsync();
            }

            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                var result =  await _roleManger.CreateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "failed ");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.ID)
                return BadRequest();

            //if (employee.ImageName is not null)
            //{
            //    DocumentSettings.DeleteFile(employee.ImageName, "images");
            //}
            //employee.ImageName = DocumentSettings.UploadFile(employee.Image, "images");


            if (ModelState.IsValid)
            {
                var roleFromDB = await _roleManger.FindByIdAsync(id);
                if (roleFromDB is null)
                    return NotFound();

                //var usesViewModel = _mapper.Map<ApplicationUser>(model);
                // update manual 

                roleFromDB.Name = model.RoleName;
                

                var result = await _roleManger.UpdateAsync(roleFromDB);


                //int count = _employeeRepository.Update(employeeViewModel);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Update(string id) => await Details(id, "Update");


        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); // Error 400

            var roleFromDB = await _roleManger.FindByIdAsync(id);
            if (roleFromDB is null)
                return NotFound();

            var role = _mapper.Map<RoleViewModel>(roleFromDB);

            return View(ViewName, role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.ID)
                return BadRequest();

            //if (employee.ImageName is not null)
            //{
            //    DocumentSettings.DeleteFile(employee.ImageName, "images");
            //}
            //employee.ImageName = DocumentSettings.UploadFile(employee.Image, "images");


            if (ModelState.IsValid)
            {
                var roleFromDB = await _roleManger.FindByIdAsync(id);
                if (roleFromDB is null)
                    return NotFound();

                //var usesViewModel = _mapper.Map<ApplicationUser>(model);
                // update manual 



                var result = await _roleManger.DeleteAsync(roleFromDB);


                //int count = _employeeRepository.Update(employeeViewModel);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string id) => await Details(id, "Delete");

        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {

           var role =await _roleManger.FindByIdAsync(roleId);
           if(role is null)
                return NotFound();
            ViewData["RoleId"] = roleId;
            ViewData["RoleName"] = role.Name;
           var UsersInRole = new List<UsersInRoleViewModel>();
           var UserInDB = await _userManager.Users.ToListAsync();
            foreach (var user in UserInDB)
            {
                var UserInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    UserInRole.IsSelected = true;
                else
                    UserInRole.IsSelected = false;

                UsersInRole.Add(UserInRole);
            }
            return View(UsersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId , List<UsersInRoleViewModel> users)
        {
            var role = await _roleManger.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            if(ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser,role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser,role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                        }
                    }
                   
                }
                return RedirectToAction(nameof(Update) , new {id = roleId});

            }

            return View(users);
        }
    }
}
