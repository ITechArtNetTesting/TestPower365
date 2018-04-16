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

		private readonly TextBox activeDirectoryTextBox ;

		private readonly Button addGroupButton ;

		private readonly Button addTenantButton ;

		private readonly Button addTenantsAccordianButton ;

		private readonly Button addTenantSmallButton ;

		private readonly Label addTenantsStepSuccessIconLabel ;

		private readonly TextBox chooseFilesInput ;

		private readonly Button configureDomainsAccordianButton ;

		private readonly Label configureDomainStepSuccessIconLabel ;

		private readonly Button configureTenantsAccordianButton ;

		private readonly Label configureTenantsStepSuccessIconLabel ;

		private readonly Button defineCriteriaAccordianButton ;

		private readonly Label defineCriteriaStepSuccessIconLabel ;

		private readonly TextBox descriptionTextBox ;

		private readonly Button disabledSubmitButton ;

		private readonly Button enabledNextButton ;

		private readonly Button enterProjectDescriptionAccordianButton ;

		private readonly Button enterProjectNameAccordianButton ;

		private readonly Label enterProjectNameStepSuccessIconLabel ;

		private readonly Label expandedAreaLabel ;

		private readonly Button expandedFirstTenantButton ;

		private readonly Button expandedSecondTenantButton ;

		private readonly Button exportedUsers ;

		private readonly Label failedStatusIconLabel ;

		private readonly Label failedToUpladLabel ;

		private readonly Button finishAccordionButton ;

		private readonly Label finishStepSuccessIconLabel ;

		private readonly Button firstTenantButton ;

		private readonly Label howDoYouLikeToIdentifyLabel ;

		private readonly Label howDoYouLikeToMatchLabel ;

		private readonly Button identifyTenantsAccordianButton ;

		private readonly Label identifyTenantsStepSuccessIconLabel ;

		private readonly Button identifyUsersAccordianButton ;

		private readonly Label identifyUsersStepSuccessIconLabel ;

		private readonly Button okButton ;

		private readonly TextBox projectNameTextBox ;

		private readonly Button replaceUsersButton ;

		private readonly Button sampleFileButton ;

		private readonly Button secondTenantButton ;

		private readonly Label selectUsersByGroupLabel ;

		private readonly Button submitButton ;

		private readonly Label successFilterStatusLabel ;

		private readonly Label successStatusIconLabel ;

		private readonly Label tenantLabel ;

		private readonly Button uploadFilesButton ;

		private readonly Button verifyUsersAccrordianButton ;

		private readonly Label verifyUsersStepSuccessIconLabel ;

		private readonly Label wereUploadedLabel ;

		private readonly Label whichDomainLabel ;
        

        public ProjectDetailsForm(Guid driverId) : base(TitleLocator, "Project details form",driverId)
		{
            this.driverId = driverId;
            activeDirectoryTextBox =new TextBox(By.XPath("//div[@aria-expanded='true']//input[contains(@class, 'k-input')]"), "Active directory textvox",driverId);
            addGroupButton = new Button(By.Id("addGroupsBtn"), "Add group button",driverId);
            addTenantButton =new Button(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]"), "Add tenant button",driverId);
            addTenantsAccordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Add')][contains(text(), 'Tenants')]"),"Add your tenants accordian button",driverId);
            addTenantSmallButton =new Button(By.XPath("//div[@aria-expanded='true']//button[text()[contains(.,'Add Tenant')]]"),"Small add tenant button",driverId);
            addTenantsStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Add')][contains(text(), 'Tenants')]/..//i[contains(@class, 'icon-success')]"),"Second step success icon",driverId);
            chooseFilesInput =new TextBox(By.XPath("//div[contains(@class, 'modal fade in')]//input[@type='file']"), "Choose files input",driverId);
            configureDomainsAccordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Configure Your Domains')]"),"Configure your domains accordian button",driverId);
            configureDomainStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Configure Your Domains')]/..//i[contains(@class, 'icon-success')]"),"Configure domains step success icon label",driverId);
            configureTenantsAccordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Configure Your Tenants')]"), "Configure your tenants accordian",driverId);
            configureTenantsStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Configure Your Tenants')]/..//i[contains(@class, 'icon-success')]"),"Configure your tenants success icon label",driverId);
            defineCriteriaAccordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Define Your Matching Criteria')]"), "Define your matching criteria",driverId);
            defineCriteriaStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Define Your Matching Criteria')]/..//i[contains(@class, 'icon-success')]"),"Define matching criteria success icon label",driverId);
            descriptionTextBox =new TextBox(By.XPath("//div[@aria-expanded='true']//textarea[contains(@data-bind, 'projectDescription')]"),"Description textbox",driverId);
            disabledSubmitButton =new Button(By.XPath("//div[@aria-expanded='true']//button[@class='btn btn-large raised btn-success'][@disabled='']"),"Disabled submit button",driverId);
            enabledNextButton =new Button(By.XPath("//div[@aria-expanded='true']//button[not(@disabled)][contains(@class, 'raised')]"),"Enabled Next button",driverId);
            enterProjectDescriptionAccordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Enter a Project Description')]"),"Enter project description accordian button",driverId);
            enterProjectNameAccordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Enter a Project Name')]"), "Enter project name accordian button",driverId);
            enterProjectNameStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Enter a Project Name')]/..//i[contains(@class, 'icon-success')]"),"First step success icon",driverId);
            expandedFirstTenantButton =new Button(By.XPath("//div[@aria-expanded='true']//td//i[contains(@class, 'fa-question-circle')]/../following::td[1]//button[contains(@aria-expanded, 'true')]"),"Expanded first tenant dropdown",driverId);
            expandedSecondTenantButton =new Button(By.XPath("//div[@aria-expanded='true']//td//i[contains(@class, 'fa-long-arrow-right')]/../following::td[1]//button[contains(@aria-expanded, 'true')]"),"Extended second tenant button",driverId);
            expandedAreaLabel = new Label(By.XPath("//div[@aria-expanded='true']//h3"),"Expanded area label",driverId);
            exportedUsers =new Button(By.XPath("//div[@aria-expanded='true']//a[contains(@href, 'ExportUserMigrationsForDomain')]"),"Exported users button",driverId);
            failedStatusIconLabel =new Label(By.XPath("//i[contains(@class, 'icon-danger')]/..//span[contains(text(), 'FINISH')]"), "Failed status icon",driverId);
            failedToUpladLabel =new Label(By.XPath("//div[@aria-expanded='true']//*[contains(text(), 'failed to upload')]"), "Failed to upload label",driverId);
            finishAccordionButton = new Button(By.XPath("//h5//span[contains(text(), 'FINISH')]"),"Finish accordian button",driverId);
            finishStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'FINISH')]/..//i[contains(@class, 'icon-success')]"),"Fifth step success icon",driverId);
            firstTenantButton =new Button(By.XPath("//div[@aria-expanded='true']//td//i[contains(@class, 'fa-question-circle')]/../following::td[1]//div"),"First tenant button",driverId);
            howDoYouLikeToIdentifyLabel =new Label(By.XPath("//div[@aria-expanded='true']//h3[contains(text(), 'How would you like to identify your')]"),"How would you like to identify your... label",driverId);
            howDoYouLikeToMatchLabel =new Label(By.XPath("//div[@aria-expanded='true']//h3[contains(text(), 'How would you like to match users?')]"),"How would you like to match users? label",driverId);
            identifyTenantsAccordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Identify Your')]"), "Identify tenants accordion button",driverId);
            identifyTenantsStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Identify Your')]/..//i[contains(@class, 'icon-success')]"),"Identify tenants step success icon label",driverId);
            identifyUsersAccordianButton =new Button(By.XPath("//span[contains(text(), 'Identify Your Users')]"), "Identify users accordian button",driverId);
            identifyUsersStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Identify Your Users')]/..//i[contains(@class, 'icon-success')]"),"Third step success icon",driverId);
            okButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//button[text()='OK']"), "OK button",driverId);
            projectNameTextBox =new TextBox(By.XPath("//div[@aria-expanded='true']//input[contains(@data-bind, 'projectName')]"),"Project name textbox",driverId);
            replaceUsersButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//label[@for='replace4']"), "Replace users radiobutton",driverId);
            sampleFileButton =new Button(By.XPath("//div[contains(@class, 'modal fade in')]//a[contains(@href, 'Download')]"), "Sample file button",driverId);
            secondTenantButton =new Button(By.XPath("//div[@aria-expanded='true']//td//i[contains(@class, 'fa-long-arrow-right')]/../following::td[1]//button"),"Second tenant button",driverId);
            selectUsersByGroupLabel =new Label(By.XPath("//div[@aria-expanded='true']//label[contains(text(), 'Select users by group membership')]"),"Select users by group membership label",driverId);
            submitButton =new Button(By.XPath("//div[@aria-expanded='true']//button[@class='btn btn-large raised btn-success'][not(@disabled='')]"),"Submit button",driverId);
            successFilterStatusLabel =new Label(By.XPath("//div[@aria-expanded='true']//i[contains(@class, 'icon-success')][contains(@data-bind, 'filtersStatus')]"),"Success groups filter status label",driverId);
            successStatusIconLabel =new Label(By.XPath("//div[contains(@class, 'modal fade in')]//i[contains(@class, 'icon-success')]"), "Status icon",driverId);
            tenantLabel =new Label(By.XPath("//div[@aria-expanded='true']//h4[contains(@data-bind, 'tenantName')]"), "Tenant label",driverId);
            uploadFilesButton =new Button(By.XPath("//div[@aria-expanded='true']//a[contains(@data-bind, 'openUploadDialog')]"),"Upload file button",driverId);
            verifyUsersAccrordianButton =new Button(By.XPath("//h5//span[contains(text(), 'Verify Uploaded Users')]"), "Verify users accordian button",driverId);
            verifyUsersStepSuccessIconLabel =new Label(By.XPath("//span[contains(text(), 'Verify Uploaded Users')]/..//i[contains(@class, 'icon-success')]"),"Fourth step success icon",driverId);
            wereUploadedLabel =new Label(By.XPath("//div[@aria-expanded='true']//*[contains(text(), 'were uploaded')]"), "Were uploaded label",driverId);
            whichDomainLabel =new Label(By.XPath("//div[@aria-expanded='true']//h3[contains(text(), 'Which domains would you like to migrate?')]"),"Which domains would you like to migrate? label",driverId);
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
					tenant + " tenant remove button",driverId);
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
				group + " tooltip",driverId);
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
            //((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("arguments[0].scrollIntoView();", element);
            ((IJavaScriptExecutor)Driver.GetDriver(driverId)).ExecuteScript("arguments[0].scrollIntoView();", element);
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
            //Store.MainHandle = Browser.GetDriver().CurrentWindowHandle;
            Store.MainHandle = Driver.GetDriver(driverId).CurrentWindowHandle;
            //var finder = new PopupWindowFinder(Browser.GetDriver());
            var finder = new PopupWindowFinder(Driver.GetDriver(driverId));
            addTenantButton.WaitForElementPresent();
			addTenantButton.WaitForElementIsClickable();
			try
			{
                //var popupWindowHandle =
                //	finder.Click(
                //		Browser.GetDriver()
                //			.FindElement(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]")));
                var popupWindowHandle =
                    finder.Click(
                        Driver.GetDriver(driverId)
                            .FindElement(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]")));
                //Browser.GetDriver().SwitchTo().Window(popupWindowHandle);
                Driver.GetDriver(driverId).SwitchTo().Window(popupWindowHandle);
            }
			catch (Exception)
			{
				Log.Info("Add tenant button is not ready");
                //var popupWindowHandle =
                //	finder.Click(
                //		Browser.GetDriver()
                //			.FindElement(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]")));
                var popupWindowHandle =
                    finder.Click(
                        Driver.GetDriver(driverId)
                            .FindElement(By.XPath("//div[@aria-expanded='true']//button[contains(@data-bind, 'addTenant')]")));
                //Browser.GetDriver().SwitchTo().Window(popupWindowHandle);
                Driver.GetDriver(driverId).SwitchTo().Window(popupWindowHandle);
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
            //((IJavaScriptExecutor) Browser.GetDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
            ((IJavaScriptExecutor)Driver.GetDriver(driverId)).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 10)");
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
			var tenantSteps = new AddTenantsSteps(driverId);
			tenantSteps.PerformTwoTenantsAdding();
		}

		public void AddTenant(string tenant, string password)
		{
			var tenantSteps = new AddTenantsSteps(driverId);
			tenantSteps.PerformOneTenantAdding(tenant, password);
		}

		public void AddTwoTenants(string sourceTenant, string sourcePassword, string targetTenant, string targetPassword)
		{
			var tenantSteps = new AddTenantsSteps(driverId);
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
				$"{name} option",driverId);
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