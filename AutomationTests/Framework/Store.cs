using System.Collections.Generic;

namespace Product.Framework
{
	public class Store
	{
		public string ProjectName;
		public string MainHandle;
		public string AllUsersCount;
		public string ReadyUsersCount;
		public string ClientName;
		public string FirstName;
		public string LastName;
		public string Phone;
		public string Email;
		public string Address;
		public string City;
		public string State;
		public string Country;
		public string Zip;
		public string LinkName;
		public string LinkUrl;
		public List<string> SourceList = new List<string>();
		public List<string> TargetList = new List<string>();
		public List<string> StateList = new List<string>();
		public List<string> GroupList = new List<string>();
		public List<string> ProgressList = new List<string>();
		public List<string> MailboxSizeList = new List<string>();
		public List<string> ArchiveSizeList = new List<string>();
		public List<string> ProfileList = new List<string>();
		public bool IsComplete = false;
		public bool IsWaitRequired = false;
		public string TenantLog;
		public string ProbeSourceMailbox;
		public string ProbeTargetMailbox;
		public string AccessUrl;
		public string AccessKey;

		public Dictionary<string, bool> PfValidationDictionary = new Dictionary<string, bool>
		{
			{ "folderExistance", false },
			{ "itemExistance", false },
			{ "addMailFolder1", false },
			{ "addFolderTree1", false },
			{ "addMailFolder2", false },
			{ "addContancs", false },
			{ "addCalendar", false },
			{ "addFolderDelete", false },
			{ "addFolderMove", false },
			{ "addFolderRename", false},
			{ "addNewItem1", false },
			{ "addNewItem2", false },
			{ "addNewItem3", false },
			{ "addContact", false },
			{ "addMeeting", false },
			{ "addCustomItem", false },
			{ "smtpRewritting", false },
			{ "x400Rewritting", false },
			{ "addNewItem4", false },
			{ "addPDL", false },
			{ "validateAddMailFolder1", false },
			{ "validateAddFolderTree", false },
			{ "mailEnable", false },
			{ "addSendAsPerms", false },
			{ "addProxy", false },
			{ "validateAddMailFolder2", false },
			{ "validateAddContacts", false },
			{ "validateCalendar", false },
			{ "validateAddMailFolder3", false },
			{ "validateAddItem1", false },
			{ "validateAddContact", false },
			{ "validateAddMeeting", false },
			{ "validateCustomItem", false },
			{ "validateSmtp", false },
			{ "validatex400", false },
			{ "validateAddItem2", false },
			{ "validateMove", false },
			{ "modifyFoldersPerms", false },
			{ "addAttachment", false },
			{ "modifyItem", false },
			{ "modifyContact", false },
			{ "modifyNote", false },
			{ "folderRename", false },
			{ "folderMove", false },
			{ "folderDelete", false },
			{ "moveItem", false },
			{ "deleteItem", false },
			{ "validateFolderPerms", false },
			{ "validateAttachment", false },
			{ "validateModifyItem", false },
			{ "validateModifyContact", false },
			{ "validateModifyNote", false },
			{ "validateRenameFolder", false },
			{ "validateMoveFolder", false },
			{ "validateDeleteFolder", false },
			{ "validateMoveItem", false },
			{ "validateDeleteItem", false },
			{ "validateMailEnable", false },
			{ "validateAddSendAsPerms", false },
			{ "validateProxy", false }
		};
	}
}