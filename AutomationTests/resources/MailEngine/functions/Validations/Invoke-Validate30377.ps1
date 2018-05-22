function Invoke-Validate30378{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30377")){
			$Script:TestResults["Test30377"].ValidationResult = "Failed"	
			$Script:TestResults["Test30377"].OverAllResult = "Failed"	
			$Okay = $false
			$Script:TestResults["Test30377"].ValidationLastRun = (Get-Date)
	
			if($HasNoteFailed -band $Okay){
				$Script:TestResults["Test30377"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30377"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test30377"].ValidationResult = "Failed"	
				$Script:TestResults["Test30377"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}