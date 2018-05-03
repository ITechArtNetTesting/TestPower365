function Invoke-Test22497{
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
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Calendar,$MailboxName)   
 		$CalendarFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
		$Appointment = New-Object Microsoft.Exchange.WebServices.Data.Appointment -ArgumentList $service
		$Appointment.Subject = "Test22497-" + (Get-Date).ToString()
		$Appointment.Start = (Get-Date)
		$Appointment.End = (Get-Date).AddHours(1)
		$Appointment.Save($CalendarFolder.Id)
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$Appointment.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Appointment.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.OrginalId = $EntryIdVal
        $HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-",""))
		Write-host "Part 1 - Appointment Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Calendar' -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide
		$TargetMessage = Invoke-P365FindAppointment -TargetMailbox -MessageId $EntryIdVal 
		#Move Contact to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$NewFolder.DisplayName = "Test22497"
		$NewFolder.FolderClass = "IPF.Appointment"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test22497")  
		#Do the Search  
		$findFolderResults = $CalendarFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($CalendarFolder.Id)  
			Write-host ("Folder Created")  
			$Appointment = $Appointment.Move($NewFolder.Id)
		}  
		else{  
		    Write-host ("Folder already Exist with that Name")  
			$Appointment= $Appointment.Move($findFolderResults.Folders[0].Id)
			
		} 		
		$Appointment.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Appointment.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.MovedId = $EntryIdVal
		Write-host("Moved Appointment")
		$Appointment = New-Object Microsoft.Exchange.WebServices.Data.Appointment -ArgumentList $service
		$Appointment.Subject = "Test22497-" + (Get-Date).ToString()
		$Appointment.Start = (Get-Date)
		$Appointment.End = (Get-Date).AddHours(1)
		$Appointment.Save($CalendarFolder.Id)
		$Appointment.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Appointment.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.NewId = $EntryIdVal
		"Create New Appointment In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "22497"
		$TestResults.Description = "Appointment Move Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.Data = $Data
		$TestResults.TestResult = "Succeeded"
		$Script:TestResults.OverAllResult = "InComplete"
		if($Script:TestResults.ContainsKey("Test22497")){
			$Script:TestResults["Test22497"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22497",$TestResults)
		}		
		Write-Host "Done" -ForegroundColor Green
     }
}