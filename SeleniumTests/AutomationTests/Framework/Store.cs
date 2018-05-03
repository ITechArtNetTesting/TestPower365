using System.Collections.Generic;

namespace Product.Framework
{
	public class Store
	{
		public static string ProjectName;
		public static string MainHandle;
		public static string AllUsersCount;
		public static string ReadyUsersCount;
		public static string ClientName;
		public static string FirstName;
		public static string LastName;
		public static string Phone;
		public static string Email;
		public static string Address;
		public static string City;
		public static string State;
		public static string Country;
		public static string Zip;
		public static string LinkName;
		public static string LinkUrl;
		public static List<string> SourceList = new List<string>();
		public static List<string> TargetList = new List<string>();
		public static List<string> StateList = new List<string>();
		public static List<string> GroupList = new List<string>();
		public static List<string> ProgressList = new List<string>();
		public static List<string> MailboxSizeList = new List<string>();
		public static List<string> ArchiveSizeList = new List<string>();
		public static List<string> ProfileList = new List<string>();
		public static bool IsComplete = false;
		public static bool IsWaitRequired = false;
		public static string TenantLog;
		public static string ProbeSourceMailbox;
		public static string ProbeTargetMailbox;
		public static string AccessUrl;
		public static string AccessKey;

		public static Dictionary<string, bool> PfValidationDictionary = new Dictionary<string, bool>
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