<#
.SYNOPSIS
  Builds the puzzle-piece template icons listed in icons.json.

.DESCRIPTION
  Usage: powershell -File build\icons\make-icons.ps1 [-Name Grasshopper2] [-Preview]

  Each icons.json entry takes parts of an SVG logo and places them on a puzzle piece, on a 256x256 canvas:
    source   SVG to take the artwork from, relative to icons.json.
    piece    Piece shape, relative to icons.json (default puzzle-piece.png). A .png uses its alpha as the shape and shadow;
             an .svg is drawn as vector shapes (black, 256x256 canvas) and gets a generated shadow.
    outputs  .ico files to write, relative to icons.json.
    plainOutputs  .ico or .png (256px) files without the dark-theme opt-out pixel, e.g. the extension's own icon (optional).
    fill     Radial gradient for the piece: centre cx/cy, radius r, and [offset, colour] stops.
    rim      Light edge inside the piece (width in px, colour), so dark artwork doesn't merge into dark themes. Width 0 turns it off.
    outline  Thin line at the very edge (width in px, colour), so light pieces don't fade into light themes. SVG pieces only.
    shadow   Opacity of the soft shadow around the piece (default 0.45).
    layers   Artwork to place, in drawing order:
               select       CSS selector of the element(s) in the source SVG.
               x, y         Where the centre of the selected artwork goes.
               size         Height of the selected artwork in px.
               rotate       Degrees clockwise (optional).
               fill, stroke, strokeWidth   Overrides for the artwork's colours/line width (optional; width in source SVG units).
  Requires Microsoft Edge, which renders the SVG.
