function Invoke-Validate30379{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30379")){
			$Script:TestResults["Test30379"].ValidationResult = "Failed"	
			$Script:TestResults["Test30379"].OverAllResult = "Failed"	
			$Okay = $false
			$Script:TestResults["Test30379"].ValidationLastRun = (Get-Date)
			$Script:TestResults["Test30379"].Data.Folder2
			$Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test30379"].Data.Folder
			$HasNoteFailed = $true
			foreach($ItemId in $Script:TestResults["Test30379"].Data.ItemIds){
				$Okay = $true;
				$btBinaryTreeMid = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Microsoft.Exchange.WebServices.Data.DefaultExtendedPropertySet]::PublicStrings, "BTOriginalMid", [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
				$sfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo($btBinaryTreeMid,[System.Convert]::ToBase64String($ItemId))
				$ivItemView = New-Object Microsoft.Exchange.WebServices.Data.ItemView(1) 
				$fiItems =  $Folder.FindItems($sfSearchFilter,$ivItemView)
				write-host ("Number of Items found" + $fiItems.Items.Count)
				if($fiItems.Items.Count -eq 1){
					
				}
				else{
					$HasNoteFailed = $false;
				}
			
			}
			if($HasNoteFailed -band $Okay){
				$Script:TestResults["Test30379"].ValidationResult = "Succeeded"
				$Script:TestResults["Test30379"].OverAllResult = "Succeeded"  

			}
			else{
				$Script:TestResults["Test30379"].ValidationResult = "Failed"	
				$Script:TestResults["Test30379"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}