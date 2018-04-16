using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class SyncAddressBooksForm : BaseWizardStepForm
	{

        private Guid driverId;
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Would you like to create a unified address book')]");

		public SyncAddressBooksForm(Guid driverId) : base(TitleLocator, "How would you like to sync Address Books form",driverId)
		{
            this.driverId = driverId;
            dontSyncAtAllLabel = new Label(By.XPath("//label[contains(@for, 'abDontSync')]"), "Don`t sync at all label",driverId);
            dontSyncAtAllRadioButton = new RadioButton(By.Id("abDontSync"), "Don`t sync at all radio",driverId);
            fromSourceToTargetLabel = new Label(By.XPath("//label[contains(@for, 'abSyncSrcTgt')]"), "From source to target label",driverId);
            fromSourceToTargetRadioButton = new RadioButton(By.Id("abSyncSrcTgt"), "From source to target radio",driverId);
            fromTargetToSourceLabel = new Label(By.XPath("//label[contains(@for, 'abSyncTgtSrc')]"), "From target to source label",driverId);
            fromTargetToSourceRadioButton = new RadioButton(By.Id("abSyncTgtSrc"), "From target to source radio",driverId);
            inBothDirectionsLabel = new Label(By.XPath("//label[contains(@for, 'abSyncBiDi')]"), "In both directions label",driverId);
            inBothDirectionsRadioButton = new RadioButton(By.Id("abSyncBiDi"), "In both directions radio",driverId);
        }

		private readonly Label dontSyncAtAllLabel ;
		private readonly RadioButton dontSyncAtAllRadioButton ;
		private readonly Label fromSourceToTargetLabel ;
		private readonly RadioButton fromSourceToTargetRadioButton ;
		private readonly Label fromTargetToSourceLabel ;
		private readonly RadioButton fromTargetToSourceRadioButton ;
		private readonly Label inBothDirectionsLabel ;
		private readonly RadioButton inBothDirectionsRadioButton ;

		public void SelectDontSyncAtAll()
		{
			Log.Info("Select don`t sync at all");
			dontSyncAtAllLabel.Click();
			try
			{
				dontSyncAtAllRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				dontSyncAtAllLabel.Click();
			}
		}

		public void SelectFromSourceToTarget()
		{
			Log.Info("Select from source to target");
			fromSourceToTargetLabel.Click();
			try
			{
				fromSourceToTargetRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				fromSourceToTargetLabel.Click();
			}
		}

		public void SelectFromTargetToSource()
		{
			Log.Info("Selecting from target to source");
			fromTargetToSourceLabel.Click();
			try
			{
				fromTargetToSourceRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Selecting from target to source");
				fromTargetToSourceLabel.Click();
			}
		}

		public void SelectInBothDirections()
		{
			Log.Info("Selecting in both directions");
			inBothDirectionsLabel.Click();
			try
			{
				inBothDirectionsRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Selecting in both directions");
				inBothDirectionsLabel.Click();
			}
		}
	}
}
