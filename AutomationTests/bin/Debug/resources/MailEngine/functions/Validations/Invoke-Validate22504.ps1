function Invoke-Validate22504{
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
		if($Script:TestResults.ContainsKey("Test22504")){
			$OriginalTask= Invoke-P365FindTask -TargetMailbox -MessageId $Script:TestResults["Test22504"].Data.OrginalId
			$NewTask = Invoke-P365FindTask -TargetMailbox -MessageId $Script:TestResults["Test22504"].Data.NewId
			$MovedTask = Invoke-P365FindTask -TargetMailbox -MessageId $Script:TestResults["Test22504"].Data.MovedId -FolderPath '\Tasks\test22504'
		    $TaskOkay = $false	
			$Script:TestResults["Test22504"].ValidationLastRun = (Get-Date)
			if($OriginalTask -eq $null){
				write-host ("Original Task Okay Removed")
				if($MovedTask -ne $null){
					write-host ("Moved Task Okay")
					if($NewTask -ne $null){
						write-host ("New Task Okay")
						$TaskOkay = $true;
					}
				}
			}
			if($TaskOkay){
				$Script:TestResults["Test22504"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22504"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test22504"].ValidationResult = "Failed"	
				$Script:TestResults["Test22504"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}