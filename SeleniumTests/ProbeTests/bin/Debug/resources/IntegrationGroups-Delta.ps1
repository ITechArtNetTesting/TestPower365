param([string]$slogin = "corp31\administrator", [string]$spassword = "Password31", [string]$SourceGrp3 = "P365AutoGroup3", [string]$SourceGrp3MemberRemove = "C31-GrpMember1", [string]$SourceGrp3OwnerRemove = "C31-GrpMember5", [string]$uri="https://10.1.178.10/PowerShell")
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
$CredSp = New-CredentialFromClear $slogin $spassword
$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck
$ResultCode=0
#Connecting to Source On-Prem
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Source")
            $SessionP = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri $uri -Credential $CredSp -Authentication Basic -SessionOption $so
            Import-PSSession $SessionP

		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

$ResultCode = 0

#TC 31818 - Verify group delta sync can remove group members and owners, Removing Group Members

if ($ResultCode -eq 0)
    {
		try
		{
           Remove-DistributionGroupMember -Identity $SourceGrp3 -Member $SourceGrp3MemberRemove -Confirm:$false
            $ResultText = 'Group Member Removed correctly'
            Write-Host($ResultText)
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
           Set-DistributionGroup -Identity $SourceGrp3 -Managedby $SourceGrp3OwnerRemove 
            $ResultText = 'Group Owner Removed correctly'
           Write-Host($ResultText)
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }
    Remove-pssession $SessionP
