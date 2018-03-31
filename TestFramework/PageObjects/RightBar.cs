using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Waiters;

namespace TestFramework.PageObjects
{
    public class RightBar : BasePage, IRightBar
    {
        [FindsBy(How = How.XPath, Using = "//*/li[@class='notranslate']//button")]
        IWebElement ClientDropDown { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/li[@class='notranslate']//ul/li")]
        IList<IWebElement> ListOfClients { get; set; }

        public void ChooseClient(string clientName)
        {            
            foreach (var client in ListOfClients)
            {
                if (client.Text == clientName.ToUpper())
                {                                        
                    Perform.Click(client);
                    break;
                }
            }            
        }

        public void ClickClientDropDown()
        {            
            Perform.Click(ClientDropDown);
        }
    }
}
