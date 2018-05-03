function Invoke-Validate30096{
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
		if($Script:TestResults.ContainsKey("Test30096")){
		$Folder  = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30096"].Data.Folder1)
		$TestResults = "" | Select TestCase,Description,TestLastRun,TestResult,Data,ValidationLastRun,ValidationResult,OverAllResult
		$Script:TestResults["Test30096"].ValidationResult = "Failed"	
		$Script:TestResults["Test30096"].OverAllResult = "Failed"	
		$Script:TestResults["Test30096"].ValidationLastRun = (Get-Date)	
		if($folder -ne $Null){
			$okay = $false
			#test permissions
			$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
			$psPropertySet.Add([Microsoft.Exchange.WebServices.Data.FolderSchema]::Permissions)
			$folder.Load($psPropertySet)
			foreach($fperm in $folder.Permissions){  
				if($fperm.UserId.PrimarySmtpAddress -ne $null){  
					write-host ($fperm.UserId.PrimarySmtpAddress)
					if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test30096"].Data.SecondTargetPermission.ToLower()){
						$okay = $true;
					}  
				}  
			}
			if($okay){
				$Script:TestResults["Test30096"].OverAllResult = "Successful"	
				$Script:TestResults["Test30096"].ValidationResult = "Succeeded"	
				$Folder2  = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath ($Script:TestResults["Test30096"].Data.Folder2)
				$Folder2.Load($psPropertySet)
				foreach($fperm in $Folder2.Permissions){  
					if($fperm.UserId.PrimarySmtpAddress -ne $null){  
						write-host ($fperm.UserId.PrimarySmtpAddress)
						if($fperm.UserId.PrimarySmtpAddress.ToLower() -eq $Script:TestResults["Test30096"].Data.SecondTargetPermission.ToLower()){
								$Script:TestResults["Test30096"].OverAllResult = "Failed"	
								$Script:TestResults["Test30096"].ValidationResult = "Failed"	
						}  
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