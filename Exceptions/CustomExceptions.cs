using System;

namespace EmployeeShifts.Exceptions
{
    /// <summary>
    /// Excepción personalizada para errores relacionados con empleados.
    /// </summary>
    public class EmployeeException : Exception
    {
        public EmployeeException() { }
        public EmployeeException(string message) : base(message) { }
        public EmployeeException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    /// <summary>
    /// Excepción personalizada para errores relacionados con turnos.
    /// </summary>
    public class ShiftException : Exception
    {
        public ShiftException() { }
        public ShiftException(string message) : base(message) { }
        public ShiftException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    /// <summary>
    /// Excepción personalizada para errores de asignación de turnos.
    /// </summary>
    public class ShiftAssignmentException : Exception
    {
        public ShiftAssignmentException() { }
        public ShiftAssignmentException(string message) : base(message) { }
        public ShiftAssignmentException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    /// <summary>
    /// Excepción personalizada para errores de validación.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    /// <summary>
    /// Excepción personalizada para cuando no se encuentra un recurso.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
