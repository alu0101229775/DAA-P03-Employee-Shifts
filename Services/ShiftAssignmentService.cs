using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeShifts.Models;
using EmployeeShifts.Exceptions;

namespace EmployeeShifts.Services
{
    /// <summary>
    /// Servicio para asignar y desasignar turnos a empleados.
    /// Implementa Responsabilidad Única (SRP).
    /// </summary>
    public class ShiftAssignmentService : IShiftAssignmentService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IShiftConflictValidator _conflictValidator;
        private readonly object _lockObject = new object();

        public ShiftAssignmentService(
            IEmployeeRepository employeeRepository,
            IShiftRepository shiftRepository,
            IShiftConflictValidator conflictValidator)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _shiftRepository = shiftRepository ?? throw new ArgumentNullException(nameof(shiftRepository));
            _conflictValidator = conflictValidator ?? throw new ArgumentNullException(nameof(conflictValidator));
        }

        public void AssignShiftToEmployee(int employeeId, int shiftId)
        {
            if (employeeId <= 0)
                throw new ValidationException("El ID del empleado debe ser positivo.");

            if (shiftId <= 0)
                throw new ValidationException("El ID del turno debe ser positivo.");

            lock (_lockObject)
            {
                try
                {
                    var employee = _employeeRepository.GetById(employeeId);
                    var shift = _shiftRepository.GetById(shiftId);

                    if (_conflictValidator.HasConflict(employee, shift))
                        throw new ShiftAssignmentException($"El turno '{shift.Name}' entra en conflicto con otros turnos del empleado.");

                    employee.AssignShift(shift);
                }
                catch (NotFoundException ex)
                {
                    throw new ShiftAssignmentException($"No se pudo asignar el turno: {ex.Message}", ex);
                }
                catch (InvalidOperationException ex)
                {
                    throw new ShiftAssignmentException($"Error en la asignación: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new ShiftAssignmentException($"Error inesperado al asignar el turno: {ex.Message}", ex);
                }
            }
        }

        public void RemoveShiftFromEmployee(int employeeId, int shiftId)
        {
            if (employeeId <= 0)
                throw new ValidationException("El ID del empleado debe ser positivo.");

            if (shiftId <= 0)
                throw new ValidationException("El ID del turno debe ser positivo.");

            lock (_lockObject)
            {
                try
                {
                    var employee = _employeeRepository.GetById(employeeId);
                    var shift = _shiftRepository.GetById(shiftId);

                    employee.RemoveShift(shift);
                }
                catch (NotFoundException ex)
                {
                    throw new ShiftAssignmentException($"No se pudo remover el turno: {ex.Message}", ex);
                }
                catch (InvalidOperationException ex)
                {
                    throw new ShiftAssignmentException($"Error en la remoción: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new ShiftAssignmentException($"Error inesperado al remover el turno: {ex.Message}", ex);
                }
            }
        }

        public IEnumerable<Shift> GetEmployeeShifts(int employeeId)
        {
            if (employeeId <= 0)
                throw new ValidationException("El ID del empleado debe ser positivo.");

            try
            {
                var employee = _employeeRepository.GetById(employeeId);
                return employee.AssignedShifts;
            }
            catch (NotFoundException ex)
            {
                throw new ShiftAssignmentException($"No se encontró el empleado: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ShiftAssignmentException($"Error al obtener los turnos del empleado: {ex.Message}", ex);
            }
        }

        public IEnumerable<Employee> GetShiftEmployees(int shiftId)
        {
            if (shiftId <= 0)
                throw new ValidationException("El ID del turno debe ser positivo.");

            try
            {
                _shiftRepository.GetById(shiftId);
                var employees = _employeeRepository.GetAll();
                return employees.Where(e => e.AssignedShifts.Any(s => s.Id == shiftId)).ToList();
            }
            catch (NotFoundException ex)
            {
                throw new ShiftAssignmentException($"No se encontró el turno: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ShiftAssignmentException($"Error al obtener los empleados del turno: {ex.Message}", ex);
            }
        }
    }
}
