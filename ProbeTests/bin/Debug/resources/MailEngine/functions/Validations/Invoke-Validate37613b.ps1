function Invoke-Validate37613b{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test37613b")){
			$session = Connect-RemotePowershell -TargetMailbox
			$PermissionOkay = $false
			$Script:TestResults["Test37613b"].ValidationLastRun = (Get-Date)
			#try{
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-RecipientPermission");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TargetMailbox);
				$gpAddmbPerms.Parameters.Add("Trustee", $Script:TestResults["Test37613b"].Data);
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
				$Script:TestResults["Test37613b"].ValidationResult = "Succeeded"
				$Script:TestResults["Test37613b"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test37613b"].ValidationResult = "Failed"	
				$Script:TestResults["Test37613b"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}