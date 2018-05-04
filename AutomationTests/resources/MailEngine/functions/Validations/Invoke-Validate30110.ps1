function Invoke-Validate30110{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30110")){
			$session = Connect-RemotePowershell -TargetMailbox
			
			$Script:TestResults["Test30110"].ValidationLastRun = (Get-Date)
			$Folder = $null
			Try{
				$Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30110"].Data.Folder)
			}catch{

			}
			
			$session.Runspace.Dispose()
			if($Folder -eq $null){
				$Script:TestResults["Test30110"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30110"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test30110"].ValidationResult = "Failed"	
				$Script:TestResults["Test30110"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}