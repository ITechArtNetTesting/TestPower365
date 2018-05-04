function Invoke-Validate37613c{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test37613c")){
			$session = Connect-RemotePowershell -TargetMailbox
			$PermissionOkay = $false
			$Script:TestResults["Test37613c"].ValidationLastRun = (Get-Date)
			#try{
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-Mailbox");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TargetMailbox);
				$plPileLine.Commands.Add($gpAddmbPerms);
				$Result = $plPileLine.Invoke();
				if($Result -ne $null){
					$al = $Result.GrantSendOnBehalfTo
					foreach($Entry in $al){
						Write-Host $Entry
						$PermissionOkay = $true	
					}
										
				}
				else{
					write-host "No Permissions Found"
				}
			
			#}catch{

			#}
			$session.Runspace.Dispose()
			if($PermissionOkay){
				$Script:TestResults["Test37613c"].ValidationResult = "Succeeded"
				$Script:TestResults["Test37613c"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test37613c"].ValidationResult = "Failed"	
				$Script:TestResults["Test37613c"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}