using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TopSpeed.Web.Data;
using TopSpeed.Web.Models;

namespace TopSpeed.Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IWebHostEnvironment _WebHostEnvironment;//which is healful to handle the image upload in our projects;
        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbContext;
            _WebHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbcontext.Brand.ToList();

            return View(brands);
        }


        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(Brand brand)
        {
            string webrootpath = _WebHostEnvironment.WebRootPath;//which is helpful to access  webroot path;

            var file = HttpContext.Request.Form.Files;

            if(file.Count > 0 )
            {
                string  newfilename=Guid.NewGuid().ToString();

                var upload=Path.Combine(webrootpath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using(var filestream=new FileStream(Path.Combine(upload, newfilename+extension), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                brand.BrandLogo=@"\images\brand\"+newfilename+extension;

                
            }

            if(ModelState.IsValid)
            {
                _dbcontext.Brand.Add(brand);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand=_dbcontext.Brand.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpGet]

        public IActionResult Edit(Guid id)
        {
            Brand brand = _dbcontext.Brand.Find(id);
            return View(brand);
        }

        [HttpPost]

        public IActionResult Edit(Brand brand)
        {
            string webrootpath = _WebHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newfilename = Guid.NewGuid().ToString();

                var upload = Path.Combine(webrootpath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                //delete old image

                var objFromDb=_dbcontext.Brand.AsNoTracking().FirstOrDefault(x=>x.Id==brand.Id);

                if(objFromDb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webrootpath, objFromDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var filestream = new FileStream(Path.Combine(upload, newfilename + extension), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                brand.BrandLogo = @"\images\brand\" + newfilename + extension;


            }

            if (ModelState.IsValid)
            {
                var objFromDb = _dbcontext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                 
                objFromDb.Name = brand.Name;
                objFromDb.EstablishedYear = brand.EstablishedYear;

                if (objFromDb !=null)
                {
                    objFromDb.BrandLogo = brand.BrandLogo;
                }

                _dbcontext.Update(objFromDb);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var  brand=_dbcontext.Brand.Find(id);
            return View(brand);
        }

        [HttpPost]

        public IActionResult Delete(Brand brand)
        {
            string webrootpath = _WebHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(brand.BrandLogo))
            {

                var objFromDb = _dbcontext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (objFromDb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webrootpath, objFromDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

            }

            
                _dbcontext.Remove(brand);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            
            

        }




      

    }
}
