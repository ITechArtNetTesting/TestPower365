function Invoke-Validate30135{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30135")){
			$Okay = $true
			$Processed = $false
			$Script:TestResults["Test30135"].ValidationLastRun = (Get-Date)			
			foreach($folder in $Script:TestResults["Test30135"].Data){
				write-host $folder
				$Processed = $true
				$Folderbn  = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $folder
				if($Folderbn -eq $null){
					$Script:TestResults["Test30135"].ValidationResult = "Failed"	
					$Script:TestResults["Test30135"].OverAllResult = "Failed"
					$Okay = $false	
					break
				}
			}	


			if($Okay -band $Processed){
				$Script:TestResults["Test30135"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30135"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test30135"].ValidationResult = "Failed"	
				$Script:TestResults["Test30135"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}