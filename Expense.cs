/****************************************************************************************
 * Expense Class - holds date, amount, category and description
 * Category as a string (or maybe an enum?)
 * Question: Use decimal or double for amounts? Decimal is recommended for financial data!
 * Categories: "Groceries", "Housing", "Restaurants", "Hobbies", "Car Services", "Communication", "Insurances", "Fuel", "Others"
 ****************************************************************************************/

namespace PersonalExpenseTracker;

public class Expense
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }  // e.g. "Groceries", "Transport", etc.
    public string Description { get; set; }

    public Expense(DateTime date, decimal amount, string category, string description)
    {
        Date = date;
        Amount = amount;
        Category = category;
        Description = description;
    }

    //public string FormatHuf(decimal amount) => $"{amount.ToString("N0", huCulture)} Ft";

    public override string ToString()
    {
        return $"{Date.ToShortDateString()} | {Program.FormatHuf(Amount)} | {Category} | {Description}";
    }
}