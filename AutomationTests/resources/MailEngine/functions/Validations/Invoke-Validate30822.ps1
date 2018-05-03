function Invoke-Validate30822{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30822")){
			$session = Connect-RemotePowershell -TargetMailbox
			$AddressOkay = $false
			$Script:TestResults["Test30822"].ValidationLastRun = (Get-Date)
			#try{
				$plPileLine = $session.runspace.CreatePipeline();
				$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-MailPublicFolder");
				$gpAddmbPerms.Parameters.Add("Identity", $Script:TestResults["Test30822"].Data.Folder);
				$plPileLine.Commands.Add($gpAddmbPerms);
				$Result = $plPileLine.Invoke();
				if($Result -ne $null){
					foreach($EmailAddress in $Result.EmailAddresses){
						$cval = ("smtp:" + $Script:TestResults["Test30822"].Data.TargetProxyAddress.ToLower())
						write-host $EmailAddress + " : " + $cval
						if($EmailAddress.ToLower() -eq $cval){
							$AddressOkay = $true
						}	
					}
				}					
				else{
					write-host "No Folder Found"
				}
			
			#}catch{

			#}
			$session.Runspace.Dispose()
			if($AddressOkay){
				$Script:TestResults["Test30822"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30822"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test30822"].ValidationResult = "Failed"	
				$Script:TestResults["Test30822"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}