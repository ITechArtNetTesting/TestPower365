using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Steps.Interfaces
{
    public interface IMicrosoftLoginPageSteps
    {
        void LogIn(string email,string password);
    }
}
