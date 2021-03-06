﻿using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.Test.CommonTests;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.Menu

{
    [TestFixture]
    public class HelpURLShouldBeAliasedToBTDomain_TC39025 : TestBase
    {
        private string CorrectDomain;
        private string _client;
        private string _username;
        private string _password;
         

        public HelpURLShouldBeAliasedToBTDomain_TC39025() : base() { }

      
        [SetUp]
        public void InitializeTest()
        {
            CorrectDomain = Automation.Settings.HelpURL;
            var client = Automation.Settings.GetByReference<Client>("client1");
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
        }

        [Test]
        [Category("UI")]
        [Category("Menu")]
        public void HelpURLShouldBeAliasedToBTDomain_39025()
        {
           var _helpPage = Automation.Common
                        .SingIn(_username, _password)
                        .GetPage<PageBase>()
                        .Menu
                        .ClickHelp();                     
            Assert.IsTrue(_helpPage.GetUrl().Contains(CorrectDomain),"The help link incorrect");
        }
    }
}
