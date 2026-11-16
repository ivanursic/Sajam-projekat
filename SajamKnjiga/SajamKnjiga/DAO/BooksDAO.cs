using SajamKnjiga.Model;
using SajamKnjiga.Storage;

namespace SajamKnjiga.DAO;

public class BooksDAO
{
    private readonly Storage<Book> _storage = new("books.txt");
    private readonly List<Book> _books;

    public BooksDAO()
    {
        _books = _storage.Load();
    }

    private int GenerateId()
    {
        return _books.Count == 0 ? 0 : _books.Max(b => b.Id) + 1;
    }

    public void AddBook(Book book)
    {
        book.Id = GenerateId();
        _books.Add(book);
        _storage.Save(_books);
    }

    public void UpdateBook(Book book)
    {
        Book? oldBook = GetBookById(book.Id);

        if (oldBook == null) return;

        oldBook.ISBN = book.ISBN;
        oldBook.Title = book.Title;
        oldBook.Genre = book.Genre;
        oldBook.PublicationYear = book.PublicationYear;
        oldBook.Price = book.Price;
        oldBook.PageCount = book.PageCount;
        oldBook.AuthorIds = book.AuthorIds;
        oldBook.PublisherId = book.PublisherId;
        oldBook.PurchasedByVisitorIds = book.PurchasedByVisitorIds;
        oldBook.WishlistedByVisitorIds = book.WishlistedByVisitorIds;

        _storage.Save(_books);
    }

    public void RemoveBook(int id)
    {
        Book? book = GetBookById(id);

        if (book == null) return;

        _books.Remove(book);
        _storage.Save(_books);
    }

    private Book? GetBookById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public List<Book> GetAllBooks()
    {
        return _books;
    }
}
