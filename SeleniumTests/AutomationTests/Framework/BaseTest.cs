using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework.Steps;
using System.Management.Automation;
using System.Threading;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Product.Framework
{
	/// <summary>
	///     Class BaseTest.
	/// </summary>
	/// <seealso cref="BaseEntity" />
	[TestClass]
	public class BaseTest : BaseEntity
	{
		protected virtual string downloadPath {get { return "../../download/"; }}
		protected static TestContext _testContext;
		private string _baseUrl;
		public UserSteps User;

		private void GetResolution()
		{
			Log.Info("Current resolution is: " + Screen.PrimaryScreen.Bounds.Width + " x " + Screen.PrimaryScreen.Bounds.Height);
		}

		/// <summary>
		///     Sets up.
		/// </summary>
		[TestInitialize]
		public virtual void SetUp()
		{
			RunOnce();
			RunConfigurator.RunPath = "resources/run.xml";
			RunConfigurator.DownloadPath = downloadPath;
			RunConfigurator.ResourcesPath = "resources/";
			if (_testContext == null)
			{
				RunConfigurator.RunPath = "resources/probeRun.xml";
				Directory.CreateDirectory(downloadPath);
				RunConfigurator.DownloadPath = downloadPath;
				RunConfigurator.ResourcesPath = "resources/";
				if (Screen.PrimaryScreen.Bounds.Width != 1920)
				{
					ChangeResolutionToFHD();
				}
			}
			RunConfigurator.ClearDownloads();
			User = new UserSteps();
			_baseUrl = RunConfigurator.GetValue("baseurl");
			GetResolution();
			Browser.GetInstance(RunConfigurator.DownloadPath);
			Browser.GetDriver().Manage().Window.Maximize();
			Browser.GetDriver().Navigate().GoToUrl(_baseUrl);
		}

		
		/// <summary>
		///     Tears down.
		/// </summary>
		[TestCleanup]
		public void TearDown()
		{
            Browser.GetDriver()?.Close();
			Browser.GetDriver()?.Quit();
		}

		/// <summary>
		///     Runs the once.
		/// </summary>
		private void ChangeResolutionToFHD()
		{
			using (PowerShell PowerShellInstance = PowerShell.Create())
			{
				PowerShellInstance.AddScript("Set-DisplayResolution -Width 1920 -Height 1080 -Force");
				PowerShellInstance.Invoke();
			}
		}

		public void RunOnce()
		{
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
		}
	}
}