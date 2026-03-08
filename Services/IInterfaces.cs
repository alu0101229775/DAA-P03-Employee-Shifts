using System;
using System.Collections.Generic;
using EmployeeShifts.Models;

namespace EmployeeShifts.Services
{
    /// <summary>
    /// Contrato para el repositorio de empleados (Patrón Repository - Inversión de Control).
    /// Implementa el principio de Segregación de Interfaz (ISP).
    /// </summary>
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int employeeId);
        Employee GetById(int employeeId);
        IEnumerable<Employee> GetAll();
    }

    /// <summary>
    /// Contrato para el repositorio de turnos.
    /// </summary>
    public interface IShiftRepository
    {
        void Add(Shift shift);
        void Update(Shift shift);
        void Delete(int shiftId);
        Shift GetById(int shiftId);
        IEnumerable<Shift> GetAll();
        IEnumerable<Shift> GetByDateRange(DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Contrato para asignación de turnos (Responsabilidad Única).
    /// </summary>
    public interface IShiftAssignmentService
    {
        void AssignShiftToEmployee(int employeeId, int shiftId);
        void RemoveShiftFromEmployee(int employeeId, int shiftId);
        IEnumerable<Shift> GetEmployeeShifts(int employeeId);
        IEnumerable<Employee> GetShiftEmployees(int shiftId);
    }

    /// <summary>
    /// Contrato para validación de conflictos de turnos.
    /// </summary>
    public interface IShiftConflictValidator
    {
        bool HasConflict(Employee employee, Shift shift);
        IEnumerable<ShiftConflict> DetectAllConflicts();
    }

    /// <summary>
    /// Contrato para búsqueda y filtrado de empleados.
    /// </summary>
    public interface IEmployeeSearchService
    {
        IEnumerable<Employee> SearchByRole(EmployeeRole role);
        IEnumerable<Employee> SearchByName(string namePattern);
        Employee SearchByEmail(string email);
    }

    /// <summary>
    /// Contrato para generación de reportes.
    /// </summary>
    public interface IReportGenerator
    {
        string GenerateEmployeeShiftReport(int employeeId);
        string GenerateScheduleReport(DateTime startDate, DateTime endDate);
        string GenerateConflictReport();
    }
}
