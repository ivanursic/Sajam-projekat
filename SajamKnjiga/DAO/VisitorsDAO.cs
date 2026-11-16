using SajamKnjiga.Model;
using SajamKnjiga.Storage;

namespace SajamKnjiga.DAO;

public class VisitorsDAO
{
    private readonly List<Visitor> _visitors;
    private readonly Storage<Visitor> _storage;

    public VisitorsDAO()
    {
        _storage = new Storage<Visitor>("visitors.txt");
        _visitors = _storage.Load();
    }

    private int GenerateId()
    {
        if (_visitors.Count == 0) return 0;
        return _visitors.Max(v => v.Id) + 1;
    }

    public Visitor AddVisitor(Visitor visitor)
    {
        visitor.Id = GenerateId();
        _visitors.Add(visitor);
        _storage.Save(_visitors);
        return visitor;
    }

    public Visitor? UpdateVisitor(Visitor visitor)
    {
        Visitor? oldVisitor = GetVisitorById(visitor.Id);
        if (oldVisitor is null) return null;

        oldVisitor.FirstName = visitor.FirstName;
        oldVisitor.LastName = visitor.LastName;
        oldVisitor.DateOfBirth = visitor.DateOfBirth;
        oldVisitor.AddressId = visitor.AddressId;
        oldVisitor.PhoneNumber = visitor.PhoneNumber;
        oldVisitor.Email = visitor.Email;
        oldVisitor.MembershipCardNumber = visitor.MembershipCardNumber;
        oldVisitor.MembershipYear = visitor.MembershipYear;
        oldVisitor.Status = visitor.Status;
        oldVisitor.AverageReviewRating = visitor.AverageReviewRating;
        oldVisitor.PurchaseIds = new List<int>(visitor.PurchaseIds);
        oldVisitor.WishlistBookIds = new List<int>(visitor.WishlistBookIds);

        _storage.Save(_visitors);
        return oldVisitor;
    }

    public Visitor? RemoveVisitor(int id)
    {
        Visitor? visitor = GetVisitorById(id);
        if (visitor is null) return null;

        _visitors.Remove(visitor);
        _storage.Save(_visitors);
        return visitor;
    }

    private Visitor? GetVisitorById(int id)
    {
        return _visitors.Find(v => v.Id == id);
    }

    public List<Visitor> GetAllVisitors()
    {
        return _visitors;
    }
}