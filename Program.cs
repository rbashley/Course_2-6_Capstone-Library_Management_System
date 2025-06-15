// Library Management System

const int maxBooks = 5;         // Maximum number of books in the library
const int maxBorrowed = 3;      // Maximum books a user can borrow at once

// List of available books
List<string> books = new();

// Maps username to their list of borrowed books
Dictionary<string, List<string>> userBorrowedBooks = new();

// Maps book title to the username who borrowed it
Dictionary<string, string> borrowedBookOwners = new();

string currentUser = ""; // Tracks the currently logged-in user

// Prompt the user to enter a username and ensure their record exists
void PromptForUser()
{
    Console.Write("Enter your username: ");
    var user = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(user))
    {
        Console.WriteLine("Invalid username. Try again.");
        PromptForUser();
    }
    else
    {
        currentUser = user;
        EnsureCurrentUser();
    }
}

// Ensure the current user has a borrowed books list
void EnsureCurrentUser()
{
    if (!userBorrowedBooks.ContainsKey(currentUser))
        userBorrowedBooks[currentUser] = new List<string>();
}

// Display all books in the library, showing their status
void DisplayBooks()
{
    var allTitles = new HashSet<string>(books);
    foreach (var kvp in borrowedBookOwners)
        allTitles.Add(kvp.Key);

    if (allTitles.Count == 0)
    {
        Console.WriteLine("  (No books in the library)");
        return;
    }

    Console.WriteLine("Library books:");
    foreach (var title in allTitles)
    {
        if (books.Contains(title))
            Console.WriteLine($"- {title} (Available)");
        else if (borrowedBookOwners.TryGetValue(title, out var borrower))
            Console.WriteLine($"- {title} (Borrowed by {borrower})");
    }
}

// Display the books borrowed by the current user
void DisplayBorrowedBooks()
{
    var borrowed = userBorrowedBooks[currentUser];
    Console.WriteLine($"{currentUser}'s borrowed books:");
    if (borrowed.Count == 0)
    {
        Console.WriteLine("  (No books borrowed)");
        return;
    }
    for (int i = 0; i < borrowed.Count; i++)
        Console.WriteLine($"{i + 1}. {borrowed[i]}");
}

// Add a new book to the library (if space and not a duplicate)
void AddBook()
{
    int totalBooks = books.Count + borrowedBookOwners.Count;
    if (totalBooks >= maxBooks)
    {
        Console.WriteLine("The library is full. No more books can be added.");
        return;
    }
    Console.Write("Enter the title of the book to add: ");
    var title = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Invalid title.");
        return;
    }
    if (books.Contains(title) || borrowedBookOwners.ContainsKey(title))
    {
        Console.WriteLine("That book already exists in the library.");
        return;
    }
    books.Add(title);
    Console.WriteLine("Book added.");
}

// Remove a book from the library (only if it's available)
void RemoveBook()
{
    if (books.Count == 0)
    {
        Console.WriteLine("The library is empty. No books to remove.");
        return;
    }
    Console.Write("Enter the title of the book to remove: ");
    var title = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Invalid title.");
        return;
    }
    if (books.Remove(title))
        Console.WriteLine("Book removed.");
    else
        Console.WriteLine("Book not found or is currently borrowed.");
}

// Search for a book and display its status
void SearchBook()
{
    Console.Write("Enter the title of the book to search for: ");
    var title = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Invalid title.");
        return;
    }
    if (books.Contains(title))
        Console.WriteLine($"\"{title}\" is available in the library.");
    else if (borrowedBookOwners.TryGetValue(title, out var borrower))
        Console.WriteLine($"\"{title}\" is currently borrowed by {borrower}.");
    else
        Console.WriteLine($"\"{title}\" is not in the collection.");
}

// Borrow a book if available and user is under the limit
void BorrowBook()
{
    var borrowed = userBorrowedBooks[currentUser];
    if (borrowed.Count >= maxBorrowed)
    {
        Console.WriteLine($"You have reached the borrowing limit ({maxBorrowed} books). Return a book before borrowing another.");
        return;
    }
    if (books.Count == 0)
    {
        Console.WriteLine("No available books to borrow.");
        return;
    }
    Console.Write("Enter the title of the book to borrow: ");
    var title = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Invalid title.");
        return;
    }
    if (books.Remove(title))
    {
        borrowed.Add(title);
        borrowedBookOwners[title] = currentUser;
        Console.WriteLine($"You have borrowed \"{title}\".");
    }
    else if (borrowedBookOwners.ContainsKey(title))
    {
        Console.WriteLine("That book is already borrowed.");
    }
    else
    {
        Console.WriteLine("Book not found.");
    }
}

// Check in a book the user has borrowed
void CheckInBook()
{
    var borrowed = userBorrowedBooks[currentUser];
    if (borrowed.Count == 0)
    {
        Console.WriteLine("You have no books to check in.");
        return;
    }
    Console.Write("Enter the title of the book to check in: ");
    var title = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Invalid title.");
        return;
    }
    if (borrowed.Remove(title))
    {
        books.Add(title);
        borrowedBookOwners.Remove(title);
        Console.WriteLine($"You have checked in \"{title}\".");
    }
    else
    {
        Console.WriteLine("That book is not currently checked out by you.");
    }
}

// Main program loop
while (true)
{
    if (string.IsNullOrEmpty(currentUser))
        PromptForUser();

    EnsureCurrentUser();

    Console.WriteLine($"\nChoose an action: add, remove, search, borrow, checkin, display, showborrowed, switchuser, exit");
    Console.Write("Action: ");
    var action = Console.ReadLine()?.Trim().ToLower();
    switch (action)
    {
        case "add":
            AddBook();
            break;
        case "remove":
            RemoveBook();
            break;
        case "search":
            SearchBook();
            break;
        case "borrow":
            BorrowBook();
            break;
        case "checkin":
            CheckInBook();
            break;
        case "display":
            DisplayBooks();
            break;
        case "showborrowed":
            DisplayBorrowedBooks();
            break;
        case "switchuser":
            currentUser = "";
            break;
        case "test":
            // Hidden: Populate library with 5 classical books for testing
            books.Clear();
            userBorrowedBooks.Clear();
            borrowedBookOwners.Clear();
            var classics = new List<string>
            {
                "Pride and Prejudice",
                "Moby Dick",
                "War and Peace",
                "Great Expectations",
                "The Odyssey"
            };
            foreach (var title in classics)
                books.Add(title);
            Console.WriteLine("Test library populated with classical books.");
            break;
        case "exit":
            return;
        default:
            Console.WriteLine("Invalid action.");
            break;
    }
}
