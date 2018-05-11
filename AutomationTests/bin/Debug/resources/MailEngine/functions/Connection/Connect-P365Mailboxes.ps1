function Connect-P365Mailboxes{
    param( 
    	[Parameter(Position=0, Mandatory=$true)] [string]$SourceMailboxName,
		[Parameter(Position=1, Mandatory=$true)] [System.Management.Automation.PSCredential]$SourceCredentials,
		[Parameter(Position=2, Mandatory=$false)] [switch]$useImpersonation,
		[Parameter(Position=3, Mandatory=$true)] [string]$TargetMailboxName,
		[Parameter(Position=4, Mandatory=$true)] [System.Management.Automation.PSCredential]$TargetCredentials,
		[Parameter(Position=5, Mandatory=$false)] [string]$SourceURL,
		[Parameter(Position=6, Mandatory=$false)] [string]$TargetURL
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
	 }
}