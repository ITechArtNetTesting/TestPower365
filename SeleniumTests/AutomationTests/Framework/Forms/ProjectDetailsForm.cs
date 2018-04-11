using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Product.Framework.Elements;
using Product.Framework.Steps;

namespace Product.Framework.Forms
{
	public class ProjectDetailsForm : BaseForm
	{
		private static readonly By TitleLocator =
			By.XPath("//*[@id='breadcrumbsContainer']//strong[contains(text(), 'Setup')]");

		private readonly TextBox activeDirectoryTextBox =
			new TextBox(By.XPath("//div[@aria-expanded='true']//input[contains(@class, 'k-input')]"), "Active directory textvox");

		private readonly Button addGroupButton = new Button(By.Id("addGroupsBtn"), "Add group button");

		private readonly Button addTenantButton =
			new Button(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]"), "Add tenant button");

		private readonly Button addTenantsAccordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Add')][contains(text(), 'Tenants')]"),
				"Add your tenants accordian button");

		private readonly Button addTenantSmallButton =
			new Button(By.XPath("//div[@aria-expanded='true']//button[text()[contains(.,'Add Tenant')]]"),
				"Small add tenant button");

		private readonly Label addTenantsStepSuccessIconLabel =
			new Label(
				By.XPath("//span[contains(text(), 'Add')][contains(text(), 'Tenants')]/..//i[contains(@class, 'icon-success')]"),
				"Second step success icon");

		private readonly TextBox chooseFilesInput =
			new TextBox(By.XPath("//div[contains(@class, 'modal fade in')]//input[@type='file']"), "Choose files input");

