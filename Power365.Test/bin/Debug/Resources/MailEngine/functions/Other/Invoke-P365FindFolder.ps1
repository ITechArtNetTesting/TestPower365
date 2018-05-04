function Invoke-P365FindFolder{
    param( 
		[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
		[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
		[Parameter(Position=4, Mandatory=$false)] [switch]$Archive,
		[Parameter(Position=2, Mandatory=$false)] [Byte[]]$FolderId,
		[Parameter(Position=3, Mandatory=$false)] [String]$ParentFolderPath
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
		if([String]::IsNullOrEmpty($ParentFolderPath)){
			$folderid= new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::Inbox,$MailboxName)   
			$InboxFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)
			write-host "Inbox Folder Found"
		}
		else{
			if($Archive.IsPresent){
				$InboxFolder = Get-P365FolderFromPath -FolderPath $ParentFolderPath -TargetMailbox -Archive
			}else{
				$InboxFolder = Get-P365FolderFromPath -FolderPath $ParentFolderPath -TargetMailbox
			}			
			if($InboxFolder -ne $null){
			    write-host "Folder Found"
			}
			else{
				write-host "Folder not Found"
			}
			
		}
		##FindMessage Message
		$btBinaryTreeMid = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition([Microsoft.Exchange.WebServices.Data.DefaultExtendedPropertySet]::PublicStrings, "BTOriginalFid", [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
		$sfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo($btBinaryTreeMid,[System.Convert]::ToBase64String($FolderId))
		$fvItemView = New-Object Microsoft.Exchange.WebServices.Data.FolderView(100) 
		$fiItems =  $InboxFolder.FindFolders($sfSearchFilter,$fvItemView)
		write-host ("Number of Folders found" + $fiItems.Folders.Count)
		if($fiItems.Folders.Count -eq 1){
			return, $fiItems.Folders[0]
		}
	}
}