using SoftUni;
using SoftUni.Data;


using (var context = new SoftUniContext())
{
    //string employeesFullInfo = StartUp.GetEmployeesFullInformation(context);

    //string employeesWithSalaryBiggerThan50000 = StartUp.GetEmployeesWithSalaryOver50000(context);

    //string employeesFromResearchAndDevelopment = StartUp.GetEmployeesFromResearchAndDevelopment(context);

    //string addressTextResult = StartUp.AddNewAddressToEmployee(context).TrimEnd();

    //string employeesInPeriod = StartUp.GetEmployeesInPeriod(context);

    string addressesByTown = StartUp.GetAddressesByTown(context);

    //string employee147 = StartUp.GetEmployee147(context);

    Console.WriteLine(addressesByTown);
}