#>
param(
  [string]$Config = (Join-Path $PSScriptRoot 'icons.json'),
  [string[]]$Name,
  # Writes <name>.png previews next to icons.json instead of the .ico files.
  [switch]$Preview,
  [int[]]$Sizes = @(256, 128, 96, 80, 64, 56, 48, 40, 32, 24, 20, 16),
  # Renders this many times larger and scales down, which smooths the piece outline.
  [int]$Supersample = 4,
  [string]$Edge = "${env:ProgramFiles(x86)}\Microsoft\Edge\Application\msedge.exe"
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing

if (-not (Test-Path $Edge)) { $Edge = "$env:ProgramFiles\Microsoft\Edge\Application\msedge.exe" }
if (-not (Test-Path $Edge)) { throw "Microsoft Edge not found; pass -Edge <path to msedge.exe>." }

$baseDir = Split-Path -Parent (Resolve-Path $Config)
$icons = Get-Content $Config -Raw | ConvertFrom-Json
if ($Name) { $icons = @($icons | Where-Object { $Name -contains $_.name }) }

$work = Join-Path ([IO.Path]::GetTempPath()) "rhino-icons-$PID"
New-Item -ItemType Directory -Force $work | Out-Null

function Invoke-Edge([string[]]$EdgeArgs) {
  $out = Join-Path $work 'edge-out.txt'
  # Own profile keeps Edge from syncing or touching the user's browser profile.
  $edgeArgs = @('--headless=new', '--disable-gpu', '--hide-scrollbars', '--no-first-run', "--user-data-dir=`"$work\profile`"") + $EdgeArgs
  Start-Process $Edge -ArgumentList $edgeArgs -Wait -WindowStyle Hidden -RedirectStandardOutput $out -RedirectStandardError "$work\edge-err.txt"
  if (Test-Path $out) { Get-Content $out -Raw }
}

function New-IconPage($icon) {
  $svg = Get-Content (Join-Path $baseDir $icon.source) -Raw
  $svg = $svg -replace '<\?xml[^>]*\?>', '' -replace '<!DOCTYPE[^>]*>', ''
  $stops = ($icon.fill.stops | ForEach-Object { "<stop offset='$($_[0])' stop-color='$($_[1])'/>" }) -join ''
  $rimWidth = if ($icon.rim) { [double]$icon.rim.width } else { 0 }
  $rimColor = if ($icon.rim) { $icon.rim.color } else { '#ffffff' }
  $outlineWidth = if ($icon.outline) { [double]$icon.outline.width } else { 0 }
  $outlineColor = if ($icon.outline) { $icon.outline.color } else { '#000000' }
  $shadow = if ($null -ne $icon.shadow) { $icon.shadow } else { 0.45 }
  $layers = ConvertTo-Json -InputObject @($icon.layers) -Depth 5 -Compress
  $pieceFile = Join-Path $baseDir $(if ($icon.piece) { $icon.piece } else { 'puzzle-piece.png' })
  if ($pieceFile -like '*.svg') {
    $shape = [regex]::Match((Get-Content $pieceFile -Raw), '(?s)<svg[^>]*>(.*)</svg>').Groups[1].Value
    # Rim is the inner half of a stroke along the outline, so it matches the crisp clipped edge.
    $pieceDefs = @"
  <clipPath id="icon-clip">$shape</clipPath>
  <filter id="icon-shadow" x="-10%" y="-10%" width="120%" height="120%"><feGaussianBlur stdDeviation="3"/><feOffset dy="1"/></filter>
  <style>#icon-rim * { fill: none; stroke: $rimColor; stroke-width: $(2 * $rimWidth)px; }
  #icon-outline * { fill: none; stroke: $outlineColor; stroke-width: $(2 * $outlineWidth)px; }</style>
"@
    $pieceBody = @"
 <g filter="url(#icon-shadow)" opacity="$shadow">$shape</g>
 <g clip-path="url(#icon-clip)"><rect width="256" height="256" fill="url(#icon-fill)"/><g id="icon-art"/>$(if ($rimWidth -gt 0) { "<g id=`"icon-rim`">$shape</g>" })$(if ($outlineWidth -gt 0) { "<g id=`"icon-outline`">$shape</g>" })</g>
"@
  }
  else {
    $mask = 'data:image/png;base64,' + [Convert]::ToBase64String([IO.File]::ReadAllBytes($pieceFile))
    $pieceDefs = @"
  <filter id="icon-piece" x="0" y="0" width="256" height="256" filterUnits="userSpaceOnUse" color-interpolation-filters="sRGB">
   <feImage xlink:href="$mask" x="0" y="0" width="256" height="256" result="mask"/>
   <!-- The mask's soft shadow is not part of the piece; keep only its solid area. -->
   <feComponentTransfer in="mask" result="piece"><feFuncA type="linear" slope="20" intercept="-19"/></feComponentTransfer>
   <feMorphology in="piece" operator="erode" radius="$rimWidth" result="inner"/>
   <feComposite in="SourceGraphic" in2="inner" operator="in" result="art"/>
   <feFlood flood-color="$rimColor"/>
   <feComposite in2="piece" operator="in" result="rim"/>
   <feFlood flood-color="#000" flood-opacity="$shadow"/>
   <feComposite in2="mask" operator="in" result="shadow"/>
   <feMerge><feMergeNode in="shadow"/><feMergeNode in="rim"/><feMergeNode in="art"/></feMerge>
  </filter>
"@
    $pieceBody = ' <g filter="url(#icon-piece)"><rect width="256" height="256" fill="url(#icon-fill)"/><g id="icon-art"/></g>'
  }
@"
<!doctype html><html><body style="margin:0;background:transparent;overflow:hidden">
<div id="src" style="position:absolute;left:-5000px;top:0">$svg</div>
<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="256" height="256" viewBox="0 0 256 256" style="position:absolute;left:0;top:0">
 <defs>
  <radialGradient id="icon-fill" gradientUnits="userSpaceOnUse" cx="$($icon.fill.cx)" cy="$($icon.fill.cy)" r="$($icon.fill.r)">$stops</radialGradient>
$pieceDefs
 </defs>
