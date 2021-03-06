function Invoke-Test22623{
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
  		$EmailMessage.Subject = "Test22623 - " + (Get-Date).ToString()
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
		$HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-",""))
		Write-host "Part 1 - EmailMessage Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Inbox' -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide
		$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal 
		"Create New Email In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
	    if($TargetMessage -ne $null){			
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $EntryIdVal
			$EmailMessage.Delete([Microsoft.Exchange.WebServices.Data.DeleteMode]::HardDelete)
		}
		else{
			$TestResults.TestResult = "Failed"			
		}		
		$TestResults.TestCase = "22623"
		$TestResults.Description = "Email Delete"
		$TestResults.TestLastRun = (Get-Date)
		if($Script:TestResults.ContainsKey("Test22623")){
			$Script:TestResults["Test22623"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22623",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"
		Write-Host "Done" -ForegroundColor Green
     }
}