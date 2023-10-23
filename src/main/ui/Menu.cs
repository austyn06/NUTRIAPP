using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NutriApp.UI;

/// <summary>
/// Interface which defines a method handle() to handle menu interactions.
/// The 'actions' dictionary maps user menu options to corresponding invokers. 
/// Which is used to trigger specific commands.
/// </summary>
public interface Menu
{
    void Handle();
}

public interface Help
{
    public List<String> GetOptions();
}

/// <summary>
/// Allows you to pass actions(no return, no param functions) to be invoked like a command.
/// Used to swap menus
/// </summary>
public class ActionInvoker : Invoker
{
    private Action action;

    public ActionInvoker(Action action)
    {
        this.action = action;
    }
    public void Invoke()
    { 
        action?.Invoke();   
    }
}

class FitnessMenu : Menu, Help
{
    private Dictionary<string, Invoker> actions;
    private UIController uIController;


    public FitnessMenu(UIController uiController)
    {
        this.uIController = uiController;
        var addWorkout = new AddWorkoutCommand(uiController.app);
        var setFitness = new SetFitnessGoalCommand(uiController.app);
        var setWeightGoal = new SetWeightGoalCommand(uiController.app);
        var viewTargetCalories = new ViewTargetCaloriesCommand(uiController.app);
        var setWeight = new SetWeightCommand(uiController.app);
        
        actions = new Dictionary<string, Invoker>
        {
            {"Add Workout", new PTAddWorkoutInvoker(addWorkout)},
            {"Set Fitness Goal", new PTSetFitnessGoalInvoker(setFitness)},
            {"Set Weight Goal", new PTSetWeightGoalInvoker(setWeightGoal, uIController.app)},
            {"View Target Calories", new PTViewTargetCaloriesInvoker(viewTargetCalories)},
            {"Set Weight", new PTSetWeightInvoker(setWeight)},
            {"Main Menu", new ActionInvoker(() => uIController.menu = new MainMenu(uIController))},
            {"Help", new PTHelpInvoker(this)}
        };

        new PTAddWorkoutUpdater(addWorkout, uIController.app);
        new PTSetFitnessGoalUpdater(setFitness, uIController.app);
        new PTSetWeightGoalInvoker(setWeightGoal, uIController.app);
        new PTViewTargetCaloriesUpdater(viewTargetCalories, uIController.app);
        
        uIController.app.SubscribeDayEndEvent(PromptWeight);
    }

    public void Handle()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("welcome to the fitness menu");
        while (true)
        {
            Console.WriteLine("please enter a Command (Help to get options)");
            string input = Console.ReadLine();
            if (!actions.ContainsKey(input))
            {
                Console.WriteLine(input + " is not a valid input");
            }
            else
            {
                actions[input].Invoke();
            }
        }       
    }

    /// <summary>
    /// Prompts the weight of user at the end of the day.
    /// (dev-note)Doing the invoker asyncronously was causing problems with the input. So i moved it to manually.
    /// </summary>
    /// <param name="datetime"></param>
    public void PromptWeight(DateTime datetime)
    {
        // var invoker = new PTSetWeightInvoker(new SetWeightCommand(uIController.app));
        // invoker.Invoke();
    }

    public List<string> GetOptions()
    {
        return actions.Keys.ToList();
    }
}


class FoodMenu : Menu, Help
{
    private Dictionary<string, Invoker> actions;
    private UIController uIController;

    public FoodMenu(UIController uiController)
    {
        this.uIController = uiController;
     
        

        
        var consumeMeal = new ConsumeMealCommand(uiController.app);
        var createRecipe = new CreateRecipesCommand(uiController.app);
        var getShoppingList = new GetShoppingListCommand(uiController.app);
        var purchaseFood = new PurchaseFoodCommand(uiController.app);
        var searchIngredients = new SearchingIngredientsCommand(uiController.app, new PTSearchIngredientsUpdater(uiController.app));
        var viewMeals = new ViewMealsCommand();
        var viewRecipes = new ViewRecipesCommand();
        
        actions = new Dictionary<string, Invoker>
            {
                {"Consume Meal", new PTConsumeMealInvoker(consumeMeal)},
                {"Create Recipe", new PTCreateRecipesInvoker(createRecipe, uiController.app)},
                {"Get Shopping List", new PTGetShoppingListInvoker(getShoppingList)},
                {"Purchase Food", new PTPurchaseFoodInvoker(purchaseFood)},
                {"Search Ingredients", new PTSearchIngredientsInvoker(searchIngredients)},
                {"View Meals", new PTViewMealsInvoker(viewMeals)},
                {"View Recipes", new PTViewRecipesInvoker(viewRecipes)},
                {"Main Menu", new ActionInvoker(() => uiController.menu = new MainMenu(uiController))},
                {"Help", new PTHelpInvoker(this)}
            };
        
        new PTConsumeMealUpdater(consumeMeal, uiController.app);
        new PTCreateRecipesUpdater(createRecipe, uiController.app);
        new PTGetShoppingListUpdater(getShoppingList, uiController.app);
        new PTPurchaseFoodUpdater(purchaseFood, uiController.app);
        new PTViewMealsUpdater(viewMeals, uiController.app);
        new PTViewRecipesUpdater(viewRecipes, uiController.app);

    }

