using System;

namespace EmployeeShifts.Models
{
    /// <summary>
    /// Representa un conflicto de turnos entre dos empleados.
    /// </summary>
    public class ShiftConflict
    {
        public int EmployeeId1 { get; set; }
        public string EmployeeName1 { get; set; }
        public int EmployeeId2 { get; set; }
        public string EmployeeName2 { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ConflictReason { get; set; }

        public override string ToString()
        {
            return $"Conflicto: {EmployeeName1} y {EmployeeName2} en turno '{ShiftName}' ({StartTime:yyyy-MM-dd HH:mm} - {EndTime:yyyy-MM-dd HH:mm}). Razón: {ConflictReason}";
        }
    }
}
