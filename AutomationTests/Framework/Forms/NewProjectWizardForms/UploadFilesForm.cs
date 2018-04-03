using System.IO;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class UploadFilesForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Upload your user list')]");
		private readonly Button downloadSampleButton =
			new Button(By.XPath("//a[contains(@href, 'DownloadUserMatchTemplate')]"), "Download sample button");
		private readonly Button selectFileInputButton = new Button(By.XPath("//input[@type='file']"), "Select file input");
		private readonly Label uploadedFileLabel = new Label(By.XPath("//h4[contains(@data-bind, 'file().name')]"),
			"Uploaded file label");

		public UploadFilesForm() : base(TitleLocator, "Upload your files form")
		{
		}
		public UploadFilesForm(By _TitleLocator, string _name) : base(_TitleLocator, _name)
		{
		}
		public void SelectFile(string fileName)
		{
			Log.Info("Selecting file");
			selectFileInputButton.GetElement().SendKeys(Path.GetFullPath(RunConfigurator.ResourcesPath + fileName));
		}

		public void DownloadSample()
		{
			Log.Info("Downloading sample");
			downloadSampleButton.Click();
		}

		public void WaitUntillFileUploaded()
		{
			Log.Info("Waiting untill file uploaded");
			uploadedFileLabel.WaitForElementPresent();
		}
	}
}