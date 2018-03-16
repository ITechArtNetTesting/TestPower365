using System;
using System.Linq;
using ITechArtTestFramework.Drivers;
using ITechArtTestFramework.PageObjects;
using ITechArtTestFramework.StaticClasses;

namespace ITechArtTestFramework.Steps
{
    public abstract class BaseSteps
    {
        public PageObject WorkPage;

        public void InitPageObjects(Driver driver)
        {
            var pageObject = GetType().GetFields(FrameworkConstants.BindingFlags).Where(field => field.FieldType.IsSubclassOf(typeof(PageObject))).FirstOrDefault();

            if (GetType().GetFields(FrameworkConstants.BindingFlags).Where(field => field.FieldType.IsSubclassOf(typeof(PageObject))).Count() > 1)
            {
                throw new Exception("Steps must have only one PageObject");
            }
            pageObject.SetValue(this, Activator.CreateInstance(pageObject.FieldType));
            WorkPage = (PageObject)pageObject.GetValue(this);
            WorkPage.InitWebelements(driver);
        }
    }
}
