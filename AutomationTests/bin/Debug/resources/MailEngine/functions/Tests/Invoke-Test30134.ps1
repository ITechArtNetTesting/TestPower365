function Invoke-Test30134 {
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
        $TestResults.TestCase = "30134"
        $TestResults.Description = "Mail Disable Delete Test"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30134-" + (Get-Date).ToString("s")
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
            $FolderName1 = "Test30134-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"
            $NewFolder1.Save($NewFolder.Id) 
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder2.DisplayName = $FolderName2
            $NewFolder2.FolderClass = "IPF.Note"
            $NewFolder2.Save($NewFolder1.Id)
            $NewFolder3 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName3 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder3.DisplayName = $FolderName3
            $NewFolder3.FolderClass = "IPF.Note"
            $NewFolder3.Save($NewFolder2.Id)
            $NewFolder4 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName4 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder4.DisplayName = $FolderName4
            $NewFolder4.FolderClass = "IPF.Note"
            $NewFolder4.Save($NewFolder3.Id) 
            $NewFolder5 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName5 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder5.DisplayName = $FolderName5
            $NewFolder5.FolderClass = "IPF.Note"
            $NewFolder5.Save($NewFolder4.Id)
            $NewFolder6 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName6 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder6.DisplayName = $FolderName6
            $NewFolder6.FolderClass = "IPF.Note"
            $NewFolder6.Save($NewFolder5.Id)   
            $NewFolder7 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName7 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder7.DisplayName = $FolderName7
            $NewFolder7.FolderClass = "IPF.Note"
            $NewFolder7.Save($NewFolder6.Id) 
            $NewFolder8 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName8 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder8.DisplayName = $FolderName8
            $NewFolder8.FolderClass = "IPF.Note"
            $NewFolder8.Save($NewFolder7.Id)
            $NewFolder9 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName9 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder9.DisplayName = $FolderName9
            $NewFolder9.FolderClass = "IPF.Note"
            $NewFolder9.Save($NewFolder8.Id)   
            $NewFolder10 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName10 = "Test301342-" + (Get-Date).ToString("s")
            $NewFolder10.DisplayName = $FolderName10
            $NewFolder10.FolderClass = "IPF.Note"
            $NewFolder10.Save($NewFolder9.Id)   
            $NewFolder1 = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder1.Id, $psPropertySet)
            $NewFolder1.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            
            $okay = $true
            $Folders = @()
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3 + "\" + $FolderName4)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3 + "\" + $FolderName4 + "\" + $FolderName5)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3 + "\" + $FolderName4 + "\" + $FolderName5 + "\" + $FolderName6)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3 + "\" + $FolderName4 + "\" + $FolderName5 + "\" + $FolderName6 + "\" + $FolderName7)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3 + "\" + $FolderName4 + "\" + $FolderName5 + "\" + $FolderName6 + "\" + $FolderName7 + "\" + $FolderName8)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3 + "\" + $FolderName4 + "\" + $FolderName5 + "\" + $FolderName6 + "\" + $FolderName7 + "\" + $FolderName8 + "\" + $FolderName9)
            $Folders += ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2 + "\" + $FolderName3 + "\" + $FolderName4 + "\" + $FolderName5 + "\" + $FolderName6 + "\" + $FolderName7 + "\" + $FolderName8 + "\" + $FolderName9 + "\" + $FolderName10)
            $TestResults.Data = $Folders
        }
        $Script:TestResults.OverAllResult = "InComplete"		
	    if($okay){			
			$TestResults.TestResult = "Succeeded"
		}
		else{
			$TestResults.TestResult = "Failed"			
		}		
		$TestResults.TestCase = "30134"
		$TestResults.Description = "Folder depth Check"
		$TestResults.TestLastRun = (Get-Date)
        if ($Script:TestResults.ContainsKey("Test30134")) {
            $Script:TestResults["Test30134"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30134", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
		
    }
}