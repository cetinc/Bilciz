using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;


public class veriGonder : MonoBehaviour 
{
	public string secretKey = "12345";//Şifreleme için gerekli doğrulama kodu..
	public string PostUrl = "http://www.cetciz.com/unity3D/veriGonder.php?";//Server'da veri gönderme özelliği bulunan php dosyasının yolu..


	void Start () 
	{
	 StartCoroutine(PostData());//Data Post fonksiyonu..
	}


	void Update () 
	{
	}	

	
	IEnumerator PostData()//Veri gönderme işlemleri
	{
		//Veriler hazırlanıyor
		string _nickName = "test333";
		string _faceID = "0";
		string _faceDurum = "false";
		string _faceAdi = "";

		Debug.Log ("girdi:");

		//Veriler şifreleniyor..
		string hash = Md5Sum(_nickName + _faceID + _faceDurum + _faceAdi + secretKey).ToLower();
		Debug.Log ("hash:"+hash);
		
		WWWForm form = new WWWForm();

		//Veriler ekleniyor..
		form.AddField("nickName",_nickName);
		form.AddField("faceID",_faceID);
		form.AddField("faceDurum",_faceDurum);
		form.AddField("faceAdi",_faceAdi);
		form.AddField("hash",hash);
		form.AddField("durum","kullaniciekle");

		
		WWW www = new WWW(PostUrl,form);//Post ediliyor..

		yield return www;

		Debug.Log ("www:"+www.text);
		
    	if(www.text == "done")//işlem tamam
    	{
       		//StartCoroutine("GetData");
			Debug.Log("işlem tamam!");
    	}
		else 
		{
			Debug.Log("hata!:" + www.error);
		}
	}
	


	//Md5 Şifreleme işlemleri
	public string Md5Sum(string input)
	{
    	System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
    	byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
    	byte[] hash = md5.ComputeHash(inputBytes);
 
    	StringBuilder sb = new StringBuilder();
    	for (int i = 0; i < hash.Length; i++)
    	{
    	    sb.Append(hash[i].ToString("X2"));
    	}
    	return sb.ToString();
	}
}
