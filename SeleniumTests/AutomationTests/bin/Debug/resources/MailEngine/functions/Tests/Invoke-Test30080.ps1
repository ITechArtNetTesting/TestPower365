function Invoke-Test30080 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 2, Mandatory = $true)] [String]$FirstSourcePermission,
        [Parameter(Position = 3, Mandatory = $true)] [String]$SecondSourcePermission,
        [Parameter(Position = 4, Mandatory = $true)] [String]$FirstTargetPermission,
        [Parameter(Position = 5, Mandatory = $true)] [String]$SecondTargetPermission,
        [Parameter(Position = 6, Mandatory = $false)][switch]$RunDelta
    )  
    Begin {
        $Data = "" | Select OrginalId, MovedId, NewId
        if ($TargetMailbox.IsPresent) {
            $service = $Script:TargetService
            $MailboxName = $Script:TargetMailbox
        }
        else {
            $service = $Script:SourceService
            $MailboxName = $Script:SourceMailbox
        }
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath \Automation\Tests -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30080-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder1, Folder2, SourceId, FirstTargetPermission, SecondTargetPermission
        $findFolderResults = $pfRoot.FindFolders($SfSearchFilter, $fvFolderView)  
        if ($findFolderResults.TotalCount -eq 0) {  
            Write-host ("Folder Doesn't Exist") 
            $PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Reviewer  
            $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission 
            $newfp.UserId.PrimarySmtpAddress = $FirstSourcePermission 
            $newfp.CanCreateItems = $true
            $newfp.DeleteItems = [Microsoft.Exchange.WebServices.Data.PermissionScope]::All;
            $newfp.ReadItems = [Microsoft.Exchange.WebServices.Data.FolderPermissionReadAccess]::FullDetails;
            $newfp.EditItems = [Microsoft.Exchange.WebServices.Data.PermissionScope]::Owned
            $NewFolder.Permissions.Add($newfp)  
            $NewFolder.Save($pfRoot.Id)  
            $NewFolder.Load()
            Write-host ("Folder Created")
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
            $NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder.Id, $psPropertySet)
            $NewFolder.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            $data.Folder1 = ("\Automation\tests\" + $FolderName)
            $data.SourceId = $NewFolder.Id
            $data.FirstTargetPermission = $FirstTargetPermission
            $data.SecondTargetPermission = $SecondTargetPermission
            $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName1 = "Test30080-Child" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"			
            $NewFolder1.Save($NewFolder.Id)
            $NewFolder1.Load($psPropertySet)
            $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission 
            $newfp.UserId.PrimarySmtpAddress = $SecondSourcePermission 
            $newfp.CanCreateItems = $true
            $newfp.DeleteItems = [Microsoft.Exchange.WebServices.Data.PermissionScope]::All;
            $newfp.ReadItems = [Microsoft.Exchange.WebServices.Data.FolderPermissionReadAccess]::FullDetails;
            $newfp.EditItems = [Microsoft.Exchange.WebServices.Data.PermissionScope]::Owned
            $NewFolder1.Permissions.Add($newfp)   	    
            $NewFolder1.Update()
            $data.Folder2 = ("\Automation\tests\" + $FolderName + "\" + $FolderName1)
            #clear perms
        }  
        else {  
            throw ("Folder already Exist with that Name")  
		
        } 	
       #$tfile = New-P365TranslationFile -SourceAddress $FirstSourcePermission -TargetAddress $FirstTargetPermission
        #New-P365TranslationFile -SourceAddress $SecondSourcePermission -TargetAddress $SecondTargetPermission -FileName $tfile
        
        # Write-host "Part 1 - Message Created"
        #Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\\Automation\tests\" + $FolderName) -TargetCopyPath "\\Automation\tests"

        # Invoke-P365MailboxCopy	-mappingfile $tfile	
        #$Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ("\Automation\tests\" + $FolderName)
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30080"
        $TestResults.Description = "Folder Permission"
        $TestResults.TestLastRun = (Get-Date)		
        if ($NewFolder1 -ne $Null) {
             #test permissions
            $TestResults.Data = $data
            $TestResults.TestResult = "Succeeded"
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30080")) {
            $Script:TestResults["Test30080"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30080", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            $tfile = New-P365TranslationFile -SourceAddress $FirstSourcePermission -TargetAddress $FirstTargetPermission
            New-P365TranslationFile -SourceAddress $SecondSourcePermission -TargetAddress $SecondTargetPermission -FileName $tfile
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\\Automation\tests\" + $FolderName) -TargetCopyPath "\\Automation\tests"
        }
        Write-Host "Done" -ForegroundColor Green
    }

}