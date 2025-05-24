/****************************************************************************************
 * Personal Expense Tracker - Console App
 * 
 * Features:
 *  Expense data model (classes, encapsulation)
 *  Adding new expenses (user input)
 *  Listing expenses (with optional filtering)
 *  Categories (grouping, LINQ)
 *  Simple statistics/reports (total spent, by category/date)
 *  Saving/loading data (File I/O: JSON)
 *  Nice console UI (menus, error handling)?
 ****************************************************************************************/

namespace PersonalExpenseTracker;
using System.Globalization;
using System.IO;
using System.Text.Json;

public class Program
{
    // List to store all expenses in memory
    static List<Expense> expenses = new();

    // Define the file name for saving/loading data
    const string FileName = "expenses.json";

    // Using the CultureInfo system and always get Hungarian formatting:
    static readonly CultureInfo huCulture = new CultureInfo("hu-HU");

    // Predefined categories
    static List<string> categories = new List<string>
    {
        "Groceries", "Housing", "Restaurants", "Hobbies", "Car Services", "Communication", "Insurances", "Fuel", "Others"
    };

    #region Main function
    static void Main()
    {
        Console.WriteLine("=== Personal Expense Tracker ===\n");

        LoadExpenses();

        // Menu for adding or listing expenses, or exiting
        while (true)
        {
            Console.WriteLine("1. Add new expense");
            Console.WriteLine("2. List all expenses");
            Console.WriteLine("3. List expenses by category");
            Console.WriteLine("4. List expenses by date range");
            Console.WriteLine("5. Show statistics");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            if (option == "1")
                AddExpense();
            else if (option == "2")
                ListExpenses();
            else if (option == "3")
                ListByCategory();
            else if (option == "4")
                ListByDateRange();
            else if (option == "5")
                ShowStatistics();
            else if (option == "6")
                break;
            else
                Console.WriteLine("Invalid choice!\n");
        }

        SaveExpenses();
    }
    #endregion

    #region Adding and Listing Expenses
    /// <summary>
    /// Method to prompt the user for each field (date, amount, category, description) and to add the new expense to the list
    /// </summary>
    // Input validation as well
    static void AddExpense()
    {
        Console.Write("Date (yyyy-MM-dd): ");
        DateTime date;
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            Console.Write("Invalid date! Please use yyyy-MM-dd: ");

        Console.Write("Amount: ");
        decimal amount;
        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount < 0)
            Console.Write("Invalid amount! Please enter a positive number: ");

        // Show category options
        Console.WriteLine("Categories:");
        for (int i = 0; i < categories.Count; ++i)
            Console.WriteLine($"{i + 1}. {categories[i]}");
        Console.Write("Choose a category (number): ");
        int catIndex;
        while (!int.TryParse(Console.ReadLine(), out catIndex) || catIndex < 1 || catIndex > categories.Count)
            Console.Write("Invalid choice! Enter a number from the list: ");
        string category = categories[catIndex - 1];

        Console.Write("Description: ");
        string description = Console.ReadLine();

