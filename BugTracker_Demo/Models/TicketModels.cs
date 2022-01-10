using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker_Demo.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string Progress { get; set; }
        public string UserName { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime Date { get; set; }
    }

    public class FinishedTicketModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public string Confirmed { get; set; }

    }

    public class SearchModel
    {
        public string search { get; set; }
        public string type { get; set; }
        public string organ { get; set; }
    }

    public class TicketColumnModel
    {
        public string search { get; set; }
    }
}
