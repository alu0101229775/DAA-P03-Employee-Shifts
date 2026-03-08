using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmployeeShifts.Models;
using EmployeeShifts.Exceptions;

namespace EmployeeShifts.Services
{
    /// <summary>
    /// Servicio para generar reportes de turnos y empleados.
    /// Implementa Responsabilidad Única (SRP).
    /// </summary>
    public class ReportGenerator : IReportGenerator
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IShiftConflictValidator _conflictValidator;
        private readonly object _lockObject = new object();

        public ReportGenerator(
            IEmployeeRepository employeeRepository,
            IShiftRepository shiftRepository,
            IShiftConflictValidator conflictValidator)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _shiftRepository = shiftRepository ?? throw new ArgumentNullException(nameof(shiftRepository));
            _conflictValidator = conflictValidator ?? throw new ArgumentNullException(nameof(conflictValidator));
        }

        public string GenerateEmployeeShiftReport(int employeeId)
        {
            if (employeeId <= 0)
                throw new ValidationException("El ID del empleado debe ser positivo.");

            lock (_lockObject)
            {
                try
                {
                    var employee = _employeeRepository.GetById(employeeId);
                    var report = new StringBuilder();

                    report.AppendLine("=".PadRight(80, '='));
                    report.AppendLine($"REPORTE DE TURNOS ASIGNADOS - EMPLEADO: {employee.Name}");
                    report.AppendLine("=".PadRight(80, '='));
                    report.AppendLine();
                    report.AppendLine($"ID Empleado:     {employee.Id}");
                    report.AppendLine($"Email:           {employee.Email}");
                    report.AppendLine($"Rol:             {employee.Role}");
                    report.AppendLine($"Fecha Creación:  {employee.CreatedDate:yyyy-MM-dd HH:mm:ss}");
                    report.AppendLine();
                    report.AppendLine($"Total de turnos asignados: {employee.AssignedShifts.Count}");
                    report.AppendLine();

                    if (employee.AssignedShifts.Count == 0)
                    {
                        report.AppendLine("No hay turnos asignados a este empleado.");
                    }
                    else
                    {
                        report.AppendLine("-".PadRight(80, '-'));
                        report.AppendLine($"{"ID Turno",-10} {"Nombre",-20} {"Inicio",-20} {"Fin",-20} {"Tipo",-10}");
                        report.AppendLine("-".PadRight(80, '-'));

                        foreach (var shift in employee.AssignedShifts.OrderBy(s => s.StartTime))
                        {
                            report.AppendLine($"{shift.Id,-10} {shift.Name,-20} {shift.StartTime:yyyy-MM-dd HH:mm,-20} {shift.EndTime:yyyy-MM-dd HH:mm,-20} {shift.Type,-10}");
                        }
                    }

                    report.AppendLine();
                    report.AppendLine("=".PadRight(80, '='));

                    return report.ToString();
                }
                catch (NotFoundException ex)
                {
                    throw new ValidationException($"Error al generar reporte: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error inesperado al generar reporte: {ex.Message}", ex);
                }
            }
        }

        public string GenerateScheduleReport(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ValidationException("La fecha de inicio debe ser anterior a la fecha de fin.");

            lock (_lockObject)
            {
                try
                {
                    var shifts = _shiftRepository.GetByDateRange(startDate, endDate).ToList();
                    var employees = _employeeRepository.GetAll().ToList();
                    var report = new StringBuilder();

                    report.AppendLine("=".PadRight(100, '='));
                    report.AppendLine($"REPORTE DE CALENDARIO - DEL {startDate:yyyy-MM-dd} AL {endDate:yyyy-MM-dd}");
                    report.AppendLine("=".PadRight(100, '='));
                    report.AppendLine();
                    report.AppendLine($"Total de turnos en el período: {shifts.Count}");
                    report.AppendLine();

                    if (shifts.Count == 0)
                    {
                        report.AppendLine("No hay turnos en el período especificado.");
                    }
                    else
                    {
                        report.AppendLine("-".PadRight(100, '-'));
                        report.AppendLine($"{"ID",-6} {"Turno",-20} {"Inicio",-20} {"Fin",-20} {"Tipo",-15} {"Empleados",-18}");
                        report.AppendLine("-".PadRight(100, '-'));

                        foreach (var shift in shifts.OrderBy(s => s.StartTime))
                        {
                            var shiftEmployees = employees
                                .Where(e => e.AssignedShifts.Any(s => s.Id == shift.Id))
                                .Select(e => e.Name)
                                .ToList();

                            var employeeNames = shiftEmployees.Count > 0 
                                ? string.Join(", ", shiftEmployees) 
                                : "Sin asignar";

                            report.AppendLine($"{shift.Id,-6} {shift.Name,-20} {shift.StartTime:yyyy-MM-dd HH:mm,-20} {shift.EndTime:yyyy-MM-dd HH:mm,-20} {shift.Type,-15} {employeeNames,-18}");
                        }
                    }

                    report.AppendLine();
                    report.AppendLine("=".PadRight(100, '='));

                    return report.ToString();
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error al generar reporte de calendario: {ex.Message}", ex);
                }
            }
        }

        public string GenerateConflictReport()
        {
            lock (_lockObject)
            {
                try
                {
                    var conflicts = _conflictValidator.DetectAllConflicts().ToList();
                    var report = new StringBuilder();

                    report.AppendLine("=".PadRight(120, '='));
                    report.AppendLine("REPORTE DE CONFLICTOS DE TURNOS");
                    report.AppendLine("=".PadRight(120, '='));
                    report.AppendLine();
                    report.AppendLine($"Total de conflictos detectados: {conflicts.Count}");
                    report.AppendLine();

                    if (conflicts.Count == 0)
                    {
                        report.AppendLine("No hay conflictos de turnos detectados.");
                    }
                    else
                    {
                        report.AppendLine("-".PadRight(120, '-'));
                        foreach (var conflict in conflicts.OrderByDescending(c => c.StartTime))
                        {
                            report.AppendLine(conflict.ToString());
                        }
                    }

                    report.AppendLine();
                    report.AppendLine("=".PadRight(120, '='));

                    return report.ToString();
                }
                catch (Exception ex)
                {
                    throw new ValidationException($"Error al generar reporte de conflictos: {ex.Message}", ex);
                }
            }
        }
    }
}
