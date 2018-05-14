function Invoke-Validate36315{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test36315")){
			$session = Connect-RemotePowershell -TargetMailbox
			$AddressOkay = $false
			$Script:TestResults["Test36315"].ValidationLastRun = (Get-Date)
			#try{
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-MailPublicFolder");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TestResults["Test36315"].Data.Folder);
				$plPileLine.Commands.Add($gpAddmbPerms);
				$Result = $plPileLine.Invoke();
				if($Result -ne $null){
					$AddressOkay = $true
				}					
				else{
					write-host "No Folder Found"
				}
			
			#}catch{

			#}
			$session.Runspace.Dispose()
			if($AddressOkay){
				$Script:TestResults["Test36315"].ValidationResult = "Succeeded"
				$Script:TestResults["Test36315"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test36315"].ValidationResult = "Failed"	
				$Script:TestResults["Test36315"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}