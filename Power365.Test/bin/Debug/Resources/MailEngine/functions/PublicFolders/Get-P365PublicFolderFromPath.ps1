function Get-P365PublicFolderFromPath {
    param (
        [Parameter(Position = 0, Mandatory = $false)] [switch]$SourceMailbox,
        [Parameter(Position = 1, Mandatory = $false)] [switch]$TargetMailbox,
        [Parameter(Position = 2, Mandatory = $false)] [switch]$Archive,
        [Parameter(Position = 0, Mandatory = $true)] [string]$FolderPath
		  )
    process {
        if ($TargetMailbox.IsPresent) {
            $service = $Script:TargetService
            $MailboxName = $Script:TargetMailbox
            $creds = New-Object System.Net.NetworkCredential($Script:TargetPSCreds.UserName.ToString(), $Script:TargetPSCreds.GetNetworkCredential().password.ToString()) 
        }
        else {
            $service = $Script:SourceService
            $MailboxName = $Script:SourceMailbox
            $creds = New-Object System.Net.NetworkCredential($Script:SourcePSCreds.UserName.ToString(), $Script:SourcePSCreds.GetNetworkCredential().password.ToString()) 
        }
        $ExchangeVersion = [Microsoft.Exchange.WebServices.Data.ExchangeVersion]::Exchange2013_SP1
        $AutoDiscoverService = New-Object  Microsoft.Exchange.WebServices.Autodiscover.AutodiscoverService($ExchangeVersion);
        $AutoDiscoverService.Credentials = $creds
        $AutoDiscoverService.EnableScpLookup = $false;
        $AutoDiscoverService.RedirectionUrlValidationCallback = {$true};
        $AutoDiscoverService.PreAuthenticate = $true;
        $AutoDiscoverService.KeepAlive = $false;      
        $gsp = $AutoDiscoverService.GetUserSettings($MailboxName, [Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::PublicFolderInformation);
        $PublicFolderInformation = $null
        if ($gsp.Settings.TryGetValue([Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::PublicFolderInformation, [ref] $PublicFolderInformation)) {
            write-host ("Public Folder Routing Information Header : " + $PublicFolderInformation)  
            $service.HttpHeaders.Add("X-AnchorMailbox", $PublicFolderInformation)           
					
        } 
        ## Find and Bind to Folder based on Path  
        #Define the path to search should be seperated with \  
        #Bind to the MSGFolder Root  
        $fldId = Get-P365PublicFolderIdFromPath -FolderPath $FolderPath -SmtpAddress $MailboxName -service $service -TargetMailbox:$TargetMailbox.IsPresent
        $fldFolderId = New-Object Microsoft.Exchange.WebServices.Data.FolderId($fldId)
        $Folder = [Microsoft.Exchange.WebServices.Data.Folder]::Bind($service, $fldFolderId)
        return $Folder
    }
}