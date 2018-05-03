function Get-P365PublicFolderIdFromPath {
	
    [CmdletBinding()]
    param (
        [Parameter(Position = 0, Mandatory = $true)]
        [Microsoft.Exchange.WebServices.Data.ExchangeService]
        $service,
		
        [Parameter(Position = 1, Mandatory = $true)]
        [String]
        $FolderPath,
		
        [Parameter(Position = 2, Mandatory = $true)]
        [String]
        $SmtpAddress,
		
        [Parameter(Position = 3, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 4, Mandatory = $false)] [switch]$TargetMailbox
    )
    process {
        ## Find and Bind to Folder based on Path  
        #Define the path to search should be seperated with \  
        #Bind to the MSGFolder Root  
        $folderid = new-object Microsoft.Exchange.WebServices.Data.FolderId([Microsoft.Exchange.WebServices.Data.WellKnownFolderName]::PublicFoldersRoot)
        $psPropset = new-object Microsoft.Exchange.WebServices.Data.PropertySet([Microsoft.Exchange.WebServices.Data.BasePropertySet]::FirstClassProperties)
        $PR_REPLICA_LIST = New-Object Microsoft.Exchange.WebServices.Data.ExtendedPropertyDefinition(0x6698, [Microsoft.Exchange.WebServices.Data.MapiPropertyType]::Binary);
        $psPropset.Add($PR_REPLICA_LIST)
        $tfTargetFolder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $folderid, $psPropset)
        $PR_REPLICA_LIST_Value = $null
        if ($tfTargetFolder.TryGetProperty($PR_REPLICA_LIST, [ref]$PR_REPLICA_LIST_Value)) {
            $GuidAsString = [System.Text.Encoding]::ASCII.GetString($PR_REPLICA_LIST_Value, 0, 36);
            $HeaderAddress = new-object System.Net.Mail.MailAddress($service.HttpHeaders["X-AnchorMailbox"])
            $pfHeader = $GuidAsString + "@" + $HeaderAddress.Host
            write-host ("Root Public Folder Routing Information Header : " + $pfHeader)
            if($service.HttpHeaders.ContainsKey("X-PublicFolderMailbox")){
                $service.HttpHeaders["X-PublicFolderMailbox"] = $pfHeader
            }else{
                $service.HttpHeaders.Add("X-PublicFolderMailbox", $pfHeader)
            }
            
        }
        #Split the Search path into an array  
        $fldArray = $FolderPath.Split("\")
        #Loop through the Split Array and do a Search for each level of folder 
        for ($lint = 1; $lint -lt $fldArray.Length; $lint++) {
            #Perform search based on the displayname of each folder level 
            $fvFolderView = new-object Microsoft.Exchange.WebServices.Data.FolderView(1)
            $fvFolderView.PropertySet = $psPropset
            $SfSearchFilter = new-object Microsoft.Exchange.WebServices.Data.SearchFilter+IsEqualTo([Microsoft.Exchange.WebServices.Data.FolderSchema]::DisplayName, $fldArray[$lint])
            $findFolderResults = $service.FindFolders($tfTargetFolder.Id, $SfSearchFilter, $fvFolderView)
            if ($findFolderResults.TotalCount -gt 0) {
                foreach ($folder in $findFolderResults.Folders) {
                    $tfTargetFolder = $folder
                }
            }
            else {
                "Error Folder Not Found"
                $tfTargetFolder = $null
                break
            }
        }
        if ($tfTargetFolder -ne $null) {
            $PR_REPLICA_LIST_Value = $null
            if ($tfTargetFolder.TryGetProperty($PR_REPLICA_LIST, [ref]$PR_REPLICA_LIST_Value)) {
                $GuidAsString = [System.Text.Encoding]::ASCII.GetString($PR_REPLICA_LIST_Value, 0, 36);
                $HeaderAddress = new-object System.Net.Mail.MailAddress($service.HttpHeaders["X-AnchorMailbox"])
                $pfHeader = $GuidAsString + "@" + $HeaderAddress.Host
                write-host ("Target Public Folder Routing Information Header : " + $pfHeader)
                Set-PublicFolderContentRoutingHeader -service $service -MailboxName $SmtpAddress -pfAddress $pfHeader -TargetMailbox:$TargetMailbox.IsPresent
            }
            return $tfTargetFolder.Id.UniqueId.ToString()
        }
        else {
            throw "Folder not found"
        }
    }
}
