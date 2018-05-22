function Connect-P365Mailboxes{
    param( 
    	[Parameter(Position=0, Mandatory=$true)] [string]$SourceMailboxName,
		[Parameter(Position=1, Mandatory=$true)] [System.Management.Automation.PSCredential]$SourceCredentials,
		[Parameter(Position=2, Mandatory=$false)] [switch]$useImpersonation,
		[Parameter(Position=3, Mandatory=$true)] [string]$TargetMailboxName,
		[Parameter(Position=4, Mandatory=$true)] [System.Management.Automation.PSCredential]$TargetCredentials,
		[Parameter(Position=5, Mandatory=$false)] [string]$SourceURL,
		[Parameter(Position=6, Mandatory=$false)] [string]$TargetURL,
		[Parameter(Position=7, Mandatory=$false)] [string]$SourceAutoDiscoverOverRide,
		[Parameter(Position=8, Mandatory=$false)] [string]$TargetAutoDiscoverOverRide,
		[Parameter(Position=9, Mandatory=$false)] [string]$SourcePowerShellOverRide,
		[Parameter(Position=10, Mandatory=$false)] [string]$TargetPowerShellOverRide,
		[Parameter(Position=11, Mandatory=$false)] [string]$LogPath
    )  
 	Begin
	 {
		$Script:SourceService = Connect-Exchange -MailboxName $SourceMailboxName -Credentials $SourceCredentials -url $SourceURL
		$Script:SourceMailbox = $SourceMailboxName
		if($useImpersonation.IsPresent){
			$Script:SourceService.ImpersonatedUserId = new-object Microsoft.Exchange.WebServices.Data.ImpersonatedUserId([Microsoft.Exchange.WebServices.Data.ConnectingIdType]::SmtpAddress, $SourceMailboxName)
		}
		$Script:TargetService = Connect-Exchange -MailboxName $TargetMailboxName -Credentials $TargetCredentials -url $TargetURL
		$Script:TargetMailbox = $TargetMailboxName
		if($useImpersonation.IsPresent){
			$Script:SourceService.ImpersonatedUserId = new-object Microsoft.Exchange.WebServices.Data.ImpersonatedUserId([Microsoft.Exchange.WebServices.Data.ConnectingIdType]::SmtpAddress, $TargetMailboxName)
		}
		Write-Host "Connect to Source and Target Mailbox"
		$Script:TestResults = New-Object 'system.collections.generic.dictionary[[string],[psobject]]'
		$Script:SourcePSCreds = $SourceCredentials
		$Script:TargetPSCreds = $TargetCredentials
		$Script:SourceOnPrem = $false
		$Script:TargetOnPrem = $false
		$Script:SourceAutoDiscoverOverRide = ""
		$Script:TargetAutoDiscoverOverRide = ""
		$Script:SourcePowerShellOverRide = ""
		$Script:TargetPowerShellOverRide = ""
		$Script:LogPath = "C:\temp"
		if(![String]::IsNullOrEmpty($SourceAutoDiscoverOverRide)){
			$Script:SourceOnPrem = $true
			$Script:SourceAutoDiscoverOverRide = $SourceAutoDiscoverOverRide
		}
		if(![String]::IsNullOrEmpty($TargetAutoDiscoverOverRide)){
			$Script:TargetOnPrem = $true
			$Script:TargetAutoDiscoverOverRide = $TargetAutoDiscoverOverRide
		}
		if(![String]::IsNullOrEmpty($SourcePowerShellOverRide)){
			$Script:SourceOnPrem = $true
			$Script:SourcePowerShellOverRide = $SourcePowerShellOverRide
		}
		if(![String]::IsNullOrEmpty($TargetPowerShellOverRide)){
			$Script:TargetOnPrem = $true
			$Script:TargetPowerShellOverRide = $TargetPowerShellOverRide
		}
		if(![String]::IsNullOrEmpty($LogPath)){
			$Script:LogPath = $LogPath
		}
	 }
}