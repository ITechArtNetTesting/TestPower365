function Get-P365FolderFromPath{
	param (
        	[Parameter(Position=0, Mandatory=$false)] [switch]$SourceMailbox,
			[Parameter(Position=1, Mandatory=$false)] [switch]$TargetMailbox,
			[Parameter(Position=2, Mandatory=$false)] [switch]$Archive,
			[Parameter(Position=0, Mandatory=$true)] [string]$FolderPath
		  )
	process{
        if($TargetMailbox.IsPresent){
			$service = $Script:TargetService
			$MailboxName = $Script:TargetMailbox
		}
		else{
			$service = $Script:SourceService
			$MailboxName = $Script:SourceMailbox
		}
		## Find and Bind to Folder based on Path  
		#Define the path to search should be seperated with \  
		#Bind to the MSGFolder Root  
		$folderid = new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::MsgFolderRoot,$MailboxName)   
		if($Archive.IsPresent){
			$folderid = new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::ArchiveMsgFolderRoot,$MailboxName) 
		}
		$tfTargetFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service,$folderid)  
		#Split the Search path into an array  
		$fldArray = $FolderPath.Split("\") 
		 #Loop through the Split Array and do a Search for each level of folder 
		for ($lint = 1; $lint -lt $fldArray.Length; $lint++) { 
	        #Perform search based on the displayname of each folder level 
	        $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1) 
			if(![string]::IsNullOrEmpty($PropertySet)){
				$fvFolderView.PropertySet = $PropertySet
			}
	        $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName,$fldArray[$lint]) 
	        $findFolderResults = $service.FindFolders($tfTargetFolder.Id,$SfSearchFilter,$fvFolderView) 
	        if ($findFolderResults.TotalCount -gt 0){ 
	            foreach($folder in $findFolderResults.Folders){ 
	                $tfTargetFolder = $folder                
	            } 
	        } 
	        else{ 
	            Write-host ("Error Folder Not Found check path and try again")  
	            $tfTargetFolder = $null  
	            break  
	        }     
	    }  
		if($tfTargetFolder -ne $null){
			return, [Microsoft.Exchange.WebServices.Data.Folder]$tfTargetFolder
		}
		else{
			return, $null
		}
	}
}