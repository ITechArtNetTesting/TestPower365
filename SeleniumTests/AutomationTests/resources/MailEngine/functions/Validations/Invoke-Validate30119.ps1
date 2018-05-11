function Invoke-Validate30119{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30119")){
			$session = Connect-RemotePowershell -TargetMailbox
			$ForwardOkay = $false
			$Script:TestResults["Test30119"].ValidationLastRun = (Get-Date)
			#try{
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-MailPublicFolder");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TestResults["Test30119"].Data.Folder);
				$plPileLine.Commands.Add($gpAddmbPerms);
				$Result = $plPileLine.Invoke();
				if($Result -ne $null){
					write-host $Result.ForwardingAddress	
					write-host $Result.Identity
					$plPileLine.Stop()
					$plPileLine = $session.runspace.CreatePipeline();
					$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-Recipient");
					$gpAddmbPerms.Parameters.Add("Identity", $Result.ForwardingAddress);
					$plPileLine.Commands.Add($gpAddmbPerms);
					$Result1 = $plPileLine.Invoke();
					if($Result1 -ne $null){
						if($Result1.PrimarySmtpAddress -eq $Script:TestResults["Test30119"].Data.TargetForwardingAddress){
							$ForwardOkay = $true
						}

					}
				}
				else{
					write-host "No Folder Found"
				}
			
			#}catch{

			#}
			$session.Runspace.Dispose()
			if($ForwardOkay){
				$Script:TestResults["Test30119"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30119"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test30119"].ValidationResult = "Failed"	
				$Script:TestResults["Test30119"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}