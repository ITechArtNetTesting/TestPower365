function Invoke-Validate30387b {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox
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
        if ($Script:TestResults.ContainsKey("Test30387b")) {
            $TargetFolder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30387b"].Data.Folder1)
            $SourceFolder = Get-P365PublicFolderFromPath -SourceMailbox -FolderPath ($Script:TestResults["Test30387b"].Data.Folder1)
            $Okay = Invoke-P365ValidateFolderItems -SourceFolder $SourceFolder -TargetFolder $TargetFolder
            $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
            $Script:TestResults["Test30387b"].ValidationResult = "Failed"	
            $Script:TestResults["Test30387b"].OverAllResult = "Failed"	
            $Script:TestResults["Test30387b"].ValidationLastRun = (Get-Date)	
            if ($Okay) {
                    $TargetFolder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30387b"].Data.Folder2)
                    $SourceFolder = Get-P365PublicFolderFromPath -SourceMailbox -FolderPath ($Script:TestResults["Test30387b"].Data.Folder2)
                    $Okay = Invoke-P365ValidateFolderItems -SourceFolder $SourceFolder -TargetFolder $TargetFolder
                    if($Okay){
                        $Script:TestResults["Test30387b"].OverAllResult = "Successful"	
                        $Script:TestResults["Test30387b"].ValidationResult = "Succeeded"
                    }

            }
            Write-Host "Done" -ForegroundColor Green
        }
        else {
            Write-Host "No Test exists run test first" -ForegroundColor Red
        }
		
    }
}