function Invoke-Validate22513{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		if($TargetMailbox.IsPresent){
			$service = $Script:TargetService
			$MailboxName = $Script:TargetMailbox
		}
		else{
			$service = $Script:SourceService
			$MailboxName = $Script:SourceMailbox
		}
		if($Script:TestResults.ContainsKey("Test22513")){
			$OriginalJournal = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22513"].Data.OrginalId -FolderPath '\Journal'
			$NewJournal= Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22513"].Data.NewId -FolderPath '\Journal'
			$MovedJournal = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22513"].Data.MovedId -FolderPath '\Journal\test22513'
		    $JournalOkay = $false	
			$Script:TestResults["Test22513"].ValidationLastRun = (Get-Date)
			if($OriginalJournal -eq $null){
				write-host ("Original Journal Okay Removed")
				if($MovedJournal -ne $null){
					write-host ("Moved Journal Okay")
					if($NewJournal -ne $null){
						write-host ("New Journal Okay")
						$JournalOkay = $true;
					}
				}
			}
			if($JournalOkay){
				$Script:TestResults["Test22513"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22513"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test22513"].ValidationResult = "Failed"	
				$Script:TestResults["Test22513"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}