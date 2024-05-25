using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.Tickets.Domain.Integrations.AmadeusApi;

public class Error
{
    public int Status { get; set; }
    public int Code { get; set; }
    public string Title { get; set; }
    public string? Detail { get; set; }
    public Source? Source { get; set; }
}
