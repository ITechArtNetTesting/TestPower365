function Invoke-Test22504{
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
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Tasks,$MailboxName)   
 		$TasksFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
		$Task = New-Object Microsoft.Exchange.WebServices.Data.Task -ArgumentList $service
		$Task.Subject = "Test22504-" + (Get-Date).ToString()
		$Task.StartDate = (Get-Date)
		$Task.DueDate = (Get-Date).AddDays(1)
		$Task.Save($TasksFolder.Id)
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$Task.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Task.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.OrginalId = $EntryIdVal
        $HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-",""))
		Write-host "Part 1 - Task Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Tasks' -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide
		$TargetMessage = Invoke-P365FindTask -TargetMailbox -MessageId $EntryIdVal 
		#Move Contact to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$NewFolder.DisplayName = "Test22504"
		$NewFolder.FolderClass = "IPF.Task"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test22504")  
		#Do the Search  
		$findFolderResults = $TasksFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($TasksFolder.Id)  
			Write-host ("Folder Created")  
			$Task = $Task.Move($NewFolder.Id)
		}  
		else{  
		    Write-host ("Folder already Exist with that Name")  
			$Task= $Task.Move($findFolderResults.Folders[0].Id)
			
		} 		
		$Task.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Task.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.MovedId = $EntryIdVal
		Write-host("Moved Task")
		$Task = New-Object Microsoft.Exchange.WebServices.Data.Task -ArgumentList $service
		$Task.Subject = "Test22504-" + (Get-Date).ToString()
		$Task.StartDate = (Get-Date)
		$Task.DueDate = (Get-Date).AddDays(1)
		$Task.Save($TasksFolder.Id)
		$Task.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Task.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.NewId = $EntryIdVal
		"Create New Task In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "22504"
		$TestResults.Description = "Task Move Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.Data = $Data
		$TestResults.TestResult = "Succeeded"
		$Script:TestResults.OverAllResult = "InComplete"
		if($Script:TestResults.ContainsKey("Test22504")){
			$Script:TestResults["Test22504"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22504",$TestResults)
		}		
		Write-Host "Done" -ForegroundColor Green
     }
}