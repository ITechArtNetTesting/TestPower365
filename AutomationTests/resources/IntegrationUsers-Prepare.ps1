param([string]$slogin = "admin@BTCloud9.Power365.Cloud", [string]$spassword = "Password32", [string]$mailbox = "P365AutoRemoteMBX1", [string]$smtp = "smtp:P365AutoRemoteMBX1@corp31.cmtsandbox.com", [string]$remoteshare = "P365AutoRemoteShare1", [string]$remoteroom = "P365AutoRemoteRoom1", [string]$mailboxremote2 = "P365AutoRemoteMBX2", [string]$mailboxremote2contact = "P365AutoRemoteMBX2@corp31.cmtsandbox.com", [string]$mailboxremote4 = "P365AutoRemoteMBX4", [string]$remoteequip1 = "P365AutoRemoteEquip1", [string]$sam1 = "P365AutoSAM1", [string]$sam1upn = "P365AutoSAM1Test", [string]$sam2 = "P365AutoSAM2", [string]$adlogin = "corp32\administrator", [string]$adpassword = "Password32")
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
$RemoteMailbox = $mailbox
$MBXTypeSource = "UserMailbox"
$MBXUsageLocation = 'US'
$HiddenFromAddressListsEnabled = 'True'
$ForwardingSmtpAddress = $smtp
$EmailAddressPolicyEnabled = [System.Convert]::ToBoolean("False")


$RemoteMailbox2 = $mailboxremote2
$ContactTypeSource = 'MailContact'
$ContactTargetAddress = $mailboxremote2contact

$RemoteMailbox4 = $mailboxremote4
$ArchiveMailboxSetting = '*Archive*'

$RemoteSharedMailbox = $remoteshare 
$SharedMBXTypeSource = 'SharedMailbox'

$RemoteRoomMailbox = $remoteroom
$RoomMBXTypeSource = 'RoomMailbox'

$RemoteEquipMailbox = $remoteequip1
$EquipMBXTypeSource = 'EquipmentMailbox'

$SourceSamAccountMBX1 = $sam1
$SourceSamAccountUPNPrefix = $sam1upn

$SourceSamAccountMBX2 = $sam2


$Creds = New-CredentialFromClear $slogin $spassword
$Credtp = New-CredentialFromClear $adlogin $adpassword
$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck
$ResultCode = 0

##Connecting to Target Tenant
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Target")
            $SessionT = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $Creds -Authentication Basic -AllowRedirection
            Import-PSSession $SessionT
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

#####Starting validation steps#####

##TC32195, 32189, 32395 - Validating Verify DirSync will properly set the msExchangeRemoteRecipientType to 1 when target object has no active mailbox, enable the EmailAddressPolicy setting to true

