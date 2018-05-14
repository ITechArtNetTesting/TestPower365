function Invoke-Validate39557{
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
		if($Script:TestResults.ContainsKey("Test39557")){
		$Folder  = Get-P365FolderFromPath -TargetMailbox -FolderPath \Inbox
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$Script:TestResults["Test39557"].ValidationResult = "Failed"	
		$Script:TestResults["Test39557"].OverAllResult = "Failed"	
		$Script:TestResults["Test39557"].ValidationLastRun = (Get-Date)	
		if($folder -ne $Null){
			$okay = $true
			#test permissions
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
			$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
			$folder.Load($psPropertySet)
			foreach($fperm in $folder.Permissions){  
				if($fperm.UserId.PrimarySmtpAddress -ne $null){  
					write-host ($fperm.UserId.PrimarySmtpAddress)
					if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test39557"].Data.FirstTargetPermission.ToLower()){
						$okay = $false
					}  
				}  
			}
			if($okay){
				$Script:TestResults["Test39557"].OverAllResult = "Succeeded" 	
				$Script:TestResults["Test39557"].ValidationResult = "Succeeded"	
			}

		}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}