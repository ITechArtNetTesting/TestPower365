function Invoke-Test22537{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=2, Mandatory=$true)] [String]$SourceAddress,
		[Parameter(Position=3, Mandatory=$true)] [String]$TargetAddress
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
  		$EmailMessage.Subject = "Test22537 - " + (Get-Date).ToString()
  		#Add Recipients    
  		$EmailMessage.ToRecipients.Add($SourceAddress)  
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
		$tfile = New-P365TranslationFile -SourceAddress $SourceAddress -TargetAddress $TargetAddress
		Write-host "Part 1 - Message Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile $tfile -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Inbox'
		$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "22537"
		$TestResults.Description = "Address Translation Test"
		$TestResults.TestLastRun = (Get-Date)
		$Script:TestResults.OverAllResult = "InComplete"
		if($TargetMessage -ne $null){
			$TargetMessage.Load()
			$tokay = $false
			foreach($rcp in $TargetMessage.ToRecipients){	
				if($rcp.Address.ToLower() -eq $TargetAddress.ToLower()){
					$tokay = $true
				}
			}
			if($tokay){
				$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
				$NewFolder.DisplayName = "Test22537"
				$NewFolder.FolderClass = "IPF.Note"
				#Define Folder Veiw Really only want to return one object  
				$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
				#Define a Search folder that is going to do a search based on the DisplayName of the folder  
				$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test22537")  
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
				$TestResults.TestResult = "Succeeded"
				$TestResults.Data = "" | Select Id,TargetAddress
				$TestResults.Data.Id = $EntryIdVal
				$TestResults.Data.TargetAddress = $TargetAddress
				Remove-Item -Path $tfile
			}
			else{
				$TestResults.TestResult = "Failed"
			}

		}
		else{
			$TestResults.TestResult = "Failed"			
		}
		if($Script:TestResults.ContainsKey("Test22537")){
			$Script:TestResults["Test22537"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22537",$TestResults)
		}
		
		Write-Host "Done" -ForegroundColor Green
     }
}