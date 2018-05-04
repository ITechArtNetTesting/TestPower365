function Invoke-P365FindMessageReply{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=4, Mandatory=$false)] [switch]$Archive,
		[Parameter(Position=2, Mandatory=$false)] [String]$MessageId,
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
			$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$MailboxName)   
			$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
			write-host "Inbox Folder Found"
		}
		else{
			if($Archive.IsPresent){
				$InboxFolder = Get-P365FolderFromPath -FolderPath $FolderPath -TargetMailbox -Archive
			}else{
				$InboxFolder = Get-P365FolderFromPath -FolderPath $FolderPath -TargetMailbox
			}			
			if($InboxFolder -ne $null){
			    write-host "Folder Found"
			}
			else{
				write-host "Folder not Found"
			}
			
		}
		##FindMessage Message
		$InReplyTo = new-object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x1042,[Microsoft.Exchange.WebServices.Data.MapiPropertyType]::String);
		$sfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo($InReplyTo,$MessageId)
		$ivItemView = New-Object Microsoft.Exchange.WebServices.Data.ItemView(1) 
		$fiItems =  $InboxFolder.FindItems($sfSearchFilter,$ivItemView)
		write-host ("Number of Items found" + $fiItems.Items.Count)
		if($fiItems.Items.Count -eq 1){
			return, $fiItems.Items[0]
		}
	}
}