
using EntityFrameworkIntroduction;
using EntityFrameworkIntroduction.Data;
using EntityFrameworkIntroduction.Models;

using (SoftUniContext context = new SoftUniContext())
{
    string result = await StartUp.GetEmployeesFullInformationAsync(context);

    Console.WriteLine(result);

}