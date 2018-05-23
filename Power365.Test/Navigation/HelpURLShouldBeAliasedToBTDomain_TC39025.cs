using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.Navigation
{
    [TestClass]
    public class HelpURLShouldBeAliasedToBTDomain_TC39025 : TestBase
    {
        private string CorrectDomain;
        private string _client;
        private string _username;
        private string _password;

        private PageBase _basePage;
        private HelpPage _helpPage;

        public HelpURLShouldBeAliasedToBTDomain_TC39025() : base(LogManager.GetLogger(typeof(HelpURLShouldBeAliasedToBTDomain_TC39025))) { }

      
        [TestInitialize]
        public void InitializeTest()
        {
            CorrectDomain = Automation.Settings.HelpURL;
            var client = Automation.Settings.GetByReference<Client>("client1");
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
        }

        [TestMethod]
        public void HelpURLShouldBeAliasedToBTDomain_39025()
        {           
            _basePage = Automation.Common
                        .SingIn(_username, _password)
                        .GetPage<PageBase>();
            _helpPage=_basePage.Menu.ClickHelp();            
            Assert.IsTrue(_helpPage.GetUrl().Contains(CorrectDomain),"The help link incorrect");
        }
    }
}
