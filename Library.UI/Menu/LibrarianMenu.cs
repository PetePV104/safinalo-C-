using Library.Aplication.Services;
using Library.Domain.Models;
using Spectre.Console;

namespace Library.UI.Menu;

internal sealed class LibrarianMenu
{
    private readonly User _user;
    private readonly LibraryService _library;
    private readonly UserService _userService;

    public LibrarianMenu(User user, LibraryService library, UserService userService)
    {
        _user = user;
        _library = library;
        _userService = userService;
    }

    public void ShowMenu()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"Librarian menu - {Markup.Escape(_user.Name)}")
                .AddChoices("List users", "Add admin", "Remove user", "List borrow requests", "Back"));

            try
            {
                switch (choice)
                {
                    case "List users": ListUsers(); break;
                    case "Add admin": AddAdmin(); break;
                    case "Remove user": RemoveUser(); break;
                    case "List borrow requests": ManageBorrowRequests(); break;
                    case "Back": return;
                }
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine($"[red]Operation failed:[/] {Markup.Escape(exception.Message)}");
            }
        }
    }

    private void ListUsers()
    {
        var table = new Table().Border(TableBorder.Rounded)
            .AddColumns("Id", "Name", "Email", "Role", "Verified");
        foreach (var user in _library.GetUsers())
            table.AddRow(user.Id.ToString(), user.Name, user.Email, user.Role, user.IsVerified.ToString());
        AnsiConsole.Write(table);
    }

    private void AddAdmin()
    {
        var name = AnsiConsole.Ask<string>("Name:");
        var email = AnsiConsole.Ask<string>("Email:");
        var password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
        _userService.RegisterUser(name, email, password, "Admin");
        AnsiConsole.MarkupLine("[green]Admin added and verification email sent.[/]");
    }

    private void RemoveUser()
    {
        var email = AnsiConsole.Ask<string>("Email of user to remove:");
        _library.RemoveUser(email);
        AnsiConsole.MarkupLine("[green]User removed.[/]");
    }

    private void ManageBorrowRequests()
    {
        var requests = _library.GetBooks()
            .Where(book => book.BorrowRequestByEmail is not null)
            .ToList();

        if (requests.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no pending borrow requests.[/]");
            return;
        }

        foreach (var book in requests)
        {
            AnsiConsole.MarkupLine($"Book [cyan]{Markup.Escape(book.Title)}[/] requested by [cyan]{Markup.Escape(book.BorrowRequestByEmail!)}[/]");
            var decision = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Choose an action:")
                .AddChoices("Accept", "Decline"));

            if (decision == "Accept")
            {
                _library.ApproveBorrowRequest(book.Id);
                AnsiConsole.MarkupLine("[green]Borrow request accepted.[/]");
            }
            else
            {
                _library.DeclineBorrowRequest(book.Id);
                AnsiConsole.MarkupLine("[yellow]Borrow request declined.[/]");
            }
        }
    }
}