function Invoke-Test32348 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 2, Mandatory = $true)] [String]$Attachment,
        [Parameter(Position = 4, Mandatory = $false)] [switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath,
		[Parameter(Mandatory = $true)][String]$TargetRootPath
    )  
    Begin {
        if ($TargetMailbox.IsPresent) {
            $service = $Script:TargetService
            $MailboxName = $Script:TargetMailbox
        }
        else {
            $service = $Script:SourceService
            $MailboxName = $Script:SourceMailbox
        }
		
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "32348"
        $TestResults.Description = "Public Folder StickyNote"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.ContactsFolder($service)  
        $FolderName = "Test32348-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder,MessageId,FileName
        
        $findFolderResults = $pfRoot.FindFolders($SfSearchFilter, $fvFolderView)  
        if ($findFolderResults.TotalCount -eq 0) {  
            Write-host ("Folder Doesn't Exist") 
            $NewFolder.Save($pfRoot.Id)  
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder.Id, $psPropertySet)
            $NewFolder.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.ContactsFolder($service)  
            $FolderName1 = "Test32348-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $data.Folder =  $RootPath + $FolderName + "\" + $FolderName1
            $NewFolder1.Save($NewFolder.Id)
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
            $Contact.Save($NewFolder1.Id)
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $Contact.Load($psPropertySet)
            $EntryIdVal = $null		
            [Void]$Contact.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
            $data.MessageId = $EntryIdVal
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
	        $data.FileName = [System.IO.Path]::GetFileName($Attachment)
            $Contact.Attachments.AddFileAttachment($Attachment)
            $Contact.Update([Microsoft.Exchange.WebServices.Data.ConflictResolutionMode]::AlwaysOverwrite)
            $TestResults.Data = $data               
            $TestResults.TestResult = "Succeeded"
		
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test32348")) {
            $Script:TestResults["Test32348"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test32348", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
		
    }
}