﻿namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using FastFood.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;

        public ItemsController(FastFoodContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Create()
        {
            List<CreateItemViewModel> createItemView = await _context.Categories
                .AsNoTracking()
                .ProjectTo<CreateItemViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(createItemView);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemInputModel model)
        {
            var item = _mapper.Map<Item>(model);

            await _context.Items.AddAsync(item);

            await _context.SaveChangesAsync();

            return RedirectToAction("All", "Items");
        }

        public async Task<IActionResult> All()
        {
            ItemsAllViewModels[] items = await _context.Items
                .ProjectTo<ItemsAllViewModels>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return View(items);
        }
    }
}
