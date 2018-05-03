function Invoke-Test22513{
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
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Journal,$MailboxName)   
 		$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
 		$Journal = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$Journal.Subject = "Test22513 - " + (Get-Date).ToString()
  		#Add Recipients    
  		$Journal.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
  		$Journal.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
		$Journal.Body.Text = "Body" 
		$Journal.ItemClass = "IPM.Activity"
		#Set Sent Message Flags which means message wont appear as a Draft  
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$PR_Flags = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(3591, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer);  
		$Journal.IsRead = $false  
		$Journal.Save($InboxFolder.Id)  
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$Journal.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Journal.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.OrginalId = $EntryIdVal
        $HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-",""))
		Write-host "Part 1 - Journal Item Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Journal'
		$TargetMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $EntryIdVal 
		#Move EmailMessage to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$NewFolder.DisplayName = "Test22513"
		$NewFolder.FolderClass = "IPF.Journal"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test22513")  
		#Do the Search  
		$findFolderResults = $InboxFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($InboxFolder.Id)  
			Write-host ("Folder Created")  
			$Journal = $Journal.Move($NewFolder.Id)
		}  
		else{  
		    Write-host ("Folder already Exist with that Name")  
			$Journal= $Journal.Move($findFolderResults.Folders[0].Id)
			
		} 		
		$Journal.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Journal.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.MovedId = $EntryIdVal
		Write-host("Moved Journal")
 		$Journal = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
  		$Journal.Subject = "Test22513 - " + (Get-Date).ToString()
  		#Add Recipients    
  		$Journal.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
  		$Journal.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
		$Journal.Body.Text = "Body" 
		$Journal.ItemClass = "IPM.Activity"
		#Set Sent Message Flags which means message wont appear as a Draft  
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$PR_Flags = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(3591, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer);  
		$Journal.SetExtendedProperty($PR_Flags,"1")
		$Journal.Save($InboxFolder.Id)  
		$Journal.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Journal.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.NewId = $EntryIdVal
		"Create New Journal In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "22513"
		$TestResults.Description = "Journal Move Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.Data = $Data
		$TestResults.TestResult = "Succeeded"
		if($Script:TestResults.ContainsKey("Test22513")){
			$Script:TestResults["Test22513"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22513",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"
		Write-Host "Done" -ForegroundColor Green
     }
}