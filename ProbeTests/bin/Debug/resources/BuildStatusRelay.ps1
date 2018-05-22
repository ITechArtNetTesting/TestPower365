Param(
   [string]$webHookUri,
   [string]$buildDefinition,
   [string]$buildNumber
)

$currentDate = Get-Date

$card = "{
	`"@type`": `"MessageCard`",
	`"@context`": `"http://schema.org/extensions`",
	`"summary`": `"Build Complete`",
	`"themeColor`": `"0078D7`",
	`"title`": `"Build Complete: $($buildDefinition)`",
	`"sections`": [
		{
			`"activityTitle`": `"$($buildDefinition)`",
			`"activitySubtitle`": `"$($currentDate)`",
			`"facts`": [
				{
					`"name`": `"Build:`",
					`"value`": `"$($buildNumber)`"
				}
			]
		}
	]
}"

Invoke-RestMethod -Method post -ContentType 'Application/Json' -Body $card -Uri $webHookUri