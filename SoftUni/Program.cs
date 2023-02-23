using SoftUni;
using SoftUni.Data;


using (var context = new SoftUniContext())
{
    string result = StartUp.GetEmployeesFullInformation(context);
    Console.WriteLine(result);
}
