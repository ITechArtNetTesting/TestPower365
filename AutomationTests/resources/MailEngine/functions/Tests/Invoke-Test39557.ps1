function Invoke-Test39557 {
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
        $Root = Get-P365FolderFromPath -FolderPath \Inbox -SourceMailbox
        $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
        $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
        $psPropertySet.Add($PR_ENTRYID)
        $psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
	$EntryIdVal = $null
        $Root.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
        $Root.Load($psPropertySet)
        $PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Owner  
        $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($FirstSourcePermission, $PermissiontoAdd)  
        $Root.Permissions.Add($newfp)  
        $Root.Update()
        $data = "" | Select Folder, FirstTargetPermission
        $data.FirstTargetPermission = $FirstTargetPermission        
        $tfile = New-P365TranslationFile -SourceAddress $FirstSourcePermission -TargetAddress $FirstTargetPermission
           # Write-host "Part 1 - Message Created"
        Invoke-P365MailboxCopy	-mappingfile $tfile	
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "39557"
        $TestResults.Description = "Folder Permission"
        $TestResults.TestLastRun = (Get-Date)		
        if ($Root -ne $Null) {
             #test permissions
            $TestResults.Data = $data
            $TestResults.TestResult = "Succeeded"
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test39557")) {
            $Script:TestResults["Test39557"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test39557", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            Invoke-P365MailboxRollback
        }
        Write-Host "Done" -ForegroundColor Green
    }

}