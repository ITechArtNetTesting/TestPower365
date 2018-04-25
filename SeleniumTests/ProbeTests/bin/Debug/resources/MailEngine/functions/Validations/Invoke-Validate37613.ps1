function Invoke-Validate37613{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test37613")){
			$session = Connect-RemotePowershell -TargetMailbox
			$PermissionOkay = $false
			$Script:TestResults["Test37613"].ValidationLastRun = (Get-Date)
			#try{
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-MailboxPermission");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TargetMailbox);
				$gpAddmbPerms.Parameters.Add("User", $Script:TestResults["Test37613"].Data);
				$plPileLine.Commands.Add($gpAddmbPerms);
				$Result = $plPileLine.Invoke();
				if($Result -ne $null){
					$Result
					$PermissionOkay = $true						
				}
				else{
					write-host "No Permissions Found"
				}
			
			#}catch{

			#}
			$session.Runspace.Dispose()
			if($PermissionOkay){
				$Script:TestResults["Test37613"].ValidationResult = "Succeeded"
				$Script:TestResults["Test37613"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test37613"].ValidationResult = "Failed"	
				$Script:TestResults["Test37613"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}