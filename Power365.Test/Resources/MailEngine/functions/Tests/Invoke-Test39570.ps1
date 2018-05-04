function Invoke-Test39570{
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
  		$EmailMessage.Subject = "Test39570 - " + (Get-Date).ToString()
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
		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$EmailMessage.Subject = "Test39570 - " + (Get-Date).ToString()
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
		$EntryIdVal2 = $null
		[Void]$EmailMessage.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal2)  
        
		Write-host "Part 1 - Message Created"
		Invoke-P365MailboxCopy
		$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal
		$TargetMessage2 = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal2
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "39570"
		$TestResults.Description = "Rollback delete Item"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.Data = "" | Select Message1,Message2
		if($TargetMessage -ne $null -band $TargetMessage2 -ne $null){
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data.Message1 = $EntryIdVal
			$TestResults.Data.Message2 = $EntryIdVal2
		}
		else{
			$TestResults.TestResult = "Failed"			
		}
		if($Script:TestResults.ContainsKey("Test39570")){
			$Script:TestResults["Test39570"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test39570",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"		
		$EmailMessage.Delete([Microsoft.Exchange.WebServices.Data.DeleteMode]::HardDelete)
		Write-Host "Done" -ForegroundColor Green
     }
}