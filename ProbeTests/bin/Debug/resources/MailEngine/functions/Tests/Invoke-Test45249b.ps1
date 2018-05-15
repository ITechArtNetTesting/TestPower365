function Invoke-Test45249b {
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
        $TestResults.TestCase = "45249b"
        $TestResults.Description = "DeletedItemr Item Copy Test"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        $folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::RecoverableItemsPurges,$MailboxName)   
        $Folder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
        $data = "" | Select Folder1,Folder2,Folder3,ItemIds
        $data.ItemIds = New-Object "System.Collections.Generic.List[Byte[]]"
        for ($Im = 0; $Im -lt 10; $Im++) {
                $Email = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $service
                $Email.Subject = "Email test - " + $Im
                #$Post.ToRecipients.Add($Script:SourceMailbox) 
                $Email.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
                $Email.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
                $Email.Body.Text = "Body" 
                $Email.Save($Folder.Id)            
                $PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
                $psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
                $psPropertySet.Add($PR_ENTRYID)
                $Email.Load($psPropertySet)
                $EntryIdVal = $null		
                [Void]$Email.TryGetProperty($PR_ENTRYID, [ref]$EntryIdVal)  
                $data.ItemIds.Add($EntryIdVal)      
        }
        if($Folder -ne $null){             
            $TestResults.Data = $data
            $TestResults.TestResult = "Succeeded"
            $session.Runspace.Close()
            $session.Runspace.Dispose()
        }
        else {
            $TestResults.TestResult = "Failed"
        }
        
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test45249b")) {
            $Script:TestResults["Test45249b"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test45249b", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            Invoke-P365MailboxCopy
        }
		
    }
}