<?php
$Json = [];
foreach (array_diff(scandir('.'), [".", "..", "index.php"]) as $value)
    array_push($Json, [$value => hash_file('md5', $value)]);
exit(json_encode($Json, true));