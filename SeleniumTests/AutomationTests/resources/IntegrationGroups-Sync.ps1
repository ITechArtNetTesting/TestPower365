param([string]$slogin = "admin@BTCloud9.Power365.Cloud", [string]$spassword = "Password32", [string]$SourceGrp1 = "P365AutoGroup1", [string]$SourceGrp1TargetMail = "P365AutoGroup1@corp32.cmtsandbox.com", [string]$SourceGrp1TargetMember ="C31-GrpMember1", [string]$SourceGrp2 = "P365AutoGroup2", [string]$SourceGrp2TargetOwner ="C31-GrpMember1", [string]$SourceGrp3 = "P365AutoGroup3", [string]$SourceGrp4 = "P365AutoGroup4", [string]$SourceGrp5 = "P365AutoGroup5", [string]$SourceGrp6 = "P365CloudGroup1", [string]$SourceGrp3TargetMember = "P365AutoGroup5", [string]$SourceGrp3TargetMemberRemove ="C31-GrpMember1", [string]$SourceGrp3TargetOwnerRemove ="C31-GrpMember2", [string]$adlogin = "corp32\administrator", [string]$adpassword="Password32", [string]$uri = "https://10.1.178.55/PowerShell")
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

$SourceGrpTargetType ="MailUniversalDistributionGroup"
$SourceGrp4TargetExternalAuth = "True"
$SourceGrp5TargetDescription = "Test"
$EmailAddressPolicyEnabled = [System.Convert]::ToBoolean("False")

$so = New-PSSessionOption -SkipCACheck -SkipCNCheck -SkipRevocationCheck 
$Creds = New-CredentialFromClear $slogin $spassword
$Credtp = New-CredentialFromClear $adlogin $adpassword
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

##TC #31803  Verify group sync can create group in target

if ($ResultCode -eq 0)
{
    If ($GrpType = get-distributiongroup $SourceGrp1 | select-object RecipientTypeDetails)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}