		private readonly Button configureDomainsAccordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Configure Your Domains')]"),
				"Configure your domains accordian button");

		private readonly Label configureDomainStepSuccessIconLabel =
			new Label(By.XPath("//span[contains(text(), 'Configure Your Domains')]/..//i[contains(@class, 'icon-success')]"),
				"Configure domains step success icon label");

		private readonly Button configureTenantsAccordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Configure Your Tenants')]"), "Configure your tenants accordian");

		private readonly Label configureTenantsStepSuccessIconLabel =
			new Label(By.XPath("//span[contains(text(), 'Configure Your Tenants')]/..//i[contains(@class, 'icon-success')]"),
				"Configure your tenants success icon label");

		private readonly Button defineCriteriaAccordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Define Your Matching Criteria')]"), "Define your matching criteria");

		private readonly Label defineCriteriaStepSuccessIconLabel =
			new Label(
				By.XPath("//span[contains(text(), 'Define Your Matching Criteria')]/..//i[contains(@class, 'icon-success')]"),
				"Define matching criteria success icon label");

		private readonly TextBox descriptionTextBox =
			new TextBox(By.XPath("//div[@aria-expanded='true']//textarea[contains(@data-bind, 'projectDescription')]"),
				"Description textbox");

		private readonly Button disabledSubmitButton =
			new Button(
				By.XPath("//div[@aria-expanded='true']//button[@class='btn btn-large raised btn-success'][@disabled='']"),
				"Disabled submit button");

		private readonly Button enabledNextButton =
			new Button(By.XPath("//div[@aria-expanded='true']//button[not(@disabled)][contains(@class, 'raised')]"),
				"Enabled Next button");

		private readonly Button enterProjectDescriptionAccordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Enter a Project Description')]"),
				"Enter project description accordian button");

		private readonly Button enterProjectNameAccordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Enter a Project Name')]"), "Enter project name accordian button");

		private readonly Label enterProjectNameStepSuccessIconLabel =
			new Label(By.XPath("//span[contains(text(), 'Enter a Project Name')]/..//i[contains(@class, 'icon-success')]"),
				"First step success icon");

		private readonly Label expandedAreaLabel = new Label(By.XPath("//div[@aria-expanded='true']//h3"),
			"Expanded area label");

		private readonly Button expandedFirstTenantButton =
			new Button(
				By.XPath(
					"//div[@aria-expanded='true']//td//i[contains(@class, 'fa-question-circle')]/../following::td[1]//button[contains(@aria-expanded, 'true')]"),
				"Expanded first tenant dropdown");

		private readonly Button expandedSecondTenantButton =
			new Button(
				By.XPath(
					"//div[@aria-expanded='true']//td//i[contains(@class, 'fa-long-arrow-right')]/../following::td[1]//button[contains(@aria-expanded, 'true')]"),
				"Extended second tenant button");

		private readonly Button exportedUsers =
			new Button(By.XPath("//div[@aria-expanded='true']//a[contains(@href, 'ExportUserMigrationsForDomain')]"),
				"Exported users button");

		private readonly Label failedStatusIconLabel =
			new Label(By.XPath("//i[contains(@class, 'icon-danger')]/..//span[contains(text(), 'FINISH')]"), "Failed status icon");

		private readonly Label failedToUpladLabel =
			new Label(By.XPath("//div[@aria-expanded='true']//*[contains(text(), 'failed to upload')]"), "Failed to upload label");

		private readonly Button finishAccordionButton = new Button(By.XPath("//h5//span[contains(text(), 'FINISH')]"),
			"Finish accordian button");

		private readonly Label finishStepSuccessIconLabel =
			new Label(By.XPath("//span[contains(text(), 'FINISH')]/..//i[contains(@class, 'icon-success')]"),
				"Fifth step success icon");

		private readonly Button firstTenantButton =
			new Button(
				By.XPath("//div[@aria-expanded='true']//td//i[contains(@class, 'fa-question-circle')]/../following::td[1]//div"),
				"First tenant button");

		private readonly Label howDoYouLikeToIdentifyLabel =
			new Label(By.XPath("//div[@aria-expanded='true']//h3[contains(text(), 'How would you like to identify your')]"),
				"How would you like to identify your... label");

		private readonly Label howDoYouLikeToMatchLabel =
			new Label(By.XPath("//div[@aria-expanded='true']//h3[contains(text(), 'How would you like to match users?')]"),
				"How would you like to match users? label");

		private readonly Button identifyTenantsAccordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Identify Your')]"), "Identify tenants accordion button");

		private readonly Label identifyTenantsStepSuccessIconLabel =
			new Label(By.XPath("//span[contains(text(), 'Identify Your')]/..//i[contains(@class, 'icon-success')]"),
				"Identify tenants step success icon label");

		private readonly Button identifyUsersAccordianButton =
			new Button(By.XPath("//span[contains(text(), 'Identify Your Users')]"), "Identify users accordian button");

		private readonly Label identifyUsersStepSuccessIconLabel =
			new Label(By.XPath("//span[contains(text(), 'Identify Your Users')]/..//i[contains(@class, 'icon-success')]"),
				"Third step success icon");

		private readonly Button okButton =
			new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[text()='OK']"), "OK button");

		private readonly TextBox projectNameTextBox =
			new TextBox(By.XPath("//div[@aria-expanded='true']//input[contains(@data-bind, 'projectName')]"),
				"Project name textbox");

		private readonly Button replaceUsersButton =
			new Button(By.XPath("//div[contains(@class, 'modal fade in')]//label[@for='replace4']"), "Replace users radiobutton");

		private readonly Button sampleFileButton =
			new Button(By.XPath("//div[contains(@class, 'modal fade in')]//a[contains(@href, 'Download')]"), "Sample file button");

		private readonly Button secondTenantButton =
			new Button(
				By.XPath("//div[@aria-expanded='true']//td//i[contains(@class, 'fa-long-arrow-right')]/../following::td[1]//button"),
				"Second tenant button");

		private readonly Label selectUsersByGroupLabel =
			new Label(By.XPath("//div[@aria-expanded='true']//label[contains(text(), 'Select users by group membership')]"),
				"Select users by group membership label");

		private readonly Button submitButton =
			new Button(
				By.XPath("//div[@aria-expanded='true']//button[@class='btn btn-large raised btn-success'][not(@disabled='')]"),
				"Submit button");

		private readonly Label successFilterStatusLabel =
			new Label(
				By.XPath("//div[@aria-expanded='true']//i[contains(@class, 'icon-success')][contains(@data-bind, 'filtersStatus')]"),
				"Success groups filter status label");

		private readonly Label successStatusIconLabel =
			new Label(By.XPath("//div[contains(@class, 'modal fade in')]//i[contains(@class, 'icon-success')]"), "Status icon");

		private readonly Label tenantLabel =
			new Label(By.XPath("//div[@aria-expanded='true']//h4[contains(@data-bind, 'tenantName')]"), "Tenant label");

		private readonly Button uploadFilesButton =
			new Button(By.XPath("//div[@aria-expanded='true']//a[contains(@data-bind, 'openUploadDialog')]"),
				"Upload file button");

		private readonly Button verifyUsersAccrordianButton =
			new Button(By.XPath("//h5//span[contains(text(), 'Verify Uploaded Users')]"), "Verify users accordian button");

		private readonly Label verifyUsersStepSuccessIconLabel =
			new Label(By.XPath("//span[contains(text(), 'Verify Uploaded Users')]/..//i[contains(@class, 'icon-success')]"),
				"Fourth step success icon");

		private readonly Label wereUploadedLabel =
			new Label(By.XPath("//div[@aria-expanded='true']//*[contains(text(), 'were uploaded')]"), "Were uploaded label");

		private readonly Label whichDomainLabel =
			new Label(
				By.XPath("//div[@aria-expanded='true']//h3[contains(text(), 'Which domains would you like to migrate?')]"),
				"Which domains would you like to migrate? label");

		public ProjectDetailsForm() : base(TitleLocator, "Project details form")
		{
		}

		public void SetProjectName(string name)
		{
			Log.Info($"Setting {name} project name");
			projectNameTextBox.ClearSetText(name);
			Store.ProjectName = name;
		}

		public void ActivateNextButton()
		{
			Log.Info("Activating next button");
			expandedAreaLabel.Click();
		}

		public void AssertAddTenantButtonIsPresent()
		{
			Log.Info("Asserting small add tenant button appear");
			addTenantSmallButton.WaitForElementPresent();
		}

		public void RemoveTenant(string tenant)
		{
			Log.Info("Removing tenant: " + tenant);
			var tenantRemoveButton =
				new Button(By.XPath($"//h4[contains(text(), '{tenant}')]/../..//button[contains(@data-bind, 'removeTenant')]"),
					tenant + " tenant remove button");
			tenantRemoveButton.Click();
		}

		public void SelectVerifyUsersAccordian()
		{
			Log.Info("Selecting verify users accordian");
			verifyUsersAccrordianButton.Click();
		}

		public void ClickSmallAddTenantButton()
		{
			Log.Info("Clicking small add tenant button");
			addTenantSmallButton.Click();
			//NOTE: Temporary sleep. Can be removed
			Thread.Sleep(3000);
			addTenantButton.WaitForElementPresent();
		}

		public void WaitForGroupFilterSuccessStatus()
		{
			Log.Info("Waiting till group filter success status appear");
			successFilterStatusLabel.WaitForElementPresent();
		}

		public void SetActiveDirectory(string activeDirectory)
		{
			Log.Info("Setting active directory: " + activeDirectory);
			activeDirectoryTextBox.ClearSetText(activeDirectory);
		}

		public void AddGroup()
		{
			Log.Info("Adding group");
			addGroupButton.Click();
		}

		public void SelectActiveDirectoryTooltip(string group)
		{
			Log.Info("Selecting active directory tooltip: " + group);
			var tooltipLabel = new Label(By.XPath($"//ul[contains(@aria-hidden, 'false')]//li[contains(text(), '{group}')]"),
				group + " tooltip");
			tooltipLabel.Click();
		}

		public void SelectUsersByGroupRadio()
		{
			Log.Info("Setting select users by group radio button");
			selectUsersByGroupLabel.Click();
			try
			{
				activeDirectoryTextBox.WaitForElementPresent(7000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				selectUsersByGroupLabel.Click();
			}
		}

		public void ScrollToElement(IWebElement element)
		{
			((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView();", element);
		}

		public void SelectFinishStep()
		{
			Log.Info("Selecting finish step");
			Thread.Sleep(1000);
			ScrollToElement(finishAccordionButton.GetElement());
			finishAccordionButton.Click();
		}

		public void SelectAddTenantsAccordian()
		{
			Log.Info("Selecting add tenants accordian");
			Thread.Sleep(1000);
			addTenantsAccordianButton.Click();
		}

		public void SelectDefineCriteriaAccordian()
		{
			Log.Info("Selecting define matching criteria accordian");
			Thread.Sleep(1000);
			defineCriteriaAccordianButton.Click();
		}

		public void SelectConfigureDomainsAccordian()
		{
			Log.Info("Selecting configure domains accordian");
			Thread.Sleep(1000);
			configureDomainsAccordianButton.Click();
		}

		public void SelectConfigureTenantsAccordian()
		{
			Log.Info("Selecting configure your tenants accordian");
			Thread.Sleep(1000);
			configureTenantsAccordianButton.Click();
		}

		public void SelectProjectNameStep()
		{
			Log.Info("Selecting Enter Project name accordian");
			Thread.Sleep(1000);
			enterProjectNameAccordianButton.Click();
		}

		public void SelectProjectDescriptionStep()
		{
			Log.Info("Selecting Enter Project Description accordian");
			Thread.Sleep(1000);
			enterProjectDescriptionAccordianButton.Click();
		}

		public void SelectIdentifyYourStep()
		{
			Log.Info("Selecting Identify Your accordian");
			Thread.Sleep(1000);
			identifyTenantsAccordianButton.Click();
		}

		public void AssertWizardIsFailed()
		{
			Log.Info("Asserting wizard is failed");
			disabledSubmitButton.WaitForElementPresent();
			failedStatusIconLabel.WaitForElementPresent();
		}

		public void SetProjectDescription(string description)
		{
			Log.Info($"Setting {description} project description");
			descriptionTextBox.SetText(description);
		}

		public void GoNext()
		{
			Log.Info("Going next");
			enabledNextButton.Click();
		}

		public void OpenOffice365LoginFormPopup()
		{
			Log.Info("Switching to new window");
			Store.MainHandle = Browser.GetDriver().CurrentWindowHandle;
			var finder = new PopupWindowFinder(Browser.GetDriver());
			addTenantButton.WaitForElementPresent();
			addTenantButton.WaitForElementIsClickable();
			try
			{
				var popupWindowHandle =
					finder.Click(
						Browser.GetDriver()
							.FindElement(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]")));
				Browser.GetDriver().SwitchTo().Window(popupWindowHandle);
			}
			catch (Exception)
			{
				Log.Info("Add tenant button is not ready");
				var popupWindowHandle =
					finder.Click(
						Browser.GetDriver()
							.FindElement(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]")));
				Browser.GetDriver().SwitchTo().Window(popupWindowHandle);
			}
		}

		public void WaitForTenantAdded()
		{
			Log.Info("Waiting till tenant added");
			tenantLabel.WaitForElementPresent();
		}

		public void WaitForBothTenantsAdded()
		{
			Log.Info("Waiting till the second tenant added");
			tenantLabel.WaitForElementPresent(2);
		}

		public void OpenUploadDialog()
		{
			Log.Info("Opening upload files dialog");
			uploadFilesButton.Click();
			try
			{
				chooseFilesInput.WaitForElementPresent(10000);
			}
			catch (Exception)
			{
				Log.Info("Modal dialog is not ready");
				uploadFilesButton.Click();
			}
		}

		public void ChooseFile()
		{
			Log.Info("Selecting file");
			chooseFilesInput.GetElement().SendKeys(Path.GetFullPath("resources/MailOnlyCSVC7toC9SML.csv"));
		}

		public void ChooseFile(string fileName)
		{
			Log.Info("Selecting file");
			try
			{
				chooseFilesInput.WaitForElementPresent(10000);
			}
			catch (Exception)
			{
				Log.Info("Modal dialog is not ready");
				uploadFilesButton.Click();
			}
			chooseFilesInput.GetElement().SendKeys(Path.GetFullPath(RunConfigurator.ResourcesPath + fileName));
		}

		public void WaitTillFileIsUploaded()
		{
			Log.Info("Waiting till file is uploaded");
			successStatusIconLabel.WaitForElementPresent();
		}

		public void ConfirmUploading()
		{
			Log.Info("Confirming upload");
			okButton.Click();
		}

		public void WaitTillUserUploaded()
		{
			Log.Info("Waiting till users are uploaded");
			wereUploadedLabel.WaitForElementPresent();
		}

		public void WaitTillUsersFailedToUpload()
		{
			Log.Info("Waiting till users are failed to upload");
			failedToUpladLabel.WaitForElementPresent();
		}

		public void WaitTillUsersAmountAppear()
		{
			Log.Info("Waitong till user amount appears");
			exportedUsers.WaitForElementPresent();
		}

		public void Submit()
		{
			Log.Info("Submitting");
			submitButton.Click();
		}

		public new void ScrollToTheBottom()
		{
			Log.Info("Scrolling to the bottom of the page");
			((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
		}

		public void OpenFirstTenantDropDown()
		{
			Log.Info("Opening first tenant dropdown");
			firstTenantButton.Click();
			try
			{
				expandedFirstTenantButton.WaitForElementPresent(7000);
			}
			catch (Exception)
			{
				Log.Info("Dropdown is not expanded");
				firstTenantButton.Click();
			}
		}

		public void AddTwoTenants()
		{
			var tenantSteps = new AddTenantsSteps();
			tenantSteps.PerformTwoTenantsAdding();
		}

		public void AddTenant(string tenant, string password)
		{
			var tenantSteps = new AddTenantsSteps();
			tenantSteps.PerformOneTenantAdding(tenant, password);
		}

		public void AddTwoTenants(string sourceTenant, string sourcePassword, string targetTenant, string targetPassword)
		{
			var tenantSteps = new AddTenantsSteps();
			tenantSteps.PerformTwoTenantsAdding(sourceTenant, sourcePassword, targetTenant, targetPassword);
		}

		public void OpenSecondTenantDropDown()
		{
			Log.Info("Opening second tenant dropdown");
			secondTenantButton.Click();
			try
			{
				expandedSecondTenantButton.WaitForElementPresent(7000);
			}
			catch (Exception)
			{
				Log.Info("Dropdown is not expanded");
				secondTenantButton.Click();
			}
		}

		public void SelectDropDownOption(string name)
		{
			Log.Info($"Selecting {name} option");
			var optionButton = new Button(By.XPath($"//button[@aria-expanded='true']/..//a[contains(text(), '{name}')]"),
				$"{name} option");
			optionButton.Click();
		}

		public void OpenFirstDomainDropdown()
		{
			Log.Info("Opening first domain dropdown");
			firstTenantButton.Click();
		}

		public void OpenSecondDomainDropdown()
		{
			Log.Info("Opening second domain dropdown");
			secondTenantButton.Click();
		}

		public void WaitTillDefineMatchingBlockIsExpanded()
		{
			Log.Info("Waiting till 'Which domains would you like to migrate?' block is expanded");
			howDoYouLikeToMatchLabel.WaitForElementPresent();
		}

		public void WaitTillIdentifyBlockIsExpanded()
		{
			Log.Info("Waiting till How would you like to identify block is expanded");
			howDoYouLikeToIdentifyLabel.WaitForElementPresent();
		}

		public void OpenIdentifyUsersAccordian()
		{
			Log.Info("Opening Identify users accordian");
			Thread.Sleep(1000);
			identifyUsersAccordianButton.Click();
		}

		public void SelectReplaceUsers()
		{
			Log.Info("Selecting replace users option");
			replaceUsersButton.Click();
		}

		public void DownloadSample()
		{
			Log.Info("Downloading sample file");
			sampleFileButton.Click();
		}

		public void VerifyEnterProjectNameStepIsSuccessful()
		{
			Log.Info("Verifying enter project name step is successful");
			enterProjectNameStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyAddTenantsStepIsSuccessful()
		{
			Log.Info("Verifying add tenants step is successful");
			addTenantsStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyIdentifyUsersStepIsSuccessful()
		{
			Log.Info("Verifying identify users step is successful");
			identifyUsersStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyUsersStepIsSuccessful()
		{
			Log.Info("Verifying verify users step is successful");
			verifyUsersStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyConfigureTenantsStepIsSuccessful()
		{
			Log.Info("Verifying configure tenants step is successful");
			configureTenantsStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyConfigureDomainsStepIsSuccessful()
		{
			Log.Info("Verifying configure domains step is successful");
			configureDomainStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyDefineCriteriaStepIsSuccessful()
		{
			Log.Info("Verifying define matching criteria step is successful");
			defineCriteriaStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyIdentifyTenantsStepIsSuccessful()
		{
			Log.Info("Verifying identify tenants step is successful");
			identifyTenantsStepSuccessIconLabel.WaitForElementPresent();
		}

		public void VerifyFinishStepIsSuccessful()
		{
			Log.Info("Verifying finish step is successful");
			finishStepSuccessIconLabel.WaitForElementPresent();
		}
	}
}