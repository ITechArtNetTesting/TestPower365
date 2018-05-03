function Invoke-Test39551{
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
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$Script:TargetMailbox)   
 		$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($Script:TargetService,$folderid)
 		$EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $Script:TargetService  
  		$EmailMessage.Subject = "Test39551 - " + (Get-Date).ToString()
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
		
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::ArchiveMsgFolderRoot,$Script:TargetMailbox)   
 		$ArchiveFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($Script:TargetService,$folderid)
		#Move EmailMessage to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($Script:TargetService)  
		$NewFolder.DisplayName = "Test39551"
		$NewFolder.FolderClass = "IPF.Note"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test39551")  
		#Do the Search  
		$findFolderResults = $ArchiveFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($ArchiveFolder.Id)  
			$NewFolder.Load()
			Write-host ("Folder Created")  
			$EmailMessage.Save($ArchiveFolder.Id)  
			$EmailMessage.Load()
			write-host ("Message Moved")
		}  
		else{  
		    Write-host ("Folder already Exist with that Name")  
			$EmailMessage.Save($findFolderResults.Folders[0].Id)
			$EmailMessage.Load()
		} 		
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "39551"
		$TestResults.Description = "Archive Rollback Message"
		$TestResults.TestLastRun = (Get-Date)
		$Script:TestResults.OverAllResult = "InComplete"
		
		if($EmailMessage -ne $null){
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $EmailMessage.Id
		}
		else{
			$TestResults.TestResult = "Failed"			
		}
		if($Script:TestResults.ContainsKey("Test39551")){
			$Script:TestResults["Test39551"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test39551",$TestResults)
		}		
		Write-Host "Done" -ForegroundColor Green
     }
}