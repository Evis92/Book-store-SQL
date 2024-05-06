using DataAccess;

namespace BookStoreUI;

public class ApplicationContextManager
{
    public static BookStoreDbContext? Context { get; private set; } = new BookStoreDbContext();

    public static void Initialize(BookStoreDbContext context)
    {
        Context = context;
    }
}