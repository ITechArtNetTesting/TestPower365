function Invoke-Validate22497{
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
		if($Script:TestResults.ContainsKey("Test22497")){
			$OriginalAppointment= Invoke-P365FindAppointment -TargetMailbox -MessageId $Script:TestResults["Test22497"].Data.OrginalId
			$NewAppointment = Invoke-P365FindAppointment -TargetMailbox -MessageId $Script:TestResults["Test22497"].Data.NewId
			$MovedAppointment = Invoke-P365FindAppointment -TargetMailbox -MessageId $Script:TestResults["Test22497"].Data.MovedId -FolderPath '\Calendar\test22497'
		    $AppointmentOkay = $false	
			$Script:TestResults["Test22497"].ValidationLastRun = (Get-Date)
			if($OriginalAppointment -eq $null){
				write-host ("Original Appointment Okay Removed")
				if($MovedAppointment -ne $null){
					write-host ("Moved Appointment Okay")
					if($NewAppointment -ne $null){
						write-host ("New Appointment Okay")
						$AppointmentOkay = $true;
					}
				}
			}
			if($AppointmentOkay){
				$Script:TestResults["Test22497"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22497"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test22497"].ValidationResult = "Failed"	
				$Script:TestResults["Test22497"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}