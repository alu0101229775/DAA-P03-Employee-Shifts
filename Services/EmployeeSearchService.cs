using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeShifts.Models;
using EmployeeShifts.Exceptions;

namespace EmployeeShifts.Services
{
    /// <summary>
    /// Servicio para búsqueda y filtrado de empleados.
    /// Implementa Responsabilidad Única (SRP).
    /// </summary>
    public class EmployeeSearchService : IEmployeeSearchService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly object _lockObject = new object();

        public EmployeeSearchService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public IEnumerable<Employee> SearchByRole(EmployeeRole role)
        {
            lock (_lockObject)
            {
                try
                {
                    var employees = _employeeRepository.GetAll();
                    return employees.Where(e => e.Role == role).ToList();
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error al buscar empleados por rol: {ex.Message}", ex);
                }
            }
        }

        public IEnumerable<Employee> SearchByName(string namePattern)
        {
            if (string.IsNullOrWhiteSpace(namePattern))
                throw new ValidationException("El patrón de búsqueda no puede estar vacío.");

            lock (_lockObject)
            {
                try
                {
                    var employees = _employeeRepository.GetAll();
                    var pattern = namePattern.ToLower().Trim();
                    return employees.Where(e => e.Name.ToLower().Contains(pattern)).ToList();
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error al buscar empleados por nombre: {ex.Message}", ex);
                }
            }
        }

        public Employee SearchByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("El email no puede estar vacío.");

            lock (_lockObject)
            {
                try
                {
                    var employees = _employeeRepository.GetAll();
                    var employee = employees.FirstOrDefault(e => e.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                    if (employee == null)
                        throw new NotFoundException($"No se encontró un empleado con el email: {email}");

                    return employee;
                }
                catch (NotFoundException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error al buscar empleado por email: {ex.Message}", ex);
                }
            }
        }
    }
}
