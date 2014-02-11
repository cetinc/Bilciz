<?php

$dbName = 'cetciz_unity';
$secretKey = "12345";

function dbConnect()
{
	global  $dbName;
	global  $secretKey;

	$link = mysql_connect('localhost', 'cetciz_test', 'k#gHU^cVRqHA');
	mysql_query("SET NAMES UTF8");
	
	if(!$link)
	{
		fail("Couldn´t connect to database server");
	}
	
	if(!@mysql_select_db($dbName))
	{
		fail("Couldn´t find database $dbName");
	}
	
	return $link;
	}
	

function safe($variable)
{
	$variable = addslashes(trim($variable));//türkçe olayını hallediyoruz
	return $variable;
}

function fail($errorMsg)
{
	print $errorMsg;
	exit;
}

?>