if ($ResultCode -eq 0)
{
  Try 
    {
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
    If ($MBXType = get-mailbox $RemoteMailbox | select-object RecipientType)
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

        if ($MBXTypeSource.CompareTo($MBXType.RecipientType) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'RemoteMailbox Created Successful'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Mailbox not created correctly"
            $ResultText = 'RemoteMailbox Creation failed'
             Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
{
    If ($MBXType = get-mailbox $RemoteMailbox | select-object EmailAddressPolicyEnabled)
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
# contain or like 
        if ($EmailAddressPolicyEnabled.CompareTo($MBXType.EmailAddressPolicyEnabled) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'RemoteMailbox Email Address Policy enabled Successful'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "RemoteMailbox Email Address Policy not enabled"
            $ResultText = 'RemoteMailbox Email Address Policy not enabled'
            Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32195, 32189, 32395 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32195, 32189, 32395 Failed'
     Write-Host($ResultText)
 }



##TC 32188 - Validating Verify the location attribute of the Mailbox will be set correctly in the target.

if ($ResultCode -eq 0)
{
  Try 
    {
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
    If ($MBXLocation = get-msoluser -SearchString  $RemoteMailbox | select-object UsageLocation)
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

        if ($MBXUsageLocation.CompareTo($MBXLocation.UsageLocation) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Mailbox location set correctly'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Mailbox location set incorrectly"
            $ResultText = 'Mailbox location set incorrectly'
             Write-Host($ResultText)
        
        }
 }

if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32188 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32188 Failed'
     Write-Host($ResultText)
 }


##TC 32180 - Validating Verify the target mailbox addressbook will be hidden.
##TC 32396 - Verify after the newly created user finished prepare stage, it will have the Forwarding Rule set to the source mailbox and hidden in the GAL


if ($ResultCode -eq 0)
{
    If ($MBXAddressBook = get-mailbox $RemoteMailbox | select-object HiddenFromAddressListsEnabled)
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

        if ($HiddenFromAddressListsEnabled.CompareTo(($MBXAddressBook.HiddenFromAddressListsEnabled | Out-String -Stream)) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Mailbox addressbook is successfully hidden.'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Mailbox addressbook is not hidden."
            $ResultText = 'Mailbox addressbook is not hidden.'
             Write-Host($ResultText)
        
        }
 }
 
 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32180, 32396 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32180, 32396 Failed'
     Write-Host($ResultText)
 }   

##TC 32181 - Validating Verify the target mailbox forwarding rule will be set.
##TC 32396 - Verify after the newly created user finished prepare stage, it will have the Forwarding Rule set to the source mailbox and hidden in the GAL

if ($ResultCode -eq 0)
{
    If ($MBXForwarding = get-mailbox $RemoteMailbox | select-object ForwardingSmtpAddress)
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

        if ($ForwardingSmtpAddress.CompareTo(($MBXForwarding.ForwardingSmtpAddress | Out-String -Stream)) -eq 0)
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
    $ResultText = 'TC32181, 32396 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32181, 32396 Failed'
     Write-Host($ResultText)
 }    

##TC 32196 - Validating Verify the mailbox will be set to Shared Mailbox in the target

if ($ResultCode -eq 0)
{
  Try 
    {
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {
	        If (get-msoluser -SearchString  $RemoteSharedMailbox)
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
    If ($MBXType = get-mailbox $RemoteSharedMailbox | select-object RecipientTypeDetails)
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

        if ($SharedMBXTypeSource.CompareTo($MBXType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Shared Mailbox Created Successful'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Shared Mailbox not created correctly"
            $ResultText = 'Shared Mailbox Creation failed'
             Write-Host($ResultText)
        
        }
 }


if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32196 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32196 Failed'
     Write-Host($ResultText)
 } 


##TC 28066 - Validating Verify the Room Mailbox will be set to Room Mailbox in the target.
##TC 39398 - Verify Source Room/Resource Mailbox will be created in target with the correct type

if ($ResultCode -eq 0)
{
  Try 
    {
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {
	        If (get-msoluser -SearchString  $RemoteRoomMailbox)
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
    If ($MBXType = get-mailbox $RemoteRoomMailbox | select-object RecipientTypeDetails)
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

        if ($RoomMBXTypeSource.CompareTo($MBXType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Room Mailbox Created Successful'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Room Mailbox not created correctly"
            $ResultText = 'Room Mailbox Creation failed'
             Write-Host($ResultText)
        
        }
 }

if ($ResultCode -eq 0)
 {
    $ResultText = 'TC28066, 39398 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC28066, 39398 Failed'
     Write-Host($ResultText)
 } 


##TC 32390 - Verify P365 DirSync can create user and contact during prepare stage.

## $RemoteRoomMailbox2 changed to $ContactTargetAddress
if ($ResultCode -eq 0)
{
    If ($ContactType = get-mailcontact $RemoteMailbox2 | select-object RecipientTypeDetails)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Mail Contact not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}

##MBXType changed to $ContactType
if ($ResultCode -eq 0)
{

        if ($ContactTypeSource.CompareTo($ContactType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Target Contact Created Successful'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target Contact not created correctly"
            $ResultText = 'Target Contact Creation failed'
             Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32390 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32390 Failed'
     Write-Host($ResultText)
 } 

##TC 39398 - Verify Source Room/Resource Mailbox will be created in target with the correct type

if ($ResultCode -eq 0)
{
  Try 
    {
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {
	        If (get-msoluser -SearchString  $RemoteEquipMailbox)
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
    If ($MBXType = get-mailbox $RemoteEquipMailbox | select-object RecipientTypeDetails)
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

        if ($EquipMBXTypeSource.CompareTo($MBXType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Room Mailbox Created Successful'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Room Mailbox not created correctly"
            $ResultText = 'Room Mailbox Creation failed'
            Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC39398 Passed'
     Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC39398 Failed'
     Write-Host($ResultText)
 } 


 ##TC 32179 - Verify ABS will Sync Source User Primary SMTP to Created Target Contact as TargetAddresse

if ($ResultCode -eq 0)
{
  Try 
    {
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {
	        If (get-msoluser -SearchString  $RemoteMailbox2)
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
    If ($MBXType = get-contact $RemoteMailbox2 | select-object WindowsEmailAddress)
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
        if ($ContactTargetAddress.CompareTo($MBXType.WindowsEmailAddress) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Contact Target Address created with Source Mailbox Primary SMTP'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Contact Target Address did not created with Source Mailbox Primary SMTPy"
            $ResultText = 'Contact Target Address created incorrectly'
             Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32179 Passed'
    Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32179 Failed'
    Write-Host($ResultText)
 } 



 ##TC 28078 - Verify DirSync enable RemoteMailbox Archive in Target during Prepare Command

if ($ResultCode -eq 0)
{
  Try 
    {
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {
	        If (get-msoluser -SearchString  $RemoteMailbox4)
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
    If ($MBXType = get-mailbox $RemoteMailbox4 | select-object ArchiveName)
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
        if ($MBXType.ArchiveName -like $ArchiveMailboxSetting)
        {
            $ResultCode = 0
            $ResultText = 'Target Mailbox created with Archive as expected'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target mailbox did not created with archive"
            $ResultText = 'Target mailbox did not created with archive'
            Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC28078 Passed'
    Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC28078 Failed'
    Write-Host($ResultText)
 } 

  
Write-Host ("Removing Target Session")
Remove-pssession $SessionT
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Target On-Prem")
            $SessionP = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri "https://10.1.178.55/PowerShell" -Credential $Credtp -Authentication Basic -SessionOption $so
            Import-PSSession $SessionP

		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

##TC 32623 - Verify users will be created with SAM Account name equal to source UPN prefix

if ($ResultCode -eq 0)
{
    If ($MBXType = get-remotemailbox $SourceSamAccountMBX1 | select-object SamAccountName)
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
Write-Host($MBXType.SamAccountName)
        if ($SourceSamAccountUPNPrefix.CompareTo($MBXType.SamAccountName) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Mailbox SamAccountName Created with Source UPN Prefix.'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Mailbox SamAccountName Created incorrectly"
            $ResultText = 'Mailbox SamAccountName Created incorrectly'
             Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32623 Passed'
    Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32623 Failed'
     Write-Host($ResultText)
 } 


 ##TC 32624 - Verify conflict handling when SAM account name already exists in target before user created

if ($ResultCode -eq 0)
{
    If ($MBXType = get-remotemailbox $SourceSamAccountMBX2 | select-object SamAccountName)
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

        if ($SourceSamAccountMBX2.CompareTo($MBXType.SamAccountName) -eq -1 -or $SourceSamAccountMBX2.CompareTo($MBXType.SamAccountName) -eq 1)
        {
            $ResultCode = 0
            $ResultText = 'SamAccountName renamed correctly per conflict resolution.'
            Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "SamAccountName renamed incorrectly per conflict resolution"          
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32624 Passed'
    Write-Host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32624 Failed'
    Write-Host($ResultText)
 } 



Write-Host ("Removing Target On-Prem Session")
Remove-pssession $SessionP