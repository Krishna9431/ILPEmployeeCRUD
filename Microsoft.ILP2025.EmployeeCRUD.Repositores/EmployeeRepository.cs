
using Microsoft.ILP2025.EmployeeCRUD.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.ILP2025.EmployeeCRUD.Repositores
{
    public class EmployeeRepository : IEmployeeRepository
    {
        // private readonly string filePath = "employee.json";
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "employee.json");


        private async Task<List<EmployeeEntity>> ReadFromFileAsync()
        {
            if (!File.Exists(filePath))
                return new List<EmployeeEntity>();

            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<EmployeeEntity>>(json) ?? new List<EmployeeEntity>();
        }

        private async Task WriteToFileAsync(List<EmployeeEntity> employees)
        {
            string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<List<EmployeeEntity>> GetAllEmployees()
        {
            return await ReadFromFileAsync();
        }

        public async Task<EmployeeEntity> GetEmployee(int id)
        {
            var employees = await ReadFromFileAsync();
            return employees.FirstOrDefault(e => e.Id == id);
        }

        public async Task CreateEmployee(EmployeeEntity employee)
        {
            var employees = await ReadFromFileAsync();
            employee.Id = employees.Any() ? employees.Max(e => e.Id) + 1 : 1;
            employees.Add(employee);
            await WriteToFileAsync(employees);
        }

        public async Task UpdateEmployee(EmployeeEntity updatedEmployee)
        {
            var employees = await ReadFromFileAsync();
            var index = employees.FindIndex(e => e.Id == updatedEmployee.Id);
            if (index != -1)
            {
                employees[index] = updatedEmployee;
                await WriteToFileAsync(employees);
            }
        }

        public async Task DeleteEmployee(int id)
        {
            var employees = await ReadFromFileAsync();
            var employeeToRemove = employees.FirstOrDefault(e => e.Id == id);
            if (employeeToRemove != null)
            {
                employees.Remove(employeeToRemove);
                await WriteToFileAsync(employees);
            }
        }
    }
}
