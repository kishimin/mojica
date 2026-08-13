$workflowPath = Join-Path $PSScriptRoot '..\..\.github\workflows\api-ci.yml'
$workflow = Get-Content -Raw -LiteralPath $workflowPath

$expectedRules = @(
    'schedule:'
    'workflow_dispatch:'
    "github.event_name == 'pull_request' && '[{`"size`":`"Small`",`"filter`":`"FullyQualifiedName~SmallTests`"}]'"
    "github.event_name == 'push' && '[{`"size`":`"Small`",`"filter`":`"FullyQualifiedName~SmallTests`"},{`"size`":`"Medium`",`"filter`":`"FullyQualifiedName~MediumTests`"}]'"
    "|| '[{`"size`":`"Large`",`"filter`":`"FullyQualifiedName~LargeTests`"}]'"
    "github.event_name == 'pull_request' || github.event_name == 'push'"
)

$missingRules = $expectedRules | Where-Object { -not $workflow.Contains($_) }

if ($missingRules.Count -gt 0)
{
    throw "Missing API CI timing rules: $($missingRules -join ', ')"
}
