function Invoke-P365ValidateFolderItems{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=1, Mandatory=$false)] [psObject]$SourceFolder,
		[Parameter(Position=1, Mandatory=$false)] [psObject]$TargetFolder

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
		$fldcol = @{}
		Import-Module ($script:ModuleRoot + '\engine\btT2TPSModule.psd1') -Force
		$PR_ENTRYID = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x0FFF,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary)
		$btBinaryTreeMid = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Microsoft.Exchange.WebServices.Data.DefaultExtendedPropertySet]::PublicStrings, "BTOriginalMid", [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
		$psPropertySet = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)  
		$psPropertySet.Add($PR_ENTRYID)
		$psPropertySet.Add($btBinaryTreeMid)
		$rvValue = $false;
		#try{
			$ivItemView =  New-Object Microsoft.Exchange.WebServices.Data.ItemView(1000)    
			$ivItemView.PropertySet = $psPropertySet
			$fiItems = $null    
			do{    
				$fiItems = $TargetFolder.Service.FindItems($TargetFolder.Id,$ivItemView)    
				#[Void]$service.LoadPropertiesForItems($fiItems,$psPropset)  
				foreach($Item in $fiItems.Items){      
					$EntryIdValue = $null
					if($Item.TryGetProperty($btBinaryTreeMid,[ref]$EntryIdValue)){
					  $HexValue = [System.BitConverter]::ToString($EntryIdValue).Replace("-","")
					   $fldcol.Add($HexValue.Tolower(),"")
					}
				}    
				$ivItemView.Offset += $fiItems.Items.Count    
			}while($fiItems.MoreAvailable -eq $true) 
			$ivItemView =  New-Object Microsoft.Exchange.WebServices.Data.ItemView(1000)    
			$ivItemView.PropertySet = $psPropertySet
			$fiItems = $null 
			$fail = $false   
			do{    
				$fiItems = $SourceFolder.Service.FindItems($SourceFolder.Id,$ivItemView)    
				#[Void]$service.LoadPropertiesForItems($fiItems,$psPropset)  
				foreach($Item in $fiItems.Items){      
					 $EntryIdValue = $null
					 if($Item.TryGetProperty($PR_ENTRYID,[ref]$EntryIdValue)){
						$HexValue = [System.BitConverter]::ToString($EntryIdValue).Replace("-","")
					    if(!$fldcol.ContainsKey($HexValue.Tolower())){
							$fail = $true;	
					   		}
						   else{
							   write-host("ItemFound")
						   }
					}
				}    
				$ivItemView.Offset += $fiItems.Items.Count    
			}while($fiItems.MoreAvailable -eq $true) 
			if(!$fail){
				$rvValue = $true;
			}
		#}catch{

		#}



		return $rvValue
     }
}