function Invoke-Test30513 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 6, Mandatory = $false)][switch]$RunDelta,
		[Parameter(Mandatory = $true)][String]$RootPath,
		[Parameter(Mandatory = $true)][String]$TargetRootPath
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
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath $RootPath -SourceMailbox
        #Move Contact to New folder
        $NewFolder = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
        $FolderName = "Test30513-" + (Get-Date).ToString("s")
        $NewFolder.DisplayName = $FolderName
        $NewFolder.FolderClass = "IPF.Note"
        #Define Folder Veiw Really only want to return one object  
        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)  
        #Define a Search folder that is going to do a search based on the DisplayName of the folder  
        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $NewFolder.DisplayName)  
        #Do the Search  
        $EntryIdVal = $null
        $data = "" | Select Folder1, Folder2, Folder3, MessageId,Location,StartTime,EndTime
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
            $data.Folder1 = ($RootPath + "\" + $FolderName)
            $NewFolder1 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName1 = $FolderName
            $NewFolder1.DisplayName = $FolderName1
            $NewFolder1.FolderClass = "IPF.Appointment"			
            $NewFolder1.Save($NewFolder.Id)            
            $NewFolder2 = new-object Microsoft.Exchange.WebServices.Data.Folder($service)  
            $FolderName2 = $FolderName
            $NewFolder2.DisplayName = $FolderName2
            $NewFolder2.FolderClass = "IPF.Appointment"			
            $NewFolder2.Save($NewFolder1.Id)
            $data.Folder2 = ($RootPath + "\" + $FolderName + "\" + $FolderName1)
            $data.Folder3 = ($RootPath + "\" + $FolderName + "\" + $FolderName1 + "\" + $FolderName2)
            $Appointment = New-Object Microsoft.Exchange.WebServices.Data.Appointment -ArgumentList $service
            $Appointment.Subject = "Appointment test - 30513 "
            $Appointment.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
            $Appointment.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
            $Appointment.Body.Text = "Body" 
            $Appointment.Start = Get-Date
            $Appointment.End = (Get-Date).AddHours(1)
            $Appointment.Location = "The Location"
            $Appointment.Save($NewFolder2.Id,[Microsoft.Exchange.WebServices.Data.SendInvitationsMode]::SendToNone)                
            $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
            $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
            $psPropertySet.Add($PR_ENTRYID)
            $Appointment.Load($psPropertySet)
            $EntryIdVal = $null		
            [Void]$Appointment.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)  
            $data.MessageId = $EntryIdVal
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            $Appointment.Start = Get-Date
            write-host "Current TimeZone " + $Appointment.TimeZone   
            $data.StartTime = (Get-Date).AddDays(1)    
            $data.EndTime = (Get-Date).AddDays(1).AddHours(1)     
            $Appointment.Start = $data.StartTime
            $Appointment.End=  $data.EndTime     
            $Appointment.Update([Microsoft.Exchange.WebServices.Data.ConflictResolutionMode]::AlwaysOverwrite,[Microsoft.Exchange.WebServices.Data.SendInvitationsOrCancellationsMode]::SendToNone)

        }  
        else {  
            throw ("Folder already Exist with that Name")  
		
        }     
        $TestResults = "" | Select TestCase, Description, TestLastRun, TestResult, Data, ValidationLastRun, ValidationResult, OverAllResult
        $TestResults.TestCase = "30513"
        $TestResults.Description = "Appointment Subject Change Check"
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
        if ($Script:TestResults.ContainsKey("Test30513")) {
            $Script:TestResults["Test30513"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test30513", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $RootPath) -TargetCopyPath ("\" + $TargetRootPath)
        }
        Write-Host "Done" -ForegroundColor Green
    }

}