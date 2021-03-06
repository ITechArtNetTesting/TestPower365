﻿using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.MailOnlyTests.SortingAndFilterTests
{
	[TestClass]
	public class SortingTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		[TestMethod]
		[TestCategory("MailOnly")]
        [TestCategory("SeleniumLegacy")]
        [TestCategory("UI")]
        //22169
        public void Automation_MO_SortingTest()
		{
		    string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
		    string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
		    string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
		    string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project3']/..//name");
		    string filename = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project3']/..//metaname[text()='file2']/..//filename");
		   
            try
            {
                LoginAndReloadFile(login, password, client, projectName, filename);
		        User.AtProjectOverviewForm().OpenUsersList();
             
                User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortSource();
              
                User.AtUsersForm().AssertSourceSorted();
		        User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortTarget();
		        User.AtUsersForm().AssertTargetSorted();
                             
                User.AtUsersForm().StoreEntriesData();
		        User.AtUsersForm().SortStatus();
             
                User.AtUsersForm().AssertStatusSorted();
		        User.AtUsersForm().StoreEntriesData();
            }
            catch (Exception)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
        }
	}
}