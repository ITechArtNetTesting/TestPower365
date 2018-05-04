function Invoke-Test39545{
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
  
		$PidTagInternetMessageId = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(4149,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::String);
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$Script:TargetMailbox)   
		$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($Script:TargetService,$folderid)
		#Create Subfolder Folder and Item
		$FolderName = "Test33953-" + (Get-Date).ToString("s")
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($Script:TargetService)  
		$NewFolder.DisplayName = $FolderName
		$NewFolder.FolderClass = "IPF.Note"
		$NewFolder.Save($InboxFolder.Id)
		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $Script:TargetService
  		$EmailMessage.Subject = "Test33953 - " + (Get-Date).ToString()
  		#Add Recipients    
  		$EmailMessage.ToRecipients.Add($Script:TargetMailbox)  
  		$EmailMessage.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
  		$EmailMessage.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
  		$EmailMessage.Body.Text = "Body" 
  		$EmailMessage.From = $Script:TargetMailbox
		#Set Sent Message Flags which means message wont appear as a Draft  
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$PR_Flags = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(3591, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer);  
		$EmailMessage.SetExtendedProperty($PR_Flags,"1")
		$EmailMessage.SetExtendedProperty($PidTagInternetMessageId,($FolderName + "@blhblh.com"))
		$EmailMessage.IsRead = $false  
		$EmailMessage.Save($NewFolder.Id)
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$EmailMessage.Load($psPropertySet)
		$EmailMessage.Reply("test",$true)
		sleep -Seconds 5
		$TargetMessage = Invoke-P365FindMessageReply -TargetMailbox -MessageId ($FolderName + "@blhblh.com")  -FolderPath ("\Inbox") 		
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "39545"
		$TestResults.Description = "Rollback reply remove test"
		$TestResults.TestLastRun = (Get-Date)
		if($TargetMessage -ne $Null){
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = ($FolderName + "@blhblh.com")
		}
		else{
			$TestResults.TestResult = "Failed"			
		}
		if($Script:TestResults.ContainsKey("Test39545")){
			$Script:TestResults["Test39545"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test39545",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"
		Write-Host "Done" -ForegroundColor Green
     }
}