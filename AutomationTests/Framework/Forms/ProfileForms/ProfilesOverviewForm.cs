using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.ProfileForms
{
    public class ProfilesOverviewForm : BaseForm
    {
        private static readonly By TitleLocator = By.XPath("//*[contains(text(), 'Double-click on any profile details')]");

        public ProfilesOverviewForm() : base(TitleLocator, "Profiles overview form")
        {
        }
        private Button addProfileButton => new Button(By.XPath("//div[contains(@class, 'ibox-content')]//*[contains(text(), 'Add Profile')]"), "Add profile button");
        
        private string ProfileLabelLocator = "//*[contains(text(), '{0}')]";
        private string ProfileModifyLocator = "//tr[.//*[contains(text(), '{0}')]]//div[contains(@class, 'table-actions')]//*[contains(text(), 'Modify')]";
        public void AddProfile()
        {
            Log.Info("Adding profile");
            addProfileButton.Click();
        }

        public void HoverOverProfileLabel(string label)
        {
            Log.Info("Hovering over profile label: " + label);
            new Label(By.XPath(String.Format(ProfileLabelLocator, label)), "Profile Label").Move();
        }

        public void ModifyProfile(string profile)
        {
            Log.Info("Modifying profile: "+profile);
            HoverOverProfileLabel(profile);
            new Button(By.XPath(String.Format(ProfileModifyLocator, profile)), "Profile modify button").Click();
        }
    }
}
