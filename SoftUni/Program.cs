
using Softuni;
using Softuni.Data;

using (SoftUniContext context = new SoftUniContext())
{
    string result = await StartUp.GetEmployeesFullInformationAsync(context);

    Console.WriteLine(result);

}