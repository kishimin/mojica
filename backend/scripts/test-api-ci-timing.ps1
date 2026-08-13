$workflowPath = Join-Path $PSScriptRoot '..\..\.github\workflows\api-ci.yml'
$workflow = Get-Content -Raw -LiteralPath $workflowPath

$expectedRules = @(
    'schedule:'
    'workflow_dispatch:'
    "github.event_name == 'pull_request' || github.event_name == 'push'"
    "github.event_name == 'push'"
    "github.event_name == 'schedule' || github.event_name == 'workflow_dispatch'"
)

$missingRules = $expectedRules | Where-Object { -not $workflow.Contains($_) }

if ($missingRules.Count -gt 0)
{
    throw "Missing API CI timing rules: $($missingRules -join ', ')"
}
