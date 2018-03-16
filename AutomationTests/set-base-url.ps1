Param
(
    [Parameter(Mandatory=$true)]
    [string]$instanceName,
    [Parameter(Mandatory=$false)]
    [string]$subInstanceName
)

$runXmlPath = "$PSScriptRoot\resources\run.xml"
[xml]$runXml = Get-Content -Path $runXmlPath

$instanceName = $instanceName.ToLower();
if (!$subInstanceName) {
    $subInstanceName = 'pa'
}

$baseUrl = "https://bt-$instanceName-$subInstanceName-web-ui.azurewebsites.net/"

$runXml.DocumentElement.baseurl = $baseUrl

$runXml.Save($runXmlPath)