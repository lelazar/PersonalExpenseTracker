# Personal Expense Tracker (Console App)

A simple, modern C# console application for tracking your daily expenses.  
**Built for Hungary, with support for Hungarian Forint (Ft) and no decimal amounts.**

---

## Features

- **Add expenses**: Input the date, amount, category, and description of each expense.
- **View all expenses**: List every expense added, sorted and formatted.
- **Filter by category**: View expenses for a selected category (e.g., Groceries, Housing).
- **Filter by date range**: See expenses within a specific time window.
- **Statistics & reports**:
  - Total, average, highest, and lowest expense.
  - Spending grouped by category and by month.
  - Most expensive day.
  - Top 3 individual expenses.
  - Total spent in a custom date range.
- **Automatic saving/loading**: Your expenses are saved in `expenses.json` (JSON format) so you never lose data between runs.
- **Hungarian Forint formatting**: Amounts are shown as whole numbers with `Ft` (e.g., `15 000 Ft`).

---

## Categories Supported

- Groceries
- Housing
- Restaurants
- Hobbies
- Car Services
- Communication
- Insurances
- Fuel
- Others

---

## How to Use

1. **Clone or download** this repository.
2. **Open in Visual Studio** (or any compatible C# IDE).
3. **Build and run** the console application.
4. Use the menu to add expenses, view, filter, and generate statistics.
5. **Your data is saved automatically** to `expenses.json` in the project directory.

---

## Technologies Used

- C# (.NET 6/7 recommended, but works with modern .NET Core)
- `System.Text.Json` for serialization
- LINQ for filtering, grouping, and statistics
- Clean, readable code with comments and input validation

---

## Example Output

=== Personal Expense Tracker ===

Add new expense

List all expenses

List expenses by category

List expenses by date range

Show statistics

Exit

Choose an option: 5


=== Expense Statistics ===
Total spent: 18 750 Ft
Average expense: 3 125 Ft
Highest single expense: 5 000 Ft
Lowest single expense: 1 000 Ft

Total spent by category:
Groceries: 7 000 Ft
Housing: 4 500 Ft
...

Most expensive day: 2025-05-21 (Total spent: 8 200 Ft)
Top 3 biggest expenses:

2025-05-21 | 5 000 Ft | Housing | Rent

2025-05-21 | 3 200 Ft | Groceries | Lidl

2025-05-19 | 2 900 Ft | Fuel | MOL


---

## Customization

- You can edit or add categories by modifying the category list in the source code.
- Currency formatting is customizable—designed for Hungarian conventions (no decimals, `Ft` after amount).
- Easily extend the project for more advanced features, or port to WPF for a GUI.

---

## License

This project is open source and free to use for personal or educational purposes.

---

## Author

Developed by Levente Lazar.  
Contributions, improvements, or feedback are welcome!