        expenses.Add(new Expense(date, amount, category, description));
        Console.WriteLine("Expense added!\n");
    }

    static void ListExpenses()
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("No expenses to show.\n");
            return;
        }
        Console.WriteLine("All expenses:");
        foreach (var e in expenses)
            Console.WriteLine(e);

        Console.WriteLine();
    }
    #endregion

    #region Filtering functions
    static void ListByCategory()
    {
        Console.WriteLine("Categories:");
        for (int i = 0; i < categories.Count; ++i)
            Console.WriteLine($"{i + 1}. {categories[i]}");
        Console.Write("Choose a category (number): ");

        int catIndex;

        while (!int.TryParse(Console.ReadLine(), out catIndex) || catIndex < 1 || catIndex > categories.Count)
            Console.Write("Invalid choice! Enter a number from the list: ");
        string category = categories[catIndex - 1];

        var filtered = expenses.Where(e => e.Category == category);

        if (!filtered.Any())
        {
            Console.WriteLine($"No expenses found for category: {category}\n");
            return;
        }

        Console.WriteLine($"Expenses in category: {category}");
        foreach (var exp in filtered)
            Console.WriteLine(exp);
        Console.WriteLine();
    }

    static void ListByDateRange()
    {
        Console.Write("Start date (yyyy-MM-dd): ");
        DateTime startDate;
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            Console.Write("Invalid date! Please use yyyy-MM-dd: ");

        Console.Write("End date (yyyy-MM-dd): ");
        DateTime endDate;
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            Console.Write("Invalid date! Please use yyyy-MM-dd: ");

        // End date is inclusive
        var filtered = expenses.Where(e => e.Date >= startDate && e.Date <= endDate);

        if (!filtered.Any())
        {
            Console.WriteLine($"No expenses found between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}\n");
            return;
        }

        Console.WriteLine($"Expenses from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:");
        foreach (var exp in filtered)
            Console.WriteLine(exp);
        Console.WriteLine();
    }

    #endregion

    #region Statistics
    static void ShowStatistics()
    {
        if (!expenses.Any())
        {
            Console.WriteLine("No expenses available to show statistics.\n");
            return;
        }

        decimal totalSpent = expenses.Sum(e => e.Amount);
        decimal averageSpent = expenses.Average(e => e.Amount);
        decimal highest = expenses.Max(e => e.Amount);
        decimal lowest = expenses.Min(e => e.Amount);

        Console.WriteLine("=== Expense Statistics ===");
        Console.WriteLine($"Total spent: {FormatHuf(totalSpent)}");
        Console.WriteLine($"Average expense: {FormatHuf(averageSpent)}");
        Console.WriteLine($"Highest single expense: {FormatHuf(highest)}");
        Console.WriteLine($"Lowest single expense: {FormatHuf(lowest)}");

        // Total spent by category
        var byCategory = expenses
            .GroupBy(e => e.Category)
            .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
            .OrderByDescending(g => g.Total);

        Console.WriteLine("Total spent by category:");
        foreach (var cat in byCategory)
            Console.WriteLine($" {cat.Category}: {FormatHuf(cat.Total)}");
        Console.WriteLine();

        // Highest single expense per category
        Console.WriteLine("Highest single expense per category:");
        var maxPerCategory = expenses
            .GroupBy(e => e.Category)
            .Select(g => new { Category = g.Key, Max = g.Max(e => e.Amount) });

        foreach (var cat in maxPerCategory)
            Console.WriteLine($" {cat.Category}: {FormatHuf(cat.Max)}");
        Console.WriteLine();

        // Total by month/year
        Console.WriteLine("Total spent by month:");
        var byMonth = expenses
            .GroupBy(e => new { e.Date.Year, e.Date.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Total = g.Sum(e => e.Amount)
            })
            .OrderByDescending(g => g.Year)
            .ThenByDescending(g => g.Month);

        foreach (var m in byMonth)
            Console.WriteLine($" {m.Year}-{m.Month:D2}: {FormatHuf(m.Total)}");
        Console.WriteLine();

        // Most expensive day (total spent per day)
        var byDay = expenses
            .GroupBy(e => e.Date.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(e => e.Amount) })
            .OrderByDescending(g => g.Total)
            .ToList();

        if (byDay.Any())
        {
            var maxDay = byDay.First();
            Console.WriteLine($"Most expensive day: {maxDay.Date:yyyy-MM-dd} (Total spent: {FormatHuf(maxDay.Total)})");
        }
        Console.WriteLine();

        // Top 3 biggest individual expenses
        var top3 = expenses
            .OrderByDescending(e => e.Amount)
            .Take(3)
            .ToList();

        Console.WriteLine("Top 3 biggest expenses:");
        for (int i = 0; i < top3.Count; ++i)
        {
            var exp = top3[i];
            Console.WriteLine($"  {i + 1}. {exp.Date:yyyy-MM-dd} | {FormatHuf(exp.Amount)} | {exp.Category} | {exp.Description}");
        }
        Console.WriteLine();
    }

    #endregion

    #region Save and Load methods
    static void SaveExpenses()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(expenses, options);
        File.WriteAllText(FileName, json);
        Console.WriteLine("Expenses saved successfully.\n");
    }

    static void LoadExpenses()
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine("No saved expenses found. Starting fresh.\n");
            return;
        }

        try
        {
            string json = File.ReadAllText(FileName);
            expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
            Console.WriteLine("Expenses loaded successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading expenses: {ex.Message}");
            expenses = new List<Expense>();
        }
    }
    #endregion

    public static string FormatHuf(decimal amount) => $"{amount.ToString("N0", huCulture)} Ft";
}