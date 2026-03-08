namespace EmployeeShifts.Models
{
    /// <summary>
    /// Define los roles disponibles para los empleados.
    /// </summary>
    public enum EmployeeRole
    {
        Manager = 1,
        Supervisor = 2,
        Worker = 3
    }

    /// <summary>
    /// Define los tipos de turnos disponibles.
    /// </summary>
    public enum ShiftType
    {
        Morning = 1,
        Afternoon = 2,
        Night = 3,
        FullDay = 4
    }
}
