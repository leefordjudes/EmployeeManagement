using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Mary", Department = Dept.HR, Email = "mary@pragimtech.com" },
                new Employee() { Id = 2, Name = "John", Department = Dept.IT, Email = "john@pragimtech.com" },
                new Employee() { Id = 3, Name = "Sam", Department = Dept.IT, Email = "sam@pragimtech.com" }
            };
        }
        public Employee GetEmployee(int? Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }
        public Employee GetFirstEmployee()
        {
            return _employeeList.FirstOrDefault();
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee Add(Employee employee)
        {
            employee.Id= _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee emp = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (emp != null)
            {
                emp.Name = employeeChanges.Name;
                emp.Email = employeeChanges.Email;
                emp.Department = employeeChanges.Department;
            }
           
            return emp;
        }

        public Employee Delete(int id)
        {
            Employee emp = _employeeList.FirstOrDefault(e=>e.Id==id);
            if (emp != null)
            {
                _employeeList.Remove(emp);
            }
            return emp;
        }
    }
}
