using SajamKnjiga.Model;
using SajamKnjiga.Storage;

namespace SajamKnjiga.DAO;

public class PublishersDAO
{
    private readonly List<Publisher> _publishers;
    private readonly Storage<Publisher> _storage;

    public PublishersDAO()
    {
        _storage = new Storage<Publisher>("publishers.txt");
        _publishers = _storage.Load();
    }

    private int GenerateId()
    {
        return _publishers.Count == 0 ? 0 : _publishers.Max(p => p.Id) + 1;
    }

    public Publisher AddPublisher(Publisher publisher)
    {
        publisher.Id = GenerateId();
        _publishers.Add(publisher);
        _storage.Save(_publishers);
        return publisher;
    }

    public Publisher? UpdatePublisher(Publisher publisher)
    {
        Publisher? oldPublisher = GetPublisherById(publisher.Id);
        if (oldPublisher is null) return null;

        oldPublisher.Code = publisher.Code;
        oldPublisher.Name = publisher.Name;
        oldPublisher.HeadAuthorId = publisher.HeadAuthorId;
        oldPublisher.AuthorIds = new List<int>(publisher.AuthorIds);
        oldPublisher.BookIds = new List<int>(publisher.BookIds);

        _storage.Save(_publishers);
        return oldPublisher;
    }

    public Publisher? RemovePublisher(int id)
    {
        Publisher? publisher = GetPublisherById(id);
        if (publisher is null) return null;

        _publishers.Remove(publisher);
        _storage.Save(_publishers);
        return publisher;
    }

    private Publisher? GetPublisherById(int id)
    {
        return _publishers.Find(p => p.Id == id);
    }

    public List<Publisher> GetAllPublishers()
    {
        return _publishers;
    }
}