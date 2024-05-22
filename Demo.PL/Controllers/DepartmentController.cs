using Microsoft.AspNetCore.Mvc;
using Demo.BLL.Repositories;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.BLL;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Demo.PL.ViewModels;
using AutoMapper;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller  
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController( IUnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task< IActionResult >Index()
        {
            var departments = await _unitOfWork.DapartmentRepository.GetAll();
            var model = _mapper.Map<IEnumerable< DepartmentViewModel>>(departments);

            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            if(ModelState.IsValid)
            {
               var model =  _mapper.Map<Department> (department);
                _unitOfWork.DapartmentRepository.Add(model);
                var count = await _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
          
            return View();
        }
        [HttpGet]
        public Task<IActionResult> Update(int? Id)
        {
          return Details(Id,nameof(Update));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update( [FromRoute] int? Id, DepartmentViewModel department)
        {
            if (Id != department.Id)
                return BadRequest();
           
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<Department>(department);

                _unitOfWork.DapartmentRepository.Update(model);
                var count = await _unitOfWork.Complete();

                if (count > 0)
                  {
                      return RedirectToAction(nameof(Index));
                  }
            }

            return View(department);
        }
        [HttpGet]
        public Task<IActionResult> Delete(int? Id)
        {
            return Details(Id, nameof(Delete));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? Id, DepartmentViewModel department)
        {
            if (Id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var model = _mapper.Map<Department>(department);

                _unitOfWork.DapartmentRepository.Delete(model);
                var count = await _unitOfWork.Complete();
                if (count > 0)
               {
                   return RedirectToAction(nameof(Index));
               }
            }
            return View(department);
        }
        public async Task<IActionResult> Details(int? Id , string ViewName="Details") 
        {
            if (Id is null)
                return BadRequest();
          var department= await _unitOfWork.DapartmentRepository.Get(Id.Value);
            var model = _mapper.Map<DepartmentViewModel>(department);

            if (model is null)
                return NotFound();

            return View(ViewName, model);

        }
    }
}
