function Invoke-Validate37614{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test37614")){
			$session = Connect-RemotePowershell -TargetMailbox
			$PermissionOkay = $false
			$Script:TestResults["Test37614"].ValidationLastRun = (Get-Date)
			try{
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-MailboxPermission");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TargetMailbox);
				$gpAddmbPerms.Parameters.Add("User", $Script:TestResults["Test37614"].Data.TargetPermission);
				$plPileLine.Commands.Add($gpAddmbPerms);
				$Result = $plPileLine.Invoke();
				if($Result -ne $null){
					$Result
					$PermissionOkay = $true						
				}
				else{
					write-host "No Permissions Found"
				}
				$plPileLine.Stop();
				if($PermissionOkay){
					$PermissionOkay = $false
					$plPileLine = $session.runspace.CreatePipeline();
					$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-MailboxPermission");
					$gpAddmbPerms.Parameters.Add("Identity", $Script:TargetMailbox);
					$gpAddmbPerms.Parameters.Add("User", $Script:TestResults["Test37614"].Data.TargetPermission2);
					$plPileLine.Commands.Add($gpAddmbPerms);
					$Result = $plPileLine.Invoke();
					if($Result -ne $null){
						$Result
						$PermissionOkay = $true						
					}
					else{
						write-host "No Permissions Found"
					}
				}
				else{
					$PermissionOkay = $false
				}
			}catch{

			}
			$session.Runspace.Dispose()
			if($PermissionOkay){
				$Script:TestResults["Test37614"].ValidationResult = "Succeeded"
				$Script:TestResults["Test37614"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test37614"].ValidationResult = "Failed"	
				$Script:TestResults["Test37614"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}