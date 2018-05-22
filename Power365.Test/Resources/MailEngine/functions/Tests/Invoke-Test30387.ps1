function Invoke-Test30387 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 6, Mandatory = $false)][switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath
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
        $FolderName = "Test30387-" + (Get-Date).ToString("s")
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
        }  
        else {  
            throw ("Folder already Exist with that Name")  
		
        } 	
        
        # Write-host "Part 1 - Message Created"
        Invoke-p365SyncPublicFolder -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
	    Invoke-p365CopyPublicFolder -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
        # Invoke-P365MailboxCopy	-mappingfile $tfile	
        $Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ("\" + $RootPath + "\" + $FolderName)
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30387"
        $TestResults.Description = "Fill Data"
        $TestResults.TestLastRun = (Get-Date)		
        if ($folder -ne $Null) {
             #test permissions
            $TestResults.Data = ($RootPath + "\" + $FolderName) 
            $TestResults.TestResult = "Succeeded"
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30387")) {
            $Script:TestResults["Test30387"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30387", $TestResults)
        }
        Invoke-P365FillPublicFolder -Folder $NewFolder
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365CopyPublicFolder -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
        }
        Write-Host "Done" -ForegroundColor Green
    }

}