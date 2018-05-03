function Invoke-Test30388 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 6, Mandatory = $false)][switch]$RunDelta
    )  
    Begin {
        $Data = "" | Select OrginalId, MovedId, NewId
        if ($TargetMailbox.IsPresent) {
            $service = $Script:TargetService
            $MailboxName = $Script:TargetMailbox
        }
        else {
            $service = $Script:SourceService
            $MailboxName = $Script:SourceMailbox
        }
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath \Automation\Tests -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30388-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder1, Folder2,Folder3,ItemIds
        
        $findFolderResults = $pfRoot.FindFolders($SfSearchFilter, $fvFolderView)  
        if ($findFolderResults.TotalCount -eq 0) {  
            Write-host ("Folder Doesn't Exist") 
            $NewFolder.Save($pfRoot.Id)  
            $NewFolder.Load()
            Write-host ("Folder Created")
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
            $NewFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder.Id, $psPropertySet)
            $NewFolder.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
            $data.Folder1 = ("\Automation\tests\" + $FolderName)
            $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName1 = $FolderName
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"			
            $NewFolder1.Save($NewFolder.Id)
            $NewFolder1.Load($psPropertySet)
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = $FolderName
            $NewFolder2.DisplayName = $FolderName2
            $NewFolder2.FolderClass = "IPF.Note"			
            $NewFolder2.Save($NewFolder1.Id)
            $data.Folder2 = ("\Automation\tests\" + $FolderName + "\" + $FolderName1)
            $data.Folder3 = ("\Automation\tests\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2)
            $data.ItemIds = New-Object "System.Collections.Generic.List[Byte[]]"
            for ($Im = 0; $Im -lt 10; $Im++) {
                    $Email = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $NewFolder2.Service
                    $Email.Subject = "Email test - " + $Im
                    #$Post.ToRecipients.Add($Script:SourceMailbox) 
                    $Email.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
                    $Email.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
                    $Email.Body.Text = "Body" 
                    $Email.Save($NewFolder2.Id)            
                    $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
                    $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
                    $psPropertySet.Add($PR_ENTRYID)
                    $Email.Load($psPropertySet)
                    $EntryIdVal = $null		
                    [Void]$Email.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)  
                    $data.ItemIds.Add($EntryIdVal)      
            }
            #clear perms
        }  
        else {  
            throw ("Folder already Exist with that Name")  
		
        }     
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30388"
        $TestResults.Description = "Nested Folder Item"
        $TestResults.TestLastRun = (Get-Date)		
        if ($NewFolder2 -ne $Null) {
             #test permissions
            $TestResults.Data = $data
            $TestResults.TestResult = "Succeeded"
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30388")) {
            $Script:TestResults["Test30388"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30388", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\\Automation\tests\" + $FolderName) -TargetCopyPath "\\Automation\tests"
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\\Automation\tests\" + $FolderName) -TargetCopyPath "\\Automation\tests"
        }
        Write-Host "Done" -ForegroundColor Green
    }

}