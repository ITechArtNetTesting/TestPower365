function Invoke-Test22490{
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
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$MailboxName)   
 		$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
 		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$EmailMessage.Subject = "Test22384 - " + (Get-Date).ToString()
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
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$EmailMessage.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$EmailMessage.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.OrginalId = $EntryIdVal
        $HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-",""))
		Write-host "Part 1 - EmailMessage Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Inbox' -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide
		$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal 
		#Move EmailMessage to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$NewFolder.DisplayName = "Test22490"
		$NewFolder.FolderClass = "IPF.Note"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test22490")  
		#Do the Search  
		$findFolderResults = $InboxFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($InboxFolder.Id)  
			Write-host ("Folder Created")  
			$EmailMessage = $EmailMessage.Move($NewFolder.Id)
		}  
		else{  
		    Write-host ("Folder already Exist with that Name")  
			$EmailMessage= $EmailMessage.Move($findFolderResults.Folders[0].Id)
			
		} 		
		$EmailMessage.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$EmailMessage.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.MovedId = $EntryIdVal
		Write-host("Moved EmailMessage")
 		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$EmailMessage.Subject = "22490 - " + (Get-Date).ToString()
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
		$EmailMessage.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$EmailMessage.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.NewId = $EntryIdVal
		"Create New Email In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "22490"
		$TestResults.Description = "Email Move Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.Data = $Data
		$TestResults.TestResult = "Succeeded"
		if($Script:TestResults.ContainsKey("Test22490")){
			$Script:TestResults["Test22490"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22490",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"
		Write-Host "Done" -ForegroundColor Green
     }
}