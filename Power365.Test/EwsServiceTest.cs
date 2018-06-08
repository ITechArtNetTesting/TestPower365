using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using BinaryTree.Power365.Test.Services;
using Microsoft.Exchange.WebServices.Data;

namespace BinaryTree.Power365.Test
{
    [TestFixture]
    public class EwsServiceTest
    {
        [Test]
        [Category("Ews")]
        public void CanAuthenticateO365()
        {
            EwsService service = new EwsService("admin@BTCloud7.Power365.Cloud", "Password31");

            var inbox = service.GetWellKnownFolder(Microsoft.Exchange.WebServices.Data.WellKnownFolderName.Inbox);

            var entryId = service.GetEntryId(inbox);

            var result = service.FindFoldersByDisplayName(inbox, "test", 1);

            //var newFolder = service.NewObject<Folder>();
            //newFolder.DisplayName = "test";
            
        }
    }
}
