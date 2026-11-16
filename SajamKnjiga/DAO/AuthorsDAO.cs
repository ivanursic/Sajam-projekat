using SajamKnjiga.Model;
using SajamKnjiga.Storage;

namespace SajamKnjiga.DAO;

public class AuthorsDAO
{
    private readonly Storage<Author> _storage = new("authors.txt");
    private readonly List<Author> _authors;

    public AuthorsDAO()
    {
        _authors = _storage.Load();
    }

    private int GenerateId()
    {
        return _authors.Count == 0 ? 0 : _authors.Max(a => a.Id) + 1;
    }

    public void AddAuthor(Author author)
    {
        author.Id = GenerateId();
        _authors.Add(author);
        _storage.Save(_authors);
    }

    public void UpdateAuthor(Author author)
    {
        Author? oldAuthor = GetAuthorById(author.Id);

        if (oldAuthor == null) return;

        oldAuthor.FirstName = author.FirstName;
        oldAuthor.LastName = author.LastName;
        oldAuthor.DateOfBirth = author.DateOfBirth;
        oldAuthor.AddressId = author.AddressId;
        oldAuthor.PhoneNumber = author.PhoneNumber;
        oldAuthor.Email = author.Email;
        oldAuthor.IdentityCardNumber = author.IdentityCardNumber;
        oldAuthor.YearsOfExperience = author.YearsOfExperience;
        oldAuthor.BookIds = author.BookIds;

        _storage.Save(_authors);
    }

    public void RemoveAuthor(int id)
    {
        Author? author = GetAuthorById(id);

        if (author == null) return;

        _authors.Remove(author);
        _storage.Save(_authors);
    }

    private Author? GetAuthorById(int id)
    {
        return _authors.FirstOrDefault(a => a.Id == id);
    }

    public List<Author> GetAllAuthors()
    {
        return _authors;
    }
}
