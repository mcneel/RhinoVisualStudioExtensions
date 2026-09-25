#!/usr/bin/env pwsh
<#
.SYNOPSIS
Summarises the projects built by GenerateTemplates.proj as markdown, for the GitHub job summary.
#>
param(
  [string]$TemplateDir = (Join-Path $PSScriptRoot '../artifacts/templates'),
  [string]$SummaryPath = $env:GITHUB_STEP_SUMMARY
)

$ErrorActionPreference = 'Stop'

function Get-Diagnostics([string]$logPath) {
  if (-not (Test-Path $logPath)) { return @() }
  Get-Content $logPath |
    Where-Object { $_ -match '(?:error|warning)\s*(?<code>[A-Z]*\d*)\s*:\s*(?<message>.*?)(?:\s+\[[^\]]*\])?$' } |
    ForEach-Object { if ($Matches.code) { "$($Matches.code): $($Matches.message)" } else { $Matches.message } } |
    Sort-Object -Unique
}

$root = (Resolve-Path $TemplateDir).Path
$projects = Get-ChildItem $root -Recurse -File -Include *.csproj, *.vbproj, *.vcxproj |
  Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' } |
  ForEach-Object {
    $dir = $_.Directory.FullName
    $errorLog = Join-Path $dir 'build.errors.log'
    $warnings = @(Get-Diagnostics (Join-Path $dir 'build.warnings.log'))
    $errors = @(Get-Diagnostics $errorLog)
    $status = if (-not (Test-Path $errorLog)) { 'Generated only' }
      elseif ($errors.Count -gt 0) { 'Failed' }
      elseif ($warnings.Count -gt 0) { 'Warnings' }
      else { 'Clean' }
    [pscustomobject]@{
      Name = [IO.Path]::GetRelativePath($root, $dir).Replace('\', '/')
      Status = $status
      Warnings = $warnings
      Errors = $errors
    }
  } |
  Sort-Object Name

function Format-Cell([string]$text) { $text.Replace('|', '\|') }

$failed = @($projects | Where-Object Status -eq 'Failed')
$warned = @($projects | Where-Object Status -eq 'Warnings')
$clean = @($projects | Where-Object Status -eq 'Clean')
$generated = @($projects | Where-Object Status -eq 'Generated only')

$os = if ($IsWindows) { 'Windows' } elseif ($IsMacOS) { 'macOS' } else { 'Linux' }
$md = [Collections.Generic.List[string]]::new()
$md.Add("## Templates ($os)")
$md.Add('')
$md.Add("| Result | Projects |")
$md.Add("| --- | --- |")
$md.Add("| :x: Failed | $($failed.Count) |")
$md.Add("| :warning: Warnings | $($warned.Count) |")
$md.Add("| :white_check_mark: Clean | $($clean.Count) |")
$md.Add("| :fast_forward: Generated only | $($generated.Count) |")
$md.Add('')

if ($failed.Count -gt 0) {
  $md.Add('### Failed')
  $md.Add('')
  $md.Add('| Project | Errors |')
  $md.Add('| --- | --- |')
  foreach ($project in $failed) {
    $md.Add("| $($project.Name) | $(($project.Errors | ForEach-Object { Format-Cell $_ }) -join '<br>') |")
  }
  $md.Add('')
}

if ($warned.Count -gt 0) {
  $md.Add('### Warnings')
  $md.Add('')
  $byWarning = $warned | ForEach-Object { $p = $_; $p.Warnings | ForEach-Object { [pscustomobject]@{ Warning = $_; Project = $p.Name } } } |
    Group-Object Warning | Sort-Object Count -Descending
  foreach ($group in $byWarning) {
    $md.Add("<details><summary>$([Net.WebUtility]::HtmlEncode($group.Name)) ($($group.Count) projects)</summary>")
    $md.Add('')
    $group.Group | ForEach-Object { $md.Add("- $($_.Project)") }
    $md.Add('')
    $md.Add('</details>')
  }
  $md.Add('')
}

foreach ($section in @(@{ Title = 'Clean'; Items = $clean }, @{ Title = 'Generated only'; Items = $generated })) {
  if ($section.Items.Count -eq 0) { continue }
  $md.Add("<details><summary>$($section.Title) ($($section.Items.Count) projects)</summary>")
  $md.Add('')
  $section.Items | ForEach-Object { $md.Add("- $($_.Name)") }
  $md.Add('')
  $md.Add('</details>')
  $md.Add('')
}

if ($SummaryPath) {
  $md | Add-Content $SummaryPath
} else {
  $md
}
