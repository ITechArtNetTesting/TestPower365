function Invoke-Validate30111{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30111")){
			$session = Connect-RemotePowershell -TargetMailbox
			$Okay = $false
			$Script:TestResults["Test30111"].ValidationLastRun = (Get-Date)
			#try{
				$Script:TestResults["Test30111"].Data.Folder
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-PublicFolder");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TestResults["Test30111"].Data.Folder);
				$plPileLine.Commands.Add($gpAddmbPerms);
				$Result = $plPileLine.Invoke();
				if($Result -ne $null){
					write-host $Result | fl
					$Result.MailEnabled
					if(!$Result.MailEnabled){
						$Okay = $true
					}
					
				}
				else{
					write-host "No Folder Found"
				}
			
			#}catch{

			#}
			$session.Runspace.Dispose()
			if($Okay){
				$Script:TestResults["Test30111"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30111"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test30111"].ValidationResult = "Failed"	
				$Script:TestResults["Test30111"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}