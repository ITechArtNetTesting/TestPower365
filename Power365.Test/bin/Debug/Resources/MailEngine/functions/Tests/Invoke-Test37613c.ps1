function Invoke-Test37613c{
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
		$TestResults.TestCase = "37613c"
		$TestResults.Description = "Full Access Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.TestResult = "Failed"	
		#Clear Target
		try{
					$al = New-Object System.Collections.ArrayList
					$plPileLine = $TargetSession.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Set-Mailbox");
					$rfRemove.Parameters.Add("Identity", $Script:TargetMailbox);
					$rfRemove.Parameters.Add("GrantSendOnBehalfTo", $al)
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
					$al = New-Object System.Collections.ArrayList
					$plPileLine = $session.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Set-Mailbox");
					$rfRemove.Parameters.Add("Identity", $Script:SourceMailbox);
					$rfRemove.Parameters.Add("GrantSendOnBehalfTo",$al);
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
			$al = New-Object System.Collections.ArrayList
			$al.Add($SourcePermission)
			$plPileLine = $session.runspace.CreatePipeline();
			$gpAddMailboxPerms = New-Object System.Management.Automation.Runspaces.Command("Set-Mailbox");
			$gpAddMailboxPerms.Parameters.Add("Identity", $Script:SourceMailbox);
			$gpAddMailboxPerms.Parameters.Add("GrantSendOnBehalfTo", $al);
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
		if($Script:TestResults.ContainsKey("Test37613c")){
			$Script:TestResults["Test37613c"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test37613c",$TestResults)
		}
		
     }
}