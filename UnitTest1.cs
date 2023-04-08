using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace TestProject1_Nunit
{
    [TestFixture]
    public class Tests
    {
        
        public IWebDriver Driver;

        public string IamgePath { get; private set; }

        private ExtentReports _extent;
        private ExtentHtmlReporter _htmlReporter;
        private string AppUrl;

        private ExtentTest test;

        [OneTimeSetUp]
        public void Setup()
        {
            AppUrl = "https://contactappyhra.herokuapp.com";

            // Inicializar ExtentReports
            _extent = new ExtentReports();

            // Configurar ExtentHtmlReporter
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string solutionPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\" + @"Reportes/Report.html"));
            _htmlReporter = new ExtentHtmlReporter(solutionPath);
            _htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            _htmlReporter.Config.DocumentTitle = "Reporte de prueba";
            _htmlReporter.Config.ReportName = "Resultados de la prueba";

            // Adjuntar el reporter a ExtentReports
            _extent.AttachReporter(_htmlReporter);
        }

        [SetUp]
        public void BeforeTest()
        {
            Driver = new ChromeDriver("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe");
            test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TestLoginSuccess()
        {
            test.Log(Status.Info, "Inicio el test de el login.");

            Driver.Navigate().GoToUrl(AppUrl);
            Driver.Manage().Window.Size = new System.Drawing.Size(1051, 845);
            Driver.FindElement(By.Id("email")).SendKeys("email@dominio.prueba");
            Driver.FindElement(By.Id("password")).SendKeys("password");
            Driver.FindElement(By.Id("submit")).Click();
            var res = Driver.FindElement(By.Id("alert")).Text;
            Assert.That(res, Is.EqualTo("Ha iniciado seccion correctamente"));

            ShopPrintScreen();
            test.Log(Status.Pass, "Se completó el test de el login");

            Driver.Close();
        }

        [Test]
        public void TestAddContactSuccess()
        {
            test.Log(Status.Info, "Inicio el test de agregar contacto.");

            Driver.Navigate().GoToUrl(AppUrl +"/home");
            Driver.Manage().Window.Size = new System.Drawing.Size(1552, 880);
            Driver.FindElement(By.Id("FirstName")).Click();
            Driver.FindElement(By.Id("FirstName")).SendKeys("Yohan");
            Driver.FindElement(By.Name("LastName")).SendKeys("Rijo");
            Driver.FindElement(By.Name("Email")).SendKeys("20201@itla.edu.do");
            Driver.FindElement(By.Name("Mobile")).SendKeys("141421");
            ShopPrintScreen();

            Driver.FindElement(By.CssSelector(".btn-primary")).Click();
            var res = Driver.FindElement(By.Id("alert")).Text;
            Assert.That(res, Is.EqualTo("Contacto agregado exitosamente"));

            ShopPrintScreen();
            test.Log(Status.Pass, "Se completó el test de agregar contacto.");

            Driver.Close();
        }

        [Test]
        public void TestEditContactSuccess()
        {
            test.Log(Status.Info, "Inicio el test de editar contacto.");

            Driver.Navigate().GoToUrl(AppUrl + "/home");
            Driver.Manage().Window.Size = new System.Drawing.Size(1552, 880);
            Driver.FindElement(By.LinkText("Update")).Click();

            Driver.FindElement(By.Id("FirstName")).Click();
            Driver.FindElement(By.Id("FirstName")).SendKeys("Yohan");
            Driver.FindElement(By.Name("LastName")).SendKeys("Rijo");
            Driver.FindElement(By.Name("Email")).SendKeys("20201@itla.edu.do");
            Driver.FindElement(By.Name("Mobile")).SendKeys("11421");
            ShopPrintScreen();

            Driver.FindElement(By.CssSelector(".btn-primary")).Click();

            var res = Driver.FindElement(By.Id("alert")).Text;
            Assert.That(res, Is.EqualTo("Contacto actualizado exitosamente"));

            ShopPrintScreen();
            test.Log(Status.Pass, "Se completó el test de editar contacto.");

            Driver.Close();
        }

        [Test]
        public void TestSearchContactSuccess()
        {
            test.Log(Status.Info, "Inicio el test de buscar contacto.");

            Driver.Navigate().GoToUrl(AppUrl + "/home");
            Driver.Manage().Window.Size = new System.Drawing.Size(1053, 848);
            Driver.FindElement(By.Id("inputSearch")).Click();
            Driver.FindElement(By.Id("inputSearch")).SendKeys("Yohan");
            ShopPrintScreen();
            var res = Driver.FindElement(By.Id("contactCount")).Text;
            Assert.That(res, Is.Not.EqualTo("0"));

            ShopPrintScreen();
            test.Log(Status.Pass, "Se completó el test de buscar contacto.");

            Driver.Close();
        }

        [Test]
        public void TestDeleteContactSuccess()
        {
            test.Log(Status.Info, "Inicio el test de eliminar contacto.");

            Driver.Navigate().GoToUrl(AppUrl + "/home");
            Driver.Manage().Window.Size = new System.Drawing.Size(1552, 880);
            Driver.FindElement(By.LinkText("Delete")).Click();

            var res = Driver.FindElement(By.Id("alert")).Text;
            Assert.That(res, Is.EqualTo("Contacto eliminado exitosamente"));

            ShopPrintScreen();
            test.Log(Status.Pass, "Se completó el test de eliminar contacto.");
            Driver.Close();
        }

        [TearDown]
        public void AfterTest()
        {
            _extent.Flush();
        }

        [OneTimeTearDown]
        public void AfterTests()
        {
            _extent.Flush();
        }

        private void ShopPrintScreen()
        {
            // Tomar una captura de pantalla y adjuntarla al reporte
            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string imgPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\" + @"Reportes/" + test.Model.Name + ".png"));
            screenshot.SaveAsFile(imgPath, ScreenshotImageFormat.Png);
            test.AddScreenCaptureFromPath(imgPath);
        }

    }
}