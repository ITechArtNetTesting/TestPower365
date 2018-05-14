function Invoke-Test22625{
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
		$HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-","")) 
		Write-host "Part 1 - Appointment Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Calendar' -SourceOnPremise $Script:SourceOnPrem -TargetOnPremise $Script:TargetOnPrem -SourceAutoDiscoverOverRide $Script:SourceAutoDiscoverOverRide -TargetAutoDiscoverOverRide $Script:TargetAutoDiscoverOverRide
		$TargetMessage = Invoke-P365FindAppointment -TargetMailbox -MessageId $EntryIdVal 
		"Create New Appointment In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
	    if($TargetMessage -ne $null){			
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $EntryIdVal
			$Appointment.Delete([Microsoft.Exchange.WebServices.Data.DeleteMode]::HardDelete)
		}
		else{
			$TestResults.TestResult = "Failed"			
		}		
		$TestResults.TestCase = "22625"
		$TestResults.Description = "Appointment Delete"
		$TestResults.TestLastRun = (Get-Date)
		if($Script:TestResults.ContainsKey("Test22625")){
			$Script:TestResults["Test22625"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22625",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"
		Write-Host "Done" -ForegroundColor Green
     }
}