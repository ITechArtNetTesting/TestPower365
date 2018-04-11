function Invoke-Validate22507{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {
		if($TargetMailbox.IsPresent){
			$service = $Script:TargetService
			$MailboxName = $Script:TargetMailbox
		}
		else{
			$service = $Script:SourceService
			$MailboxName = $Script:SourceMailbox
		}
		if($Script:TestResults.ContainsKey("Test22507")){
			Write-Host ("First Folder " + $Script:TestResults["Test22507"].Data.OrginalFolderPath)			
			$OriginalMessage= Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22507"].Data.OrginalMessageId -FolderPath $Script:TestResults["Test22507"].Data.OrginalFolderPath
			Write-Host ("Second Folder " + $Script:TestResults["Test22507"].Data.NewFolderPath)
			$NewMessage = Invoke-P365FindMessage -TargetMailbox -MessageId $Script:TestResults["Test22507"].Data.NewMessageId -FolderPath $Script:TestResults["Test22507"].Data.NewFolderPath
		    $Okay = $false	
			$Script:TestResults["Test22507"].ValidationLastRun = (Get-Date)
			if($OriginalMessage -ne $null){
				write-host ("Moved Email Okay")
				if($NewMessage -ne $null){
					write-host ("New Email Okay")
					$Okay = $true;
				}
			}
			if($Okay){
				$Script:TestResults["Test22507"].ValidationResult = "Succeeded"
				$Script:TestResults["Test22507"].OverAllResult = "Successful"

			}
			else{
				$Script:TestResults["Test22507"].ValidationResult = "Failed"	
				$Script:TestResults["Test22507"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}