using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker_Demo.Models
{
    public class TicketViewModel
    {
        public List<TicketModel> ticketList { get; set; }
        public UserModel userModel { get; set; }
    }

    public class TicketBuildModel
    {
        public TicketModel ticketModel { get; set; }
        public UserModel userModel { get; set; }
    }

    public class ProjectBuildModel
    {
        public ProjectModel projectModel { get; set; }
        public UserModel userModel { get; set; }
    }

    public class ProjectViewModel
    {
        public ProjectModel projectModel { get; set; }
        public List<TicketModel> ticketList { get; set; }
        public UserModel userModel { get; set; }
    }

    public class ProjectFinishModel
    {
        public ProjectModel projectModel { get; set; }
        public List<TicketModel> ticketList { get; set; }
        public UserModel userModel { get; set; }

    }

    public class NoteHybridModel
    {
        public NoteModel noteModel { get; set; }
        public UserModel userModel { get; set; }
    }
    public class TicketDetailModel
    {
        public TicketModel ticketModel { get; set; }
        public NoteModel noteModel { get; set; }
        public List<NoteModel> noteList { get; set; }
        public UserModel userModel { get; set; }
    }

    public class TicketSearchModel
    {
        public SearchModel searchModel { get; set; }
        public UserModel userModel { get; set; }
    }

    public class IndexSearchModel
    {
        public TicketColumnModel ticketColumnModel { get; set; }
        public UserModel userModel { get; set; }
    }

    public class ProjectHybridModel
    {
        public ProjectModel Project { get; set; }

        public List<TicketModel> TicketList { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
    }
}
