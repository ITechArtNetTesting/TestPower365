﻿using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
   public  class ProfileHandleFoldersForm : BaseWizardStepForm
    {
        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleFoldersThatCannotBeSynced']");


        public ProfileHandleFoldersForm() : base(TitleLocator, "Profile handle folders items form")
        {
        }
    }
}
