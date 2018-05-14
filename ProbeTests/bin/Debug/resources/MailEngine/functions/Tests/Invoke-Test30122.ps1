function Invoke-Test30122 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 4, Mandatory = $false)] [switch]$RunDelta
    )  
    Begin {
        if ($TargetMailbox.IsPresent) {
            $session = Connect-RemotePowershell -TargetMailbox
            $service = $Script:TargetService
            $MailboxName = $Script:TargetMailbox
        }
        else {
            $session = Connect-RemotePowershell -SourceMailbox
            $service = $Script:SourceService
            $MailboxName = $Script:SourceMailbox
        }
		
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30122"
        $TestResults.Description = "Mail Disable Test"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath \Automation\Tests -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30122-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder1, Folder2, SourceId, FirstTargetPermission, SecondTargetPermission
        $findFolderResults = $pfRoot.FindFolders($SfSearchFilter, $fvFolderView)  
        if ($findFolderResults.TotalCount -eq 0) {  
            Write-host ("Folder Doesn't Exist") 
            $NewFolder.Save($pfRoot.Id)  
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder.Id, $psPropertySet)
            $NewFolder.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName1 = "Test30122-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"
            $NewFolder1.Save($NewFolder.Id) 
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = "f2-Test301222-" + (Get-Date).ToString("s")
            $NewFolder2.DisplayName = $FolderName2
            $NewFolder2.FolderClass = "IPF.Note"
            $NewFolder2.Save($NewFolder.Id) 
            $NewFolder1 = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder1.Id, $psPropertySet)
            $NewFolder1.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
			
        }
        #Clear Target
        try {
            $al = New-Object System.Collections.ArrayList
            $plPileLine = $session.runspace.CreatePipeline();
            $rfRemove = New-Object System.Management.Automation.Runspaces.Command("Enable-MailPublicFolder");
            $rfRemove.Parameters.Add("Identity", ([System.BitConverter]::ToString($EntryIdVal).Replace("-", "")));
            $rfRemove.Parameters.Add("Confirm", $false);
            $plPileLine.Commands.Add($rfRemove);
            $RsResultsresults = $plPileLine.Invoke();
            if ($plPileLine.Error.Count > 0) {
                throw new Exception("Error add permissions to Mailbox");
            }
            else {
                $TestResults.TestResult = "Succeeded"
            }
            $plPileLine.Stop()
            sleep -Seconds 10
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\\Automation\tests\" + $FolderName) -TargetCopyPath "\\Automation\tests"
            $data = "" | Select Folder1,Folder2
            $data.Folder1 = ("\Automation\Tests\" + $FolderName + "\" + $FolderName2)
            $NewFolder2.DisplayName = "Renamed12234"
            $data.Folder2 = ("\Automation\Tests\" + $FolderName + "\" + "Renamed12234")
            $NewFolder2.Update()
            $TestResults.Data = $data               
            $TestResults.TestResult = "Succeeded"
			
			
        }
        catch {}
        $session.Runspace.Close()
        $session.Runspace.Dispose()
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30122")) {
            $Script:TestResults["Test30122"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30122", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\\Automation\tests\" + $FolderName) -TargetCopyPath "\\Automation\tests"
        }
		
    }
}