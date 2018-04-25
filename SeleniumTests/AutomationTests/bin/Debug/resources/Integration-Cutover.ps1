param([string]$tlogin = "admin@BTCloud9.Power365.Cloud", [string]$tpassword = "Password32", [string]$slogin = "admin@BTCloud7.Power365.Cloud", [string]$spassword = "Password31", [string]$mailboxremote5 = "P365AutoRemoteMBX5", [string]$mailboxremote5SMTP = "smtp:P365AutoRemoteMBX5@corp32.cmtsandbox.com", [string]$Equipremote2 = 'P365AutoRemoteEquip2', [string]$Roomremote2 ='P365AutoRemoteRoom2', [string]$mailboxremote5X500 = "x500:/o=FirstOrg/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=bf6efc15-5d43-423d-8b0a-46c05d947212",[string]$adlogin = "corp32\administrator", [string]$adpassword = "Password32", [string]$uri = "https://10.1.178.55/PowerShell", [string]$ForwardingSmtpAddressSource = "smtp:P365AutoRemoteMBX5@corp32.cmtsandbox.com")
function New-CredentialFromClear([string]$username, [string]$password)
{
	$ss = new-object System.Security.SecureString
	$password.ToCharArray() | % { $ss.AppendChar($_) }
	return new-object System.Management.Automation.PSCredential($username, $ss)
}

####Environment Information############
[System.Int32] $SleepSeconds = 10
[System.Int32] $TimeOutMinutes = 1
$Timeout = new-timespan -Seconds $TimeOutMinutes
$loop = 1
$Date = Get-Date -Format "MMddHHmm"
$RemoteMailbox = $mailboxremote5
$MBXTypeSource = "UserMailbox"
$MBXUsageLocation = 'US'
$HiddenFromAddressListsEnabled = 'True'
$ForwardingSmtpAddress = $smtp
$EmailAddressPolicyEnabled = [System.Convert]::ToBoolean("False")
$HiddenFromAddressListsEnabledSource = 'False'

$RemoteMailbox5 = $mailboxremote5
$RemoteMailbox5X500 = $mailboxremote5X500
$RemoteMailbox5SMTPAddress = $mailboxremote5SMTP

$RemoteEquip = $Equipremote2
$EquipMBXTypeSource = 'EquipmentMailbox'
$RemoteRoom = $Roomremote2
$RoomMBXTypeSource = 'RoomMailbox'

$ExchangeUserAccountControlSource ='AccountDisabled'
$UserAccountControlTarget = "False"

$Creds = New-CredentialFromClear $slogin $spassword
$Credt = New-CredentialFromClear $tlogin $tpassword
$Credtp = New-CredentialFromClear $adlogin $adpassword

$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck
$ResultCode = 0

if ($sw -eq $null)
{
    $sw = New-Object -TypeName System.Diagnostics.Stopwatch
}
##Connecting to Target Tenant
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Target Cloud")
            $SessionT = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $Credt -Authentication Basic -AllowRedirection
            Import-PSSession $SessionT
            Import-Module msonline
            Connect-MsolService -Credential $Credt
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }



Start-Sleep -Seconds 1

#####Starting validation steps#####

##TC28545 Verify Coex Cutover will not modify the target mailbox PrimarySMTP address

if ($ResultCode -eq 0)
{
  Try 
    {
    $sw.Restart()
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {
	        If (get-msoluser -SearchString  $RemoteMailbox)
	        {
		        $loop = 0
                $ResultCode = 0
	        }
	        Else
	        {
		        Write-Error 'Mailbox not found'
		        Start-Sleep -Seconds $SleepSeconds
	        }
        }
    }
    Catch
    {
	    Write-Error "Can not locate the mailbox"
    }
}

