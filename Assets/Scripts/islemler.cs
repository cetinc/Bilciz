using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

public class islemler : MonoBehaviour {

	private bool kopyalasil = false;
	private SQLiteDB db;
	private string dbAdi;
	private string dbpath;
	private static  SQLiteQuery qr;
	public UILabel txtSELECT;
	//private string queryCreate = "CREATE TABLE IF NOT EXISTS KULLANICI (id INTEGER PRIMARY KEY, isim TEXT, sayi NUMERIC);";
	//private string queryInsert = "INSERT INTO KULLANICI (isim,sayi) VALUES(?,?);";

	// Use this for initialization
	void Start () {


	
	}

	void databaseAc(){



		//////////////////////////////////////////////////////////////////////////////////////////
		
		db = new SQLiteDB();		
		dbAdi = "database.db";
		
		byte[] bytes = null;		
		
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		dbpath = "file://" + Application.streamingAssetsPath + "/" + dbAdi;
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
		string dbpath = Application.streamingAssetsPath + "/" + dbAdi;	           
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



	IEnumerator Download( WWW www )
	{
			yield return www;
		}
		

	void Update () {
		
	}
	
	public void VeritabaniOlustur( )
	{
		db.Open(dbAdi);
		qr = new SQLiteQuery(db, "CREATE TABLE IF NOT EXISTS KULLANICI (id INTEGER PRIMARY KEY, isim TEXT, sayi NUMERIC);"); 
		qr.Step();												
		qr.Release();   
		db.Close();		
	}

	public void  VeriEkle(GameObject obje)
	{

		db = new SQLiteDB();		
		dbAdi = "database.db";
		
	

		dbpath = Application.persistentDataPath + "/" + dbAdi;

		db.Open (dbpath);
		UnityEngine.Debug.Log(dbpath);
		
		string insert = obje.GetComponent<butonOlaylari>().InsertCumlesi;
		string aa = "adsiz";
		int bb = 12;

	
		try {	
			UnityEngine.Debug.Log("girdi");
			qr = new SQLiteQuery(db, insert); 
			qr.Bind(aa);
			qr.Bind(bb);
			qr.Step();
			qr.Release(); 
			db.Close();
			//return true;
			UnityEngine.Debug.Log("kapattı");
			
				} catch (Exception ex) {
			//return false;
			UnityEngine.Debug.Log("hata"+ex);

				}

	}

	public void VeriGetir(GameObject obje)
	{

		if (kopyalasil == false) databaseKopyala ();

		databaseAc ();


		string select = obje.GetComponent<butonOlaylari>().SelectCumlesi;
		//GetComponent<GUIText>().color = go.GetComponent<TextProperties>().renk;


		List<string> liste = new List<string>();

				
		try {

				qr = new SQLiteQuery(db, select); 
				
					while(qr.Step())
						
					{	
						liste.Add(qr.GetString("isim") + " - " + qr.GetInteger("sayi").ToString());
						
					}
				
				db.Close();
				
				txtSELECT.text = "";
				
					
					for (int i = 0; i < liste.Count; i++) {

						txtSELECT.text = txtSELECT.text + liste[i] + "\n";
					}	

		} 

		catch (Exception ex) {
			UnityEngine.Debug.Log("hata"+ex);
		}


	
	}


	public void VeriSil(GameObject obje)
	{
		SQLiteQuery qr;

		try {

			databaseAc ();
			qr = new SQLiteQuery(db, "DELETE FROM KULLANICI WHERE isim = 'Nazif';"); 
			qr.Step();												
			qr.Release();   
			db.Close();	

			UnityEngine.Debug.Log("silme");


			
				} catch (Exception ex) {
			UnityEngine.Debug.Log("hata"+ex);
				}
	
	}


}


