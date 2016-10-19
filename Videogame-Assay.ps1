$bonsaiPath = "bonsai/Bonsai64.exe"
$gamesPath = "Games"
$dataPath = "Data"

# Push into script location
Push-Location $PSScriptRoot

# Get subject name and current date time
$subjectName = Read-Host "Enter Subject Id"
$date = [DateTimeOffset]::Now
Write-Host

function Select-Game ($basePath)
{
	# Walk through all available games
	$games = Get-ChildItem -Path $basePath

	# List all available workflows
	$choices = @()
	Write-Host "Available [$($basePath)]:"
	foreach ($game in $games)
	{
		# List all games
		if ($game.Attributes -eq "Directory")
		{
			$choices += $game
			Write-Host "$($choices.length). $([io.path]::GetFileNameWithoutExtension($game.Name))"
		}
	}
	
	# Write the default option
	Write-Host "0. Go back"
	Write-Host

	# Read selection
	$choiceId = Read-Host "Enter Selection"
	$choice = $choices[($choiceId)-1]
	Write-Host
	
	# Go back
	if ($choiceId -eq 0)
	{
		return 0
	}
	
	# Repeat choice
	if (!$choice)
	{
		return $null
	}

	# Check if this is an actual game
	$entryPoint = Join-Path -Path $choice.FullName -ChildPath ($choice.Name + ".bonsai")
	if (Test-Path $entryPoint)
	{
		return Get-Item $entryPoint
	}
	else
	{
	    # Try to select from available sub-games and go back if needed
	    $result = $null
		while (!$result)
		{
		    $childPath = Join-Path -Path $basePath -ChildPath $choice.Name
			$result = Select-Game $childPath
			if ($result -eq 0)
			{
				return $null
			}
		}
		
		return $result
	}
}

# Select which game to run
$selectedGame = $null
while (!$selectedGame)
{
	$selectedGame = Select-Game $gamesPath
	if ($selectedGame -eq 0)
	{
		return
	}
}

# Compute session data path
$dataPath = Join-Path -Path $dataPath -ChildPath $subjectName
$dataPath = Join-Path -Path $dataPath -ChildPath ($date).ToString("yyyy_MM_dd-HH_mm")

# Copy protocol control scripts
New-Item $dataPath -type directory
robocopy $selectedGame.DirectoryName $dataPath /e

# Run bonsai
$bonsaiPath = (Get-Item $bonsaiPath).FullName
Push-Location $dataPath
& $bonsaiPath $selectedGame.Name | Out-Host
Pop-Location
