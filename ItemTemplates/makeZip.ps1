
$directories = Get-ChildItem -Attributes Directory

foreach($dir in $directories) {
    rm -force ./$dir.zip
    Compress-Archive $dir/* -DestinationPath ./$dir.zip
}