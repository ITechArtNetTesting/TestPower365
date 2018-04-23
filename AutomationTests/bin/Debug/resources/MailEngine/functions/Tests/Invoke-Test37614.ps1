function Invoke-Test37614{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=2, Mandatory=$true)] [string]$SourcePermission,
		[Parameter(Position=3, Mandatory=$true)] [string]$TargetPermission,
		[Parameter(Position=4, Mandatory=$true)] [string]$SourcePermission2,
		[Parameter(Position=5, Mandatory=$true)] [string]$TargetPermission2
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
		$TestResults.TestCase = "37614"
		$TestResults.Description = "Full Access Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.TestResult = "Failed"	
		#Clear Target
		try{
					$plPileLine = $TargetSession.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Remove-MailboxPermission");
					$rfRemove.Parameters.Add("Identity", $Script:TargetMailbox);
					$rfRemove.Parameters.Add("User", $TargetPermission);
					$rfRemove.Parameters.Add("AccessRights", "FullAccess");
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
		}catch{}
		try{
					$plPileLine = $TargetSession.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Remove-MailboxPermission");
					$rfRemove.Parameters.Add("Identity", $Script:TargetMailbox);
					$rfRemove.Parameters.Add("User", $TargetPermission2);
					$rfRemove.Parameters.Add("AccessRights", "FullAccess");
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
		}catch{}
						try{
					$plPileLine = $session.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Remove-MailboxPermission");
					$rfRemove.Parameters.Add("Identity", $Script:SourceMailbox);
					$rfRemove.Parameters.Add("User", $SourcePermission);
					$rfRemove.Parameters.Add("AccessRights", "FullAccess");
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
		}catch{}
		try{
					$plPileLine = $session.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Remove-MailboxPermission");
					$rfRemove.Parameters.Add("Identity", $Script:SourceMailbox);
					$rfRemove.Parameters.Add("User", $SourcePermission2);
					$rfRemove.Parameters.Add("AccessRights", "FullAccess");
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
		}catch{}
		#try{

			$plPileLine = $null;
			$plPileLine = $session.runspace.CreatePipeline();
			$gpAddmbPerms = new-object System.Management.Automation.Runspaces.Command("Get-MailboxPermission");
			$gpAddmbPerms.Parameters.Add("Identity", $Script:SourceMailbox);
			$gpAddmbPerms.Parameters.Add("User", $SourcePermission);
			$plPileLine.Commands.Add($gpAddmbPerms);
			$Result = $plPileLine.Invoke();
			if($Result -ne $null){
					$plPileLine = $session.runspace.CreatePipeline();
					$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Remove-MailboxPermission");
					$rfRemove.Parameters.Add("Identity", $Script:SourceMailbox);
					$rfRemove.Parameters.Add("User", $SourcePermission);
					$rfRemove.Parameters.Add("AccessRights", "FullAccess");
					$rfRemove.Parameters.Add("Confirm", $false);
					$plPileLine.Commands.Add($rfRemove);
					$RsResultsresults = $plPileLine.Invoke();
					if ($plPileLine.Error.Count > 0)
					{
						throw new Exception("Error add permissions to Mailbox");
					}
			}
			$plPileLine.Stop();
			$plPileLine = $session.runspace.CreatePipeline();
			$gpAddMailboxPerms = New-Object System.Management.Automation.Runspaces.Command("Add-MailboxPermission");
			$gpAddMailboxPerms.Parameters.Add("Identity", $Script:SourceMailbox);
			$gpAddMailboxPerms.Parameters.Add("User", $SourcePermission);
			$gpAddMailboxPerms.Parameters.Add("AccessRights", "FullAccess");
			$gpAddMailboxPerms.Parameters.Add("AutoMapping", $true);
			$plPileLine.Commands.Add($gpAddMailboxPerms);
			$RsResultsresults = $plPileLine.Invoke();
			$plPileLine.Stop();

		

			$tfile = New-P365TranslationFile -SourceAddress $SourcePermission -TargetAddress $TargetPermission
			Invoke-P365MailboxPermissionsCopy -MappingFile $tfile
			$plPileLine.Stop();
			$plPileLine = $session.runspace.CreatePipeline();
			$gpAddMailboxPerms = New-Object System.Management.Automation.Runspaces.Command("Add-MailboxPermission");
			$gpAddMailboxPerms.Parameters.Add("Identity", $Script:SourceMailbox);
			$gpAddMailboxPerms.Parameters.Add("User", $SourcePermission2);
			$gpAddMailboxPerms.Parameters.Add("AccessRights", "FullAccess");
			$gpAddMailboxPerms.Parameters.Add("AutoMapping", $true);
			$plPileLine.Commands.Add($gpAddMailboxPerms);
			$RsResultsresults = $plPileLine.Invoke();
    		$plPileLine.Stop();
			
			$Data = "" | Select TargetPermission,TargetPermission2
			$Data.TargetPermission = $TargetPermission
			$Data.TargetPermission2 = $TargetPermission2
			$TestResults.TestResult = "Succeeded"
			$TestResults.Data = $Data

		#}
		#catch{
		
		#}
		$session.Runspace.Dispose()
		$TargetSession.Runspace.Dispose()
		$Script:TestResults.OverAllResult = "InComplete"
		if($Script:TestResults.ContainsKey("Test37614")){
			$Script:TestResults["Test37614"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test37614",$TestResults)
		}
		
     }
}