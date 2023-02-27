using SoftUni;
using SoftUni.Data;


using (var context = new SoftUniContext())
{
    //string employeesFullInfo = StartUp.GetEmployeesFullInformation(context);

    //string employeesWithSalaryBiggerThan50000 = StartUp.GetEmployeesWithSalaryOver50000(context);

    //string employeesFromResearchAndDevelopment = StartUp.GetEmployeesFromResearchAndDevelopment(context);

    //string addressTextResult = StartUp.AddNewAddressToEmployee(context).TrimEnd();

    //string employeesInPeriod = StartUp.GetEmployeesInPeriod(context);

    //string addressesByTown = StartUp.GetAddressesByTown(context);

    //string employee147 = StartUp.GetEmployee147(context);

    //string deparmtentsWithMorethan5Employees = StartUp.GetDepartmentsWithMoreThan5Employees(context);

    //string last10Projects = StartUp.GetLatestProjects(context);

    //string employeesWithIncreasedSalaries = StartUp.IncreaseSalaries(context);

    //string employeesWithNameStartingWithSA = StartUp.GetEmployeesByFirstNameStartingWithSa(context);

    //string tenProjects = StartUp.DeleteProjectById(context);

    string removedAddressesInfo = StartUp.RemoveTown(context);

    Console.WriteLine(removedAddressesInfo);
}
