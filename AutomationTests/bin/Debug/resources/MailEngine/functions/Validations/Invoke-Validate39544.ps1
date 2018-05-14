function Invoke-Validate39544{
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
		if($Script:TestResults.ContainsKey("Test39544")){
			$TargetFolder =  Get-P365FolderFromPath -FolderPath $Script:TestResults["Test39544"].Data -TargetMailbox		
			$Script:TestResults["Test39544"].ValidationLastRun = (Get-Date)
			if($TargetFolder -eq $null){
				$Script:TestResults["Test39544"].ValidationResult = "Succeeded"
				$Script:TestResults["Test39544"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test39544"].ValidationResult = "Failed"	
				$Script:TestResults["Test39544"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}