using SampleServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SampleServices.Controllers
{
    public class SchoolController : Controller
    {
        // GET: School\
        //string Baseurl = "http://localhost:64610/api/";
        //public async Task<ActionResult> Index()
        //{
        //    List<StudentViewModel> StudentInfo = new List<StudentViewModel>();
        //    using (var client = new HttpClient())
        //    {
        //        //Passing service base url
        //        client.BaseAddress = new Uri(Baseurl);
        //        client.DefaultRequestHeaders.Clear();
        //        //Define request data format
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        //        HttpResponseMessage Res = await client.GetAsync("Student?id=8");
        //        //Checking the response is successful or not which is sent using HttpClient
        //        if (Res.IsSuccessStatusCode)
        //        {
        //            //Storing the response details recieved from web api
        //            var StudentResponse = Res.Content.ReadAsStringAsync().Result;
        //            //Deserializing the response recieved from web api and storing into the Employee list
        //            StudentInfo = JsonConvert.DeserializeObject<List<StudentViewModel>>(StudentResponse);
        //        }
        //        //returning the employee list to view
        //        return View(StudentInfo);
        //    }
        //}
        /// <summary>
        /// Alternative method for consuming the Web Api
        /// </summary>
        /// <returns></returns>
        // GET: Student
        public ActionResult Index()
        {
            IEnumerable<StudentViewModel> students = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64610/api/");
                //HTTP GET
                Task<HttpResponseMessage> responseTask = client.GetAsync("Student");
                responseTask.Wait();

                HttpResponseMessage result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Task<IList<StudentViewModel>> readTask = result.Content.ReadAsAsync<IList<StudentViewModel>>();
                    readTask.Wait();

                    students = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    students = Enumerable.Empty<StudentViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(students);
        }
        // GET: School/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: School/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: School/Create
        [HttpPost]
        public ActionResult Create(StudentViewModel studentViewModel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64610/api/Student");
                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync<StudentViewModel>("Student", studentViewModel);
                postTask.Wait();

                HttpResponseMessage res = postTask.Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            return View(studentViewModel);
        }

        // GET: School/Edit/5
        public ActionResult Edit(int id)
        {
            IEnumerable<StudentViewModel> viewModel = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64610/api/");
                Task<HttpResponseMessage> resTask = client.GetAsync("Student?id=" + id.ToString());
                resTask.Wait();

                HttpResponseMessage res = resTask.Result;
                if (res.IsSuccessStatusCode)
                {
                    Task<IList<StudentViewModel>> readTask = res.Content.ReadAsAsync<IList<StudentViewModel>>();
                    readTask.Wait();
                    viewModel = readTask.Result;
                }
            }
            foreach (var item in viewModel)
            {
                return View(item);
            }
            return View();
        }

        // POST: School/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, StudentViewModel studentView)
        {
           
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:64610/api/Student");
                    Task<HttpResponseMessage> putTask = client.PutAsJsonAsync<StudentViewModel>("Student", studentView);
                    putTask.Wait();

                    HttpResponseMessage res = putTask.Result;
                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            
            return View(studentView);
        }

        // GET: School/Delete/5
        public ActionResult Delete(int id)
        {
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64610/api/");
                var deleteTask = client.DeleteAsync("Student?StudentID=" + id.ToString());

                var res = deleteTask.Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        // POST: School/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
