$ErrorActionPreference = "Stop"

$workflowPath = Join-Path $PSScriptRoot "..\..\.github\workflows\api-ci.yml"
$workflow = Get-Content -Raw -LiteralPath $workflowPath
$coverageParts = $workflow -split "`n  coverage:", 2

if ($coverageParts.Count -ne 2) {
    throw "The API CI coverage job was not found."
}

$coverageJob = $coverageParts[1]

if ($coverageJob -notmatch "--filter" -or
    $coverageJob -notmatch "github.event_name == 'pull_request'" -or
    $coverageJob -notmatch "FullyQualifiedName~SmallTests" -or
    $coverageJob -notmatch "FullyQualifiedName~MediumTests" -or
    $coverageJob -match "FullyQualifiedName~LargeTests") {
    throw "Coverage must run Small tests for pull requests and Small plus Medium tests for pushes."
}
