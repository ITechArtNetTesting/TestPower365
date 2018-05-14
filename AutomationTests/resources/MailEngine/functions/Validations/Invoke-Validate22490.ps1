function Invoke-Validate22490{
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
		if($Script:TestResults.ContainsKey("Test22490")){
			$OriginalEmail = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22490"].Data.OrginalId
			$NewEmail= Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22490"].Data.NewId
			$MovedEmail = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22490"].Data.MovedId -FolderPath '\Inbox\test22490'
		    $EmailOkay = $false	
			$Script:TestResults["Test22490"].ValidationLastRun = (Get-Date)
			if($OriginalEmail -eq $null){
				write-host ("Original Email Okay Removed")
				if($MovedEmail -ne $null){
					write-host ("Moved Email Okay")
					if($NewEmail -ne $null){
						write-host ("New Email Okay")
						$EmailOkay = $true;
					}
				}
			}
			if($EmailOkay){
				$Script:TestResults["Test22490"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22490"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22490"].ValidationResult = "Failed"	
				$Script:TestResults["Test22490"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}