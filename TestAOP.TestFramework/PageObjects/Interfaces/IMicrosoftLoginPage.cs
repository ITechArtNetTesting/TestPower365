namespace TestFramework.PageObjects.Interfaces
{
    public interface IMicrosoftLoginPage
    {
        void SendKeysToEmailPhoneOrSkypeInput(string keys);
        void SendKeysToPasswordInput(string keys);
        void ClickNextButton();
        void ClickSignInButton();
        void ClickYesToStaySignedButton();
    }
}
