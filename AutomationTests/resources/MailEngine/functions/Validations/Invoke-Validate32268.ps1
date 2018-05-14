function Invoke-Validate32268{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test32268")){
			$Script:TestResults["Test32268"].ValidationResult = "Failed"	
			$Script:TestResults["Test32268"].OverAllResult = "Failed"	
			$Okay = $false
			$Script:TestResults["Test32268"].ValidationLastRun = (Get-Date)
			$Script:TestResults["Test32268"].Data.Folder3
			$Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test32268"].Data.Folder
			if($Folder -ne $null){
				write-host $Script:TestResults["Test32268"].Data
				$Itemid = new-object Microsoft.Exchange.WebServices.Data.ItemId($Script:TestResults["Test32268"].Data.TargetMessage)   
				$Email = [Microsoft.Exchange.WebServices.Data.EmailMessage]::Bind($Script:TargetService,$Itemid)  
				if($Email -ne $null){
					$Okay =$true
				}
			}
			if($Okay){
					$Script:TestResults["Test32268"].ValidationResult = "Succeeded"
					$Script:TestResults["Test32268"].OverAllResult = "Succeeded"  
			}
			else{
				$Script:TestResults["Test32268"].ValidationResult = "Failed"	
				$Script:TestResults["Test32268"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}