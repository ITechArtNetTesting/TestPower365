function Invoke-Test32268 {
    param( 
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 4, Mandatory = $false)] [switch]$RunDelta
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
        $TestResults.TestCase = "32268"
        $TestResults.Description = "Public Folder Target Folder Test"
        $TestResults.TestLastRun = (Get-Date)
        $TestResults.TestResult = "Failed"
        Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
        ##Create Message
        $pfRoot = Get-P365PublicFolderFromPath -FolderPath \Automation\Tests -SourceMailbox
        #Move Contact to New folder
        $data = Invoke-CreateFoldersAndItems -RootFolder $pfRoot -TestNumber 32268 -RootFolderPath "\Automation\Tests"
        Invoke-p365SyncPublicFolder -SourceFolderPath ("\" + $data.Folder1) -TargetCopyPath "\\Automation\tests"
        $tfTargetFolder = Get-P365PublicFolderFromPath -FolderPath $data.Folder3 -TargetMailbox
        $EmailMessage = New-Object Microsoft.Exchange.WebServices.Data.EmailMessage -ArgumentList $Script:TargetService
  		$EmailMessage.Subject = "Test32268 - " + (Get-Date).ToString()
  		#Add Recipients    
  		$EmailMessage.ToRecipients.Add("Test@fakeAddress.BinaryTree.com")  
  		$EmailMessage.Body = New-Object Microsoft.Exchange.WebServices.Data.MessageBody  
  		$EmailMessage.Body.BodyType = [Microsoft.Exchange.WebServices.Data.BodyType]::HTML  
  		$EmailMessage.Body.Text = "Body" 
  		$EmailMessage.From = $MailboxName
		#Set Sent Message Flags which means message wont appear as a Draft  
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)  
		$PR_Flags = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(3591, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Integer);  
		$EmailMessage.SetExtendedProperty($PR_Flags,"1")
		$EmailMessage.IsRead = $false  
		write-host ("Created " + $EmailMessage.Subject)
        $EmailMessage.Save($tfTargetFolder.Id)  
        $EmailMessage.Load()
        $data1 = "" | Select TargetMessage,Folder
        $data1.Folder = $data.Folder3
        write-host $EmailMessage.Id
        $data1.TargetMessage = $EmailMessage.Id
        if($data -ne $null){         
            $TestResults.Data = $data1
           
            $TestResults.TestResult = "Succeeded"
        }
        else{
            $TestResults.TestResult = "Failed"
        }
        $Script:TestResults.OverAllResult = "InComplete"
        if ($Script:TestResults.ContainsKey("Test32268")) {
            $Script:TestResults["Test32268"] = $TestResults
        }
        else {
            $Script:TestResults.Add("Test32268", $TestResults)
        }
        if ($RunDelta.IsPresent) {
            Get-p365TestResults
            # Write-host "Part 1 - Message Created"
            Invoke-p365SyncPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $data.Folder1) -TargetCopyPath "\\Automation\tests"
            Invoke-p365CopyPublicFolder -mappingfile $tfile -SourceFolderPath ("\" + $data.Folder1) -TargetCopyPath "\\Automation\tests"
        }
		
    }
}