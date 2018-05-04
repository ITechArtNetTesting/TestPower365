function Invoke-Validate30387 {
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
        if ($Script:TestResults.ContainsKey("Test30387")) {
            $TargetFolder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30387"].Data)
            $SourceFolder = Get-P365PublicFolderFromPath -SourceMailbox -FolderPath ($Script:TestResults["Test30387"].Data)
            $Okay = Invoke-P365ValidateFolderItems -SourceFolder $SourceFolder -TargetFolder $TargetFolder
            $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
            $Script:TestResults["Test30387"].ValidationResult = "Failed"	
            $Script:TestResults["Test30387"].OverAllResult = "Failed"	
            $Script:TestResults["Test30387"].ValidationLastRun = (Get-Date)	
            if ($Okay) {
					$Script:TestResults["Test30387"].OverAllResult = "Successful"	
                    $Script:TestResults["Test30387"].ValidationResult = "Succeeded"
            }
            Write-Host "Done" -ForegroundColor Green
        }
        else {
            Write-Host "No Test exists run test first" -ForegroundColor Red
        }
		
    }
}