﻿using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;

        public HomeController(IEmployeeRepository employeeRepository,
                              IHostingEnvironment hostingEnvironment,
                              ILogger<HomeController> logger)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        [Route("~/Home")]
        [Route("~/")]
        [AllowAnonymous]
        public ViewResult Index()
        {
            IEnumerable<Employee> employees = _employeeRepository.GetAllEmployees();
            //for (int i = 0; i < employees.Count();i++ )
            //{
            //    if (employees.ElementAt(i).PhotoPath == null)
            //    { employees.ElementAt(i).PhotoPath = "/images/Jeep.webp"; }
            //    else
            //    { employees.ElementAt(i).PhotoPath = "/images/" + employees.ElementAt(i).PhotoPath; }
            //}
            return View(employees);
        }
        public JsonResult JsonResult()
        {
            return Json(new { id = 1, name = "JsonResult" });
        }

        public IActionResult GetEmployeeActionResult(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            return View(employee);
        }
        public string GetEmployee(int? id)
        {

            try
            {
                if (id == null)
                {
                    return _employeeRepository.GetFirstEmployee().Name;
                }
                else
                {
                    return _employeeRepository.GetEmployee(id).Name;
                }
            }
            catch
            {
                return _employeeRepository.GetFirstEmployee().Name; ;
            }

        }
        public JsonResult GetEmployeeJsonDetails(int id)
        {
            Employee model = _employeeRepository.GetEmployee(id);
            return Json(model);
        }
        public ObjectResult GetEmployeeObjectDetails(int id)
        {
            Employee model = _employeeRepository.GetEmployee(id);
            return new ObjectResult(model);
        }
        public ViewResult GetEmployeeDetails(int id)
        {
            Employee model = _employeeRepository.GetEmployee(id);
            return View(model);
        }
        [Route("{id?}")]
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            
            Employee employee = _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id);
            }
            //id = 1;
            Employee model = employee;//_employeeRepository.GetEmployee(id??1);
            if (model.PhotoPath == null)
            { model.PhotoPath = "/images/Jeep.webp"; }
            else
            { model.PhotoPath = "~/images/" + model.PhotoPath; }
            //return View("~/MyViews/Test.cshtml"); 
            //return View("MyViews/Test.cshtml"); 
            //return View("../MyViews/Test.cshtml"); //this points Views/MyViews/Test.cshtml
            //return View("../../MyViews/Test"); //this relative path is from Views/Home folder.
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = model,
                PageTitle = "Employee Details"
            };
           
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            //throw new Exception("User Exception from details");
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogCritical("Critical Log");
            logger.LogError("Error Log");

            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                Employee newEmployee = new Employee {
                    Name = model.Name,
                    Email=model.Email,
                    Department=model.Department,
                    PhotoPath= uniqueFileName

                };
                _employeeRepository.Add(newEmployee);
            return RedirectToAction("details",new { id=newEmployee.Id});
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        {
            Employee emp = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                Department = emp.Department,
                ExistingPhotoPath = emp.PhotoPath
            };
            return View(employeeEditViewModel);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photos != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath=Path.Combine(hostingEnvironment.WebRootPath,"images",model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }
                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var filestream= new FileStream(filePath, FileMode.Create))
                    { 
                        photo.CopyTo(filestream);
                    }
                }
            }

            return uniqueFileName;
        }
    }
}
