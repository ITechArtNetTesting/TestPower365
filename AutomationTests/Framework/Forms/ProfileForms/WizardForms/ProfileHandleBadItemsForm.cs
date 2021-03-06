﻿using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.ProfileForms.WizardForms
{
    public class ProfileHandleBadItemsForm : BaseWizardStepForm
    {

        private static readonly By TitleLocator = By.XPath("//*/span[@data-translation='HowWouldYouLikeToHandleBadItems']");


        public ProfileHandleBadItemsForm() : base(TitleLocator, "Profile handle bad items form")
        {
        }
    }
}
