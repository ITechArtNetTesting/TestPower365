param([string]$slogin = "Not_set", [string]$spassword = "Not_set", [string]$mailuser = "Not_set")
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
$Mailbox = $mailuser
$Creds = New-CredentialFromClear $slogin $spassword
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
  Try 
    {        
                        If (get-msoluser -SearchString  $Mailbox)
                        {
						get-msoluser -SearchString  $Mailbox                                      
                $ResultCode = 0
                        }
                        Else
                        {
                                        Write-Error 'Mailbox not found'
                                        Start-Sleep -Seconds $SleepSeconds
                        }       
    }
    Catch
    {
                    Write-Error "Can not locate the mailbox"
    }
}
	Write-Host ("Removing Source Session")
Remove-pssession $SessionS
