using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;


public class SoruBoslistesi
{
	public string soru;
	public string cevapA;
	public string cevapB;
	public string cevapC;
	public string cevapD;
	public int dogruCevap;
	public int seviye;
	public int kategori;
	public int soruTipi;
	
	public SoruBoslistesi(string _soru, string _cevapA, string _cevapB, string _cevapC, string _cevapD, int _dogruCevap, int _seviye, int _kategori, int _soruTipi)
	{
		this.soru = _soru;
		this.cevapA = _cevapA;
		this.cevapB = _cevapB;
		this.cevapC = _cevapC;
		this.cevapD = _cevapD;
		this.dogruCevap = _dogruCevap;
		this.seviye = _seviye;
		this.kategori = _kategori;
		this.soruTipi = _soruTipi;
	}
}


public class setup : MonoBehaviour {

	// Use this for initialization

	private SQLiteDB db;
	private string dbAdi;
	private string dbpath;
	private static  SQLiteQuery qr;
	public UILabel txtSELECT;
	private bool kopyalasil = false;

	byte[] bytes = null;


	public string GetUrl = "http://www.cetciz.com/unity3D/veriCek.php";//Server'da veri çekme özelliği bulunan php dosyasının yolu..
	public UILabel txtServer;//Ekrandaki Label objesi..
	IList<SoruBoslistesi> Sorulistesi = new List<SoruBoslistesi>();


	void Awake () {

		db = new SQLiteDB();		
		dbAdi = "database.db";	



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void veriTabaniKur(){


		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		dbpath = Application.persistentDataPath + "/" + dbAdi;

		#elif UNITY_WEBPLAYER
		dbpath = "StreamingAssets/" + dbAdi;								
	
		#elif UNITY_IPHONE
		dbpath = Application.dataPath + "/Raw/" + dbAdi;									

		#elif UNITY_ANDROID
		dbpath = Application.persistentDataPath + "/" + dbAdi;	           
		#endif


		

		if (!File.Exists (dbpath)) {
				
			databaseKopyala ();		
			StartCoroutine ("GetData");	



				} else {
				
			txtSELECT.text="dosya var";
		}

	}


	IEnumerator GetData()//Veri çekme işlemleri
	{
		
				txtServer.text = "Yükleniyor..";//Bekleme..		
				WWWForm form = new WWWForm ();		
				form.AddField ("cumle", "SELECT * FROM cetciz_unity.SORULAR ORDER by soruID ASC");//Veri çekme cümlesi..
				WWW www = new WWW (GetUrl, form);		
				yield return www;			
				if (www.text == "") { //Gelen veri boş ise hataya düşer..
						UnityEngine.Debug.Log ("Hata: " + www.error);
			
				} else {   


							

			string[] GelensoruText  = www.text.Split("=="[0]);
			string[] Sorusatir;


			foreach (string TekSoru in GelensoruText) {

				if(TekSoru!="")
				{

					Sorusatir = TekSoru.Split("|"[0]);
					int i=0;
					string Soru = null;
					string CevapA=null;
					string CevapB=null;
					string CevapC=null;
					string CevapD=null;
					int dogruCevap=0;
					int soruSeviye=0;
					
					foreach (string TekSoruSatir in Sorusatir) {

						if (i==1){Soru = TekSoruSatir;}	
						if (i==2){CevapA = TekSoruSatir;}	
						if (i==3){CevapB = TekSoruSatir;}
						if (i==4){CevapC = TekSoruSatir;}
						if (i==5){CevapD = TekSoruSatir;}
						if (i==6){dogruCevap =Convert.ToInt32(TekSoruSatir);}
						if (i==7)
						{
							soruSeviye = Convert.ToInt32(TekSoruSatir);
							Sorulistesi.Add(new SoruBoslistesi(Soru.ToString(),CevapA.ToString(),CevapB.ToString(),CevapC.ToString(),CevapD.ToString(),dogruCevap,soruSeviye,1,0));	
						}

						i++;						
						
						if (i==10) i=0;
						
					}
					}			
			}	
			tabloOlustur();
			Soruekle();

		}

		}


	void Soruekle(){

		//databaseAc ();

		for (int i = 0; i < Sorulistesi.Count; i++) {


			qr = new SQLiteQuery(db, "INSERT INTO SORULAR (soruID,soru,cevapA,cevapB,cevapC,cevapD,dogruCevap,soruSeviye,kategoriID,soruTipi) VALUES (null,?,?,?,?,?,?,?,?,?);"); 
			//qr.Bind("null");
			qr.Bind(Sorulistesi[i].soru);
			qr.Bind(Sorulistesi[i].cevapA);
			qr.Bind(Sorulistesi[i].cevapB);
			qr.Bind(Sorulistesi[i].cevapC);
			qr.Bind(Sorulistesi[i].cevapD);
			qr.Bind(Sorulistesi[i].dogruCevap);
			qr.Bind(Sorulistesi[i].seviye);
			qr.Bind(1);
			qr.Bind(0);
			qr.Step();


				}
		qr.Release(); 
		db.Close();	

		txtSELECT.text = "işlem tamam";

	}
	

	void databaseAc()
	{		
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
	}

	void databaseKopyala(){

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
		
		//UnityEngine.Debug.Log ("ahanda bu:" + dbpath);
		//////////////////////////////////////////////////////////////////////////////////////////
	}


	void tabloOlustur(){
	

		//databaseAc();
		qr = new SQLiteQuery(db, "CREATE TABLE IF NOT EXISTS SORULAR (soruID INTEGER PRIMARY KEY, soru TEXT, cevapA TEXT, cevapB TEXT, cevapC TEXT, cevapD TEXT, dogruCevap NUMERIC, soruSeviye NUMERIC, kategoriID NUMERIC, soruTipi NUMERIC);"); 
		qr.Step();												
		qr.Release();   
		//db.Close();	



	}


	IEnumerator Download( WWW www )
	{
		yield return www;
	}

}
