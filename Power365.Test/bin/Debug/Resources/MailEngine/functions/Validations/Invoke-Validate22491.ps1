function Invoke-Validate22491{
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
		if($Script:TestResults.ContainsKey("Test22491")){
			$OriginalContact = Invoke-P365FindContact -TargetMailbox -MessageId $Script:TestResults["Test22491"].Data.OrginalId
			$NewContact = Invoke-P365FindContact -TargetMailbox -MessageId $Script:TestResults["Test22491"].Data.NewId
			$MovedContact = Invoke-P365FindContact -TargetMailbox -MessageId $Script:TestResults["Test22491"].Data.MovedId -FolderPath '\Contacts\test22491'
			$ContactsOkay = $false
			$Script:TestResults["Test22491"].ValidationLastRun = (Get-Date)
			if($OriginalContact -eq $null){
				write-host ("Original Contact Okay Removed")
				if($MovedContact -ne $null){
					write-host ("Moved Contact Okay")
					if($NewContact -ne $null){
						write-host ("New Contact Okay")
						$ContactsOkay = $true;
					}
				}
			}
			if($ContactsOkay){
				$Script:TestResults["Test22491"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22491"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22491"].ValidationResult = "Failed"	
				$Script:TestResults["Test22491"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}