const int maxBooks = 5;
List<string> books = new();

void DisplayBooks()
{
    Console.WriteLine("Available books:");
    if (books.Count == 0)
    {
        Console.WriteLine("  (No books in the library)");
        return;
    }
    for (int i = 0; i < books.Count; i++)
        Console.WriteLine($"{i + 1}. {books[i]}");
}

void AddBook()
{
    if (books.Count >= maxBooks)
    {
        Console.WriteLine("The library is full. No more books can be added.");
        return;
    }
    Console.Write("Enter the title of the book to add: ");
    var title = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(title))
        books.Add(title.Trim());
    else
        Console.WriteLine("Invalid title.");
}

void RemoveBook()
{
    if (books.Count == 0)
    {
        Console.WriteLine("The library is empty. No books to remove.");
        return;
    }
    Console.Write("Enter the title of the book to remove: ");
    var title = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(title) && books.Remove(title.Trim()))
        Console.WriteLine("Book removed.");
    else
        Console.WriteLine("Book not found.");
}

while (true)
{
    Console.Write("Would you like to add or remove a book? (add/remove/exit): ");
    var action = Console.ReadLine()?.Trim().ToLower();
    switch (action)
    {
        case "add":
            AddBook();
            break;
        case "remove":
            RemoveBook();
            break;
        case "exit":
            return;
        default:
            Console.WriteLine("Invalid action. Please type 'add', 'remove', or 'exit'.");
            break;
    }
    DisplayBooks();
}
