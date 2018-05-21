function Invoke-Test30077 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 2, Mandatory = $true)] [String]$FirstSourcePermission,
        [Parameter(Position = 3, Mandatory = $true)] [String]$SecondSourcePermission,
        [Parameter(Position = 4, Mandatory = $true)] [String]$FirstTargetPermission,
        [Parameter(Position = 5, Mandatory = $true)] [String]$SecondTargetPermission,
        [Parameter(Position = 6, Mandatory = $false)][switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath,
		[Parameter(Mandatory = $true)][String]$TargetRootPath
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
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30077-" + (Get-Date).ToString("s")
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
            $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($FirstSourcePermission, $PermissiontoAdd)  
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
            $data.Folder1 = ($RootPath + "\" + $FolderName)
            $data.SourceId = $NewFolder.Id
            $data.FirstTargetPermission = $FirstTargetPermission
            $data.SecondTargetPermission = $SecondTargetPermission
              $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName1 = "Test30077-Child" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"			
            $NewFolder1.Save($NewFolder.Id)
            $NewFolder1.Load($psPropertySet)
            $PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Owner  
            $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($Script:SourceMailbox, $PermissiontoAdd)  
            $NewFolder1.Permissions.Clear()
            $NewFolder1.Permissions.Add($newfp)   	    
            $NewFolder1.Update()
            $data.Folder2 = ($RootPath + "\" + $FolderName + "\" + $FolderName1)
            #clear perms
        }  
        else {  
            throw ("Folder already Exist with that Name")  
		
        } 	
        $tfile = New-P365TranslationFile -SourceAddress $FirstSourcePermission -TargetAddress $FirstTargetPermission
        New-P365TranslationFile -SourceAddress $SecondSourcePermission -TargetAddress $SecondTargetPermission -FileName $tfile
        # Write-host "Part 1 - Message Created"
        Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)

        # Invoke-P365MailboxCopy	-mappingfile $tfile	
        $Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($RootPath + "\" + $FolderName)
        $NewFolder1.Load($psPropertySet)
        $PermissiontoAdd = [Microsoft.Exchange.WebServices.Data.FolderPermissionLevel]::Owner  
        $newfp = new-object Microsoft.Exchange.WebServices.Data.FolderPermission($SecondSourcePermission, $PermissiontoAdd)  
        $NewFolder1.Permissions.Add($newfp)  
        $NewFolder1.Update()
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30077"
        $TestResults.Description = "Folder Permission"
        $TestResults.TestLastRun = (Get-Date)		
        if ($folder -ne $Null) {
            $TestResults.TestResult = "Failed"
            #test permissions
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
            $psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
            $folder.Load($psPropertySet)            
            foreach ($fperm in $folder.Permissions) {  
                if ($fperm.UserId.PrimarySmtpAddress -ne $null) {  
                    write-host ($fperm.UserId.PrimarySmtpAddress)
                    if ($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $FirstTargetPermission.ToLower()) {
                        $TestResults.Data = $data
                        $TestResults.TestResult = "Succeeded"
                    }  
                }  
            }
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30077")) {
            $Script:TestResults["Test30077"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30077", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            $tfile = New-P365TranslationFile -SourceAddress $FirstSourcePermission -TargetAddress $FirstTargetPermission
            New-P365TranslationFile -SourceAddress $SecondSourcePermission -TargetAddress $SecondTargetPermission -FileName $tfile
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
        Write-Host "Done" -ForegroundColor Green
    }

}