if ($ResultCode -eq 0)
{
    If ($TMBXSMTP = get-mailbox $RemoteMailbox | select-object EmailAddresses)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mailbox not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

$TMBXSMTPCompare = $TMBXSMTP.EmailAddresses -contains $RemoteMailbox5SMTPAddress 

if ($ResultCode -eq 0)
{

        if ($TMBXSMTPCompare)
        {
            $ResultCode = 0
            $ResultText = 'Target Mailbox PrimarySMTP has not been modified'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target Mailbox PrimarySMTP has been modified"
            $ResultText = 'Target Mailbox PrimarySMTP has been modified'
             Write-Host($ResultText)
        
        }
 }


 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC28545 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC28545 Failed'
     Write-Host($ResultText)
 }


 ##27857- Verify Target Mailbox will be converted to Equipment Mailbox, Contact Merged with the Mailbox and Forwarding Rule Disabled per Cutover Process 

if ($ResultCode -eq 0)
{
  Try 
    {
      $sw.Restart()
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {        
	        If (get-msoluser -SearchString  $RemoteEquip)
	        {
		        $loop = 0
                $ResultCode = 0
	        }
	        Else
	        {
		        Write-Error 'Mailbox not found'
		        Start-Sleep -Seconds $SleepSeconds
	        }
        }
    }
    Catch
    {
	    Write-Error "Can not locate the mailbox"
    }
}

