using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeShifts.Models;
using EmployeeShifts.Exceptions;

namespace EmployeeShifts.Services.Repositories
{
    /// <summary>
    /// Implementación del repositorio de empleados en memoria.
    /// Implementa el patrón Repository y el principio DIP (Dependency Inversion Principle).
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Dictionary<int, Employee> _employees;
        private readonly object _lockObject = new object();

        public EmployeeRepository()
        {
            _employees = new Dictionary<int, Employee>();
        }

        public void Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "El empleado no puede ser nulo.");

            lock (_lockObject)
            {
                if (_employees.ContainsKey(employee.Id))
                    throw new EmployeeException($"Ya existe un empleado con el ID {employee.Id}.");

                try
                {
                    _employees.Add(employee.Id, employee);
                }
                catch (Exception ex)
                {
                    throw new EmployeeException($"Error al agregar el empleado: {ex.Message}", ex);
                }
            }
        }

        public void Update(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "El empleado no puede ser nulo.");

            lock (_lockObject)
            {
                if (!_employees.ContainsKey(employee.Id))
                    throw new NotFoundException($"No se encontró un empleado con el ID {employee.Id}.");

                try
                {
                    _employees[employee.Id] = employee;
                }
                catch (Exception ex)
                {
                    throw new EmployeeException($"Error al actualizar el empleado: {ex.Message}", ex);
                }
            }
        }

        public void Delete(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("El ID debe ser positivo.", nameof(employeeId));

            lock (_lockObject)
            {
                if (!_employees.Remove(employeeId))
                    throw new NotFoundException($"No se encontró un empleado con el ID {employeeId}.");
            }
        }

        public Employee GetById(int employeeId)
        {
            if (employeeId <= 0)
                throw new ArgumentException("El ID debe ser positivo.", nameof(employeeId));

            lock (_lockObject)
            {
                if (_employees.TryGetValue(employeeId, out var employee))
                    return employee;

                throw new NotFoundException($"No se encontró un empleado con el ID {employeeId}.");
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            lock (_lockObject)
            {
                return _employees.Values.ToList();
            }
        }
    }
}
