function Invoke-Test22491{
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
		$Data.OrginalId = $EntryIdVal
        $HexMessageId = ([System.BitConverter]::ToString($EntryIdVal).Replace("-",""))
		Write-host "Part 1 - Contact Created"
		Copy-T2TMailboxItem -SourceMailbox $Script:SourceMailbox -SourceCredentials $Script:SourcePSCreds -TargetMailbox $Script:TargetMailbox -TargetCredentials $Script:TargetPSCreds -mappingfile ($script:ModuleRoot + '\engine\mapping.csv') -ProcessingPath c:\temp -SkipFolderRetentionTags -CopyDumpster:$true -Delta:$true -MessageId $HexMessageId -FolderPath '\Contacts'
		$TargetMessage = Invoke-P365FindContact -TargetMailbox -MessageId $EntryIdVal 
		#Move Contact to New folder
		$NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
		$NewFolder.DisplayName = "Test22491"
		$NewFolder.FolderClass = "IPF.Contact"
		#Define Folder Veiw Really only want to return one object  
		$fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
		#Define a Search folder that is going to do a search based on the DisplayName of the folder  
		$SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,"Test22491")  
		#Do the Search  
		$findFolderResults = $ContactsFolder.FindFolders($SfSearchFilter,$fvFolderView)  
		if ($findFolderResults.TotalCount -eq 0){  
		    Write-host ("Folder Doesn't Exist")  
			$NewFolder.Save($ContactsFolder.Id)  
			Write-host ("Folder Created")  
			$Contact = $Contact.Move($NewFolder.Id)
		}  
		else{  
		    Write-host ("Folder already Exist with that Name")  
			$Contact= $Contact.Move($findFolderResults.Folders[0].Id)
			
		} 		
		$Contact.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Contact.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.MovedId = $EntryIdVal
		Write-host("Moved Contact")
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
		$Contact.Load($psPropertySet)
		$EntryIdVal = $null		
		[Void]$Contact.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
		$Data.NewId = $EntryIdVal
		"Create New Contact In place"
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "22491"
		$TestResults.Description = "Contact Move Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.Data = $Data
		$TestResults.TestResult = "Succeeded"
		if($Script:TestResults.ContainsKey("Test22491")){
			$Script:TestResults["Test22491"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test22491",$TestResults)
		}
		$Script:TestResults.OverAllResult = "InComplete"
		Write-Host "Done" -ForegroundColor Green
     }
}