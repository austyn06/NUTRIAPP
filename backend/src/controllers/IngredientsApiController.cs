﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriApp.Controllers.Models;
using NutriApp;
using NutriApp.Controllers.Middleware;
using NutriApp.Food;
using Ingredient = NutriApp.Controllers.Models.Ingredient;

namespace NutriApp.Controllers;

[Route("api/Ingredients")]
[ApiController]
public class IngredientsApiController : ControllerBase
{
    private readonly App _app;

    public IngredientsApiController(App app)
    {
        _app = app;
    }
    
    // GET api/Ingredients/{name}
    [HttpGet("{name}")]
    public ActionResult<Ingredient> GetIngredient(string name)
    {
        var ing = _app.FoodControl.GetIngredient(name);
        return ing != null ? Ingredient.FromIngredient(ing) : new Ingredient();
    }
    
    // GET api/Ingredients/search/{name}
    [HttpGet("search/{name}")]
    public ActionResult<IEnumerable<Ingredient>> SearchIngredients(string name)
    {
        var ings = _app.FoodControl.SearchIngredients(name);
        return ings.Select(Ingredient.FromIngredient).ToList();
    }
    
    // Get api/Ingredients/stock
    [HttpGet("stock")]
    [Authorize]
    public ActionResult<IEnumerable<IngredientStockModel>> GetStock()
    {
        var user = HttpContext.GetUser();
        return _app.FoodControl.GetAllIngredientStocks(user.UserName)?.ToDictionary()
            .Select(pair => new IngredientStockModel { Name = pair.Key, Quantity = double.Parse(pair.Value) })
            .ToList() ?? new List<IngredientStockModel>();
    }
    
    // PUT api/Ingredients/purchase
    [HttpPut("purchase")]
    [Authorize]
    public IActionResult PurchaseIngredients(PurchaseIngredientInfo[] info)
    {
        var user = HttpContext.GetUser();
        foreach (var purchase in info)
        {
            _app.FoodControl.EditIngredientStock(purchase.Name, purchase.Quantity, user.UserName);
        }
        return NoContent();
    }
}