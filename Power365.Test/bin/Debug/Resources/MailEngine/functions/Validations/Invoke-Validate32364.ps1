function Invoke-Validate32364 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox
    )  
    Begin {
		
        if ($Script:TestResults.ContainsKey("Test32364")) {
            $Script:TestResults["Test32364"].ValidationResult = "Failed"	
            $Script:TestResults["Test32364"].OverAllResult = "Failed"	
            $Script:TestResults["Test32364"].Data.Folder
            $Message = Invoke-P365FindPublicFolderMessage -FolderPath $Script:TestResults["Test32364"].Data.Folder -MessageId $Script:TestResults["Test32364"].Data.MessageId -TargetMailbox
            if ($Message -ne $null) {
                if ($Message.Subject -eq "Journal Modified 32364") {
                    $Script:TestResults["Test32364"].ValidationResult = "Succeeded"
                    $Script:TestResults["Test32364"].OverAllResult = "Succeeded"  		

                }

            }
            else {
                $Script:TestResults["Test32364"].ValidationResult = "Failed"	
                $Script:TestResults["Test32364"].OverAllResult = "Failed"		
            }
            Write-Host "Done" -ForegroundColor Green
        }
        else {
            Write-Host "No Test exists run test first" -ForegroundColor Red
        }
		
    }
}