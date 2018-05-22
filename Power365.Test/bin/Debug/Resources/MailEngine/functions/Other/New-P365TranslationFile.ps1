function New-P365TranslationFile{
    param( 
        [Parameter(Position=2, Mandatory=$true)] [String]$SourceAddress,
        [Parameter(Position=3, Mandatory=$true)] [String]$TargetAddress,
        [Parameter(Position=4, Mandatory=$false)] [String]$FileName,
        [Parameter(Position=5, Mandatory=$false)] [switch]$NoEXAddress
    )  
 	Begin
	 {
       if([String]::IsNullOrEmpty($FileName)){
            $FileName = ($script:ModuleRoot + '\working\' + [guid]::NewGuid().ToString() + ".csv")
        }
        if(!$NoEXAddress.IsPresent){
            $SourceAd = GetAutoDiscoverSettings -Address $SourceAddress
            $TargetAd = GetAutoDiscoverSettings -Address $TargetAddress -TargetMailbox
                
            "type,sourcemailboxdn,targetmailboxdn,displayname,sourcemailboxsmtp,targetmailboxsmtp,sourceimaddress,targetimaddress" | Out-File $FileName -Append
            $sourcemailboxdn = $SourceAd.Settings[[Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::UserDN]
            $targetmailboxdn = $TargetAd.Settings[[Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::UserDN]
            $displayname = $SourceAd.Settings[[Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::UserDisplayName]
            "Mailbox," + $sourcemailboxdn + "," + $targetmailboxdn + "," + $displayname + "," + $SourceAddress + "," + $TargetAddress + "," + $SourceAddress + "," + $TargetAddress | Out-File $FileName -Append
        }else{
            "type,sourcemailboxdn,targetmailboxdn,displayname,sourcemailboxsmtp,targetmailboxsmtp,sourceimaddress,targetimaddress" | Out-File $FileName -Append           
            "Mailbox," + $SourceAddress + "," + $TargetAddress + "," + $SourceAddress + "," + $SourceAddress + "," + $TargetAddress + "," + $SourceAddress + "," + $TargetAddress | Out-File $FileName -Append
        }
  

        return $FileName
		
	}
}

function GetAutoDiscoverSettings{
	param (
	        [Parameter(Position=3, Mandatory=$true)] [String]$Address,
			[Parameter(Position=2, Mandatory=$false)] [switch]$TargetMailbox
		  )
	process{
        $ExchangeVersion = [Microsoft.Exchange.WebServices.Data.ExchangeVersion]::Exchange2013_SP1
        $adService = New-Object Microsoft.Exchange.WebServices.AutoDiscover.AutodiscoverService($ExchangeVersion);
        if($TargetMailbox.IsPresent){
            $adService.Credentials = $Script:TargetService.Credentials
        }
        else{
             $adService.Credentials = $Script:SourceService.Credentials
        }
		$adService.EnableScpLookup = $false;
		$adService.RedirectionUrlValidationCallback = {$true}
		$UserSettings = new-object Microsoft.Exchange.WebServices.Autodiscover.UserSettingName[] 3
		$UserSettings[0] = [Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::UserDN
		$UserSettings[1] = [Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::InternalRpcClientServer
        $UserSettings[2] = [Microsoft.Exchange.WebServices.Autodiscover.UserSettingName]::UserDisplayName
        $ByPass = ""
        if($TargetMailbox.IsPresent){
            if(![String]::IsNullOrEmpty($Script:TargetAutoDiscoverOverRide)){
                $uri=[system.URI] ($Script:TargetAutoDiscoverOverRide + "/autodiscover.svc")
                $adService.Url =$uri
            } 
        }
        else{
            if(![String]::IsNullOrEmpty($Script:SourceAutoDiscoverOverRide)){
                $uri=[system.URI] ($Script:SourceAutoDiscoverOverRide + "/autodiscover.svc")
                $adService.Url =$uri
            } 
        }  
		$adResponse = $adService.GetUserSettings($Address , $UserSettings);
		return $adResponse
	}
}