function Invoke-Test30387b {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
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
        $FolderName = "Test30387b-" + (Get-Date).ToString("s")
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
            $NewFolder.Save($pfRoot.Id)  
            Invoke-P365FillPublicFolder -Folder $NewFolder
             $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
             $NewFolder1.DisplayName = $FolderName
             $NewFolder1.FolderClass = "IPF.Note"
             $NewFolder1.Save($NewFolder.Id)  
             Invoke-P365FillPublicFolder -Folder $NewFolder1
        }  
        else {  
            throw ("Folder already Exist with that Name")  
		
        } 	
        
        # Write-host "Part 1 - Message Created"
        Invoke-p365SyncPublicFolder -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
	    Invoke-p365CopyPublicFolder -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        # Invoke-P365MailboxCopy	-mappingfile $tfile	
        $Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($RootPath + "\" + $FolderName)
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30387b"
        $TestResults.Description = "Fill Data"
        $TestResults.TestLastRun = (Get-Date)		
        if ($folder -ne $Null) {
             #test permissions
            $data.Folder1 = ($RootPath + "\" + $FolderName) 
            $data.Folder2 = ($data.Folder1 + "\" + $FolderName) 
            $TestResults.Data = $data 
            $TestResults.TestResult = "Succeeded"
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30387b")) {
            $Script:TestResults["Test30387b"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30387b", $TestResults)
        }
        Invoke-P365FillPublicFolder -Folder $NewFolder
        Invoke-P365FillPublicFolder -Folder $NewFolder1
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365CopyPublicFolder -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
        Write-Host "Done" -ForegroundColor Green
    }

}