using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;

namespace Product.Tests.PublicFoldersTests
{
	[TestClass]
	public class PublicFoldersTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}

		[TestMethod]
		public void Automation_IN_PublicFoldersTest()
		{
		    try
		    {
		        bool result = false;
		        int counter = 0;
		        while (!result && counter < 3)
		        {
		            using (
		                var sourcePreparation = new PsLauncher().LaunchPowerShellInstance("PF/SourcePrepScript.ps1",
		                    $" -slogin {RunConfigurator.GetTenantValue("T5->T6", "source", "user")} -spassword {RunConfigurator.GetTenantValue("T5->T6", "source", "password")}", "x64")
		            )
		            {
		                while (!sourcePreparation.StandardOutput.EndOfStream)
		                {
		                    string line = sourcePreparation.StandardOutput.ReadLine();
		                    Log.Info(line);
		                    if (line.Contains("Public Folder successfully created"))
		                    {
		                        result = true;
		                    }
		                }
		                sourcePreparation.WaitForExit(30000);
		                counter++;
		            }
		        }
		        result = false;
		        counter = 0;
		        while (!result && counter < 3)
		        {
		            using (var targetPreparation = new PsLauncher().LaunchPowerShellInstance("PF/TargetPrepScript.ps1", $" -slogin {RunConfigurator.GetTenantValue("T5->T6", "target", "user")} -spassword {RunConfigurator.GetTenantValue("T5->T6", "target", "password")}", "x64"))
		            {
		                while (!targetPreparation.StandardOutput.EndOfStream)
		                {
		                    string line = targetPreparation.StandardOutput.ReadLine();
		                    Log.Info(line);
		                    if (line.Contains("Public Folder successfully created"))
		                    {
		                        result = true;
		                    }
		                }
		                targetPreparation.WaitForExit(30000);
		                counter++;
		            }
		        }
		        RunConfigurator.CreateFlagFolder(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//stopfolder"));
		        LoginAndSelectRole(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user"), RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password"), RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name"));
		        SelectProject(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name"));
		        try
		        {
		            User.AtProjectOverviewForm().OpenPublicFolders();
		        }
		        catch (Exception)
		        {
		            Log.Info("Total folders link is not clickable");
		            User.AtProjectOverviewForm().OpenPublicFolders();
		        }
		        try
		        {
		            User.AtPublicFolderMigrationViewForm().SyncUserByLocator(RunConfigurator.GetValueByXpath(
		                "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        }
		        catch (Exception)
		        {
		            Log.Info("PF migration view form was not opened");
		            User.AtProjectOverviewForm().OpenPublicFolders();
		            User.AtPublicFolderMigrationViewForm().SyncUserByLocator(RunConfigurator.GetValueByXpath(
		                "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        }

		        User.AtPublicFolderMigrationViewForm().ConfirmSync();
		        User.AtPublicFolderMigrationViewForm().AssertUserHaveSyncingState(RunConfigurator.GetValueByXpath(
		            "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        User.AtPublicFolderMigrationViewForm().OpenDetailsByLocator(RunConfigurator.GetValueByXpath(
		            "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobAppear(1);
		        User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobDone();
		        User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobAppear(1);
		        User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobDone();
		        User.AtPublicFolderMigrationViewForm().VerifyProccessedFolders(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='processed1']/..//amount"));
		        User.AtPublicFolderMigrationViewForm().VerifyProvisioningTimeStampsAreNotEmpty();
		        User.AtPublicFolderMigrationViewForm().VerifyContentCopyTimeStampsAreNotEmpty();
		        User.AtPublicFolderMigrationViewForm().DownloadProvisioningLogs();
		        RunConfigurator.CheckProvisioningLogsFileIsDownloadedAndNotEmpty();
		        //Note: add logs for second job
		        User.AtPublicFolderMigrationViewForm().CloseUserDetails();
		        using (var mainScript = new PsLauncher().LaunchPowerShellInstance("PF/PfAutomationScript.ps1", $" -sourceUserName {RunConfigurator.GetTenantValue("T5->T6", "source", "user")}" +
		                                                                                                       $" -sourcepasswd {RunConfigurator.GetTenantValue("T5->T6", "source", "password")}" +
		                                                                                                       $" -TargetUserName {RunConfigurator.GetTenantValue("T5->T6", "target", "user")}" +
		                                                                                                       $" -Targetpasswd {RunConfigurator.GetTenantValue("T5->T6", "target", "password")}" +
		                                                                                                       $" -StopFilePath1 {RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile1']/..//path")}" +
		                                                                                                       $" -StopFilePath2 {RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile2']/..//path")}" +
		                                                                                                       $" -AttachmentPath {Path.GetFullPath(RunConfigurator.ResourcesPath + "test.txt")}" +
		                                                                                                       $" -SendAsSourceUserName {RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//source")}" +
		                                                                                                       $" -TargetAsSourceUserName {RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//target")}" +
		                                                                                                       $" -Sourcex400Address \"{RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//sourcex400")}\"" +
		                                                                                                       $" -Targetx400Address \"{RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//targetx400")}\"" +
		                                                                                                       $" -SourceProxyAddress {RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//sourceproxy")}" +
		                                                                                                       $" -TargetProxyAddress {RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//targetproxy")}"))
		        {
		            while (!mainScript.StandardOutput.EndOfStream)
		            {
		                var line = mainScript.StandardOutput.ReadLine();
		                Log.Info(line);
		                if (line.Contains("Powershell will pause until Migration is complete - 1"))
		                {
		                    Thread.Sleep(180000);
		                    User.AtPublicFolderMigrationViewForm().OpenDetailsByLocator(RunConfigurator.GetValueByXpath(
		                        "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		                    User.AtPublicFolderMigrationViewForm().SyncFromDetails();
		                    User.AtPublicFolderMigrationViewForm().ConfirmSync();
		                    User.AtPublicFolderMigrationViewForm().AssertDetailsSyncButtonIsDisabled();
		                    User.AtPublicFolderMigrationViewForm().AssertDetailsStopButtonIsEnabled();
		                    User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobAppear(2);
		                    User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobDone();
		                    User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobAppear(2);
		                    User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobDone();
		                    User.AtPublicFolderMigrationViewForm().VerifyProccessedFolders(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='processed2']/..//amount"));
		                    RunConfigurator.CreateEmptyFile(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile1']/..//path"));
		                }
		                if (line.Contains("Powershell will pause until Migration is complete - 2"))
		                {
		                    Thread.Sleep(180000);
		                    try
		                    {
		                        User.AtPublicFolderMigrationViewForm().SyncFromDetails();
		                    }
		                    catch (Exception)
		                    {
		                        User.AtPublicFolderMigrationViewForm().RefreshData();
		                        User.AtPublicFolderMigrationViewForm().SyncFromDetails();
		                    }
		                    User.AtPublicFolderMigrationViewForm().ConfirmSync();
		                    User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobAppear(3);
		                    User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobDone();
		                    User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobAppear(3);
		                    User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobDone();
		                    RunConfigurator.CreateEmptyFile(RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile2']/..//path"));
		                }
		                if (line.Contains("Folder existance Check succeeded"))
		                {
		                    Store.PfValidationDictionary["folderExistance"] = true;
		                }
		                if (line.Contains("Source Target Item existance Check succeeded"))
		                {
		                    Store.PfValidationDictionary["itemExistance"] = true;
		                }
		                if (line.Contains("Add New MailFolder Check succeeded-1"))
		                {
		                    Store.PfValidationDictionary["addMailFolder1"] = true;
		                }
		                if (line.Contains("Add New FolderTree Check succeeded-1"))
		                {
		                    Store.PfValidationDictionary["addFolderTree1"] = true;
		                }
		                if (line.Contains("Add New MailFolder Check succeeded-2"))
		                {
		                    Store.PfValidationDictionary["addMailFolder2"] = true;
		                }
		                if (line.Contains("Add New Contacts Check succeeded"))
		                {
		                    Store.PfValidationDictionary["addContancs"] = true;
		                }
		                if (line.Contains("Add New Calendar Check succeeded"))
		                {
		                    Store.PfValidationDictionary["addCalendar"] = true;
		                }
		                if (line.Contains("Add Folder to Test Folder Delete succeeded"))
		                {
		                    Store.PfValidationDictionary["addFolderDelete"] = true;
		                }
		                if (line.Contains("Add Folder to Test Folder Move succeeded"))
		                {
		                    Store.PfValidationDictionary["addFolderMove"] = true;
		                }
		                if (line.Contains("Add Folder to Test Folder Rename succeeded"))
		                {
		                    Store.PfValidationDictionary["addFolderRename"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-1"))
		                {
		                    Store.PfValidationDictionary["addNewItem1"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-2"))
		                {
		                    Store.PfValidationDictionary["addNewItem2"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-3"))
		                {
		                    Store.PfValidationDictionary["addNewItem3"] = true;
		                }
		                if (line.Contains("Add Contact Test succeeded"))
		                {
		                    Store.PfValidationDictionary["addContact"] = true;
		                }
		                if (line.Contains("Add Meeting Test succeeded"))
		                {
		                    Store.PfValidationDictionary["addMeeting"] = true;
		                }
		                if (line.Contains("Add Custom Item Test succeeded"))
		                {
		                    Store.PfValidationDictionary["addCustomItem"] = true;
		                }
		                if (line.Contains("SMTP Address Rewriting Test succeeded"))
		                {
		                    Store.PfValidationDictionary["smtpRewritting"] = true;
		                }
		                if (line.Contains("X400 Address Rewriting Test succeeded"))
		                {
		                    Store.PfValidationDictionary["x400Rewritting"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-4"))
		                {
		                    Store.PfValidationDictionary["addNewItem4"] = true;
		                }
		                if (line.Contains("Add PDL Test succeeded"))
		                {
		                    Store.PfValidationDictionary["addPDL"] = true;
		                }
		                if (line.Contains("Validate Add Mail Folder succeeded-1"))
		                {
		                    Store.PfValidationDictionary["validateAddMailFolder1"] = true;
		                }
		                if (line.Contains("Validate Add Mail FolderTree succeeded"))
		                {
		                    Store.PfValidationDictionary["validateAddFolderTree"] = true;
		                }
		                if (line.Contains("Mail Enable Folder Test succeeded"))
		                {
		                    Store.PfValidationDictionary["mailEnable"] = true;
		                }
		                if (line.Contains("Add SendAS Permission Test succeeded"))
		                {
		                    Store.PfValidationDictionary["addSendAsPerms"] = true;
		                }
		                if (line.Contains("Add Proxy Address Test succeeded"))
		                {
		                    Store.PfValidationDictionary["addProxy"] = true;
		                }
		                if (line.Contains("Validate Add Mail Folder succeeded-2"))
		                {
		                    Store.PfValidationDictionary["validateAddMailFolder2"] = true;
		                }
		                if (line.Contains("Validate Add Contacts Folder succeeded"))
		                {
		                    Store.PfValidationDictionary["validateAddContacts"] = true;
		                }
		                if (line.Contains("Validate Add Calendar Folder succeeded"))
		                {
		                    Store.PfValidationDictionary["validateCalendar"] = true;
		                }
		                if (line.Contains("Validate Add Mail Folder succeeded-3"))
		                {
		                    Store.PfValidationDictionary["validateAddMailFolder3"] = true;
		                }
		                if (line.Contains("Validate Add Item succeeded-1"))
		                {
		                    Store.PfValidationDictionary["validateAddItem1"] = true;
		                }
		                if (line.Contains("Validate Add Contact succeeded"))
		                {
		                    Store.PfValidationDictionary["validateAddContact"] = true;
		                }
		                if (line.Contains("Validate Add Meeting succeeded"))
		                {
		                    Store.PfValidationDictionary["validateAddMeeting"] = true;
		                }
		                if (line.Contains("Validate Custom Item succeeded"))
		                {
		                    Store.PfValidationDictionary["validateCustomItem"] = true;
		                }
		                if (line.Contains("Validate SMTP Transform succeeded"))
		                {
		                    Store.PfValidationDictionary["validateSmtp"] = true;
		                }
		                if (line.Contains("Validate x400 Transform succeeded"))
		                {
		                    Store.PfValidationDictionary["validatex400"] = true;
		                }
		                if (line.Contains("Validate Add Item succeeded-2"))
		                {
		                    Store.PfValidationDictionary["validateAddItem2"] = true;
		                }
		                if (line.Contains("move Test succeeded"))
		                {
		                    Store.PfValidationDictionary["validateMove"] = true;
		                }
		                if (line.Contains("Modify Folder Permissions Test succeeded"))
		                {
		                    Store.PfValidationDictionary["modifyFoldersPerms"] = true;
		                }
		                if (line.Contains("Add Attachment Test succeeded"))
		                {
		                    Store.PfValidationDictionary["addAttachment"] = true;
		                }
		                if (line.Contains("Modify Item Test succeeded"))
		                {
		                    Store.PfValidationDictionary["modifyItem"] = true;
		                }
		                if (line.Contains("Modify Contact Item Test succeeded"))
		                {
		                    Store.PfValidationDictionary["modifyContact"] = true;
		                }
		                if (line.Contains("Modify Sticky Note succeeded"))
		                {
		                    Store.PfValidationDictionary["modifyNote"] = true;
		                }
		                if (line.Contains("Rename Folder succeeded"))
		                {
		                    Store.PfValidationDictionary["folderRename"] = true;
		                }
		                if (line.Contains("Move Folder succeeded"))
		                {
		                    Store.PfValidationDictionary["folderMove"] = true;
		                }
		                if (line.Contains("Delete Folder succeeded"))
		                {
		                    Store.PfValidationDictionary["folderDelete"] = true;
		                }
		                if (line.Contains("Move Item succeeded"))
		                {
		                    Store.PfValidationDictionary["moveItem"] = true;
		                }
		                if (line.Contains("Delete Item succeeded"))
		                {
		                    Store.PfValidationDictionary["deleteItem"] = true;
		                }
		                if (line.Contains("Validate Modify Folder Permissions succeeded"))
		                {
		                    Store.PfValidationDictionary["validateFolderPerms"] = true;
		                }
		                if (line.Contains("Validate Add Attachment succeeded"))
		                {
		                    Store.PfValidationDictionary["validateAttachment"] = true;
		                }
		                if (line.Contains("Validate Modify Item Subject succeeded"))
		                {
		                    Store.PfValidationDictionary["validateModifyItem"] = true;
		                }
		                if (line.Contains("Validate Modify Item contact succeeded"))
		                {
		                    Store.PfValidationDictionary["validateModifyContact"] = true;
		                }
		                if (line.Contains("Validate Modify StickyNote succeeded"))
		                {
		                    Store.PfValidationDictionary["validateModifyNote"] = true;
		                }
		                if (line.Contains("Validate rename folder succeeded"))
		                {
		                    Store.PfValidationDictionary["validateRenameFolder"] = true;
		                }
		                if (line.Contains("Validate move folder succeeded"))
		                {
		                    Store.PfValidationDictionary["validateMoveFolder"] = true;
		                }
		                if (line.Contains("Validate delete folder succeeded"))
		                {
		                    Store.PfValidationDictionary["validateDeleteFolder"] = true;
		                }
		                if (line.Contains("Validate Move Item succeeded"))
		                {
		                    Store.PfValidationDictionary["validateMoveItem"] = true;
		                }
		                if (line.Contains("Validate Delete Item succeeded"))
		                {
		                    Store.PfValidationDictionary["validateDeleteItem"] = true;
		                }
		                if (line.Contains("Validate Mail Enable Folder succeeded"))
		                {
		                    Store.PfValidationDictionary["validateMailEnable"] = true;
		                }
		                if (line.Contains("validate Add SendAS Permission succeeded"))
		                {
		                    Store.PfValidationDictionary["validateAddSendAsPerms"] = true;
		                }
		                if (line.Contains("Validate Add Proxy Address validation succeeded"))
		                {
		                    Store.PfValidationDictionary["validateProxy"] = true;
		                }
		            }
		            mainScript.WaitForExit(30000);
		        }
		        foreach (var check in Store.PfValidationDictionary)
		        {
		            Assert.IsTrue(check.Value, check.Key + " check failed");
		        }
            }
		    catch (Exception)
		    {
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}
