function Invoke-Validate32282{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test32282")){
			$Script:TestResults["Test32282"].ValidationResult = "Failed"	
			$Script:TestResults["Test32282"].OverAllResult = "Failed"	
			$Okay = $true
			$Script:TestResults["Test32282"].ValidationLastRun = (Get-Date)
			$Script:TestResults["Test32282"].Data.Folder2
			$Folder = Get-P365PublicFolderFromPath -TargetMailbox -FolderPath $Script:TestResults["Test32282"].Data.Folder2
			if($Folder -ne $null) 
			{
				$itemCount = $Script:TestResults["Test32282"].Data.ItemIds.Count
				if($itemCount -ne 10)
				{
					Write-Host "Found: $itemCount"
					$Okay = $false
				}

				foreach ($ItemId in $Script:TestResults["Test32282"].Data.ItemIds)
				{
					$btBinaryTreeMid = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Microsoft.Exchange.WebServices.Data.DefaultExtendedPropertySet]::PublicStrings, "BTOriginalMid", [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
					$sfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo($btBinaryTreeMid, [System.Convert]::ToBase64String($ItemId))
					$ivItemView = New-Object Microsoft.Exchange.WebServices.Data.ItemView(1) 
					$fiItems = $Folder.FindItems($sfSearchFilter, $ivItemView)
					write-host ("Number of Items found: " + $fiItems.Items.Count)
					$message = $fiItems.Items[0];

					$message.Load()
					$subject = $message.Subject.ToString()
					if ($subject -eq "Change 1234") {
						Write-Host  "Found Correct Subject: $subject"
					}
					else {
						Write-Host "Failed to find item: $ItemId"
						$Okay = $false
					}
				}
			}
			else
			{
				$Okay = $false
			}
			if($Okay){
					$Script:TestResults["Test32282"].ValidationResult = "Succeeded"
					$Script:TestResults["Test32282"].OverAllResult = "Succeeded"  
			}
			else{
				$Script:TestResults["Test32282"].ValidationResult = "Failed"	
				$Script:TestResults["Test32282"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}