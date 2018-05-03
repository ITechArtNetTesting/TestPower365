function Invoke-Test39550{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
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
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$MailboxName)   
 		$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
 		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$EmailMessage.Subject = "Test39550 - " + (Get-Date).ToString()
  		#Add Recipients    
  		$EmailMessage.ToRecipients.Add("Test@fakeAddress.BinaryTree.com")  
  		$EmailMessage.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
  		$EmailMessage.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
  		$EmailMessage.Body.Text = "Body" 
  		$EmailMessage.From = $MailboxName
		#Set Sent Message Flags which means message wont appear as a Draft  
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$PR_Flags = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(3591, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer);  
		$EmailMessage.SetExtendedProperty($PR_Flags,"1")
		$EmailMessage.IsRead = $false  
		$EmailMessage.Save($InboxFolder.Id)  
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$EmailMessage.Load($psPropertySet)
		$EntryIdVal = $null
		[Void]$EmailMessage.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
        $HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-",""))
		Write-host "Part 1 - Message Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Inbox' -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide
		$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::ArchiveMsgFolderRoot,$Script:TargetMailbox)   
 		$ArchiveFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($Script:TargetService,$folderid)
		#Move EmailMessage to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($Script:TargetService)  
		$NewFolder.DisplayName = "Test39550"
		$NewFolder.FolderClass = "IPF.Note"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test39550")  
		#Do the Search  
		$findFolderResults = $ArchiveFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($ArchiveFolder.Id)  
			$NewFolder.Load()
			Write-host ("Folder Created")  
			$TargetMessage = $TargetMessage.Move($NewFolder.Id)
		}  
		else{  
		    Write-host ("Folder already Exist with that Name")  
			$TargetMessage= $TargetMessage.Move($findFolderResults.Folders[0].Id)
			
		} 		
		$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal -Archive -FolderPath "\Test39550"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "39550"
		$TestResults.Description = "Archive Rollback Message Move"
		$TestResults.TestLastRun = (Get-Date)
		$Script:TestResults.OverAllResult = "InComplete"
		if($TargetMessage -ne $null){
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $EntryIdVal
		}
		else{
			$TestResults.TestResult = "Failed"			
		}
		if($Script:TestResults.ContainsKey("Test39550")){
			$Script:TestResults["Test39550"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test39550",$TestResults)
		}		
		Write-Host "Done" -ForegroundColor Green
     }
}