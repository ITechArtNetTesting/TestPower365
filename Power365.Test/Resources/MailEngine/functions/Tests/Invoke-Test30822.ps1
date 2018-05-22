function Invoke-Test30822{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=2, Mandatory=$true)] [string]$SourceProxyAddress,
		[Parameter(Position=3, Mandatory=$true)] [string]$TargetProxyAddress,
		[Parameter(Position=4, Mandatory=$false)] [switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath
    )  
 	Begin
	 {
		if($TargetMailbox.IsPresent){
			$session = Connect-RemotePowershell -TargetMailbox
			$service = $Script:TargetService
            $MailboxName = $Script:TargetMailbox
		}
		else{
			$session = Connect-RemotePowershell -SourceMailbox
			$service = $Script:SourceService
            $MailboxName = $Script:SourceMailbox
		}
		
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$TestResults.TestCase = "30822"
		$TestResults.Description = "Forwarding Address Test"
		$TestResults.TestLastRun = (Get-Date)
		$TestResults.TestResult = "Failed"
			       Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30072-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder, Folder2, SourceId, TargetProxyAddress
        $findFolderResults = $pfRoot.FindFolders($SfSearchFilter, $fvFolderView)  
        if ($findFolderResults.TotalCount -eq 0) {  
            Write-host ("Folder Doesn't Exist") 
			$NewFolder.Save($pfRoot.Id)  
			$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
			$psPropertySet.Add($PR_ENTRYID)
			$NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$NewFolder.Id,$psPropertySet)
			$NewFolder.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)
			
			
		}
		#Clear Target
		try{
			$al = New-Object System.Collections.ArrayList
			$plPileLine = $session.runspace.CreatePipeline();
			$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Enable-MailPublicFolder");
			$rfRemove.Parameters.Add("Identity", ("\" + $RootPath + "\" + $FolderName));
			$rfRemove.Parameters.Add("Confirm", $false);
			$plPileLine.Commands.Add($rfRemove);
			$RsResultsresults = $plPileLine.Invoke();
			if ($plPileLine.Error.Count > 0)
			{
				throw new Exception("Error add permissions to Mailbox");
			}
			else{
				$TestResults.TestResult = "Succeeded"
			}
			$plPileLine.Stop()
			sleep -Seconds 10
			$plPileLine = $session.runspace.CreatePipeline();
			$adproxy = @()
			$adproxy += "add=`"" + $SourceProxyAddress + "`""
			$rfRemove = New-Object System.Management.Automation.Runspaces.Command("Set-MailPublicFolder");
			$rfRemove.Parameters.Add("Identity", ("\" + $RootPath + "\" + $FolderName));
			$rfRemove.Parameters.Add("Emailaddresses", @{add="$SourceProxyAddress"});
			$plPileLine.Commands.Add($rfRemove);
			$RsResultsresults = $plPileLine.Invoke();
			if ($plPileLine.Error.Count > 0)
			{
				throw new Exception("Error add Address to Public Folder");
			}
			else{
				$data = "" | Select Folder,TargetProxyAddress
				$data.Folder = ("\" + $RootPath + "\" + $FolderName)
				$data.TargetProxyAddress = $TargetProxyAddress
				$TestResults.Data = $data
				$TestResults.TestResult = "Succeeded"
			}
			
			
		}catch{}
		$session.Runspace.Close()
		$session.Runspace.Dispose()
		$Script:TestResults.OverAllResult = "InComplete"
		if($Script:TestResults.ContainsKey("Test30822")){
			$Script:TestResults["Test30822"] = $TestResults
		}
		else{
			$Script:TestResults.Add("Test30822",$TestResults)
		}
		if ($RunDelta.IsPresent) {
            Get-p365TestResults
            $tfile = New-P365TranslationFile -SourceAddress $SourceProxyAddress -TargetAddress $TargetProxyAddress -NoEXAddress
                   # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
        }
		
     }
}