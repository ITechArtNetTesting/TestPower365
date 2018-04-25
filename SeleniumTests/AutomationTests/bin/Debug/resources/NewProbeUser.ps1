param([string]$group = "Not_set", [string]$slogin = "Not_set", [string]$spassword = "Not_set", [string]$tlogin = "Not_set", [string]$tpassword = "Not_set")
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
$Mailbox = [string]::Format("{0}_{1}", "MailProbe", $Date) 
$securitygroup = $group
$Creds = New-CredentialFromClear $slogin $spassword
$Credt = New-CredentialFromClear $tlogin $tpassword
$ResultCode = 0

##Connecting to Source Tenant
                if ($ResultCode -eq 0)
                {
                                try
                                {
            Write-Host ("Connecting to Source")
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

                if ($ResultCode -eq 0)
                {
                                try
                                {
            Write-Host ("Removing Old Mailbox")
            Get-MsolUser -SearchString MailProb | MSOnlineExtended\Remove-MsolUser -force:$True
                                }
                                catch
                                {
                                                $mgs = 'Unable to find previously configured mailbox'
                                                Write-Error $mgs
                                                Write-Error $_.Exception.Message
            $ResultCode = 1
                                }
    }

                if ($ResultCode -eq 0)
                {
                                try
                                {
            Write-Host ("Creating Mailbox")
            New-Mailbox  -Name $Mailbox -DisplayName $Mailbox -Shared –Alias $Mailbox
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
                        If (get-msoluser -SearchString  $Mailbox)
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

Write-Host ("Removing Source Session")
Remove-pssession $SessionS

#Connecting to Target Tenant

                if ($ResultCode -eq 0)
                {
                                try
                                {
            Write-Host ("Connecting to Target")
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

                if ($ResultCode -eq 0)
                {
                                try
                                {
            Write-Host ("Removing Old Mailbox")
            Get-MsolUser -SearchString MailProb | MSOnlineExtended\Remove-MsolUser -force:$True
                                }
                                catch
                                {
                                                $mgs = 'Unable to find previously configured mailbox'
                                                Write-Error $mgs
                                                Write-Error $_.Exception.Message
            $ResultCode = 1
                                }
    }

                if ($ResultCode -eq 0)
                {
                                try
                                {
            Write-Host ("Creating Target Mailbox")
            New-Mailbox  -Name $Mailbox -DisplayName $Mailbox -Share
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
                        If (get-msoluser -SearchString  $Mailbox)
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
            Write-Host ("Located Target Mailbox")
            Get-Mailbox $Mailbox 
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

Write-Host ("Removing Target Session")
Remove-pssession $SessionT
