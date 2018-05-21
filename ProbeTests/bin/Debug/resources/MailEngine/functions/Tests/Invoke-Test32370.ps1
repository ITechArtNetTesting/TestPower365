function Invoke-Test32370 {
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
        $TestResults.TestCase = "32370"
        $TestResults.Description = "Public Journal Move"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test32370-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder,MessageId
        
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
            $FolderName1 = "Test32370-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Journal"
            $data.Folder =  $RootPath + "\" + $FolderName + "\" + $FolderName1
            $NewFolder1.Save($NewFolder.Id)
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = "Test323702-" + (Get-Date).ToString("s")
            $NewFolder2.DisplayName = $FolderName2
            $NewFolder2.FolderClass = "IPF.Journal"
            $NewFolder2.Save($NewFolder.Id)
            $data.Folder =  $RootPath + "\" + $FolderName + "\" + $FolderName2
            $jnJournal = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service  
                #Set the Subject of the Note  
            $jnJournal.Subject = "Journal : teset"
             $jnJournal.ItemClass = "IPM.Activity"  
            #Set the Text body of the Note  
              
                #Start Note specific Extended properties   
  		    $jnJournal.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
  		    $jnJournal.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
		    $jnJournal.Body = "First Line `r`nNext Line"
               #Save the Item to the Notes Folder  
            $jnJournal.Save($NewFolder1.Id)      
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $jnJournal.Load($psPropertySet)
            $EntryIdVal = $null		
            [Void]$jnJournal.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
            $data.MessageId = $EntryIdVal
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            $NewId = $jnJournal.Move($NewFolder2.Id)
            $NewId.Load($psPropertySet)  
            [Void]$NewId.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)
            $data.MessageId = $EntryIdVal
            $TestResults.Data = $data               
            $TestResults.TestResult = "Succeeded"
		
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test32370")) {
            $Script:TestResults["Test32370"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test32370", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
		
    }
}