using System;
using System.IO;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class UploadFilesForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Upload your user list')]");
		private readonly Button downloadSampleButton ;
		private readonly Button selectFileInputButton ;
		private readonly Label uploadedFileLabel ;
        RunConfigurator configurator;

        public UploadFilesForm(Guid driverId,RunConfigurator configurator) : base(TitleLocator, "Upload your files form",driverId)
		{
            this.configurator = configurator;
            this.driverId = driverId;
            downloadSampleButton =new Button(By.XPath("//a[contains(@href, 'DownloadUserMatchTemplate')]"), "Download sample button",driverId);
            selectFileInputButton = new Button(By.XPath("//input[@type='file']"), "Select file input",driverId);
            uploadedFileLabel = new Label(By.XPath("//h4[contains(@data-bind, 'file().name')]"),"Uploaded file label",driverId);
        }
		public UploadFilesForm(By _TitleLocator, string _name,Guid driverId) : base(_TitleLocator, _name,driverId)
		{
            this.driverId = driverId;
            downloadSampleButton = new Button(By.XPath("//a[contains(@href, 'DownloadUserMatchTemplate')]"), "Download sample button", driverId);
            selectFileInputButton = new Button(By.XPath("//input[@type='file']"), "Select file input", driverId);
            uploadedFileLabel = new Label(By.XPath("//h4[contains(@data-bind, 'file().name')]"), "Uploaded file label", driverId);
        }
		public void SelectFile(string fileName)
		{
			Log.Info("Selecting file");
			selectFileInputButton.GetElement().SendKeys(Path.GetFullPath(configurator.ResourcesPath + fileName));
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