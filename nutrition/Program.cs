using System;
using System.Collections.Generic;
using System.Linq;

namespace NutritionTracker
{
    class Program
    {
        static List<Food> foodDatabase = new List<Food>();
        static List<Meal> mealLog = new List<Meal>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n*** Main Menu ***");
                Console.WriteLine("1. Manage Food Items");
                Console.WriteLine("2. Log Meal");
                Console.WriteLine("3. Generate Reports");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ManageFoodItems();
                        break;
                    case "2":
                        LogMeal();
                        break;
                    case "3":
                        GenerateReports();
                        break;
                    case "4":
                        Console.WriteLine("Exiting application. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid input! Please choose from 1 to 4.");
                        break;
                }
            }
        }

        static void ManageFoodItems()
        {
            while (true)
            {
                Console.WriteLine("\n*** Food Items Menu ***");
                Console.WriteLine("1. Add Food");
                Console.WriteLine("2. View Foods");
                Console.WriteLine("3. Remove Food");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddFood();
                        break;
                    case "2":
                        ViewFoods();
                        break;
                    case "3":
                        RemoveFood();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid input! Please choose from 1 to 4.");
                        break;
                }
            }
        }

        static void AddFood()
        {
            Console.Write("Enter food name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name) || int.TryParse(name, out _))
            {
                Console.WriteLine("Invalid food name. It must be non-empty and not a number.");
                return;
            }
            Console.Write("Enter calorie count: ");
            if (int.TryParse(Console.ReadLine(), out int calories) && calories > 0)
            {
                foodDatabase.Add(new Food { Name = name, Calories = calories });
                Console.WriteLine("Food item added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid calorie input. Must be a positive number.");
            }
        }

        static void ViewFoods()
        {
            if (!foodDatabase.Any())
            {
                Console.WriteLine("No food items in the database.");
                return;
            }

            Console.WriteLine("\n*** Food Items List ***");
            for (int i = 0; i < foodDatabase.Count; i++)
            {
                Console.WriteLine($"{i}. {foodDatabase[i].Name} - {foodDatabase[i].Calories} cal");
            }
        }

        static void RemoveFood()
        {
            ViewFoods();
            Console.Write("Enter the index of the food to remove: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < foodDatabase.Count)
            {
                foodDatabase.RemoveAt(index);
                Console.WriteLine("Food item removed successfully.");
            }
            else
            {
                Console.WriteLine("Invalid index selected.");
            }
        }

        static void LogMeal()
        {
            if (!foodDatabase.Any())
            {
                Console.WriteLine("No food items to log. Add food first.");
                return;
            }

            List<Meal> tempMeals = new List<Meal>();

            while (true)
            {
                ViewFoods();
                Console.Write("Select the index of the food to log: ");
                if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= foodDatabase.Count)
                {
                    Console.WriteLine("Invalid index.");
                    continue; // Loop again
                }

                Console.Write("Enter quantity: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity. Must be a positive number.");
                    continue; // Loop again
                }

                Food selected = foodDatabase[index];
                int totalCalories = selected.Calories * quantity;

                tempMeals.Add(new Meal
                {
                    FoodName = selected.Name,
                    Quantity = quantity,
                    TotalCalories = totalCalories,
                    Date = DateTime.Now.Date
                });

                Console.Write("Add another food? (Y/N): ");
                string choice = Console.ReadLine().Trim().ToUpper();
                if (choice != "Y")
                {
                    break; // Exit the loop
                }
            }

            // After loop, save all logged meals
            if (tempMeals.Any())
            {
                mealLog.AddRange(tempMeals);
                Console.WriteLine("Meals logged successfully.");
            }
            else
            {
                Console.WriteLine("No valid meals were logged.");
            }
        }


        static void GenerateReports()
        {
            while (true)
            {
                Console.WriteLine("\n*** Report Menu ***");
                Console.WriteLine("1. Daily Report");
                Console.WriteLine("2. Weekly/Monthly Average Report");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DailyReport();
                        break;
                    case "2":
                        WeeklyMonthlyReport();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid input! Please choose from 1 to 3.");
                        break;
                }
            }
        }

        static void DailyReport()
        {
            DateTime today = DateTime.Now.Date;
            var todayMeals = mealLog.Where(m => m.Date == today);
            int total = todayMeals.Sum(m => m.TotalCalories);

            Console.WriteLine($"\nTotal Calories Consumed on {today.ToShortDateString()}: {total} cal");
        }

        static void WeeklyMonthlyReport()
        {
            Console.Write("Enter report period (weekly/monthly): ");
            string period = Console.ReadLine().ToLower();

            DateTime startDate;
            if (period == "weekly")
            {
                startDate = DateTime.Now.AddDays(-7);//subtracting seven days from the current date
            }
            else if (period == "monthly")
            {
                startDate = DateTime.Now.AddMonths(-1);//subtracts exactly one month from the current date and time
            }
            else
            {
                Console.WriteLine("Invalid period input.");
                return;
            }

            var filteredMeals = mealLog.Where(m => m.Date >= startDate);
            int totalCalories = filteredMeals.Sum(m => m.TotalCalories);
            int days = (DateTime.Now.Date - startDate.Date).Days + 1;

            double avgCalories = days > 0 ? (double)totalCalories / days : 0;
            Console.WriteLine($"\nAverage Daily Calories ({period}): {avgCalories:F2} cal");
        }
    }

    class Food
    {
        public string Name { get; set; }
        public int Calories { get; set; }
    }

    class Meal
    {
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public int TotalCalories { get; set; }
        public DateTime Date { get; set; }
    }
}

