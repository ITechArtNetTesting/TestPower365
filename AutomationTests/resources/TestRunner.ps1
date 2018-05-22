Param(
   [string]$sauceArgument,
   [string]$secretSauceArgument
)
Write-Host No problem reading $env:sauce or $sauceArgument
Write-Host But I cannot read $env:secretSauce
Write-Host But I can read $secretSauceArgument "(but the log is redacted so I do not
           spoil the secret)"


gci env:* | sort-object name