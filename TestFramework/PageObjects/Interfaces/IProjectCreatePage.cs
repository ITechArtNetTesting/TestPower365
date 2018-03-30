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
        void UploadFile(string keys);
        void ClickSubmitButton();
        void ChooseEmailWithDiscoveryProjectType();
        void SelectTenantByName(string name);
        void SelectDomainByName(string name);
        void ChooseSelectUsersByActiveDirectoryGroup();
        void SendKeysToFindGroupInput(string groupName);
        void ClickFoundGroup();
        void SelectNoThanksToDefineMyMigrationWavesNow();
    }
}
