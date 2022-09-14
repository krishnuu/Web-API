using SampleServices.Models;
using SampleServices.SchoolDB;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SampleServices.Controllers
{
    public class StudentController : ApiController
    {

        public StudentController()
        {
        }
        #region Implement Get methods
        public IHttpActionResult GetStudentById(int id)
        {
            IList<StudentViewModel> students = null;
            using (SchoolDBEntities ctx = new SchoolDBEntities())
            {
                students = ctx.Students.Where(s => s.StudentID == id)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName
                    }).ToList<StudentViewModel>();
            }

            if (students.Count == 0)
            {
                return NotFound();
            }
            return Ok(students);
        }

        public IHttpActionResult GetAllStudentsWithAddress()
        {
            IList<StudentViewModel> stud = null;
            using (SchoolDBEntities ctx = new SchoolDBEntities())
            {
                stud = ctx.Students.Include("StudentAddress").Select(s => new StudentViewModel()
                {
                    Id = s.StudentID,
                    StudentName = s.StudentName,
                    StandardId=s.StandardId,
                    Address = s.StudentAddress == null ? null : new AddressViewModel()
                    {
                        StudentId = s.StudentAddress.StudentID,
                        Address1 = s.StudentAddress.Address1,
                        Address2 = s.StudentAddress.Address2,
                        City = s.StudentAddress.City,
                        State = s.StudentAddress.State
                    }
                }).ToList<StudentViewModel>();
            }
            if (stud == null)
            {
                return NotFound();
            }
            return Ok(stud);
        }

        public IHttpActionResult GetAllStudents(string name)
        {
            IList<StudentViewModel> studs = null;
            using (SchoolDBEntities ctx = new SchoolDBEntities())
            {
                studs = ctx.Students.Where(s => s.StudentName.ToLower() == name.ToLower())
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
                        Address = s.StudentAddress == null ? null : new AddressViewModel()
                        {
                            Address1 = s.StudentAddress.Address1,
                            Address2 = s.StudentAddress.Address2,
                            State = s.StudentAddress.State,
                            City = s.StudentAddress.City,
                            StudentId = s.StudentAddress.StudentID
                        }
                    }).ToList<StudentViewModel>();
            }

            if (studs == null)
            {
                return NotFound();
            }
            return Ok(studs);
        }

        public IHttpActionResult GetAllStudentsInNameStandard(int standardId)
        {
            IList<StudentViewModel> stu = null;
            using (SchoolDBEntities ctx = new SchoolDBEntities())
            {
                stu = ctx.Students.Include("Standard").Where(s => s.StandardId == standardId)
                    .Select(s => new StudentViewModel()
                    {
                        Id = s.StudentID,
                        StudentName = s.StudentName,
                        Address = s.StudentAddress == null ? null : new AddressViewModel()
                        {
                            StudentId = s.StudentAddress.StudentID,
                            Address1 = s.StudentAddress.Address1,
                            Address2 = s.StudentAddress.Address2,
                            City = s.StudentAddress.City,
                            State = s.StudentAddress.State
                        },
                        Standard = new StandardViewModel()
                        {
                            StandardId = s.Standard.StandardId,
                            Name = s.Standard.StandardName
                        }
                    }).ToList<StudentViewModel>();
            }

            if (stu == null)
            {
                return NotFound();
            }

            return Ok(stu);
        }


        #endregion

        #region Implement Post Methods

        [HttpPost]
        public IHttpActionResult InsertStudentDetails(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data...");
            }
            using (SchoolDBEntities ctx = new SchoolDBEntities())
            {
                ctx.Students.Add(new Student()
                {
                   // StudentID=student.Id,
                    StudentName = student.StudentName,
                    StandardId = student.StandardId
                });
                ctx.SaveChanges();
            }
            return Ok();
        }
        #endregion

        #region implement put method
        [HttpPut]
        public IHttpActionResult UpdateStudentDetails(StudentViewModel studentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid model");
            }
            using (var ctx=new SchoolDBEntities())
            {
                var oldData = ctx.Students.Where(s => s.StudentID == studentViewModel.Id).FirstOrDefault<Student>();
                if (oldData!=null)
                {
                    oldData.StudentName = studentViewModel.StudentName;
                    oldData.StandardId = studentViewModel.StandardId;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
                return Ok();
        }
        #endregion

        #region Implement Delete Methods
        [HttpDelete]
        public IHttpActionResult DeleteStudentDetails(int studentID)
        {
            if (studentID<=0)
            {
                BadRequest("Not a valid student id");
            }
            using (var ctx=new SchoolDBEntities())
            {
                var student = ctx.Students
                .Where(s => s.StudentID == studentID)
                .FirstOrDefault();

                ctx.Entry(student).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return Ok();
        }
        #endregion
    }
}
