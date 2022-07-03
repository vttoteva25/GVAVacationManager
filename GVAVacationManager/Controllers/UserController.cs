using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
//using GVAVacationManager.ActionFilters;
using GVAVacationManager.Data;
using GVAVacationManager.HelpingTools;
using GVAVacationManager.Models;
using GVAVacationManager.ViewModels.HomeVM;
using GVAVacationManager.ViewModels.UserVM;

namespace GVAVacationManager.Controllers
{
    [AuthenticationFilter]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<User> usersList = _db.Users;
            return View(usersList); 
        }

        #region LoginUser
        [HttpGet]
        public IActionResult Login()
        {
            LoginUserViewModel model = new LoginUserViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string username = model.Username;
                    string password = model.Password;
                    User user = new User();

                    if (_db.Users.Any(x => x.Username == username))
                    {
                        if (_db.Users.FirstOrDefault(x => x.Username == username).Password == Hasher.Hash(password))
                        {
                            user = _db.Users.FirstOrDefault(x => x.Username == username);
                        }
                        else
                        {
                            throw new ArgumentException("Incorrect username or password");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect username or password");
                    }

                    Logged.User = user;
                }

                catch (Exception)
                {
                    ModelState.AddModelError("loginError", "Невалидно потребителско име или парола");
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home"); ;
        }

        #endregion

        #region RegisterUser
        [HttpGet]
        public IActionResult Register()
        {
            RegisterUserViewModel model = new RegisterUserViewModel();
            model.Roles = _db.Roles;
            model.Teams = _db.Teams;
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("passConfirm", "Паролите не съвпадат!");
                    return View(model);
                }

                User user = new User();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Password = Hasher.Hash(model.Password);
                user.Username = model.Username;               


                Role role = new Role();
                if (model.RoleId.ToString()=="" || model.RoleId <= 0)
                {
                    role = _db.Roles.FirstOrDefault(x => x.RoleName == "Unassigned");
                    user.Role = role;
                }
                else
                {
                    role = _db.Roles.FirstOrDefault(x => x.Id == model.RoleId);
                    user.Role = role;
                }
                

                Team team = new Team();
                if (model.TeamId.ToString() != "" || model.TeamId <=0 )
                {
                   team = _db.Teams.FirstOrDefault(x => x.Id == model.TeamId);
                   user.TeamId = team.Id;
                }
                else
                {
                    user.TeamId = null;
                }

                try
                {
                    if (!CheckForExistingUser(model.Username))
                    {
                        _db.Users.Add(user);
                        _db.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentException("A user with the same username already exists");
                    }

                }
                catch (Exception)
                {

                    ModelState.AddModelError("usernameExists", "Потребител със същото име вече съществува");
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion
        private bool CheckForExistingUser(string username) => _db.Users.Any(x => x.Username == username);

        #region EditUser
        [Route("user/edit/{username}")]
        [HttpGet]
        public IActionResult Edit([FromRoute] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            User user = _db.Users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Username = user.Username;
            model.Password = user.Password;           
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("passConfirm", "Паролите не съвпадат!");
                    return View(model);
                }

                User updateUser = _db.Users.FirstOrDefault(x => x.Username == model.Username);                               
                updateUser.Password = Hasher.Hash(model.Password);

                if (updateUser is null)
                {
                    return NotFound();
                }
               
                _db.Users.Update(updateUser);
                _db.SaveChanges();

                Logged.User = null;

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        #endregion

        [HttpGet]
        [Route("user/profile/{username}")]
        public IActionResult Profile([FromRoute] string username)
        {
            User user = _db.Users.FirstOrDefault(x => x.Username == username);

            if (user is null)
            {
                return NotFound();
            }

            dynamic model = new ExpandoObject();

            model.User = user;
            model.Role = _db.Roles.FirstOrDefault(x => x.Id == user.RoleId);
            
            if(user.TeamId != null)
            {
                model.Team = _db.Teams.FirstOrDefault(x=>x.Id == user.TeamId);
            }

            return View(model);
        }

        public IActionResult SigningOut()
        {
            Logged.User = null;

            return RedirectToAction("Index", "Home");
        }


    }
}
