function Invoke-Test48980{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		$Data = "" | Select OrginalId,MovedId,NewId
		if($TargetMailbox.IsPresent){
			$service = $Script:TargetService
			$MailboxName = $Script:TargetMailbox
		}
		else{
			$service = $Script:SourceService
			$MailboxName = $Script:SourceMailbox
		}
		Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
		##Create Message
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Contacts,$MailboxName)   
 		$ContactsFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
		#Move Contact to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$FolderName = "Test48980-" + (Get-Date).ToString()
		$NewFolder.DisplayName = $FolderName
		$NewFolder.FolderClass = "IPF.Contact"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,$NewFolder.DisplayName)  
		#Do the Search  
		$EntryIdVal = $null
		$data = "" | Select Folder1,Folder2
		$findFolderResults = $ContactsFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($ContactsFolder.Id)  
			Write-host ("Folder Created")  
			$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
			$psPropertySet.Add($PR_ENTRYID)
			$NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$NewFolder.Id,$psPropertySet)
			$NewFolder.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)
			$data.Folder1 = $EntryIdVal
			
		}  
		else{  
		    throw ("Folder already Exist with that Name")  
		
		} 	
		Invoke-P365MailboxCopy		
		$Folder  = Invoke-P365FindFolder -TargetMailbox -FolderId $EntryIdVal -ParentFolderPath \Contacts
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "48980"
		$TestResults.Description = "Folder Delete ReCreate"
		$TestResults.TestLastRun = (Get-Date)		
		if($folder -ne $Null){
			$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$MailboxName)   
 			$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
			$NewFolder = $NewFolder.Move($InboxFolder.Id)
			$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
			$NewFolder.DisplayName = $FolderName
			$NewFolder.FolderClass = "IPF.Contact"
			#Define Folder Veiw Really only want to return one object  
			$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
			#Define a Search folder that is going to do a search based on the DisplayName of the folder  
			$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,$NewFolder.DisplayName)  
			#Do the Search  
			$EntryIdVal = $null
			$findFolderResults = $ContactsFolder.FindFolders($SfSearchFilter,$fvFolderView)  
			if ($findFolderResults.TotalCount -eq 0){  
				Write-host ("Folder Doesn't Exist")  
				$NewFolder.Save($ContactsFolder.Id)  
				Write-host ("Folder Created")  
				$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
				$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
				$psPropertySet.Add($PR_ENTRYID)
				$NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$NewFolder.Id,$psPropertySet)
				$NewFolder.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)
				$data.Folder2 = $EntryIdVal
				
			}  
			else{  
				throw ("Folder already Exist with that Name")  
			
			} 
			$TestResults.Data = $data
			$TestResults.TestResult = "Succeeded"
		}
		else{
			$TestResults.TestResult = "Failed"
		}
		$Script:TestResults.OverAllResult = "InComplete"
		if($Script:TestResults.ContainsKey("Test48980")){
			$Script:TestResults["Test48980"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test48980",$TestResults)
		}
		
		Write-Host "Done" -ForegroundColor Green
     }
}