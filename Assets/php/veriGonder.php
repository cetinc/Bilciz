<?php 

include("common.php");

	$link=dbConnect();
	
	$durum = safe($_POST['durum']);	
	
if($durum == 'kullaniciekle') 
	{	
	$nickName = safe($_POST['nickName']);
	$faceID = safe($_POST['faceID']);
	$faceDurum = safe($_POST['faceDurum']);
	$faceAdi = safe($_POST['faceAdi']);


    $real_hash = md5($nickName . $faceID . $faceDurum . $faceAdi . $secretKey); 
    if($real_hash == $hash) 
	{
		$query = "INSERT INTO $dbName .`KULLANICI` (`kullaniciID`, `nickName`, `faceID`, `faceDurum`, `faceAdi`, `seviyePuani`, `altinSayisi`, `elmasSayisi`, `genelKultur`, `edebiyat`, `tarih`, `cografya`, `kulturSanat`, `spor`, `muzik`, `sinema`, `siyaset`, `dinKulturu`, `oyunDunyasi`, `teknoloji`, `moda`, `magazin`) VALUES (NULL, '$nickName', '$faceID', '$faceDurum', '$faceAdi', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);"; 
 
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
		
		echo "done";
	} 
}
	
	
?>