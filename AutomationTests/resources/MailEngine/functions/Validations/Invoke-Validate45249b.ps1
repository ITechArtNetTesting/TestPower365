function Invoke-Validate45249b{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox
    )  
 	Begin
	 {

		if($Script:TestResults.ContainsKey("Test45249b")){
			$Script:TestResults["Test45249b"].ValidationResult = "Failed"	
			$Script:TestResults["Test45249b"].OverAllResult = "Failed"	
			$Okay = $false
			$Script:TestResults["Test45249b"].ValidationLastRun = (Get-Date)
			$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::RecoverableItemsPurges,$Script:TargetMailbox)    
			$Folder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($Script:TargetService,$folderid)
			$HasNoteFailed = $true
			foreach($ItemId in $Script:TestResults["Test45249b"].Data.ItemIds){
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
			if(($HasNoteFailed) -band $Okay){
					$Script:TestResults["Test45249b"].ValidationResult = "Succeeded"
					$Script:TestResults["Test45249b"].OverAllResult = "Successful"
			}
			else{
				$Script:TestResults["Test45249b"].ValidationResult = "Failed"	
				$Script:TestResults["Test45249b"].OverAllResult = "Failed"		
			}
			Write-Host "Done" -ForegroundColor Green
		}
		else{
		   Write-Host "No Test exists run test first" -ForegroundColor Red
		}
		
     }
}