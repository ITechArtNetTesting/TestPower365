param([string]$group = "Not_set", [string]$slogin = "Not_set", [string]$spassword = "Not_set", [string]$sclogin = "Not_set", [string]$scpassword = "Not_set", [string]$gpassword = "Not_set", [string]$for = "Not_set", [string]$ou = "Not_set")
function New-CredentialFromClear([string]$username, [string]$password)
{
	$ss = new-object System.Security.SecureString
	$password.ToCharArray() | % { $ss.AppendChar($_) }
	return new-object System.Management.Automation.PSCredential($username, $ss)
}


[System.Int32] $SleepSeconds = 10
[System.Int32] $TimeOutMinutes = 2
$Timeout = new-timespan -Seconds $TimeOutMinutes
$loop = 1
$Date = Get-Date -Format "MMddHHmm"
$Mailbox = [string]::Format("{0}_{1}", "P365UserProbe", $Date)
$securitygroup = $group
$Creds = New-CredentialFromClear $slogin $spassword
$Credsc = New-CredentialFromClear $sclogin $scpassword
$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck
$password = convertto-securestring $gpassword -asplaintext -force
$Forest = $for 
$OU = $ou 

$ResultCode = 0



##Connecting to Source On-Prem
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Source")
            $SessionS = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri "https://10.1.137.25/PowerShell" -Credential $creds -Authentication Basic -SessionOption $so
            Import-PSSession $SessionS
            #Import-Module msonline
            #Connect-MsolService -Credential $Creds
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }


    Start-Sleep -Seconds 1

#Remove previously created mailbox
if ($ResultCode -eq 0)
    {
		try
		{
            Get-RemoteMailbox P365UserProbe* | Remove-RemoteMailbox -Confirm:$false
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }


	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Creating Mailbox")
            New-RemoteMailbox -Alias $Mailbox -Name $Mailbox -UserPrincipalName $Mailbox@$Forest -LastName Test -FirstName $Mailbox -OnPremisesOrganizationalUnit $OU -Password $password  
		}
		catch
		{
			$mgs = 'Unable to create mailbox'
			Write-Error $mgs
			Write-Error $_.Exception.Message
           $ResultCode = 1
		}
    }

if ($ResultCode -eq 0)
{
  Try 
    {
        while ($sw.elapsed -lt $timeout -and $loop -eq 1)
        {
	        If (get-remotemailbox $Mailbox)
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


  ##Add MSOL Group and Group Member into Variable
  If ($loop -eq 0)
    {
  		try
		{
            Write-Host ("Adding New Mailbox to Group")
            Add-DistributionGroupMember -Member $Mailbox -Identity $securitygroup
		}
		catch
		{
			$mgs = 'Unable to add mailbox to group'
			Write-Error $mgs
			Write-Error $_.Exception.Message
           $ResultCode = 1
		}
    }


If ($loop -eq 1 -and $sw.elapsed -gt $timeout)
{
	$output.ResultCode = '99'
	Write-Error 'Unable to locate the new mailbox in time'
	$ResultCode = 1
}
}

 Remove-PSSession $SessionS

    ##Connecting to Source Tenant
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Source")
            $SessionSc= New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.outlook.com/powershell -Credential $Credsc -Authentication Basic -AllowRedirection
            Import-PSSession $SessionSc
            Import-Module msonline
            Connect-MsolService -Credential $Credsc
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

Start-Sleep -Seconds 600


	if ($ResultCode -eq 0)
	{
		try
		{
        get-msoluser -SearchString $Mailbox
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }


Remove-PSSession $SessionSc