$pieceBody
</svg>
<pre id="log" style="display:none"></pre>
<script>
const NS = 'http://www.w3.org/2000/svg', log = [];
const root = document.querySelector('#src svg');
// Some sources size themselves to 100%, which measures as nothing off-screen.
root.setAttribute('width', '512'); root.setAttribute('height', '512');
const toRoot = root.getScreenCTM().inverse();
for (const layer of $layers) {
  const g = document.getElementById('icon-art').appendChild(document.createElementNS(NS, 'g'));
  const found = root.querySelectorAll(layer.select);
  if (!found.length) { log.push('ERROR: nothing matches ' + layer.select); continue; }
  for (const el of found) {
    // Keep the transforms the element inherits from its ancestors in the source.
    const m = toRoot.multiply(el.parentNode.getScreenCTM());
    const wrap = g.appendChild(document.createElementNS(NS, 'g'));
    wrap.setAttribute('transform', 'matrix(' + [m.a, m.b, m.c, m.d, m.e, m.f].join(' ') + ')');
    const copy = wrap.appendChild(el.cloneNode(true));
    for (const e of [copy, ...copy.querySelectorAll('*')]) {
      e.removeAttribute('id');
      if (layer.fill) e.style.fill = layer.fill;
      if (layer.stroke) e.style.stroke = layer.stroke;
      if (layer.strokeWidth != null) e.style.strokeWidth = layer.strokeWidth;
    }
  }
  const b = g.getBBox(), s = layer.size / b.height;
  g.setAttribute('transform', 'translate(' + layer.x + ' ' + layer.y + ') rotate(' + (layer.rotate || 0) + ') scale(' + s + ') translate(' + -(b.x + b.width / 2) + ' ' + -(b.y + b.height / 2) + ')');
  log.push(layer.select + ': ' + found.length + ' element(s), ' + Math.round(b.width * s) + 'x' + layer.size + ' px');
}
document.getElementById('log').textContent = log.join('\n');
</script>
</body></html>
"@
}

function Write-Ico([string]$Path, [Drawing.Bitmap[]]$Frames) {
  $ms = New-Object IO.MemoryStream; $bw = New-Object IO.BinaryWriter $ms
  $data = foreach ($bmp in $Frames) {
    $w = $bmp.Width
    if ($w -ge 64) { $png = New-Object IO.MemoryStream; $bmp.Save($png, [Drawing.Imaging.ImageFormat]::Png); , $png.ToArray(); continue }
    # Small frames as classic 32bpp DIBs: bottom-up BGRA, then a 1bpp AND mask (1 = transparent).
    $dib = New-Object IO.MemoryStream; $dw = New-Object IO.BinaryWriter $dib
    $maskStride = [int]([Math]::Ceiling($w / 32.0) * 4)
    $dw.Write([int]40); $dw.Write([int]$w); $dw.Write([int]($w * 2)); $dw.Write([int16]1); $dw.Write([int16]32)
    $dw.Write([int]0); $dw.Write([int]($w * $w * 4 + $maskStride * $w)); $dw.Write([int]0); $dw.Write([int]0); $dw.Write([int]0); $dw.Write([int]0)
    for ($y = $w - 1; $y -ge 0; $y--) { for ($x = 0; $x -lt $w; $x++) { $c = $bmp.GetPixel($x, $y); $dw.Write([byte]$c.B); $dw.Write([byte]$c.G); $dw.Write([byte]$c.R); $dw.Write([byte]$c.A) } }
    for ($y = $w - 1; $y -ge 0; $y--) {
      $row = New-Object byte[] $maskStride
      for ($x = 0; $x -lt $w; $x++) { if ($bmp.GetPixel($x, $y).A -lt 128) { $row[$x -shr 3] = $row[$x -shr 3] -bor (0x80 -shr ($x -band 7)) } }
      $dw.Write($row)
    }
    $dw.Flush(); , $dib.ToArray()
  }
  $bw.Write([int16]0); $bw.Write([int16]1); $bw.Write([int16]$Frames.Count)
  $offset = 6 + 16 * $Frames.Count
  for ($i = 0; $i -lt $Frames.Count; $i++) {
    $d = if ($Frames[$i].Width -ge 256) { 0 } else { $Frames[$i].Width }
    $bw.Write([byte]$d); $bw.Write([byte]$d); $bw.Write([byte]0); $bw.Write([byte]0); $bw.Write([int16]1); $bw.Write([int16]32)
    $bw.Write([int]$data[$i].Length); $bw.Write([int]$offset); $offset += $data[$i].Length
  }
  foreach ($d in $data) { $bw.Write([byte[]]$d) }
  $bw.Flush(); [IO.File]::WriteAllBytes($Path, $ms.ToArray())
}

