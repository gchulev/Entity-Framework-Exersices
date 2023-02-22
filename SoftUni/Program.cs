using SoftUni;
using SoftUni.Data;
using SoftUni.Models;

using (var context = new SoftUniContext())
{
    var result = StartUp.GetEmployeesFullInformation(context);
    Console.WriteLine(result);
}