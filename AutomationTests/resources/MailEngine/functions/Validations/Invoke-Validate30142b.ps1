function Invoke-Validate30146b {
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
        if ($Script:TestResults.ContainsKey("Test30146b")) {
            $Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30146b"].Data.Folder1)
            $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
            $Script:TestResults["Test30146b"].ValidationResult = "Failed"	
            $Script:TestResults["Test30146b"].OverAllResult = "Failed"	
            $Script:TestResults["Test30146b"].ValidationLastRun = (Get-Date)	
            if ($folder -ne $Null) {
					$Script:TestResults["Test30146b"].OverAllResult = "Successful"	
                    $Script:TestResults["Test30146b"].ValidationResult = "Succeeded"
            }
            Write-Host "Done" -ForegroundColor Green
        }
        else {
            Write-Host "No Test exists run test first" -ForegroundColor Red
        }
		
    }
}