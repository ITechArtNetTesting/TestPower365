function Invoke-Test37613b{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=2, Mandatory=$true)] [string]$SourcePermission,
		[Parameter(Position=3, Mandatory=$true)] [string]$TargetPermission
    )  
 	Begin
	 {
		if($TargetMailbox.IsPresent){
			$session = Connect-RemotePowershell -TargetMailbox
		}
		else{
			$session = Connect-RemotePowershell -SourceMailbox
		}
		$TargetSession = Connect-RemotePowershell -TargetMailbox
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "37613b"
		$TestResults.Description = "Full Access Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.TestResult = "Failed"	
		#Clear Target
		try{
					$plPileLine = $TargetSession.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Remove-RecipientPermission");
					$rfRemove.Parameters.Add("Identity", $Script:TargetMailbox);
					$rfRemove.Parameters.Add("Trustee", $TargetPermission);
					$rfRemove.Parameters.Add("AccessRights", "SendAs");
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
					$plPileLine = $session.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Remove-RecipientPermission");
					$rfRemove.Parameters.Add("Identity", $Script:SourceMailbox);
					$rfRemove.Parameters.Add("Trustee", $SourcePermission);
					$rfRemove.Parameters.Add("AccessRights", "SendAs");
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
		}catch{}
		try{
			$plPileLine.Stop();
			$plPileLine = $null;
			$plPileLine = $session.runspace.CreatePipeline();
			$gpAddMailboxPerms = New-Object System.Management.Automation.Runspaces.Command("Add-RecipientPermission");
			$gpAddMailboxPerms.Parameters.Add("Identity", $Script:SourceMailbox);
			$gpAddMailboxPerms.Parameters.Add("Trustee", $SourcePermission);
			$gpAddMailboxPerms.Parameters.Add("AccessRights", "SendAs");
			$gpAddMailboxPerms.Parameters.Add("Confirm", $false);
			$plPileLine.Commands.Add($gpAddMailboxPerms);
			$RsResultsresults = $plPileLine.Invoke();
			$plPileLine.Stop();
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $TargetPermission	
			
		}
		catch{
		
		}
		$session.Runspace.Dispose()
		$TargetSession.Runspace.Dispose()
		$Script:TestResults.OverAllResult = "InComplete"
		if($Script:TestResults.ContainsKey("Test37613b")){
			$Script:TestResults["Test37613b"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test37613b",$TestResults)
		}
		
     }
}