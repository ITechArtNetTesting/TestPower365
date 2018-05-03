function Invoke-Validate30143{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test30143")){
			$Script:TestResults["Test30143"].ValidationResult = "Failed"	
			$Script:TestResults["Test30143"].OverAllResult = "Failed"	
			$Okay = $false
			$Script:TestResults["Test30143"].ValidationLastRun = (Get-Date)
			$Script:TestResults["Test30143"].Data.Folder2
			$Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test30143"].Data.Folder2
			$HasNoteFailed = $true
			foreach($ItemId in $Script:TestResults["Test30143"].Data.ItemIds){
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
			if((!$HasNoteFailed) -band $Okay){
				$Folder1 = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test30143"].Data.Folder1
				$Folder2 = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test30143"].Data.Folder2
				if($folder1 -ne $null -band $folder2 -ne $null){
					$Script:TestResults["Test30143"].ValidationResult = "Succeeded"
					$Script:TestResults["Test30143"].OverAllResult = "Successful"
				}

			}
			else{
				$Script:TestResults["Test30143"].ValidationResult = "Failed"	
				$Script:TestResults["Test30143"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}