using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Employee Add(Employee employee)
        {
            context.employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee emp = context.employees.Find(id);
            if (emp != null)
            {
                context.employees.Remove(emp);
                context.SaveChanges();
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.employees;
        }

        public Employee GetEmployee(int? Id)
        {
            return context.employees.Find(Id);
        }

        public Employee GetFirstEmployee()
        {
            return context.employees.FirstOrDefault();
        }

        public Employee Update(Employee employeeChanges)
        {
            var emp = context.employees.Attach(employeeChanges);
            emp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
