$x = ls data
foreach ($ci in $x) {
    cp data\$ci .\web_client\Models\data -Recurse -Force
    cp data\$ci .\desktop_client\data -Recurse -Force
}