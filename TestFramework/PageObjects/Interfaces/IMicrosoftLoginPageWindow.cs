using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.PageObjects.Interfaces
{
    public interface IMicrosoftLoginPageWindow
    {
        void ClickUseAnotherAccount();
        void SendKeysToEmailInput(string keys);
        void ClickNextButton();
        void SendKeysToPasswordInput(string keys);
    }
}
