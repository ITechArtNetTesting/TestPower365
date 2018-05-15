function Invoke-Test30385 {
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
        $TestResults.TestCase = "30385"
        $TestResults.Description = "Public Folder StickyNote"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30385-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder,ItemIds
        $data.ItemIds = New-Object "System.Collections.Generic.List[Byte[]]"
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
            $FolderName1 = "Test30385-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.StickyNote"
            $data.Folder =  $RootPath + $FolderName + "\" + $FolderName1
            $NewFolder1.Save($NewFolder.Id)
            for($Im=0;$Im -lt 10;$Im++){
                $snStickyNote = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
                #Set the Subject of the Note  
                $snStickyNote.Subject = "Stick Note Subject : " + $Im.ToString()  
                #Change the ItemClass to IPM.StickNote 
                $snStickyNote.ItemClass = "IPM.StickyNote"  
                #Set the Text body of the Note  
                $snStickyNote.Body = "First Line `r`nNext Line"  
                #Start Note specific Extended properties   
                #Define the Guid for the Notes Named properties  
                $noteGuid = new-object Guid("0006200E-0000-0000-C000-000000000046")  
                #Set the Colour of the Note  
                $snColour = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition($noteGuid,35584, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer)  
                #Set the Height of the Note  
                $snHeight = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition($noteGuid,35586, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer)  
                #Set the Width of the Note  
                $snWidth = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition($noteGuid,35587, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer)  
                #Postion from Left  
                $snLeft = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition($noteGuid,35588, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer)  
                #Postion from Top  
                $snTop = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition($noteGuid,35589, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer)  
                #Set Note props  
                $snStickyNote.SetExtendedProperty($snColour,3)  
                $snStickyNote.SetExtendedProperty($snHeight,200)  
                $snStickyNote.SetExtendedProperty($snWidth,166)  
                $snStickyNote.SetExtendedProperty($snLeft,80)  
                $snStickyNote.SetExtendedProperty($snTop,80)  
                #Save the Item to the Notes Folder  
                $snStickyNote.Save($NewFolder1.Id)               
		        $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		        $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		        $psPropertySet.Add($PR_ENTRYID)
		        $snStickyNote.Load($psPropertySet)
		        $EntryIdVal = $null		
                [Void]$snStickyNote.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
                $data.ItemIds.Add($EntryIdVal)      
            } 
            $TestResults.Data = $data               
            $TestResults.TestResult = "Succeeded"
		
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30385")) {
            $Script:TestResults["Test30385"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30385", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
		
    }
}