foreach ($icon in $icons) {
  $page = Join-Path $work "$($icon.name).html"
  [IO.File]::WriteAllText($page, (New-IconPage $icon), (New-Object Text.UTF8Encoding $false))
  $url = 'file:///' + ($page -replace '\\', '/')

  $dom = Invoke-Edge @('--dump-dom', "`"$url`"")
  $log = [regex]::Match($dom, '<pre id="log"[^>]*>([^<]*)</pre>').Groups[1].Value.Trim(); $log = [Net.WebUtility]::HtmlDecode($log)
  Write-Host "$($icon.name):"; $log -split "`n" | ForEach-Object { Write-Host "  $_" }
  if ($log -match 'ERROR') { throw "$($icon.name): fix the layer selectors in $Config." }

  $shot = Join-Path $work "$($icon.name).png"
  Invoke-Edge @("--screenshot=`"$shot`"", '--window-size=256,256', "--force-device-scale-factor=$Supersample", '--default-background-color=00000000', "`"$url`"") | Out-Null
  $full = New-Object Drawing.Bitmap ([Drawing.Image]::FromFile($shot))

  function Resize([int]$size) {
    $b = New-Object Drawing.Bitmap $size, $size, ([Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [Drawing.Graphics]::FromImage($b)
    $g.InterpolationMode = 'HighQualityBicubic'; $g.PixelOffsetMode = 'HighQuality'; $g.CompositingQuality = 'HighQuality'
    $g.DrawImage($full, 0, 0, $size, $size); $g.Dispose()
    $b
  }

  if ($Preview) {
    $out = Join-Path $baseDir "$($icon.name).png"; $b = Resize 256; $b.Save($out, [Drawing.Imaging.ImageFormat]::Png); $b.Dispose(); $full.Dispose()
    Write-Host "  preview: $out"; continue
  }

  # VS draws the frame matching its display size directly; any other size gets resized, which breaks the opt-out pixel below.
  $frames = foreach ($size in $Sizes) {
    $b = Resize $size
    # Cyan top-right pixel stops VS dark themes from inverting the icon; VS clears it when drawing.
    $b.SetPixel($size - 1, 0, [Drawing.Color]::FromArgb(255, 0, 255, 255))
    $b
  }
  foreach ($o in $icon.outputs) { $path = Join-Path $baseDir $o; Write-Ico $path $frames; Write-Host "  wrote $(Resolve-Path $path)" }
  $frames | ForEach-Object { $_.Dispose() }

  # Places VS doesn't theme would show the opt-out pixel, so these get clean frames.
  if ($icon.plainOutputs) {
    $frames = foreach ($size in $Sizes) { Resize $size }
    foreach ($o in $icon.plainOutputs) {
      $path = Join-Path $baseDir $o
      if ($o -like '*.png') { $frames[0].Save($path, [Drawing.Imaging.ImageFormat]::Png) } else { Write-Ico $path $frames }
      Write-Host "  wrote $(Resolve-Path $path)"
    }
    $frames | ForEach-Object { $_.Dispose() }
  }
  $full.Dispose()
}
Remove-Item -Recurse -Force $work -ErrorAction SilentlyContinue
