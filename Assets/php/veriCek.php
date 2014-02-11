<?php

include("common.php");
	$link=dbConnect();

	

	$cumle = safe($_POST['cumle']);
	
	$query = $cumle;
	mysql_query("SET NAMES 'utf8'"); 
    $result = mysql_query($query);    
    $my_err = mysql_error();
    if($result === false || $my_err != '')
    {
        echo "
        <pre>
            $my_err <br />
            $query <br />
        </pre>";
        die();
    }

    $num_results = mysql_num_rows($result);
	
    for($i = 0; $i < $num_results; $i++)
    {
         $row = mysql_fetch_array($result);
 echo $row['soru'] . "|" . $row['cevapA'] . "|" . $row['cevapB'] . "|" . $row['cevapC'] . "|" . $row['cevapD'] . "|" . $row['dogruCevap'] . "==";
    }
?>