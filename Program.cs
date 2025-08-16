using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }

    public Ingredient(string name, double quantity, string unit)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
    }

    public Ingredient Scale(double scaleFactor)
    {
        return new Ingredient(Name, Quantity * scaleFactor, Unit);
    }

    public static double NormalizeUnit(double quantity, string unit)
    {
        switch (unit.ToLower())
        {
            case "kg":
                return quantity * 1000; 
            case "g":
            default:
                return quantity; 
        }
    }
}

public class Recipe
{
    public List<Ingredient> Ingredients { get; set; }
    public double BaseServings { get; set; }

    public Recipe(List<Ingredient> ingredients, double baseServings)
    {
        Ingredients = ingredients;
        BaseServings = baseServings;
    }

    public List<Ingredient> ScaleRecipe(double targetServings)
    {
        double scaleFactor = targetServings / BaseServings;
        return Ingredients.Select(ingredient => ingredient.Scale(scaleFactor)).ToList();
    }

    public List<(string Name, double Required, double Available)> CheckShortages(Dictionary<string, double> pantry)
    {
        var shortages = new List<(string Name, double Required, double Available)>();

        foreach (var ingredient in Ingredients)
        {
            double requiredQuantity = Ingredient.NormalizeUnit(ingredient.Quantity, ingredient.Unit);
            double availableQuantity = pantry.ContainsKey(ingredient.Name) ? pantry[ingredient.Name] : 0;

            if (requiredQuantity > availableQuantity)
            {
                shortages.Add((ingredient.Name, requiredQuantity, availableQuantity));
            }
        }

        return shortages;
    }
}

namespace Recipe_Scaler___Pantry_Checker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ingredients = new List<Ingredient>
        {
            new Ingredient("Flour", 500, "g"),
            new Ingredient("Sugar", 200, "g"),
            new Ingredient("Butter", 1, "kg")
        };

            var recipe = new Recipe(ingredients, 4); 

            var scaledIngredients = recipe.ScaleRecipe(10);
            Console.WriteLine("Scaled Ingredients:");
            foreach (var ingredient in scaledIngredients)
            {
                Console.WriteLine($"{ingredient.Name}: {ingredient.Quantity} {ingredient.Unit}");
            }

            var pantry = new Dictionary<string, double>
        {
            { "Flour", 300 }, 
            { "Sugar", 150 }, 
            { "Butter", 0.5 } 
        };

            var shortages = recipe.CheckShortages(pantry);
            Console.WriteLine("\nShopping List for Shortages:");
            foreach (var shortage in shortages)
            {
                Console.WriteLine($"{shortage.Name}: Required {shortage.Required}g, Available {shortage.Available}g");
            }
        }
    }
}
