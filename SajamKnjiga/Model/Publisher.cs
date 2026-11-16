using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SajamKnjiga.Model;

public class Publisher
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public int HeadAuthorId { get; set; }

    public List<int> AuthorIds { get; set; }
    public List<int> BookIds { get; set; }

    public Publisher()
    {
        AuthorIds = new List<int>();
        BookIds = new List<int>();
    }
}
