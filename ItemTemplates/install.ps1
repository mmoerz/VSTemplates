
$directories = Get-ChildItem -Attributes Directory

foreach($dir in $directories) {
    rm -force ./$dir.zip
    Compress-Archive $dir/* -DestinationPath ./$dir.zip
}

$docpath = [environment]::getfolderpath("mydocuments") 
Write-Host $docpath

$zipfiles = Get-ChildItem "*.zip"
$directories = Get-ChildItem -Attributes Directory "$docpath/Visual Studio *"

foreach($zip in $zipfiles) {
    foreach($dir in $directories) {
        if (Test-Path "$dir\Templates\ItemTemplates\Visual C#\") {
            $dest = "$dir\Templates\ItemTemplates\Visual C#\" + $zip.BaseName + $zip.Extension
            copy $zip "$dest"
        }
    }
}
    