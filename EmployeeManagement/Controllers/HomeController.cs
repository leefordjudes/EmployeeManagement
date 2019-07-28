﻿using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        [Route("~/Home")]
        [Route("~/")]
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
        public ViewResult Details(int? id)
        {
            //id = 1;
            Employee model = _employeeRepository.GetEmployee(id??1);
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
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                   string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                   uniqueFileName = Guid.NewGuid().ToString()+"_"+model.Photo.FileName;
                   string filePath=Path.Combine(uploadsFolder, uniqueFileName);
                   model.Photo.CopyTo(new FileStream(filePath,FileMode.Create));
                }
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

    }
}