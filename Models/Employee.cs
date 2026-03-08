using System;
using System.Collections.Generic;

namespace EmployeeShifts.Models
{
    /// <summary>
    /// Representa un empleado en el sistema de gestión de turnos.
    /// </summary>
    public class Employee
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public EmployeeRole Role { get; private set; }
        public List<Shift> AssignedShifts { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public Employee(int id, string name, string email, EmployeeRole role)
        {
            ValidateEmployee(id, name, email);
            
            Id = id;
            Name = name;
            Email = email;
            Role = role;
            AssignedShifts = new List<Shift>();
            CreatedDate = DateTime.UtcNow;
        }

        private static void ValidateEmployee(int id, string name, string email)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del empleado debe ser positivo.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del empleado no puede estar vacío.", nameof(name));

            if (name.Length > 100)
                throw new ArgumentException("El nombre del empleado no puede exceder 100 caracteres.", nameof(name));

            if (!IsValidEmail(email))
                throw new ArgumentException("El formato del email es inválido.", nameof(email));
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void AssignShift(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift), "El turno no puede ser nulo.");

            if (AssignedShifts.Contains(shift))
                throw new InvalidOperationException("Este turno ya está asignado al empleado.");

            if (HasConflict(shift))
                throw new InvalidOperationException("El turno entra en conflicto con otro turno asignado.");

            AssignedShifts.Add(shift);
        }

        public void RemoveShift(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift), "El turno no puede ser nulo.");

            if (!AssignedShifts.Remove(shift))
                throw new InvalidOperationException("El turno no está asignado a este empleado.");
        }

        private bool HasConflict(Shift newShift)
        {
            foreach (var shift in AssignedShifts)
            {
                if (shift.OverlapsWith(newShift))
                    return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Employee employee)
                return Id == employee.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Empleado: {Name} (ID: {Id}, Rol: {Role})";
        }
    }
}
