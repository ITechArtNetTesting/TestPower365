namespace TestFramework.PageObjects.Interfaces
{
    public interface IProjectCreatePage
    {
        void ClickNextButton();
        void ClickBackButton();
        void ChooseEmailFromFileProjectType();
        void CallProjectWithKeys(string keys);
        void SendRandomKeysToDescription();
        void ClickAddTenantButton();
    }
}
