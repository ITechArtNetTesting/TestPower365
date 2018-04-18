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
            Log.Debug("Enter Automation_IN_PublicFoldersTest");
		    try
		    {
		        bool result = false;
		        int counter = 0;
		        while (!result && counter < 3)
		        {
                    Log.Debug("Starting SourcePrepScript.ps1");
		            using (
		                var sourcePreparation = new PsLauncher().LaunchPowerShellInstance("PF/SourcePrepScript.ps1",
		                    $" -slogin {configurator.GetTenantValue("T5->T6", "source", "user")} -spassword {configurator.GetTenantValue("T5->T6", "source", "password")}", "x64")
		            )
		            {
                        Log.Debug("Script has started.");
		                while (!sourcePreparation.StandardOutput.EndOfStream)
		                {
                            Log.Debug("In stream loop");
		                    string line = sourcePreparation.StandardOutput.ReadLine();
		                    Log.Info(line);
		                    if (line.Contains("Public Folder successfully created"))
		                    {
                                Log.Debug("Found Success Line");
		                        result = true;
		                    }
		                }
                        Log.Debug("Script stream end");
		                sourcePreparation.WaitForExit(30000);
                        Log.Debug("Script has finished");
		                counter++;
		            }
                    Log.DebugFormat("result: {0}, counter: {1}", result, counter);
		        }
		        result = false;
		        counter = 0;
		        while (!result && counter < 3)
		        {
                    Log.Debug("Starting TargetPrepScript.ps1");
                    using (var targetPreparation = new PsLauncher().LaunchPowerShellInstance("PF/TargetPrepScript.ps1", $" -slogin {configurator.GetTenantValue("T5->T6", "target", "user")} -spassword {configurator.GetTenantValue("T5->T6", "target", "password")}", "x64"))
		            {
                        Log.Debug("Script has started.");
                        while (!targetPreparation.StandardOutput.EndOfStream)
		                {
                            Log.Debug("In stream loop");
                            string line = targetPreparation.StandardOutput.ReadLine();
		                    Log.Info(line);
		                    if (line.Contains("Public Folder successfully created"))
		                    {
                                Log.Debug("Found Success Line");
                                result = true;
		                    }
		                }
                        Log.Debug("Script stream end");
                        targetPreparation.WaitForExit(30000);
                        Log.Debug("Script has finished");
                        counter++;
		            }
                    Log.DebugFormat("result: {0}, counter: {1}", result, counter);
                }
                Log.Debug("Starting UI Automation");
                configurator.CreateFlagFolder(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//stopfolder"));
		        LoginAndSelectRole(configurator.GetValueByXpath("//metaname[text()='client2']/..//user"), configurator.GetValueByXpath("//metaname[text()='client2']/..//password"), configurator.GetValueByXpath("//metaname[text()='client2']/../name"));
		        SelectProject(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name"));
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
		            User.AtPublicFolderMigrationViewForm().SyncUserByLocator(configurator.GetValueByXpath(
		                "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        }
		        catch (Exception)
		        {
		            Log.Info("PF migration view form was not opened");
		            User.AtProjectOverviewForm().OpenPublicFolders();
		            User.AtPublicFolderMigrationViewForm().SyncUserByLocator(configurator.GetValueByXpath(
		                "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        }

		        User.AtPublicFolderMigrationViewForm().ConfirmSync();
		        User.AtPublicFolderMigrationViewForm().AssertUserHaveSyncingState(configurator.GetValueByXpath(
		            "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        User.AtPublicFolderMigrationViewForm().OpenDetailsByLocator(configurator.GetValueByXpath(
		            "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		        User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobAppear(1);
		        User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobDone();
		        User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobAppear(1);
		        User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobDone();
		        User.AtPublicFolderMigrationViewForm().VerifyProccessedFolders(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='processed1']/..//amount"));
		        User.AtPublicFolderMigrationViewForm().VerifyProvisioningTimeStampsAreNotEmpty();
		        User.AtPublicFolderMigrationViewForm().VerifyContentCopyTimeStampsAreNotEmpty();
		        User.AtPublicFolderMigrationViewForm().DownloadProvisioningLogs();
                configurator.CheckProvisioningLogsFileIsDownloadedAndNotEmpty();
		        //Note: add logs for second job
		        User.AtPublicFolderMigrationViewForm().CloseUserDetails();

                Log.Debug("Starting PfAutomationScript.ps1");
		        using (var mainScript = new PsLauncher().LaunchPowerShellInstance("PF/PfAutomationScript.ps1", $" -sourceUserName {configurator.GetTenantValue("T5->T6", "source", "user")}" +
		                                                                                                       $" -sourcepasswd {configurator.GetTenantValue("T5->T6", "source", "password")}" +
		                                                                                                       $" -TargetUserName {configurator.GetTenantValue("T5->T6", "target", "user")}" +
		                                                                                                       $" -Targetpasswd {configurator.GetTenantValue("T5->T6", "target", "password")}" +
		                                                                                                       $" -StopFilePath1 {configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile1']/..//path")}" +
		                                                                                                       $" -StopFilePath2 {configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile2']/..//path")}" +
		                                                                                                       $" -AttachmentPath {Path.GetFullPath(configurator.ResourcesPath + "test.txt")}" +
		                                                                                                       $" -SendAsSourceUserName {configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//source")}" +
		                                                                                                       $" -TargetAsSourceUserName {configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//target")}" +
		                                                                                                       $" -Sourcex400Address \"{configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//sourcex400")}\"" +
		                                                                                                       $" -Targetx400Address \"{configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//targetx400")}\"" +
		                                                                                                       $" -SourceProxyAddress {configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//sourceproxy")}" +
		                                                                                                       $" -TargetProxyAddress {configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry5']/..//targetproxy")}"))
		        {
		            while (!mainScript.StandardOutput.EndOfStream)
		            {
                        Log.Debug("In stream loop");
                        var line = mainScript.StandardOutput.ReadLine();
		                Log.Info(line);
		                if (line.Contains("Powershell will pause until Migration is complete - 1"))
		                {
                            Log.Debug("Found line waiting for migration 1.....");
		                    Thread.Sleep(180000);
		                    User.AtPublicFolderMigrationViewForm().OpenDetailsByLocator(configurator.GetValueByXpath(
		                        "//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry4']/..//source"));
		                    User.AtPublicFolderMigrationViewForm().SyncFromDetails();
		                    User.AtPublicFolderMigrationViewForm().ConfirmSync();
		                    User.AtPublicFolderMigrationViewForm().AssertDetailsSyncButtonIsDisabled();
		                    User.AtPublicFolderMigrationViewForm().AssertDetailsStopButtonIsEnabled();
		                    User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobAppear(2);
		                    User.AtPublicFolderMigrationViewForm().WaitForProvisioningJobDone();
		                    User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobAppear(2);
		                    User.AtPublicFolderMigrationViewForm().WaitForContentCopyJobDone();
		                    User.AtPublicFolderMigrationViewForm().VerifyProccessedFolders(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='processed2']/..//amount"));
                            configurator.CreateEmptyFile(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile1']/..//path"));
		                }
		                if (line.Contains("Powershell will pause until Migration is complete - 2"))
		                {
                            Log.Debug("Found line waiting for migration 2.....");
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
                            configurator.CreateEmptyFile(configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='stopfile2']/..//path"));
		                }
		                if (line.Contains("Folder existance Check succeeded"))
		                {
                            store.PfValidationDictionary["folderExistance"] = true;
		                }
		                if (line.Contains("Source Target Item existance Check succeeded"))
		                {
                            store.PfValidationDictionary["itemExistance"] = true;
		                }
		                if (line.Contains("Add New MailFolder Check succeeded-1"))
		                {
                            store.PfValidationDictionary["addMailFolder1"] = true;
		                }
		                if (line.Contains("Add New FolderTree Check succeeded-1"))
		                {
                            store.PfValidationDictionary["addFolderTree1"] = true;
		                }
		                if (line.Contains("Add New MailFolder Check succeeded-2"))
		                {
                            store.PfValidationDictionary["addMailFolder2"] = true;
		                }
		                if (line.Contains("Add New Contacts Check succeeded"))
		                {
                            store.PfValidationDictionary["addContancs"] = true;
		                }
		                if (line.Contains("Add New Calendar Check succeeded"))
		                {
                            store.PfValidationDictionary["addCalendar"] = true;
		                }
		                if (line.Contains("Add Folder to Test Folder Delete succeeded"))
		                {
                            store.PfValidationDictionary["addFolderDelete"] = true;
		                }
		                if (line.Contains("Add Folder to Test Folder Move succeeded"))
		                {
                            store.PfValidationDictionary["addFolderMove"] = true;
		                }
		                if (line.Contains("Add Folder to Test Folder Rename succeeded"))
		                {
                            store.PfValidationDictionary["addFolderRename"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-1"))
		                {
                            store.PfValidationDictionary["addNewItem1"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-2"))
		                {
                            store.PfValidationDictionary["addNewItem2"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-3"))
		                {
                            store.PfValidationDictionary["addNewItem3"] = true;
		                }
		                if (line.Contains("Add Contact Test succeeded"))
		                {
                            store.PfValidationDictionary["addContact"] = true;
		                }
		                if (line.Contains("Add Meeting Test succeeded"))
		                {
                            store.PfValidationDictionary["addMeeting"] = true;
		                }
		                if (line.Contains("Add Custom Item Test succeeded"))
		                {
                            store.PfValidationDictionary["addCustomItem"] = true;
		                }
		                if (line.Contains("SMTP Address Rewriting Test succeeded"))
		                {
                            store.PfValidationDictionary["smtpRewritting"] = true;
		                }
		                if (line.Contains("X400 Address Rewriting Test succeeded"))
		                {
                            store.PfValidationDictionary["x400Rewritting"] = true;
		                }
		                if (line.Contains("Add Item Test succeeded-4"))
		                {
                            store.PfValidationDictionary["addNewItem4"] = true;
		                }
		                if (line.Contains("Add PDL Test succeeded"))
		                {
                            store.PfValidationDictionary["addPDL"] = true;
		                }
		                if (line.Contains("Validate Add Mail Folder succeeded-1"))
		                {
                            store.PfValidationDictionary["validateAddMailFolder1"] = true;
		                }
		                if (line.Contains("Validate Add Mail FolderTree succeeded"))
		                {
                            store.PfValidationDictionary["validateAddFolderTree"] = true;
		                }
		                if (line.Contains("Mail Enable Folder Test succeeded"))
		                {
                            store.PfValidationDictionary["mailEnable"] = true;
		                }
		                if (line.Contains("Add SendAS Permission Test succeeded"))
		                {
                            store.PfValidationDictionary["addSendAsPerms"] = true;
		                }
		                if (line.Contains("Add Proxy Address Test succeeded"))
		                {
                            store.PfValidationDictionary["addProxy"] = true;
		                }
		                if (line.Contains("Validate Add Mail Folder succeeded-2"))
		                {
                            store.PfValidationDictionary["validateAddMailFolder2"] = true;
		                }
		                if (line.Contains("Validate Add Contacts Folder succeeded"))
		                {
                            store.PfValidationDictionary["validateAddContacts"] = true;
		                }
		                if (line.Contains("Validate Add Calendar Folder succeeded"))
		                {
                            store.PfValidationDictionary["validateCalendar"] = true;
		                }
		                if (line.Contains("Validate Add Mail Folder succeeded-3"))
		                {
                            store.PfValidationDictionary["validateAddMailFolder3"] = true;
		                }
		                if (line.Contains("Validate Add Item succeeded-1"))
		                {
                            store.PfValidationDictionary["validateAddItem1"] = true;
		                }
		                if (line.Contains("Validate Add Contact succeeded"))
		                {
                            store.PfValidationDictionary["validateAddContact"] = true;
		                }
		                if (line.Contains("Validate Add Meeting succeeded"))
		                {
                            store.PfValidationDictionary["validateAddMeeting"] = true;
		                }
		                if (line.Contains("Validate Custom Item succeeded"))
		                {
                            store.PfValidationDictionary["validateCustomItem"] = true;
		                }
		                if (line.Contains("Validate SMTP Transform succeeded"))
		                {
                            store.PfValidationDictionary["validateSmtp"] = true;
		                }
		                if (line.Contains("Validate x400 Transform succeeded"))
		                {
                            store.PfValidationDictionary["validatex400"] = true;
		                }
		                if (line.Contains("Validate Add Item succeeded-2"))
		                {
                            store.PfValidationDictionary["validateAddItem2"] = true;
		                }
		                if (line.Contains("move Test succeeded"))
		                {
                            store.PfValidationDictionary["validateMove"] = true;
		                }
		                if (line.Contains("Modify Folder Permissions Test succeeded"))
		                {
                            store.PfValidationDictionary["modifyFoldersPerms"] = true;
		                }
		                if (line.Contains("Add Attachment Test succeeded"))
		                {
                            store.PfValidationDictionary["addAttachment"] = true;
		                }
		                if (line.Contains("Modify Item Test succeeded"))
		                {
                            store.PfValidationDictionary["modifyItem"] = true;
		                }
		                if (line.Contains("Modify Contact Item Test succeeded"))
		                {
                            store.PfValidationDictionary["modifyContact"] = true;
		                }
		                if (line.Contains("Modify Sticky Note succeeded"))
		                {
                            store.PfValidationDictionary["modifyNote"] = true;
		                }
		                if (line.Contains("Rename Folder succeeded"))
		                {
                            store.PfValidationDictionary["folderRename"] = true;
		                }
		                if (line.Contains("Move Folder succeeded"))
		                {
                            store.PfValidationDictionary["folderMove"] = true;
		                }
		                if (line.Contains("Delete Folder succeeded"))
		                {
                            store.PfValidationDictionary["folderDelete"] = true;
		                }
		                if (line.Contains("Move Item succeeded"))
		                {
                            store.PfValidationDictionary["moveItem"] = true;
		                }
		                if (line.Contains("Delete Item succeeded"))
		                {
                            store.PfValidationDictionary["deleteItem"] = true;
		                }
		                if (line.Contains("Validate Modify Folder Permissions succeeded"))
		                {
                            store.PfValidationDictionary["validateFolderPerms"] = true;
		                }
		                if (line.Contains("Validate Add Attachment succeeded"))
		                {
                            store.PfValidationDictionary["validateAttachment"] = true;
		                }
		                if (line.Contains("Validate Modify Item Subject succeeded"))
		                {
                            store.PfValidationDictionary["validateModifyItem"] = true;
		                }
		                if (line.Contains("Validate Modify Item contact succeeded"))
		                {
                            store.PfValidationDictionary["validateModifyContact"] = true;
		                }
		                if (line.Contains("Validate Modify StickyNote succeeded"))
		                {
                            store.PfValidationDictionary["validateModifyNote"] = true;
		                }
		                if (line.Contains("Validate rename folder succeeded"))
		                {
                            store.PfValidationDictionary["validateRenameFolder"] = true;
		                }
		                if (line.Contains("Validate move folder succeeded"))
		                {
                            store.PfValidationDictionary["validateMoveFolder"] = true;
		                }
		                if (line.Contains("Validate delete folder succeeded"))
		                {
                            store.PfValidationDictionary["validateDeleteFolder"] = true;
		                }
		                if (line.Contains("Validate Move Item succeeded"))
		                {
                            store.PfValidationDictionary["validateMoveItem"] = true;
		                }
		                if (line.Contains("Validate Delete Item succeeded"))
		                {
                            store.PfValidationDictionary["validateDeleteItem"] = true;
		                }
		                if (line.Contains("Validate Mail Enable Folder succeeded"))
		                {
                            store.PfValidationDictionary["validateMailEnable"] = true;
		                }
		                if (line.Contains("validate Add SendAS Permission succeeded"))
		                {
                            store.PfValidationDictionary["validateAddSendAsPerms"] = true;
		                }
		                if (line.Contains("Validate Add Proxy Address validation succeeded"))
		                {
                            store.PfValidationDictionary["validateProxy"] = true;
		                }
		            }
                    Log.Debug("Script stream complete");
		            mainScript.WaitForExit(30000);
                    Log.Debug("Script exit");
		        }
		        foreach (var check in store.PfValidationDictionary)
		        {
                    Log.DebugFormat("key: {0}, value: {1}", check.Key, check.Value);
		            Assert.IsTrue(check.Value, check.Key + " check failed");
		        }
            }
		    catch (Exception)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}
