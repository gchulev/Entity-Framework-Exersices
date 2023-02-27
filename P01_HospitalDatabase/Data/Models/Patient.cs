using System.ComponentModel;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            this.Prescriptions = new HashSet<PatientMedicament>();
        }
        public int PatientId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Email { get; set; }
        public bool HasInsurance { get; set; }
        public Diagnose? Diagnose { get; set; }
        public ICollection<PatientMedicament> Prescriptions { get; set; }
        public ICollection<Visitation>? Visitations { get; set; }
        public ICollection<Diagnose>? Diagnoses { get; set; }
    }
}
