using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PonyAzureProxy.Controllers
{
    public class DevHelperController : Controller
    {
        //public static WebApplication app;

        ApplicationDbContext context;
        UserManager<IdentityUser> _userManager;
        IWebHostEnvironment env;
        public DevHelperController(ApplicationDbContext context, UserManager<IdentityUser> _userManager, IWebHostEnvironment env)
        {
            this.context = context;
            this._userManager = _userManager;
            this.env = env;

        }



        // GET: DevHelperController
        public ActionResult Index()
        {
            var LogPath = Path.Combine(env.ContentRootPath, Globals.W3CLOGDIR);
            DirectoryInfo di = new DirectoryInfo(LogPath);
            foreach (var f in di.GetFiles())
            {


            }

            var logFiles = di.GetFiles().Select(x => x.Name).ToList();
            ViewBag.logFiles = logFiles;
            return View();
        }


        //[HttpGet("ViewLog/{fileName}")]
        [HttpGet]
        [Route("{controller}/{action}/{fileName}")]
        public ActionResult ViewLog(string fileName)
        {
            var txt = "";

            try
            {
                var LogFilePath = Path.Combine(env.ContentRootPath, Globals.W3CLOGDIR, fileName);
                txt = System.IO.File.ReadAllText(LogFilePath);
                ViewBag.log = txt;

            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);

            }

            return Content(txt);

        }


        public ActionResult ClearW3CLogs()
        {
            var LogPath = Path.Combine(env.ContentRootPath, Globals.W3CLOGDIR);
            DirectoryInfo di = new DirectoryInfo(LogPath);
            foreach (var f in di.GetFiles())
            {

                try
                {
                    f.Delete();
                }
                catch (Exception)
                {


                }

            }

            return View("Index");



            //var LogFilePath = Path.Combine(env.ContentRootPath, "logs", fileName);
            //foreach ( var f in )
            //var txt = System.IO.File.ReadAllText(LogFilePath);
            //ViewBag.log = txt;
            //return View("Index");

        }


        // GET: DevHelperController/Details/5
        public ActionResult Details(int id)
        {



            return View();
        }

        public ActionResult CreateSampleData(int id)
        {

            SampleData.Initialize(context, _userManager);


            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            //    var _userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();


            //    //SampleData.Initialize(scope.ServiceProvider );
            //    SampleData.Initialize(context, _userManager);
            //}


            ViewBag.Message = "Sample Users Created";

            return View("Index");
        }





        // GET: DevHelperController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DevHelperController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DevHelperController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DevHelperController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DevHelperController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DevHelperController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
