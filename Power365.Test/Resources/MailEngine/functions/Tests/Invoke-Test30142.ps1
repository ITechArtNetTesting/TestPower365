function Invoke-Test30142 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 4, Mandatory = $false)] [switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath,
		[Parameter(Mandatory = $true)][String]$TargetRootPath
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
        $TestResults.TestCase = "30142"
        $TestResults.Description = "Public Folder Move Test"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30142-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder1,Folder2,Folder3,ItemIds
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
            $FolderName1 = "Test30142-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"
            $NewFolder1.Save($NewFolder.Id) 
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = "Test301422-" + (Get-Date).ToString("s")
            $NewFolder2.DisplayName = $FolderName2
            $NewFolder2.FolderClass = "IPF.Note"
            $NewFolder2.Save($NewFolder.Id) 
            for($Im=0;$Im -lt 10;$Im++){
                $Post = New-Object Microsoft.Exchange.WebServices.Data.PostItem -ArgumentList $service
                $Post.Subject = "POST test - " + $Im
                $Post.ItemClass = "IPM.Post" 
                #$Post.ToRecipients.Add($Script:SourceMailbox) 
                $Post.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
  		        $Post.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
  		        $Post.Body.Text = "Body" 
                $Post.Save($NewFolder2.Id)            
		        $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		        $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		        $psPropertySet.Add($PR_ENTRYID)
		        $Post.Load($psPropertySet)
		        $EntryIdVal = $null		
                [Void]$Post.TryGetProperty($PR_ENTRYID,[ref]$EntryIdVal)  
                $data.ItemIds.Add($EntryIdVal)      
            } 
            $NewFolder1 = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $NewFolder1.Id, $psPropertySet)
            $NewFolder1.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)
			
        }
        
        $data.Folder1 = ($RootPath + "\" + $FolderName + "\" + $FolderName1)
        $data.Folder2 = ($RootPath + "\" + $FolderName + "\" + $FolderName2)
        $data.Folder3 = ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2)
        Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        $NewFolder2.Move($NewFolder1.Id)
        $NewFolder3 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $NewFolder3.DisplayName = $FolderName2
        $NewFolder3.FolderClass = "IPF.Note"
        $NewFolder3.Save($NewFolder.Id) 
        $TestResults.Data = $data
        $TestResults.TestResult = "Succeeded"
        $session.Runspace.Close()
        $session.Runspace.Dispose()
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test30142")) {
            $Script:TestResults["Test30142"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30142", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
		
    }
}