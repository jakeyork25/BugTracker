using BugTracker_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using BugTracker_Demo.DbServices;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;

namespace BugTracker_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
         
        public IActionResult Index(UserModel user)
        {
            var model = new TicketViewModel();
            TicketDAO ticketDAO = new TicketDAO();
            model.ticketList = ticketDAO.GetTicketAttributes("Date");
            model.userModel = user;

            return View(model);              
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginToProgram(RegisterModel user)
        {
            UserDAO userDAO = new UserDAO();
            var isValid = userDAO.UserValid(user);
            if(isValid == true)
            {
                TicketDAO ticketDAO = new TicketDAO();
                var model = new TicketViewModel();
                model.ticketList = ticketDAO.GetTicketAttributes("Date");
                var userModel = new UserModel();
                userModel.UserName = user.DesiredUserName;
                model.userModel = userModel;
                return View("Index", model);
            } else
            {
                return View("Register");
            }         
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult CreateAccount(RegisterModel user)
        {
            if(ModelState.IsValid)
            {
                UserDAO userDAO = new UserDAO();
                userDAO.AddUserToDatabase(user);
                return View("Register");
            }
            return View("Register");
        }

        public IActionResult TicketBuild(TicketBuildModel model)
        {
            return View(model);
        }

        public IActionResult ProjectBuild(UserModel user)
        {
            var model = new ProjectBuildModel();
            model.userModel = user;
            return View(model);
        }

        public IActionResult TicketView(UserModel user)
        {
            TicketDAO ticketDAO = new TicketDAO();
            var model = new TicketViewModel();
            model.ticketList = ticketDAO.GetTicketAttributes("Date");
            model.userModel = user;
            return View(model);
        }

        public IActionResult ProjectView(UserModel user)
        {
            ProjectDAO projectDAO = new ProjectDAO();
            var modelList = new List<ProjectViewModel>();
            var projectList = projectDAO.GetProjectList();
            foreach(var project in projectList)
            {
                var tempModel = new ProjectViewModel();
                tempModel.projectModel = project;
                tempModel.ticketList = projectDAO.GetTicketListFromProject(project);
                tempModel.userModel = user;
                modelList.Add(tempModel);
            }
            return View(modelList);
        }

        public IActionResult TicketDetails(TicketBuildModel model)
        {  
            var detailModel = new TicketDetailModel();
            detailModel.ticketModel = model.ticketModel;
            detailModel.userModel = model.userModel;
            NoteDAO noteDAO = new NoteDAO();
            List<NoteModel> noteList = noteDAO.RetreiveNotesForTicket(model.ticketModel.Name.ToString());
            detailModel.noteList = noteList;
            return View(detailModel);
        }

        public IActionResult GoToConfirmFinishPage(TicketBuildModel model)
        {
            return View("ConfirmFinish", model);
        }

        public IActionResult ConfirmFinish(TicketBuildModel model, string confirmed)
        {
            FinishedDAO finishedDAO = new FinishedDAO();
            TicketDAO ticketDAO = new TicketDAO();
            NoteDAO noteDAO = new NoteDAO();

            FinishedTicketModel finishedModel = new FinishedTicketModel();

            finishedModel.Name = model.ticketModel.Name;
            finishedModel.Description = model.ticketModel.Description;
            finishedModel.Type = model.ticketModel.Type;
            finishedModel.UserName = model.ticketModel.UserName;
            finishedModel.StartDate = model.ticketModel.Date;
            finishedModel.ProjectTitle = model.ticketModel.ProjectTitle;
            finishedModel.FinishDate = DateTime.Now;

            if(confirmed.Equals("true"))
            {
                finishedDAO.AddFinishedToDatabase(finishedModel);
                ticketDAO.DeleteTicketFromDatabase(model.ticketModel);
                noteDAO.DeleteAllNotesForTicketFromDatabase(model.ticketModel.Name.ToString());

                var tempModel = new TicketViewModel();
                tempModel.ticketList = ticketDAO.GetTicketAttributes("Date");
                tempModel.userModel = model.userModel;

                return View("TicketView", tempModel);
            } 
            else
            {
                var tempModel = new TicketViewModel();
                tempModel.ticketList = ticketDAO.GetTicketAttributes("Date");
                tempModel.userModel = model.userModel;

                return View("Index", tempModel);
            }           
        }

        public IActionResult ConfirmProjectFinish(ProjectFinishModel model, string confirmed)
        {
            FinishedDAO finishedDAO = new FinishedDAO();
            ProjectDAO projectDAO = new ProjectDAO();
            TicketDAO ticketDAO = new TicketDAO();
            NoteDAO noteDAO = new NoteDAO();
            ProjectModel tempProjectModel = model.projectModel;
            
            List<TicketModel> ticketList = projectDAO.GetTicketListFromProject(tempProjectModel);

            if (confirmed.Equals("true"))
            {
                foreach (var ticket in ticketList)
                {
                    FinishedTicketModel finishedModel = new FinishedTicketModel();

                    finishedModel.Name = ticket.Name;
                    finishedModel.Description = ticket.Description;
                    finishedModel.Type = ticket.Type;
                    finishedModel.UserName = ticket.UserName;
                    finishedModel.ProjectTitle = ticket.ProjectTitle;
                    finishedModel.StartDate = ticket.Date;
                    finishedModel.FinishDate = DateTime.Now;

                    finishedDAO.AddFinishedToDatabase(finishedModel);
                    ticketDAO.DeleteTicketFromDatabase(ticket);
                    noteDAO.DeleteAllNotesForTicketFromDatabase(ticket.Name.ToString());
                }

                projectDAO.DeleteProjectFromDatabase(tempProjectModel);
                var passModel = new TicketViewModel();
                passModel.ticketList = ticketDAO.GetTicketAttributes("Date");
                passModel.userModel = model.userModel;
                return View("TicketView", passModel);
            }
            else
            {
                var passModel = new TicketViewModel();
                passModel.ticketList = ticketDAO.GetTicketAttributes("Date");
                passModel.userModel = model.userModel;
                return View("Index", passModel);
            }
        }

        public IActionResult CreateNote(NoteHybridModel model)
        {
            NoteDAO noteDAO = new NoteDAO();
            noteDAO.AddNoteToDatabase(model.noteModel);
            TicketDAO ticketDAO = new TicketDAO();
            var passModel = new TicketViewModel();
            passModel.ticketList = ticketDAO.GetTicketAttributes("Date");
            passModel.userModel = model.userModel;
            return View("TicketView", passModel);
        }
         
        public IActionResult DeleteNote(NoteHybridModel model)
        {
            NoteDAO noteDAO = new NoteDAO();
            noteDAO.DeleteNoteFromDatabase(model.noteModel);
            TicketDAO ticketDAO = new TicketDAO();
            var passModel = new TicketViewModel();
            passModel.ticketList = ticketDAO.GetTicketAttributes("Date");
            passModel.userModel = model.userModel;
            return View("TicketView", passModel);
        }

        public IActionResult Click(TicketBuildModel model) 
        {
            var passModel = new TicketViewModel();
            TicketDAO ticketDAO = new TicketDAO();
            var ticketModel = model.ticketModel;
            ticketModel.Date = DateTime.Now;
            ticketDAO.AddTicketToDatabase(ticketModel);
            passModel.ticketList = ticketDAO.GetTicketAttributes("Date");
            passModel.userModel = model.userModel;
            return View("Index", passModel);
        }

        public IActionResult CreateProject(ProjectBuildModel model)
        {
            ProjectDAO projectDAO = new ProjectDAO();
            projectDAO.AddProjectToDatabase(model.projectModel);
            var passModel = new TicketViewModel();
            TicketDAO ticketDAO = new TicketDAO();
            passModel.ticketList = ticketDAO.GetTicketAttributes("Date");
            passModel.userModel = model.userModel;
            var ticketStats = ticketDAO.GetTicketAttributes("Date");
            return View("Index", passModel);
        }
        public IActionResult SearchTickets(TicketSearchModel model)
        {
            TicketDAO ticketDAO = new TicketDAO();
            var passModel = new TicketViewModel();
            passModel.ticketList = ticketDAO.SearchThroughTickets(model.searchModel, model.searchModel.organ);
            passModel.userModel = model.userModel;
            return View("TicketView", passModel);
        }

        public IActionResult ViewPriorityTickets(IndexSearchModel indexModel)
        {
            TicketDAO ticketDAO = new TicketDAO();
            var passModel = new TicketViewModel();
            passModel.ticketList = ticketDAO.GetTicketsThroughPriority(indexModel.ticketColumnModel);
            passModel.userModel = indexModel.userModel;
            return View("TicketView", passModel);
        }

        public IActionResult ViewTypeTickets(IndexSearchModel indexModel)
        {
            TicketDAO ticketDAO = new TicketDAO();
            var passModel = new TicketViewModel();
            passModel.ticketList = ticketDAO.GetTicketsThroughType(indexModel.ticketColumnModel);
            passModel.userModel = indexModel.userModel;
            return View("TicketView", passModel);
        }

        public IActionResult ViewProgressTickets(IndexSearchModel indexModel)
        {
            TicketDAO ticketDAO = new TicketDAO();
            var passModel = new TicketViewModel();
            passModel.ticketList = ticketDAO.GetTicketsThroughProgress(indexModel.ticketColumnModel);
            passModel.userModel = indexModel.userModel;
            return View("TicketView", passModel);
        }

        public IActionResult ViewUsernameTickets(IndexSearchModel indexModel)
        {
            TicketDAO ticketDAO = new TicketDAO();
            var passModel = new TicketViewModel();
            passModel.ticketList = ticketDAO.GetTicketsThroughUsername(indexModel.ticketColumnModel);
            passModel.userModel = indexModel.userModel;
            return View("TicketView", passModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Index");
        }
    }
}
