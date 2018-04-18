function New-CredentialFromClear([string]$username, [string]$password)
{
	$ss = new-object System.Security.SecureString
	$password.ToCharArray() | % { $ss.AppendChar($_) }
	return new-object System.Management.Automation.PSCredential($username, $ss)
}


[System.Int32] $SleepSeconds = 10
[System.Int32] $TimeOutMinutes = 1
$Timeout = new-timespan -Seconds $TimeOutMinutes
$loop = 1
$Date = Get-Date -Format "MMddHHmm"
$RemoteMailbox = "P365AutoRemote*" 
$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck 
$Credop = New-CredentialFromClear "corp32\administrator" "Password32"
$ResultCode = 0

#Connecting to Target On-Prem
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Target")
            $SessionOP = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri "https://10.1.178.55/PowerShell" -Credential $credop -Authentication Basic -SessionOption $so 
            Import-PSSession $SessionOP
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

#Remove previously created mailbox
if ($ResultCode -eq 0)
    {
		try
		{
            Get-RemoteMailbox P365AutoRemote* | Remove-RemoteMailbox -Confirm:$false
            Get-RemoteMailbox P365AutoSAM* | Remove-RemoteMailbox -Confirm:$false
            $ResultText = 'Target Object deleted successful'
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
            Get-distributiongroup P365AutoGroup* | Remove-distributiongroup -Confirm:$false
            $ResultText = 'Target Object deleted successful'
           Write-Host($ResultText)
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

	
Write-Host ("Removing Target On-Prem Session")
Remove-pssession $SessionOP




#Connecting to Source On-Prem

$CredSp = New-CredentialFromClear "corp31\administrator" "Password31"

$SourceGrp3 = "P365AutoGroup3"
$SourceGrp3Member = "C31-GrpMember1"
$SourceGrp3Owner = "C31-GrpMember2"

	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Source")
            $SessionSP = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri "https://10.1.178.10/PowerShell" -Credential $CredSp -Authentication Basic -SessionOption $so
            Import-PSSession $SessionSP

		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }



#TC 31818 - Adding Group Members and Owner Back to the Source Groups

if ($ResultCode -eq 0)
    {
		try
		{
            add-DistributionGroupMember -Identity $SourceGrp3 -Member $SourceGrp3Member
            $ResultText = 'Group Member added correctly'
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
            set-DistributionGroup -Identity $SourceGrp3 -managedby $SourceGrp3Owner
            $ResultText = 'Group Owner added correctly'
            Write-Host($ResultText)
		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }
    Remove-pssession $SessionSP
