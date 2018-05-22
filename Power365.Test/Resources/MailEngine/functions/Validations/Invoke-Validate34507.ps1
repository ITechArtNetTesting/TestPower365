function Invoke-Validate34507{
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
		$Script:TestResults["Test34507"].Data.Folder
		if($Script:TestResults.ContainsKey("Test34507")){
		$Folder  = Get-P365FolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test34507"].Data.Folder)
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$Script:TestResults["Test34507"].ValidationResult = "Failed"	
		$Script:TestResults["Test34507"].OverAllResult = "Failed"	
		$Script:TestResults["Test34507"].ValidationLastRun = (Get-Date)	
		if($folder -ne $Null){
			$okay = $false
			#test permissions
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
			$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
			$folder.Load($psPropertySet)
			foreach($fperm in $folder.Permissions){  
				if($fperm.UserId.PrimarySmtpAddress -ne $null){  
					write-host ($fperm.UserId.PrimarySmtpAddress)
					if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test34507"].Data.FirstTargetPermission.ToLower()){
						$Script:TestResults["Test34507"].OverAllResult = "Succeeded" 	
						$Script:TestResults["Test34507"].ValidationResult = "Succeeded"	
					}  
				}  
			}
		}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}