function Invoke-Test34507 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 2, Mandatory = $true)] [String]$FirstSourcePermission,
        [Parameter(Position = 3, Mandatory = $false)] [String]$SecondSourcePermission,
        [Parameter(Position = 4, Mandatory = $true)] [String]$FirstTargetPermission,
        [Parameter(Position = 5, Mandatory = $false)] [String]$SecondTargetPermission,
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
        $Root = Get-P365FolderFromPath -FolderPath \Calendar -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.CalendarFolder($service)  
        $FolderName = "Test34507-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        #$NewFolder.FolderClass = "IPF.Task"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder, FirstTargetPermission
        $findFolderResults = $Root.FindFolders($SfSearchFilter, $fvFolderView)  
        if ($findFolderResults.TotalCount -eq 0) {  
            Write-host ("Folder Doesn't Exist") 
            $PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Owner  
            $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($Script:SourceMailbox, $PermissiontoAdd)  
            $NewFolder.Permissions.Add($newfp)  
            $NewFolder.Save($Root.Id)  
            $NewFolder.Load()
            Write-host ("Folder Created")
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
            $NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder.Id, $psPropertySet)
            $NewFolder.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            $data.FirstTargetPermission = $FirstTargetPermission
            $NewFolder.Load($psPropertySet)
            $PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Owner  
            $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($FirstSourcePermission, $PermissiontoAdd)  
            $NewFolder.Permissions.Add($newfp)  
            $NewFolder.Update()
            $data.Folder = "\Calendar\" + $FolderName
            #clear perms
        }  
        else {  
            throw ("Folder already Exist with that Name")  
		
        } 	
       $tfile = New-P365TranslationFile -SourceAddress $FirstSourcePermission -TargetAddress $FirstTargetPermission
       # New-P365TranslationFile -SourceAddress $SecondSourcePermission -TargetAddress $SecondTargetPermission -FileName $tfile
        
        # Write-host "Part 1 - Message Created"
       # Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\\Automation\tests\" + $FolderName) -TargetCopyPath "\\Automation\tests"

        Invoke-P365MailboxCopy	-mappingfile $tfile	
       # $Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ("\Automation\tests\" + $FolderName)
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "34507"
        $TestResults.Description = "Folder Permission"
        $TestResults.TestLastRun = (Get-Date)		
        if ($NewFolder -ne $Null) {
             #test permissions
            $TestResults.Data = $data
            $TestResults.TestResult = "Succeeded"
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test34507")) {
            $Script:TestResults["Test34507"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test34507", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            $tfile = New-P365TranslationFile -SourceAddress $FirstSourcePermission -TargetAddress $FirstTargetPermission
            # Write-host "Part 1 - Message Created"
             Invoke-P365MailboxCopy	-mappingfile $tfile	
        }
        Write-Host "Done" -ForegroundColor Green
    }

}