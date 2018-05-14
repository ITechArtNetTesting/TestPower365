function Invoke-Validate30122{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30122")){
			$Script:TestResults["Test30122"].ValidationResult = "Failed"	
			$Script:TestResults["Test30122"].OverAllResult = "Failed"	
			$Okay = $false
			$Script:TestResults["Test30122"].ValidationLastRun = (Get-Date)
			$f2 = $Script:TestResults["Test30122"].Data.Folder2
			Write-Host "Folder2: $f2"
			$Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test30122"].Data.Folder2
			if($Folder -ne $null){
				$okay = $true;
				try{
					$f1 = $Script:TestResults["Test30122"].Data.Folder1
					Write-Host "Folder1: $f1"
					$Folder2 = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test30122"].Data.Folder1
					if($Folder2 -ne $null){
						$okay = $false
					}
				}catch{}
				
			}
			if($Okay){
				$Script:TestResults["Test30122"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30122"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test30122"].ValidationResult = "Failed"	
				$Script:TestResults["Test30122"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}