    /// <summary>
    /// Sets the current menu to the food menu.
    /// </summary>
    public void Handle()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("welcome to the food menu");
        while (true)
        {
            Console.WriteLine("please enter a Command (Help to get options)");
            string input = Console.ReadLine();
            if (!actions.ContainsKey(input))
            {
                Console.WriteLine(input + " is not a valid input");
            }
            else
            {
                actions[input].Invoke();
            }
        }   
    }


    public List<string> GetOptions()
    {
        return actions.Keys.ToList();
    }
}

class HistoryMenu : Menu, Help
{
    private Dictionary<string, Invoker> actions;
    private UIController uIController;

    public HistoryMenu(UIController uiController)
    {
        this.uIController = uiController;
        var viewCalories = new ViewCaloriesCommand(uiController.app);
        var viewMeals = new ViewMealsEatenCommand(uiController.app);
        var viewWeight = new ViewWeightCommand(uiController.app);
        var viewWorkouts = new ViewWorkoutsCommand(uiController.app);

            actions = new Dictionary<string, Invoker>
            {
                {"View Calories", new PTViewCaloriesInvoker(viewCalories)},
                {"View Meals", new PTViewMealsEatenInvoker(viewMeals)},
                {"View Weight", new PTViewWeightInvoker(viewWeight)},
                {"View Workouts", new PTViewWorkoutsInvoker(viewWorkouts)},
                {"Main Menu", new ActionInvoker(() => uiController.menu = new MainMenu(uiController))},
                {"Help", new PTHelpInvoker(this)}
            };
        
        new PTViewCaloriesUpdater(viewCalories, uiController.app);
        new PTViewMealsEatenUpdater(viewMeals, uiController.app);
        new PTViewWeightUpdater(viewWeight, uiController.app);
        new PTViewWorkoutsUpdater(viewWorkouts, uiController.app);

    }
    

    /// <summary>
    /// Sets the current menu to the history menu.
    /// </summary>
    public void Handle()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("welcome to the history menu");
        while (true)
        {
            Console.WriteLine("please enter a Command (Help to get options)");
            string input = Console.ReadLine();
            if (!actions.ContainsKey(input))
            {
                Console.WriteLine(input + " is not a valid input");
            }
            else
            {
                actions[input].Invoke();
            }
        }    
    }

    public List<string> GetOptions()
    {
        return actions.Keys.ToList();
    }
}


class ProfileMenu : Menu, Help
{
    private Dictionary<string, Invoker> actions;
    private UIController uIController;

    public ProfileMenu(UIController uiController)
    {
        this.uIController = uiController;
        var clearHistory = new ClearHistoryCommand(uiController.app);
        var setDayLength = new SetDayLengthCommand(uiController.app);
        var quit = new QuitCommand(uiController.app);
        actions = new Dictionary<string, Invoker>
            {
                //{"Clear History", new PTClearHistoryInvoker(clearHistory)},
                {"Set Day Length", new PTSetDayLengthInvoker(setDayLength)},
                {"Quit", new PTQuitInvoker(quit)},
                {"Main Menu", new ActionInvoker(() => uiController.menu = new MainMenu(uiController))},
                {"Help", new PTHelpInvoker(this)}
            };
        new PTClearHistoryUpdater(clearHistory, uiController.app);
        new PTSetDayLengthUpdater(setDayLength, uiController.app);
        //no updater for quit
    }

    /// <summary>
    /// Sets the current menu to the profile menu.
    /// </summary>

    public void Handle()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("welcome to the profile menu");
        while (true)
        {
            Console.WriteLine("please enter a Command (Help to get options)");
            string input = Console.ReadLine();
            if (!actions.ContainsKey(input))
            {
                Console.WriteLine(input + " is not a valid input");
            }
            else
            {
                actions[input].Invoke();
            }
        }       }

    public List<string> GetOptions()
    {
        return actions.Keys.ToList();
    }
}

    

class MainMenu : Menu
{
    private UIController uIController;
    
    private Dictionary<string, Menu> _menus;

    public MainMenu(UIController uiController)
    {
        this.uIController = uiController;
        
        _menus = new Dictionary<string, Menu>()
        {
            {"fitness", new FitnessMenu(uiController) },
            {"food", new FoodMenu(uiController)},
            {"history", new HistoryMenu(uiController)},
            {"profile", new ProfileMenu(uiController)},
        };
        
        Handle();
    }

    /// <summary>
    /// Sets the current menu to the main menu.
    /// </summary>
    public void Handle()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("welcome to the main menu");
        while (true)
        {
            Console.WriteLine("please enter a menu to navigate to(fitness, food, history, profile)");
            string input = Console.ReadLine();
            if (!_menus.ContainsKey(input))
            {
                Console.WriteLine(input + " is not a valid input");
            }
            else
            {
                uIController.menu = _menus[input];
            }
        }
    }
}