if ($ResultCode -eq 0)
{

        if ($SourceGrpTargetType.CompareTo($GrpType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Target Group created with correct type'
            Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group did not created with the correct type"
            $ResultText = 'Target group did not created with the correct type'
            Write-host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC31803 Passed'
     Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC31803 Failed'
     Write-host($ResultText)
 } 

 ##TC #31810  Verify email address policy will be enabled for target object when enabled in Dirsync

if ($ResultCode -eq 0)
{
    If ($GrpEmailPolicy = get-distributiongroup $SourceGrp1 | select-object EmailAddressPolicyEnabled)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}


if ($ResultCode -eq 0)
{
# contain or like 
        if ($EmailAddressPolicyEnabled -contains $GrpEmailPolicy.EmailAddressPolicyEnabled)
        {
            $ResultCode = 0
            $ResultText = 'Group Email Address Policy enabled Successful'
             Write-Host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Group Email Address Policy not enabled"
            $ResultText = 'Group Email Address Policy not enabled'
            Write-Host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32395 Passed'
     Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32395 Failed'
     Write-host($ResultText)
 } 

 ##TC #31805  Verify group members will be synced (Initial Sync and Delta)

if ($ResultCode -eq 0)
{
    If ($GrpMember = Get-DistributionGroupMember $SourceGrp1 | where {$_.name -Match $SourceGrp1TargetMember} | Select-Object Name)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'GroupMember not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}


if ($ResultCode -eq 0)
{

        if ($SourceGrp1TargetMember.CompareTo($GrpMember.Name) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Target Group created with correct Member'
            Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group did not created with the correct member"
            $ResultText = 'Target group did not created with the correct member'
            Write-host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC31805 Passed'
    Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC31805 Failed'
    Write-host($ResultText)
 } 


 ##TC #31806  Verify group owners will be synced (Initial and Delta)

if ($ResultCode -eq 0)
{
    If ($GrpOwner = Get-DistributionGroup $SourceGrp2 | Select-Object ManagedBy )
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}


if ($ResultCode -eq 0)
{

        if ($SourceGrp2TargetOwner.CompareTo(($GrpOwner.ManagedBy -like $SourceGrp2TargetOwner | Out-String -Stream)) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Target Group created with correct Owner'
            Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group did not created with the correct Owner"
            $ResultText = 'Target group did not created with the correct Owner'
            Write-host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC31806 Passed'
    Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC31806 Failed'
    Write-host($ResultText)
 } 

##TC #31814  Verify nested groups can be synced as members

if ($ResultCode -eq 0)
{
    If ($GrpMember = Get-DistributionGroupMember $SourceGrp3 | where {$_.name -Match $SourceGrp5} | Select-Object Name)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
               $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}


if ($ResultCode -eq 0)
{
       if ($SourceGrp3TargetMember.CompareTo($GrpMember.Name) -eq 0)
       {
            $ResultCode = 0
            $ResultText = 'Target Group created with correct nested member'
            Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group did not created with the correct nested member"
            $ResultText = 'Target group did not created with the correct nested member'
            Write-host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC31814 Passed'
    Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC31814 Failed'
    Write-host($ResultText)
 } 

  ##TC #31818  Verify group delta sync can remove group members and owners

if ($ResultCode -eq 0)
{
    If ($GrpMember = Get-DistributionGroupMember $SourceGrp3 | where {$_.name -Match $SourceGrp1TargetMember} | Select-Object Name)
	    {
            $ResultCode = 1
             Write-Error 'Member is not removed correctly'
              Start-Sleep -Seconds $SleepSeconds
	    }
	    Else
	    {
		        Write-Host 'Member is removed successfully'
                $ResultCode = 0		       
	    }

}

{
    If ($GrpOwner = Get-DistributionGroupMember $SourceGrp3 | Select-Object ManagedBy)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}



if ($ResultCode -eq 0)
{

        if ($SourceGrp3TargetMemberRemove.CompareTo($GrpMember.Name) -eq 1)
        {
            $ResultCode = 0
            $ResultText = 'Target Group Member removed successfully'
            Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group member did not removed successfully"
            $ResultText = 'Target group member did not removed successfully'
            Write-host($ResultText)
        
        }
 }

 {

        if ($SourceGrp3TargetOwner.CompareTo(($GrpOwner.ManagedBy -like $SourceGrp3TargetOwnerRemove | Out-String -Stream)) -eq 1)
        {
            $ResultCode = 0
            $ResultText = 'Target Group owner removed successfully'
           Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group owner did not removed successfully"
            $ResultText = 'Target group owner did not removed successfully'
           Write-host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC31818 Passed'
    Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC31818 Failed'
   Write-host($ResultText)
 } 


  ##TC #32493  Verify AD Description will be synced by group sync (Initial and Delta)

if ($ResultCode -eq 0)
{
    If ($Grpdescription = Get-MsOlGroup -searchstring $SourceGrp5 | Select-Object Description)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}



if ($ResultCode -eq 0)
{
        if ($SourceGrp5TargetDescription.CompareTo($Grpdescription.Description) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Target Group Member created with correct description'
           Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group member did not created with correct desecription"
            $ResultText = 'Target group member did not created with correct description'
           Write-host($ResultText)
        
        }
 }


 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32493 Passed'
    Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC32493 Failed'
   Write-host($ResultText)
 } 

##TC #31808  Verify cloud only group can be migrated by group sync

if ($ResultCode -eq 0)
{
    If ($GrpType = get-distributiongroup $SourceGrp6 | select-object RecipientTypeDetails)
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}


if ($ResultCode -eq 0)
{

        if ($SourceGrpTargetType.CompareTo($GrpType.RecipientTypeDetails) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Target Group created with correct type'
            Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group did not created with the correct type"
            $ResultText = 'Target group did not created with the correct type'
            Write-host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC31808 Passed'
     Write-host($ResultText)
 }
 Else
 {
    $ResultText = 'TC31808 Failed'
     Write-host($ResultText)
 } 


 Write-Host ("Removing Target Session")
Remove-pssession $SessionT	

#Connecting to Target On-Prem
	if ($ResultCode -eq 0)
	{
		try
		{
            Write-Host ("Connecting to Source")
            $SessionP = New-PSSession -ConfigurationName "Microsoft.Exchange" -ConnectionUri $uri -Credential $Credtp -Authentication Basic -SessionOption $so
            Import-PSSession $SessionP

		}
		catch
		{
			Write-Error $_.Exception.Message
            $ResultCode = 1
		}
    }

    ##TC 32281 - Verify AllowExternalMessages will be synced (Initial Sync and Delta)

if ($ResultCode -eq 0)
{
    If ($ExternalAuth = Get-DistributionGroup $SourceGrp4 | Select-Object RequireSenderAuthenticationEnabled )
	    {
            $ResultCode = 0
	    }
	    Else
	    {
		        Write-Error 'Group not found'
                $ResultCode = 1
		        Start-Sleep -Seconds $SleepSeconds
	    }

}


if ($ResultCode -eq 0)
{

        if ($SourceGrp4TargetExternalAuth.CompareTo(($ExternalAuth.RequireSenderAuthenticationEnabled | Out-String -Stream)) -eq 0)
        {
            $ResultCode = 0
            $ResultText = 'Target Group created with allow sender from external set to true'
           Write-host($ResultText)
        }
        Else

        {
            $ResultCode = 1
            Write-Error "Target group did not created with allow sender from external set to true"
            $ResultText = 'Target group did not created with allow sender from external set to true'
            Write-host($ResultText)
        
        }
 }

 if ($ResultCode -eq 0)
 {
    $ResultText = 'TC32281 Passed'
   Write-host($ResultText)
  }
 Else
 {
    $ResultText = 'TC32281 Failed'
    Write-host($ResultText)
 } 

Write-Host ("Removing Target On-Prem Session")
Remove-pssession $SessionP