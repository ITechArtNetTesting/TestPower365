using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test
{
    public class MailEngineSettings
    {
        public int DownloadThreadCount { get; set; }
        public int UploadThreadCount { get; set; }
        public string ProcessingPath { get; set; }

        public bool EwsImpersonation { get; set; }
        public bool TransformSmtpAndIm { get; set; }
        public bool MigrateRules { get; set; }
        public bool SkipFolderRetentionTags { get; set; }
        public bool SkipRetentionPolicies { get; set; }
        public bool CopyOutlookAutoCompleteCache { get; set; }
        public bool CopyOwaAutoCompleteCache { get; set; }
        public bool CopyRecipientCache { get; set; }
        public bool SkipDeltaFolderDelete { get; set; }
        public bool MailOnlyDelta { get; set; }
        public bool DumpFailedMessageSegments { get; set; }
        public bool UploadSegmentDump { get; set; }
        public string UploadSegementDirectory { get; set; }
        //Filters
        //DefaultFiltercopyType
        public bool SkipMailboxCacheSyncFolders { get; set; }
        public bool CopyDumpster { get; set; }
        public bool CopyLitigationHoldFolders { get; set; }
        public string MappingFile { get; set; }
        public bool DuplicateConversationHistoryCheck { get; set; }
        public bool CopyArchive { get; set; }
    }

    public class MailEngineService
    {
        //private CredentialManager _credentialManager = new CredentialManager();
        //private Control _control;
        
        //private MailEngineSettings _settings;

        //public MailEngineService(MailEngineSettings settings)
        //{
        //    _settings = settings;

        //    ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
        //    ServicePointManager.DefaultConnectionLimit = 200;

        //    _control = new Control(_settings.DownloadThreadCount, _settings.UploadThreadCount, _settings.ProcessingPath, _credentialManager, null, null, null);
        //}
        
    }
}
