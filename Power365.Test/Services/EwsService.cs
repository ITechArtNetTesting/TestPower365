using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BinaryTree.Power365.Test.Services
{
    public class EwsService : IDisposable
    {        
        private readonly ExchangeService _exchangeService;
        private readonly string _mailbox;

        private const string O365_EWSURL = "https://outlook.office365.com/EWS/Exchange.asmx";

        public EwsService(string username, string password, string mailbox = null, string ewsUrl = O365_EWSURL)
        {
            ServicePointManager.ServerCertificateValidationCallback = _certificateValidationCallback;

            _exchangeService = new ExchangeService();
            _exchangeService.Credentials = new WebCredentials(username, password);

            _mailbox = mailbox ?? username;
            if (!string.IsNullOrWhiteSpace(ewsUrl))
                _exchangeService.Url = new Uri(ewsUrl);
            else
                _exchangeService.AutodiscoverUrl(_mailbox, _redirectionUrlValidationCallback);
        }

        public T NewObject<T>()
            where T: ServiceObject
        {
            return (T)Activator.CreateInstance(typeof(T), _exchangeService);
        }

        public Folder GetWellKnownFolder(WellKnownFolderName folderName)
        {
            return Folder.Bind(_exchangeService, folderName);
        }

        public byte[] GetEntryId(ServiceObject obj)
        {
            var propertyDefinition = new ExtendedPropertyDefinition(0x0FFF, MapiPropertyType.Binary);
            var propertySet = new PropertySet(BasePropertySet.FirstClassProperties);
            propertySet.Add(propertyDefinition);
            obj.Load(propertySet);
            byte[] entryId;
            if (!obj.TryGetProperty<byte[]>(propertyDefinition, out entryId))
                return null;
            return entryId;
        }

        public FindFoldersResults FindFoldersByDisplayName(Folder searchBase, string displayName, int resultLimit = 0)
        {
            var folderView = new FolderView(resultLimit);
            var searchFilter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, displayName);
            return searchBase.FindFolders(searchFilter, folderView);
        }


        private bool _redirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
        
        private bool _certificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Dispose()
        {

        }
    }
}
