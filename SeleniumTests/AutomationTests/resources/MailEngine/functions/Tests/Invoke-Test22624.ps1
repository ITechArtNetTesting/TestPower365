function Invoke-Test22624{
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
		$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Contacts,$MailboxName)   
 		$ContactsFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
		$Contact = New-Object Microsoft.Exchange.WebServices.Data.Contact -ArgumentList $service
		#Set the GivenName
		$Contact.GivenName = "TestFirstName"
		#Set the LastName
		$Contact.Surname = "TestLastName"
		#Set Subject  
		$Contact.Subject = "TestFirstName TestLastName"
		$Contact.FileAs = "TestFirstName TestLastName"
		$Contact.CompanyName = "Test Company"
		$Contact.DisplayName = "TestFirstName TestLastName"
		$Contact.Department = "Test Department"
		$Contact.OfficeLocation = "Test Office"
		$Contact.PhoneNumbers[[Microsoft.Exchange.WebServices.Data.PhoneNumberKey]::BusinessPhone] = 1111111
		$Contact.PhysicalAddresses[[Microsoft.Exchange.WebServices.Data.PhysicalAddressKey]::Business] = New-Object  Microsoft.Exchange.WebServices.Data.PhysicalAddressEntry
		$Contact.PhysicalAddresses[[Microsoft.Exchange.WebServices.Data.PhysicalAddressKey]::Business].Street = "Test Stret"
		$Contact.PhysicalAddresses[[Microsoft.Exchange.WebServices.Data.PhysicalAddressKey]::Business].State = "Test State"
		$Contact.PhysicalAddresses[[Microsoft.Exchange.WebServices.Data.PhysicalAddressKey]::Business].City = "LA"
		$Contact.PhysicalAddresses[[Microsoft.Exchange.WebServices.Data.PhysicalAddressKey]::Business].CountryOrRegion = "US"
		$Contact.PhysicalAddresses[[Microsoft.Exchange.WebServices.Data.PhysicalAddressKey]::Business].PostalCode = 2222
		$Contact.EmailAddresses[[Microsoft.Exchange.WebServices.Data.EmailAddressKey]::EmailAddress1] = "TestAddressFake234@binaryTree.com"
		$Contact.BusinessHomePage = "test.com.au"
		$Contact.Body = "SomeNotes"
		$Contact.JobTitle = "Test Job"
		$Contact.Save($ContactsFolder.Id)
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$Contact.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Contact.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal) 
		$HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-","")) 
		Write-host "Part 1 - Contact Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Contacts'
		$TargetMessage = Invoke-P365FindContact -TargetMailbox -MessageId $EntryIdVal 
		"Create New Contact In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
	    if($TargetMessage -ne $null){			
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $EntryIdVal
			$Contact.Delete([Microsoft.Exchange.WebServices.Data.DeleteMode]::HardDelete)
		}
		else{
			$TestResults.TestResult = "Failed"			
		}		
		$TestResults.TestCase = "22624"
		$TestResults.Description = "Contact Delete"
		$TestResults.TestLastRun = (Get-Date)
		if($Script:TestResults.ContainsKey("Test22624")){
			$Script:TestResults["Test22624"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22624",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"
		Write-Host "Done" -ForegroundColor Green
     }
}