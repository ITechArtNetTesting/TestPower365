function Invoke-Test32267 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 4, Mandatory = $false)] [switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath
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
        $TestResults.TestCase = "32267"
        $TestResults.Description = "Public Folder Item Recopy Test"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test32267-" + (Get-Date).ToString("s")
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
            $FolderName1 = "Test32267-" + (Get-Date).ToString("s")
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Note"
            $NewFolder1.Save($NewFolder.Id) 
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = "Test322672-" + (Get-Date).ToString("s")
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
        
        $data.Folder1 = ("\" + $RootPath + "\" + $FolderName + "\" + $FolderName1)
        $data.Folder2 = ("\" + $RootPath + "\" + $FolderName + "\" + $FolderName2)
        Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
        Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
        $Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $data.Folder2
        $HasNoteFailed = $true
        $Okay = $false
		foreach($ItemId in $Data.ItemIds){
			$Okay = $true;
			$btBinaryTreeMid = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Microsoft.Exchange.WebServices.Data.DefaultExtendedPropertySet]::PublicStrings, "BTOriginalMid", [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
			$sfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo($btBinaryTreeMid,[System.Convert]::ToBase64String($ItemId))
			$ivItemView = New-Object Microsoft.Exchange.WebServices.Data.ItemView(1) 
			$fiItems =  $Folder.FindItems($sfSearchFilter,$ivItemView)
			write-host ("Number of Items found" + $fiItems.Items.Count)
			if($fiItems.Items.Count -eq 1){
				$fiItems.Items[0].Delete([Microsoft.Exchange.WebServices.Data.DeleteMode]::HardDelete)
			}
			else{
				$HasNoteFailed = $false;
			}
		
		}
		if(($HasNoteFailed) -band $Okay){         
            $TestResults.Data = $data
            $TestResults.TestResult = "Succeeded"
            $session.Runspace.Close()
            $session.Runspace.Dispose()
        }
        else{
            $TestResults.TestResult = "Failed"
        }
        
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test32267")) {
            $Script:TestResults["Test32267"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test32267", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath + "\" + $FolderName) -TargetCopyPath ("\" + $RootPath)
        }
		
    }
}