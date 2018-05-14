function Invoke-Validate46843{
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
		$Script:TestResults["Test46843"].Data.Folder
		if($Script:TestResults.ContainsKey("Test46843")){
		$Folder  = Get-P365FolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test46843"].Data.Folder)
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$Script:TestResults["Test46843"].ValidationResult = "Failed"	
		$Script:TestResults["Test46843"].OverAllResult = "Failed"	
		$Script:TestResults["Test46843"].ValidationLastRun = (Get-Date)	
		if($folder -ne $Null){
			if($folder.FolderClass -eq "IPF.Files"){
				$Script:TestResults["Test46843"].OverAllResult = "Succeeded" 	
				$Script:TestResults["Test46843"].ValidationResult = "Succeeded"	
			}
		}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}