using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

 
public class SoruBoslist
{
	public string soru;
	public string cevapA;
	public string cevapB;
	public string cevapC;
	public string cevapD;
	public string dogruCevap;
	public string seviye;
	public int kategori;

	public SoruBoslist(string _soru1, string _cevapA, string _cevapB, string _cevapC, string _cevapD, string _dogruCevap, string _seviye, int _kategori)
	{
		this.soru = _soru1;
		this.cevapA = _cevapA;
		this.cevapB = _cevapB;
		this.cevapC = _cevapC;
		this.cevapD = _cevapD;
		this.dogruCevap = _dogruCevap;
		this.seviye = _dogruCevap;
		this.kategori = _kategori;
	}
}



public class localVeriCek : MonoBehaviour {

	private SQLiteDB db;
	private string dbAdi;
	private string dbpath;
	private static  SQLiteQuery qr;
	public UILabel txtSELECT;
	private bool kopyalasil = false;

	void Start () {

		//databaseKopyala ();


		SoruDoldur("1", 3);
		SoruDoldur("2", 4);


	
	}


	// Update is called once per frame
	void Update () {
	
	}


	void databaseAc(){		
		//////////////////////////////////////////////////////////////////////////////////////////
		
		db = new SQLiteDB();		
		dbAdi = "database.db";
		
		byte[] bytes = null;		
		
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		dbpath = "file://" + Application.persistentDataPath + "/" + dbAdi;
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
		UnityEngine.Debug.Log (dbpath);
		#elif UNITY_WEBPLAYER
		string dbpath = "StreamingAssets/" + dbAdi;								
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
		#elif UNITY_IPHONE
		string dbpath = Application.dataPath + "/Raw/" + dbAdi;									
		try{	
			using ( FileStream fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.Read) ){
				bytes = new byte[fs.Length];
				fs.Read(bytes,0,(int)fs.Length);
			}			
		} catch (Exception e){
			log += 	"\nTest Fail with Exception " + e.ToString();
			log += 	"\n";
		}
		#elif UNITY_ANDROID
		string dbpath = Application.persistentDataPath + "/" + dbAdi;	           
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
		#endif
		
		if ( bytes != null )
		{
			
			try{	
				
				
				MemoryStream memStream = new MemoryStream();
				
				memStream.Write(bytes,0,bytes.Length);
				db.OpenStream("stream2",memStream); 
				
				
				
			} catch (Exception e){
				UnityEngine.Debug.Log ("hata:"+e);
			}
		}
		
		//////////////////////////////////////////////////////////////////////////////////////////
	}

	IEnumerator Download( WWW www )
	{
		yield return www;
	}



	void databaseKopyala(){
		
		kopyalasil = true;
		UnityEngine.Debug.Log ("kopya:"+kopyalasil);
		
		//////////////////////////////////////////////////////////////////////////////////////////
		
		db = new SQLiteDB();		
		dbAdi = "database.db";
		
		byte[] bytes = null;		
		
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		dbpath = "file://" + Application.streamingAssetsPath + "/" + dbAdi;
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
		//UnityEngine.Debug.Log (dbpath);
		#elif UNITY_WEBPLAYER
		string dbpath = "StreamingAssets/" + dbAdi;								
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
		#elif UNITY_IPHONE
		string dbpath = Application.dataPath + "/Raw/" + dbAdi;									
		try{	
			using ( FileStream fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.Read) ){
				bytes = new byte[fs.Length];
				fs.Read(bytes,0,(int)fs.Length);
			}			
		} catch (Exception e){
			log += 	"\nTest Fail with Exception " + e.ToString();
			log += 	"\n";
		}
		#elif UNITY_ANDROID
		string dbpath = Application.streamingAssetsPath + "/" + dbAdi;	           
		WWW www = new WWW(dbpath);
		Download(www);
		bytes = www.bytes;
		#endif
		
		if ( bytes != null )
		{
			
			try{	
				
				
				string filename = Application.persistentDataPath + "/" + dbAdi;
				
				//
				//
				// copy database to real file into cache folder
				using( FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write) )
				{
					fs.Write(bytes,0,bytes.Length);         
				}
				
				//File.Delete(dbpath);
				
				
				
				
				//
				// initialize database
				//
				db.Open(filename);                               
				
				//Test2(db, ref log);
				
				
				
			} catch (Exception e){
				UnityEngine.Debug.Log ("hata:"+e);
			}
			
			
		}
		
		UnityEngine.Debug.Log ("ahanda bu:" + dbpath);
		//////////////////////////////////////////////////////////////////////////////////////////
	}


	void SoruDoldur(string seviye, int sayi)
	{
		IList<SoruBoslist> Sorulistesi = new List<SoruBoslist>();

		databaseAc ();

		string select = "select * from SORULAR";

		qr = new SQLiteQuery(db, select); 

	

		while(qr.Step())			
		{	
			Sorulistesi.Add(new SoruBoslist(qr.GetString("soru").ToString(),qr.GetString("cevapA").ToString(),qr.GetString("cevapB").ToString(),qr.GetString("cevapC").ToString(),qr.GetString("cevapD").ToString(),qr.GetString("dogruCevap").ToString(),qr.GetString("soruSeviye").ToString(), qr.GetInteger("kategoriID")));			
		}
		
		db.Close();


		txtSELECT.text= Sorulistesi[0].soru;



	}

}