if ($ResultCode -eq 0)
{
    If ($EquipMBXType = get-mailbox $RemoteEquip | select-object RecipientTypeDetails)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mailbox not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

if ($ResultCode -eq 0)
{

        if ($EquipMBXTypeSource.CompareTo($EquipMBXType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'EquipMailbox Cutover Successful'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "EquipMailbox not cutover correctly"
            $ResultText = 'EquipMailbox Cutover failed'
             Write-Host($ResultText)
        
        }
 }


 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC27857 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC27857 Failed'
     Write-Host($ResultText)
 }

  ##27856- Verify Target Mailbox will be converted to Room Mailbox, Contact Merged with the Mailbox and Forwarding Rule Disabled per Cutover Process 

if ($ResultCode -eq 0)
{
  Try 
    {
     $sw.Restart()
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {        
	        If (get-msoluser -SearchString  $RemoteRoom)
	        {
		        $loop = 0
                $ResultCode = 0
	        }
	        Else
	        {
		        Write-Error 'Mailbox not found'
		        Start-Sleep -Seconds $SleepSeconds
	        }
        }
    }
    Catch
    {
	    Write-Error "Can not locate the mailbox"
    }
}

if ($ResultCode -eq 0)
{
    If ($RoomMBXType = get-mailbox $RemoteRoom | select-object RecipientTypeDetails)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mailbox not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

if ($ResultCode -eq 0)
{

        if ($RoomMBXTypeSource.CompareTo($RoomMBXType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'RoomMailbox Cutover Successful'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "RoomMailbox not cutover correctly"
            $ResultText = 'RoomMailbox Cutover failed'
             Write-Host($ResultText)
        
        }
 }

   ##27859 32404- Verify DirSync will disable the User Account Control for Room/Resource Mailbox after they are converted per prepare process

if ($ResultCode -eq 0)
{
  Try 
    {
    $sw.Restart()
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {        
	        If (get-msoluser -SearchString  $RemoteRoom)
	        {
		        $loop = 0
                $ResultCode = 0
	        }
	        Else
	        {
		        Write-Error 'Mailbox not found'
		        Start-Sleep -Seconds $SleepSeconds
	        }
        }
    }
    Catch
    {
	    Write-Error "Can not locate the mailbox"
    }
}

if ($ResultCode -eq 0)
{
    If ($UAC = get-mailbox $RemoteRoom | select-object ExchangeUserAccountControl)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mailbox not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

if ($ResultCode -eq 0)
{

        if ($ExchangeUserAccountControlSource.CompareTo($UAC.ExchangeUserAccountControl) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'RoomMailbox Cutover Successful as disabled account'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "RoomMailbox not cutover correctly"
            $ResultText = 'RoomMailbox Cutover failed, aaccount was not disabled'
             Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC27859 and 32404 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC27859 and 32404 Failed'
     Write-Host($ResultText)
 }

   ##32394 Verify Mailbox Objects will be enabled during the cutover process


if ($ResultCode -eq 0)
{
  Try 
    {
    $sw.Restart()
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {        
	        If (get-msoluser -SearchString  $RemoteRoom)
	        {
		        $loop = 0
                $ResultCode = 0
	        }
	        Else
	        {
		        Write-Error 'Mailbox not found'
		        Start-Sleep -Seconds $SleepSeconds
	        }
        }
    }
    Catch
    {
	    Write-Error "Can not locate the mailbox"
    }
}

if ($ResultCode -eq 0)
{
    If ($UAC = get-msoluser -searchstring $mailboxremote5 | select-object BlockCredential)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mailbox not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

if ($ResultCode -eq 0)
{
        if ($UserAccountControlTarget.CompareTo($UAC.BlockCredential.ToString()) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Mailbox Cutover Successful as enable account'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Mailbox not cutover correctly"
            $ResultText = 'Mailbox Cutover failed, account was not enabled'
             Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32394 and 32404 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32394 and 32404 Failed'
     Write-Host($ResultText)
 }



Write-Host ("Removing Target Session")
Remove-pssession $SessionT
Start-Sleep -Seconds 5

Write-Host ("Connecting to Source Session")
##Connecting to Source Tenant
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Source Cloud")
            $SessionS = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $Creds -Authentication Basic -AllowRedirection
            Import-PSSession $SessionS
            Import-Module msonline
            Connect-MsolService -Credential $Creds
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }



Start-Sleep -Seconds 1


##TC 32203 - Verify Cutover Process will Add Forwarding Rule to the Source Mailbox with Target User Primary Email Address and Set DeliverToMailboxandForward to True



 if ($ResultCode -eq 0)
{
    If ($MBXForwarding = get-mailbox $mailboxremote5 | select-object ForwardingSmtpAddress)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mailbox not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

if ($ResultCode -eq 0)
{

        if ($ForwardingSmtpAddressSource.CompareTo(($MBXForwarding.ForwardingSmtpAddress | Out-String -Stream)) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Mailbox Forwarding is successfully set.'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Mailbox forwarding rule is not set."
            $ResultText = 'Mailbox forwarding rule is not set.'
             Write-Host($ResultText)
        
        }
 } 
 
 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32203 Passed'
     Write-Host($ResultText)
 }

Write-Host ("Removing Source On-Prem Session")
Start-Sleep -Seconds 5
Remove-pssession $SessionS

Start-Sleep -Seconds 5
Write-Host ("Connecting to Target On-Prem Session")
#Connecting to Target On-Prem
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Target")
            $SessionOP = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri $uri -Credential $credtp -Authentication Basic -SessionOption $so 
            Import-PSSession $SessionOP
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

##TC32208 Verify during Cutover Process, Contact's LegacyExchangeDN will be added to the RemoteMailbox as x500 address during object merging
Start-Sleep -Seconds 5
if ($ResultCode -eq 0)
{
  Try 
    {
     $sw.Restart()
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {        
	        If (get-remotemailbox $RemoteMailbox)
	        {
		        $loop = 0
                $ResultCode = 0
	        }
	        Else
	        {
		        Write-Error 'Mailbox not found'
		        Start-Sleep -Seconds $SleepSeconds
	        }
        }
    }
    Catch
    {
	    Write-Error "Can not locate the mailbox"
    }
}

if ($ResultCode -eq 0)
{
    If ($TMBXX500 = get-remotemailbox $RemoteMailbox | select-object EmailAddresses)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mailbox not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

$TMBXX500Compare = $TMBXX500.EmailAddresses -contains $RemoteMailbox5X500

if ($ResultCode -eq 0)
{

        if ($TMBXX500Compare)
        {
            $ResultCode = 0
            $ResultText = 'Target contact LegacyDN has been merged to target mailbox Successful'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target contact LegacyDN has not been merged to target mailbox Successful"            
        
        }
 }


 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32208 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32208 Failed'
     Write-Host($ResultText)
 }

 Remove-PSSession $SessionOP


