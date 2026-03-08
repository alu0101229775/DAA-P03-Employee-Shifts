using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeShifts.Models;
using EmployeeShifts.Exceptions;

namespace EmployeeShifts.Services
{
    /// <summary>
    /// Servicio para detectar y validar conflictos entre turnos.
    /// Implementa Responsabilidad Única (SRP).
    /// </summary>
    public class ShiftConflictValidator : IShiftConflictValidator
    {
        private readonly IEmployeeRepository _employeeRepository;
        private static readonly object _lockObject = new object();

        public ShiftConflictValidator(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public bool HasConflict(Employee employee, Shift shift)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "El empleado no puede ser nulo.");

            if (shift == null)
                throw new ArgumentNullException(nameof(shift), "El turno no puede ser nulo.");

            lock (_lockObject)
            {
                try
                {
                    return employee.AssignedShifts.Any(s => s.OverlapsWith(shift));
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error al validar conflictos: {ex.Message}", ex);
                }
            }
        }

        public IEnumerable<ShiftConflict> DetectAllConflicts()
        {
            lock (_lockObject)
            {
                var conflicts = new List<ShiftConflict>();

                try
                {
                    var employees = _employeeRepository.GetAll().ToList();

                    for (int i = 0; i < employees.Count; i++)
                    {
                        for (int j = i + 1; j < employees.Count; j++)
                        {
                            var emp1 = employees[i];
                            var emp2 = employees[j];

                            var conflictShifts = DetectConflictsBetweenEmployees(emp1, emp2);
                            conflicts.AddRange(conflictShifts);
                        }
                    }

                    return conflicts;
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error al detectar conflictos globales: {ex.Message}", ex);
                }
            }
        }

        private List<ShiftConflict> DetectConflictsBetweenEmployees(Employee emp1, Employee emp2)
        {
            var conflicts = new List<ShiftConflict>();

            foreach (var shift1 in emp1.AssignedShifts)
            {
                foreach (var shift2 in emp2.AssignedShifts)
                {
                    if (shift1.OverlapsWith(shift2))
                    {
                        conflicts.Add(new ShiftConflict
                        {
                            EmployeeId1 = emp1.Id,
                            EmployeeName1 = emp1.Name,
                            EmployeeId2 = emp2.Id,
                            EmployeeName2 = emp2.Name,
                            ShiftId = shift1.Id,
                            ShiftName = shift1.Name,
                            StartTime = shift1.StartTime,
                            EndTime = shift1.EndTime,
                            ConflictReason = "Ambos empleados están asignados a turnos que se solapan"
                        });
                    }
                }
            }

            return conflicts;
        }
    }
}
