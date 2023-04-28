using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

		public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
		{
			_databaseContext = databaseContext;
			_configuration = configuration;
		}

        [AllowAnonymous]
		public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = DoMD5HashedString(model.Password);

                User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);

                if(user != null)
				{
					if (user.Locked)
					{
                        ModelState.AddModelError(nameof(model.Username), "User is locked.");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Username", user.Username));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
				else
				{
                    ModelState.AddModelError("", "Username or password is incorrect.");
				}
            }

            return View(model);
        }



        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        public IActionResult CreatePages()
        {
            ProfileInfoLoader();
            return View();
        }

        
        private string fileName1;
        private string fileName2;

        [HttpPost]
        public async Task<IActionResult> CreatePages([FromForm] PageModel model)
        {
            if (ModelState.IsValid)
            {
                bool javaCheckbox = Request.Form["javaCheckbox"] == "on";
                string javaLevel = Request.Form["javaLevel"];

                bool pythonCheckbox = Request.Form["pythonCheckbox"] == "on";
                string pythonLevel = Request.Form["pythonLevel"];

                bool cSharpCheckbox = Request.Form["cSharpCheckbox"] == "on";
                string cSharpLevel = Request.Form["cSharpLevel"];

                bool unityCheckbox = Request.Form["unityCheckbox"] == "on";
                string unityLevel = Request.Form["unityLevel"];

                bool figmaCheckbox = Request.Form["figmaCheckbox"] == "on";
                string figmaLevel = Request.Form["figmaLevel"];

                string backgroundColor = Request.Form["colorPicker"];
                string cardInfoColor = Request.Form["colorPicker1"];
                string foregroundColor = Request.Form["colorPicker2"];

                string selectedFont = Request.Form["fontSelect"];


                List<string> skill_names = new List<string>();
                skill_names.Add(Request.Form["skillName0"]);
                skill_names.Add(Request.Form["skillName1"]);
                skill_names.Add(Request.Form["skillName2"]);
                skill_names.Add(Request.Form["skillName3"]);
                skill_names.Add(Request.Form["skillName4"]);


                if (model.PageImage != null)
                {
                    fileName1 = Path.GetFileName(model.PageImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName1);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PageImage.CopyToAsync(fileStream);
                    }
                }

                if (model.CVPath != null)
                {
                    fileName2 = Path.GetFileName(model.CVPath.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "public", "uploads", fileName2);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CVPath.CopyToAsync(fileStream);
                    }
                }

                string fileName = $"{model.Fullname}_{model.Id}.html";

                

                PPaages page = new()
                {
                    Id = model.Id,
                    Fullname = model.Fullname,
                    CVText = model.CVText,
                    CVPath = fileName2,
                    PortfolioText = model.PortfolioText,
                    PortfolioPath = model.PortfolioPath,
                    Contact = model.Contact,
                    AboutMe = model.AboutMe,
                    PageImage = fileName1,
                    Description = model.Description,
                    PageURL = fileName
                };

                Project project = new()
                {
                    Id = model.Id,
                    ProjectImage = $"/uploads/{fileName1}",
                    Fullname = model.Fullname,
                    Description = model.Description,
                    ProjectURL = $"public/uploads/{fileName}"
                };

                var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "public", "uploads", fileName);
                if (System.IO.File.Exists(filePath1))
                {
                    ModelState.AddModelError("", "Aynı isimde zaten bir dosya mevcut");
                    ViewData["result"] = "multiple copies";
                    return View(model);
                }

                

                AddLanguages(model, javaCheckbox, skill_names, javaLevel, pythonCheckbox, pythonLevel, cSharpCheckbox, cSharpLevel, unityCheckbox, unityLevel, figmaCheckbox, figmaLevel);



                _databaseContext.PPaages.Add(page);
                _databaseContext.Projects.Add(project);
                
                int affectedRowCount = _databaseContext.SaveChanges();
                if (affectedRowCount == 0)
                {
                    ModelState.AddModelError("", "Page can not be added.");
                }
                else
                {
                    ViewData["result"] = "PageAdded";
                    
                }



                

                
                
                
                using (var streamWriter = new StreamWriter(filePath1))
                {





                    // Write the HTML content to the response stream
                    await streamWriter.WriteAsync(@"<!DOCTYPE html>
<html>
<head>
    <title>" + page.Fullname + @" - Portfolio</title>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.1.0/css/bootstrap.min.css'>
    <style>
        body {
            font-family: " + selectedFont + @", sans-serif;
            margin: 0;
            padding: 0;
        }

        .header {
            background-color: #333;
            color: #fff;
            padding: 20px;
            text-align: center;
        }

        h1 {
            margin-top: 0;
        }

        .content {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            padding: 20px;
        }

        .content-item {
            width: 400px;
            margin: 20px;
        }

        .content-item h2 {
            margin-top: 0;
        }

        .content-item p {
            margin-top: 10px;
        }

        .content-item a {
            display: inline-block;
            margin-top: 10px;
            padding: 10px 20px;
            background-color: #4CAF50;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }

        .content-item a:hover {
            background-color: #3e8e41;
        }
        body {
            color: " + foregroundColor + @";
            background-color:" + backgroundColor + @";
        }
        h1 {
            color: black;
        }
        .hover-zoom:hover {
            transform: scale(1.1);
            transition: transform 0.3s ease;
        }

        .card {
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
        }
    </style>
</head>
<body>
    <div class='container mt-5'>
        <div class='card'>
            <div class='card-header'>
                <h1 class='text-center'>" + page.Fullname + " - Porfolio" + @"</h1>
            </div>
            <div class='card-body'>
                <div class='row'>
                    <div class='col-md-6'>
                        <div class='card mb-3 hover-zoom' style='background-color: " + cardInfoColor + @"'>
                            <div class='card-body'>
                                <h2 class='card-title'>About Me</h2>
                                <p class='card-text'>" + page.AboutMe + @"</p>
                            </div>
                        </div>
                        <div class='card mb-3 hover-zoom' style='background-color: " + cardInfoColor + @"'>
                            <div class='card-body'>
                                <h2 class='card-title'>Contact Information</h2>
                                <p class='card-text'>" + page.Contact + @"</p>
                            </div>
                        </div>
                        <div class='card mb-3 hover-zoom' style='background-color: " + cardInfoColor + @"'>
<div class='card-body'>
<h2 class='card-title'>Skills</h2>
");




                    int i = 0;
                    foreach (KeyValuePair<string, string> languageLevel in model.MyDictionary)
                    {
                        string language = languageLevel.Key;
                        string level = languageLevel.Value;

                        string colorClass = "";
                        switch (i)
                        {
                            case 0:
                                colorClass = "bg-primary";
                                i++;
                                break;
                            case 1:
                                colorClass = "bg-success";
                                i++;
                                break;
                            case 2:
                                colorClass = "bg-info";
                                i++;
                                break;
                            default:
                                colorClass = "bg-secondary";
                                i = 0;
                                break;

                        }

                        string progressBar = @"
        <h5 class='card-title'>" + language + @"</h5>
        <div class='progress'>
            <div class='progress-bar " + colorClass + @"' role='progressbar' style='width: " + level + @"%;' aria-valuenow='" + level + @"' aria-valuemin='0' aria-valuemax='100'>" + level + @"%</div>
        </div> <br/>";


                        await streamWriter.WriteAsync(progressBar);
                    }

                    await streamWriter.WriteAsync(@"</div></div>
                            
                            <div class='card mb-3 hover-zoom' style='background-color: " + cardInfoColor + @"'>
                                    <div class='card-body'>
                                        <h2 class='card-title'>Project Explanation</h2>
                                        <p class='card-text'>" + page.PortfolioText + @"</p>
                                    </div>
                                </div>
                                <div class='card mb-3 hover-zoom' style='background-color: " + cardInfoColor + @"'>
                                    <div class='card-body'>
                                        <h2 class='card-title'>Project Github Link</h2>
                                        <p class='card-text'>" + page.PortfolioPath + @"</p>
                                    </div>
                                </div>
                            </div>
                            <div class='col-md-6'>                                
                                <div class='card mb-3 hover-zoom' style='background-color: " + cardInfoColor + @"'>
                                    <div class='card-body'>
                                        <h2 class='card-title'>CV</h2>
                                        <p class='card-text'>" + page.CVText + @"</p>
                                        <canvas id='pdf-canvas'></canvas>
                                        <a href='/public/uploads/" + page.CVPath + @"' class='btn btn-primary' download='" + page.CVPath + @"'>Download CV</a>
                                    </div>                                
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- Bootstrap 5 JS -->
    <script src='https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.1.0/js/bootstrap.bundle.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/pdfobject/2.1.1/pdfobject.min.js' integrity='sha512-pMmHr/SRLz0t+c1Ym83s+D9sCtqEjJRW/58hIQLwHxvjIm+JNlOCCvNmtHSTCEk9OQF3qZjvPZluJkDmyI1+lw==' crossorigin='anonymous' referrerpolicy='no-referrer'></script>
   

    <!-- PDF dosyasını yüklemek için gerekli olan PDF.js kütüphanesi -->
    <script src='https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.10.377/pdf.min.js'></script>

    <script>
        const url = '/public/uploads/" + page.CVPath + @"';
        pdfjsLib.getDocument(url).promise.then(pdf => {
            const numPages = pdf.numPages;
            pdf.getPage(1).then(page => {
                const viewport = page.getViewport({ scale: 1.0 });
                const canvas = document.getElementById('pdf-canvas');
                const context = canvas.getContext('2d');
                canvas.height = viewport.height;
                canvas.width = viewport.width;
                page.render({ canvasContext: context, viewport: viewport });
            });
        });
    </script>                        
        </body>

        </html>");


                    // Yönlendirme yapılır                    
                    
                    


                }




            }
            else
            {
                ModelState.AddModelError("", "You must fill required fields.");
            }
            
            
            ViewData["result"] = "PageAdded";
            return View("CreatePages");
        }


        [HttpPost]
        public IActionResult AddLanguages(PageModel pageModel, bool javaCheckbox, List<string> skillnames, string javaLevel, bool pythonCheckbox,
            string pythonLevel, bool cSharpCheckbox, string cSharpLevel, bool unityCheckbox, string unityLevel, bool figmaCheckbox, string figmaLevel)
        {
            


            // Dictionary'yi oluşturun
            Dictionary<string, string> languages = new Dictionary<string, string>();

            // Seçilen dilleri ve seviyelerini Dictionary'ye ekleyin
            if (javaCheckbox && !string.IsNullOrEmpty(javaLevel))
            {
                languages.Add(skillnames[0], javaLevel);
            }

            if (pythonCheckbox && !string.IsNullOrEmpty(pythonLevel))
            {
                languages.Add(skillnames[1], pythonLevel);
            }

            if (cSharpCheckbox && !string.IsNullOrEmpty(cSharpLevel))
            {
                languages.Add(skillnames[2], cSharpLevel);
            }

            if (unityCheckbox && !string.IsNullOrEmpty(unityLevel))
            {
                languages.Add(skillnames[3], unityLevel);
            }

            if (figmaCheckbox && !string.IsNullOrEmpty(figmaLevel))
            {
                languages.Add(skillnames[4], figmaLevel);
            }

            // Dictionary'yi PageModel'deki uygun özelliğe atayın
            pageModel.MyDictionary = languages;

            ViewData["result"] = "added";
            return View("CreatePages");
        }





        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Username == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists.");
                    return View(model);
                }

                string hashedPassword = DoMD5HashedString(model.Password);

                User user = new()
                {
                    Username = model.Username,
                    Password = hashedPassword
                };

                _databaseContext.Users.Add(user);
                int affectedRowCount = _databaseContext.SaveChanges();
                if (affectedRowCount == 0)
                {
                    ModelState.AddModelError("", "User can not be added.");
                }
                else
                {
                    ViewData["result"] = "RegisterSuccessfuly";
                    return View(model);
                }
            }
            return View(model);
        }

        private string DoMD5HashedString(string str)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = str + md5Salt;
            string hashed = salted.MD5();
            return hashed;
        }

        public IActionResult Profile()
        {
            ProfileInfoLoader();

            return View();
        }


        private void ProfileInfoLoader()
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

            ViewData["id"] = user.Id;
            ViewData["FullName"] = user.FullName;
            ViewData["ProfileImage"] = user.ProfileImageFileName;
        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required][StringLength(50)] string? fullname)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

                user.FullName = fullname;

                _databaseContext.SaveChanges();

                ViewData["result"] = "FullNameChanged";
            }
            ProfileInfoLoader();
            return View("Profile");
        }

        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

                string hashedPassword = DoMD5HashedString(password);

                user.Password = hashedPassword;              
                _databaseContext.SaveChanges();

                ViewData["result"] = "PasswordChanged";
            }
            ProfileInfoLoader();
            return View("Profile");
        }

        [HttpPost]
        public IActionResult ProfileChangeImage([Required] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

                string fileName = $"guid_{userId}.jpg";

                Stream stream = new FileStream($"wwwroot/uploads/{fileName}", FileMode.OpenOrCreate);

                file.CopyTo(stream);

                stream.Close();
                stream.Dispose();

                user.ProfileImageFileName = fileName;
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }
            ProfileInfoLoader();
            return View("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
