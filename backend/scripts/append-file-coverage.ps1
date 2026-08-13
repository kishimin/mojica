param(
    [Parameter(Mandatory)]
    [string[]] $CoverageFiles,

    [Parameter(Mandatory)]
    [string] $SummaryFile
)

$coverageByFile = @{}

foreach ($coverageFile in $CoverageFiles)
{
    [xml] $coverage = Get-Content -Raw -LiteralPath $coverageFile

    foreach ($class in $coverage.coverage.packages.package.classes.class)
    {
        $filename = [string] $class.filename

        if (-not $coverageByFile.ContainsKey($filename))
        {
            $coverageByFile[$filename] = @{}
        }

        foreach ($line in $class.lines.line)
        {
            $lineNumber = [int] $line.number
            $hits = [int] $line.hits

            if (-not $coverageByFile[$filename].ContainsKey($lineNumber) -or
                $hits -gt $coverageByFile[$filename][$lineNumber])
            {
                $coverageByFile[$filename][$lineNumber] = $hits
            }
        }
    }
}

$rows = foreach ($filename in $coverageByFile.Keys | Sort-Object)
{
    $lines = $coverageByFile[$filename]
    $total = $lines.Count
    $covered = @($lines.Values | Where-Object { $_ -gt 0 }).Count
    $rate = if ($total -eq 0) { 100 } else { 100 * $covered / $total }

    [pscustomobject]@{
        Filename = $filename.Replace('|', '\|')
        Covered = $covered
        Total = $total
        Rate = $rate
    }
}

$markdown = @(
    ''
    '## File coverage'
    ''
    '| File | Line coverage | Lines |'
    '| --- | ---: | ---: |'
)

$markdown += $rows | ForEach-Object {
    "| ``$($_.Filename)`` | $($_.Rate.ToString('0.00'))% | $($_.Covered) / $($_.Total) |"
}

$markdown += @(
    ''
    "Files: $($rows.Count)"
)

Add-Content -LiteralPath $SummaryFile -Value $markdown -Encoding utf8
