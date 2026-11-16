using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Model;

public class Visitor
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int AddressId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string MembershipCardNumber { get; set; }
    public int MembershipYear { get; set; }
    public VisitorStatus Status { get; set; }
    public double AverageReviewRating { get; set; }
    public List<int> PurchaseIds { get; set; }
    public List<int> WishlistBookIds { get; set; }

    public Visitor()
    {
        PurchaseIds = new List<int>();
        WishlistBookIds = new List<int>();
    }
}

public enum VisitorStatus
{
    Regular,
    VIP
}