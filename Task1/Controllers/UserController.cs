using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1.Data;
using Task1.Models;
using Task1.ViewModel.User;

namespace Task1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        AppDBContext _context;

        public UserController(AppDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var student = _context.Users.Include(x => x.Student).Where(x => x.Id == id).FirstOrDefault();
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _context.Users.Include(x => x.Student).ToList();
            return View(students);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateUserVM user)
        {
            Student student = null;
            try
            {
                 student = _context.Students.Where(s => s.FirstName == user.StudentName.Split(' ', StringSplitOptions.None)[0]
                && s.FatherName == user.StudentName.Split(' ', StringSplitOptions.None)[1]
                && s.GrandFatherName == user.StudentName.Split(' ', StringSplitOptions.None)[2]
                && s.LastName == user.StudentName.Split(' ', StringSplitOptions.None)[3]).FirstOrDefault();
            }
            catch (Exception)
            {
                ViewBag.ErrMsg = "Enter Valid Student Name";
                return View(user);
            }

            if(student == null)
            {
                ViewBag.ErrMsg = "Student is Not Exists";
                return View(user);
            }

            if (_context.Users.Where(x => x.StudentId == student.Id).FirstOrDefault() != null)
            {
                ViewBag.ErrMsg = "User is Already Exists";
                return View(user);
            }

            if (_context.Users.Where(x => x.Password == user.Password).FirstOrDefault() != null)
            {
                ViewBag.ErrMsg = "User Password is Already Exists";
                return View(user);
            }

            

            User newUser = new User();
            newUser.state = user.state;
            newUser.StudentId = student.Id;
            newUser.TimeOfRegistrationOnSystem = DateTime.Now;
            newUser.Password = user.Password;

            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.ErrMsg = "Student is already Exists";
                return View(user);
            }
             

            return RedirectToAction("GetAll");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.state.ToString() == "0")
            {
                user.state = 1;
            }
            else
            {
                user.state = 0;
            }

            _context.SaveChanges();

            return RedirectToAction("GetAll");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Include(x => x.Student).Where(x=> x.Id == id).FirstOrDefault();
            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(double id)
        {
            var user = _context.Users.Include(x => x.Student).Where(x => x.Id == (int)id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            _context.Students.Remove(user.Student);
            _context.Users.Remove(user);

            _context.SaveChanges(); 

            return RedirectToAction("GetAll");
        }

    }
}
