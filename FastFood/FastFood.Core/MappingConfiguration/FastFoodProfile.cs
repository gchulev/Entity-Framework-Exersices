namespace FastFood.Core.MappingConfiguration;

using AutoMapper;

using FastFood.Core.ViewModels.Categories;
using FastFood.Core.ViewModels.Employees;
using FastFood.Core.ViewModels.Items;
using FastFood.Core.ViewModels.Orders;
using FastFood.Models;

using Microsoft.Build.Framework;

using ViewModels.Positions;

public class FastFoodProfile : Profile
{
    public FastFoodProfile()
    {
        //Positions
        CreateMap<CreatePositionInputModel, Position>()
            .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

        CreateMap<Position, PositionsAllViewModel>()
            .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));


        //Orders
        CreateMap<CreateOrderInputModel, Order>();

        CreateMap<Order, OrderAllViewModel>()
            .ForMember(d => d.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Employee, opt => opt.MapFrom(src => src.Employee.Name))
            .ForMember(d => d.DateTime, opt => opt.MapFrom(src => src.DateTime));

        CreateMap<OrderAllViewModel, Order>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.OrderId));


        //Items
        CreateMap<Category, CreateItemViewModel>()
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.Id));

        CreateMap<CreateItemInputModel, Item>();

        CreateMap<Item, ItemsAllViewModels>()
            .ForMember(d => d.Category, opt => opt.MapFrom(src => src.Category.Name));


        //Categories
        CreateMap<CreateCategoryInputModel, Category>()
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.CategoryName));

        CreateMap<Category, CategoryAllViewModel>();


        //Employees
        CreateMap<RegisterEmployeeInputModel, Employee>();

        CreateMap<Position, RegisterEmployeeViewModel>()
            .ForMember(d => d.PositionId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Employee, EmployeesAllViewModel>()
            .ForMember(d => d.Position, opt => opt.MapFrom(src => src.Position.Name));
    }
}
