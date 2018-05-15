function Invoke-Test30135 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 4, Mandatory = $false)] [switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath,
		[Parameter(Mandatory = $true)][String]$TargetRootPath
    )  
    Begin {
        if ($TargetMailbox.IsPresent) {            
            $service = $Script:TargetService
            $MailboxName = $Script:TargetMailbox
        }
        else {
            $service = $Script:SourceService
            $MailboxName = $Script:SourceMailbox
        }
		
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30135"
        $TestResults.Description = "Mail Disable Delete Test"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30135-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $okay = $false
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
            $FolderName1 = "Test30135-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"
            $NewFolder1.Save($NewFolder.Id) 
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $NewFolder2.DisplayName = $FolderName1
            $NewFolder2.FolderClass = "IPF.Note"
            $NewFolder2.Save($NewFolder1.Id)
            $NewFolder1 = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder1.Id, $psPropertySet)
            $NewFolder1.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            
            $okay = $true
            $Folders = @()
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName1)
            $TestResults.Data = $Folders
        }
        $Script:TestResults.OverAllResult = "InComplete"		
	    if($okay){			
			$TestResults.TestResult = "Succeeded"
		}
		else{
			$TestResults.TestResult = "Failed"			
		}		
		$TestResults.TestCase = "30135"
		$TestResults.Description = "Folder depth Check"
		$TestResults.TestLastRun = (Get-Date)
        if ($Script:TestResults.ContainsKey("Test30135")) {
            $Script:TestResults["Test30135"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30135", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
		
    }
}