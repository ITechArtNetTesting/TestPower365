using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.Menu
{
    class UsersMenuForm : BaseForm
       
    {
        private static readonly By TitleLocator = By.XPath("//div[ @id='usersContainer']//*[contains(text(), 'Users')]");

        public UsersMenuForm()
             : base(TitleLocator, "User menu form")
        {
        }
       
    }
}
