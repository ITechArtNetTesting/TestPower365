function Invoke-P365FindContact{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
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
		##FindMessage Message
		if([String]::IsNullOrEmpty($FolderPath)){
			$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Contacts,$MailboxName)   
			$ContactsFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
			write-host "Contact Folder Found"
		}
		else{
			$ContactsFolder = Get-P365FolderFromPath -FolderPath $FolderPath -TargetMailbox
			if($ContactsFolder -ne $null){
			    write-host "Contact Folder Found"
			}
			else{
				write-host "Contacts Folder not Found"
			}
			
		}	
		
		$btBinaryTreeMid = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Microsoft.Exchange.WebServices.Data.DefaultExtendedPropertySet]::PublicStrings, "BTOriginalMid", [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
		$sfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo($btBinaryTreeMid,[System.Convert]::ToBase64String($MessageId))
		$ivItemView = New-Object Microsoft.Exchange.WebServices.Data.ItemView(1) 
		$fiItems =  $ContactsFolder.FindItems($sfSearchFilter,$ivItemView)
		write-host ("Number of Items found" + $fiItems.Items.Count)
		if($fiItems.Items.Count -eq 1){
			return, $fiItems.Items[0]
		}
	}
}