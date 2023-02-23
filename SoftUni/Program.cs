using SoftUni;
using SoftUni.Data;


using (var context = new SoftUniContext())
{
    string employeesFullInfo = StartUp.GetEmployeesFullInformation(context);

    string employeesWithSalaryBiggerThan50000 = StartUp.GetEmployeesWithSalaryOver50000(context);

    string employeesFromResearchAndDevelopment = StartUp.GetEmployeesFromResearchAndDevelopment(context);



    Console.WriteLine(employeesFromResearchAndDevelopment);
}
