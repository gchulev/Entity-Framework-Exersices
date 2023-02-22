using System.ComponentModel.DataAnnotations.Schema;

namespace SoftUni.Models
{
    public class EmployeeProject
    {
        public EmployeeProject()
        {
            Employees = new HashSet<Employee>();
            Projects = new HashSet<Project>();
        }
        
        public int EmployeeID { get; set; }
        
        public int ProjectID { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
