using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1.Data;
using Task1.Models;
using Task1.ViewModel.Student;

namespace Task1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {

        AppDBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StudentController(AppDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var students = _context.Students.Where(x => x.Id == id).FirstOrDefault();
            return View(students);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _context.Students.ToList();
            return View(students);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateStudentVM student)
        {
            if (ModelState.IsValid)
            {
                //check id uniquness.
                var studentCheck = _context.Students.Where(x => x.IdNumber == student.IdNumber).FirstOrDefault();
                if (studentCheck != null)
                {
                    ViewBag.ErrorMsg = "ID Number Is Already Exsists.";
                    return View(student);
                }

                Student newStudent = new Student();
                newStudent.IdNumber = student.IdNumber;
                newStudent.FirstName = student.FirstName;
                newStudent.FatherName = student.FatherName;
                newStudent.GrandFatherName = student.GrandFatherName;
                newStudent.LastName = student.LastName;
                newStudent.Gender = student.Gender;

                //upload Photograph
                //newStudent.Photograph = student.PhotographName;

                var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ImagesApp");
                var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(student.PhotographImage.FileName);
                var filePath = Path.Combine(uploadFolder, uniqueName);
                // {serverlocation}\Images\0f8fad5b-d9cb-469f-a165-70867728950e.jpg

                //student.PhotographImage.CopyTo(new FileStream(filePath, FileMode.Create));
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                student.PhotographImage.CopyTo(fileStream);
                fileStream.Close();
                newStudent.Photograph = uniqueName;
                

                //upload IdPhoto
                //newStudent.IdPhoto = student.IdPhotoName;

                var uploadFolder2 = Path.Combine(_hostingEnvironment.WebRootPath, "ImagesApp");
                var uniqueName2 = Guid.NewGuid().ToString() + Path.GetExtension(student.IdPhotoImage.FileName);
                var filePath2 = Path.Combine(uploadFolder2, uniqueName2);
                // {serverlocation}\Images\0f8fad5b-d9cb-469f-a165-70867728950e.jpg

                FileStream fileStream2 = new FileStream(filePath2, FileMode.Create);
                student.IdPhotoImage.CopyTo(fileStream2);
                fileStream2.Close();
                newStudent.IdPhoto = uniqueName2;


                newStudent.Address = student.Address;
                newStudent.MobileNumber = student.MobileNumber;
                newStudent.DateOfBirth = student.DateOfBirth;
                newStudent.DateOfEnrollmentSchool = student.DateOfEnrollmentSchool;
                newStudent.Level = student.Level;
                newStudent.EvaluateLastYear = student.EvaluateLastYear;
                newStudent.TimeOfRegistrationOnSystem = DateTime.Now;

                _context.Students.Add(newStudent);
                _context.SaveChanges();

                return RedirectToAction("GetAll");
            }
            else
            {
                return View(student);
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var student = _context.Students.Find(id);
            if(student == null)
            {
                return NotFound();
            }

            UpdateStudentVM updateStudentVM = new UpdateStudentVM();
            updateStudentVM.IdNumber = student.IdNumber;
            updateStudentVM.FirstName = student.FirstName;
            updateStudentVM.FatherName = student.FatherName;
            updateStudentVM.GrandFatherName = student.GrandFatherName;
            updateStudentVM.LastName = student.LastName;
            updateStudentVM.Gender = student.Gender;
            updateStudentVM.PhotographName = student.Photograph;
            updateStudentVM.IdPhotoName = student.IdPhoto;
            updateStudentVM.Address = student.Address;
            updateStudentVM.MobileNumber = student.MobileNumber;
            updateStudentVM.DateOfBirth = student.DateOfBirth;
            updateStudentVM.DateOfEnrollmentSchool = student.DateOfEnrollmentSchool;
            updateStudentVM.Level = student.Level;
            updateStudentVM.EvaluateLastYear = student.EvaluateLastYear;

            return View(updateStudentVM);
        }


        [HttpPost]
        public IActionResult Update(int id, UpdateStudentVM student)
        {
            var studetExists = _context.Students.Find(id);
            if (studetExists == null)
                return NotFound();
            else
            {
                if (studetExists.IdNumber != student.IdNumber) { 
                    var studentCheck = _context.Students.Where(x => x.IdNumber == student.IdNumber).FirstOrDefault();
                    if (studentCheck != null)
                    {
                        ViewBag.ErrorMsg = "ID Number Is Already Exsists.";
                        return View(student);
                    }
                }

                if (ModelState.IsValid)
                {
                    if (student.PhotographImage != null)
                    {
                        var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ImagesApp");
                        var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(student.PhotographImage.FileName);
                        var filePath = Path.Combine(uploadFolder, uniqueName);
                        // {serverlocation}\Images\0f8fad5b-d9cb-469f-a165-70867728950e.jpg

                        //Delete Old File
                        string oldFileName = studetExists.Photograph;
                        string fulloldpath = Path.Combine(uploadFolder, oldFileName);
                        System.IO.File.Delete(fulloldpath);
                        //System.IO.File.Move(sourse,destination);

                        //Save New File
                        FileStream fileStream = new FileStream(filePath, FileMode.Create);
                        student.PhotographImage.CopyTo(fileStream);
                        fileStream.Close();
                        //update Event Info
                        studetExists.Photograph = uniqueName;

                    }

                    if (student.IdPhotoImage != null)
                    {
                        var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ImagesApp");
                        var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(student.IdPhotoImage.FileName);
                        var filePath = Path.Combine(uploadFolder, uniqueName);
                        // {serverlocation}\Images\0f8fad5b-d9cb-469f-a165-70867728950e.jpg

                        //Delete Old File
                        string oldFileName = studetExists.IdPhoto;
                        string fulloldpath = Path.Combine(uploadFolder, oldFileName);
                        System.IO.File.Delete(fulloldpath);
                        //System.IO.File.Move(sourse,destination);

                        //Save New File
                        FileStream fileStream = new FileStream(filePath, FileMode.Create);
                        student.IdPhotoImage.CopyTo(fileStream);
                        fileStream.Close();
                        //update Event Info
                        studetExists.IdPhoto = uniqueName;

                    }

                    //use uto mapper here to map between them.

                    studetExists.IdNumber = student.IdNumber;
                    studetExists.FirstName = student.FirstName;
                    studetExists.FatherName = student.FatherName;
                    studetExists.GrandFatherName = student.GrandFatherName;
                    studetExists.LastName = student.LastName;
                    studetExists.Gender = student.Gender;
                    studetExists.Address = student.Address;
                    studetExists.MobileNumber = student.MobileNumber;
                    studetExists.DateOfBirth = student.DateOfBirth;
                    studetExists.DateOfEnrollmentSchool = student.DateOfEnrollmentSchool;
                    studetExists.Level = student.Level;
                    studetExists.EvaluateLastYear = student.EvaluateLastYear;
                    //studetExists.TimeOfRegistrationOnSystem = DateTime.Now;

                    _context.SaveChanges();

                    return RedirectToAction("GetAll");
                }
                else
                {
                    return View(student);
                }

            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            DeleteStudentVM deleteStudentVM = new DeleteStudentVM();
            deleteStudentVM.IdNumber = student.IdNumber;
            deleteStudentVM.FirstName = student.FirstName;
            deleteStudentVM.FatherName = student.FatherName;
            deleteStudentVM.GrandFatherName = student.GrandFatherName;
            deleteStudentVM.LastName = student.LastName;
            deleteStudentVM.Gender = student.Gender;
            deleteStudentVM.PhotographName = student.Photograph;
            deleteStudentVM.IdPhotoName = student.IdPhoto;
            deleteStudentVM.Address = student.Address;
            deleteStudentVM.MobileNumber = student.MobileNumber;
            deleteStudentVM.DateOfBirth = student.DateOfBirth;
            deleteStudentVM.DateOfEnrollmentSchool = student.DateOfEnrollmentSchool;
            deleteStudentVM.Level = student.Level;
            deleteStudentVM.EvaluateLastYear = student.EvaluateLastYear;

            return View(deleteStudentVM);
        }

        [HttpPost]
        public IActionResult Delete(double id)
        {
            var deleteStudent = _context.Students.Include(x => x.User).Where(x=> x.Id == (int)id).FirstOrDefault();
            if (deleteStudent == null)
            {
                return NotFound();
            }

            _context.Students.Remove(deleteStudent);
            if (deleteStudent.User != null)
            {
                _context.Users.Remove(deleteStudent.User);
            }

            _context.SaveChanges();

            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string searchField, string searchValue)
        {
            if(searchField == "Name")
            {
                Student student = null;
                try
                {
                    student = _context.Students.Include(x => x.User).Where(s => s.FirstName == searchValue.Split(' ', StringSplitOptions.None)[0]
                   && s.FatherName == searchValue.Split(' ', StringSplitOptions.None)[1]
                   && s.GrandFatherName == searchValue.Split(' ', StringSplitOptions.None)[2]
                   && s.LastName == searchValue.Split(' ', StringSplitOptions.None)[3]).FirstOrDefault();
                }
                catch (Exception)
                {
                    ViewBag.ErrMsg = "Enter Valid Student Name";
                    return View();
                }

                return View(student);   
            }
            else if (searchField == "ID")
            {
                Student student = _context.Students.Include(x => x.User).Where(x => x.Id == int.Parse(searchValue)).FirstOrDefault();
                if(student == null)
                {
                    ViewBag.ErrMsg = "Enter Valid Student ID";
                    return View();
                }
                return View(student);
            }
            else if (searchField == "IDNumber")
            {
                Student student = _context.Students.Include(x => x.User).Where(x => x.IdNumber == searchValue).FirstOrDefault();
                if (student == null)
                {
                    ViewBag.ErrMsg = "Enter Valid Student Ientification Number";
                    return View();
                }

                return View(student);
            }

            return View();
        }



    }
}
  