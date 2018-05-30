using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class EditMigrationProfilePage: PageBase
    {
        private static By _locator = By.Id("editMigrationProfileContainer");
        public EditMigrationProfilePage(IWebDriver webDriver)
           : base(_locator, webDriver) { }

        public ButtonElement NextButton
        {
            get
            {
                return new ButtonElement(_nextButton, WebDriver);
            }
        }

        private By _nextButton = By.XPath("//div[contains(@class, 'wizard-footer')]//*[contains(text(), 'Next')][not(@disabled='')]");
        private By _backButton = By.XPath("//div[contains(@class, 'wizard-footer')]//button[contains(@data-bind, 'goBack')]");
        private By _finishButton = By.XPath("//div[contains(@class, 'wizard-footer')]//*[contains(text(), 'Finish')]");
        private By _profileName = By.Id("profileName");
        
        //wizard pages
        private By _willUsersNeedtoUpdate = By.XPath("//*/span[@data-translation='AnEmailNotificationWillBeSentToTheSourceUser']");
        private By _doYouWantToCopyLitigationHoldSettings = By.XPath("//*/span[@data-translation='DoYouWantToCopyLitigationHoldSettingsAndData']");
        private By _wouldYouLikeToTranslateSourceEmail = By.XPath("//*/span[@data-translation='WouldYouLikeToTranslateSourceEmail']");
        private By _whatTypeOfMailboxContentWouldYouLikeToMigrate = By.XPath("//*/span[@data-translation='WhatTypeOfMailboxContentWouldYouLikeToMigrate']");
        private By _howWouldYouLikeToHandleLargeItems = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleLargeItems']");
        private By _howWouldYouLikeToHandleBadItems = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleBadItems']");
        private By _howWouldYouLikeToHandleFoldersThatCannotBeSynced = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleFoldersThatCannotBeSynced']");

        private By _youAreAlmostDoneLetsReview = By.XPath("//*/span[@data-translation='YouAreAlmostDoneLetsReview']");
        

        public void SetProfileName(string profileName)
        {
            InputElement profileNameField= new InputElement(_profileName,WebDriver);
            profileNameField.SendKeys(profileName);
        }


        public MigrationProfilesPage createSimpleProfile(string profileName)
        {
            SetProfileName("TestProfile1");
            NextButton.Click();
            IsElementVisible(_willUsersNeedtoUpdate);
            NextButton.Click();
            IsElementVisible(_doYouWantToCopyLitigationHoldSettings);           
            NextButton.Click();
            IsElementVisible(_wouldYouLikeToTranslateSourceEmail);            
            NextButton.Click();
            IsElementVisible(_whatTypeOfMailboxContentWouldYouLikeToMigrate);           
            NextButton.Click();
            IsElementVisible(_howWouldYouLikeToHandleLargeItems);           
            NextButton.Click();
            IsElementVisible(_howWouldYouLikeToHandleBadItems);
            NextButton.Click();

            IsElementVisible(_howWouldYouLikeToHandleFoldersThatCannotBeSynced);
            NextButton.Click();
            IsElementVisible(_youAreAlmostDoneLetsReview);
            return FinishClick();
        }

        public void NextClick()
        {
            NextButton.Click();
        }
        public MigrationProfilesPage FinishClick()
        {
          return  ClickElementBy<MigrationProfilesPage>(_finishButton);
        }

    }
}
