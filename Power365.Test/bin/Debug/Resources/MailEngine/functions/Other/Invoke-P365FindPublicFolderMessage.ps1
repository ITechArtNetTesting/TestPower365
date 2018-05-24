function Invoke-P365FindPublicFolderMessage{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=4, Mandatory=$false)] [switch]$Archive,
		[Parameter(Position=2, Mandatory=$false)] [Byte[]]$MessageId,
		[Parameter(Position=3, Mandatory=$false)] [String]$FolderPath
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
		$Folder = Get-P365PublicFolderFromPath -FolderPath $FolderPath -TargetMailbox
		##FindMessage Message
		$btBinaryTreeMid = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Microsoft.Exchange.WebServices.Data.DefaultExtendedPropertySet]::PublicStrings, "BTOriginalMid", [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
		$sfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo($btBinaryTreeMid,[System.Convert]::ToBase64String($MessageId))
		$ivItemView = New-Object Microsoft.Exchange.WebServices.Data.ItemView(1) 
		$fiItems =  $Folder.FindItems($sfSearchFilter,$ivItemView)
		write-host ("Number of Items found: " + $fiItems.Items.Count)
		if($fiItems.Items.Count -eq 1){
			return, $fiItems.Items[0]
		}
	}
}