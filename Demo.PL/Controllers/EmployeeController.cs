using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IDapartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
;
            _unitOfWork=unitOfWork;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index(string SearchInput)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = await _unitOfWork.EmployeeRepository.GetAll();

            }
            else
            {
                 employees = await _unitOfWork.EmployeeRepository.GetByNAme(SearchInput.ToLower());

            }

           var result = _mapper.Map<IEnumerable<EmployeeViewModel>>( employees);
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {

            if (ModelState.IsValid)
            {
                employeeViewModel.ImageName = DocumentSettings.UploadFile(employeeViewModel.Image, "images");
                var model = _mapper.Map<Employee>(employeeViewModel);

                  _unitOfWork.EmployeeRepository.Add(model);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Added";
                }
                else 
                {
                    TempData["Message"] = "Employee Not Added";

                }

                return RedirectToAction("Index");

            }

            return View();
        }
        [HttpGet]
        public Task<IActionResult> Update(int? Id)
        {

            return Details(Id, nameof(Update));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int? Id, EmployeeViewModel employeeViewModel)
        {
            if (Id != employeeViewModel.Id)
                return BadRequest();

            if(employeeViewModel.ImageName is not null)
            {
                DocumentSettings.DeleteFile(employeeViewModel.ImageName, "images");
            }
            employeeViewModel.ImageName = DocumentSettings.UploadFile(employeeViewModel.Image, "images");


            var result = _mapper.Map<Employee>(employeeViewModel);



            if (ModelState.IsValid)
            {
                 _unitOfWork.EmployeeRepository.Update(result);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employeeViewModel);
        }
        [HttpGet]
        public Task<IActionResult> Delete(int? Id)
        {
            return Details(Id, nameof(Delete));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? Id, EmployeeViewModel employeeViewModel)
        {
            if (Id != employeeViewModel.Id)
                return BadRequest();
            var result = _mapper.Map<Employee>(employeeViewModel);

            if (ModelState.IsValid)
            {
                _unitOfWork.EmployeeRepository.Delete(result);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(employeeViewModel.ImageName, "images");

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employeeViewModel);
        }
        public async Task<IActionResult> Details(int? Id, string ViewName = "Details")
        {
            if (Id is null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.Get(Id.Value);
            var result = _mapper.Map<EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();

            return View(ViewName, result);

        }
    }
}
