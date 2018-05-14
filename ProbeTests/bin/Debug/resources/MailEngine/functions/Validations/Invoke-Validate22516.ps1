function Invoke-Validate22516{
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
		if($Script:TestResults.ContainsKey("Test22516")){
			$MovedEmail = Invoke-P365FindAppointment -TargetMailbox -MessageId $Script:TestResults["Test22516"].Data.Id -FolderPath '\Calendar\test22516'
		    $EmailOkay = $false	
			$Script:TestResults["Test22516"].ValidationLastRun = (Get-Date)
			if($MovedEmail -ne $null){
				write-host ("Moved Email found okay")
				$MovedEmail.Load()
				foreach($rcp in $MovedEmail.RequiredAttendees){	
					write-host $rcp	
					if($rcp.Address.ToLower() -eq $Script:TestResults["Test22516"].Data.TargetAddress.ToLower()){
						$EmailOkay = $true;
						}
					}
			}
			if($EmailOkay){
				$Script:TestResults["Test22516"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22516"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22516"].ValidationResult = "Failed"	
				$Script:TestResults["Test22516"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}