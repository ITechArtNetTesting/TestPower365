function Invoke-Test39541{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		$service = $Script:TargetService
		$MailboxName = $Script:TargetMailbox
		Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
		##Create Message
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$MailboxName)   
		$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
		#Create Subfolder Folder and Item
		$FolderName = "Test39541-" + (Get-Date).ToString("s")
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$NewFolder.DisplayName = $FolderName
		$NewFolder.FolderClass = "IPF.Note"
		$NewFolder.Save($InboxFolder.Id)
		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$EmailMessage.Subject = "Test39541 - " + (Get-Date).ToString()
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
		$Data = "" | Select Message1,Message2,FolderPath
		$Data.FolderPath = "\Inbox\" + $FolderName
		$Data.Message1 = $EmailMessage.Id
		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$EmailMessage.Subject = "Test39541 - " + (Get-Date).ToString()
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
		$Data.Message2 = $EmailMessage.Id
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "39541"
		$TestResults.Description = "Rollback target item retention test"
		$TestResults.TestLastRun = (Get-Date)
		$Script:TestResults.OverAllResult = "InComplete"
		if($Data.Message1 -ne $Null){
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $Data
		}
		else{
			$TestResults.TestResult = "Failed"			
		}
		if($Script:TestResults.ContainsKey("Test39541")){
			$Script:TestResults["Test39541"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test39541",$TestResults)
		}
		
		Write-Host "Done" -ForegroundColor Green
     }
}