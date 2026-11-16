using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Model;

public class Purchase
{
    public int Id { get; set; }
    public int VisitorId { get; set; }
    public int BookId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }

    public Purchase()
    {
        Comment = string.Empty;
    }
}

