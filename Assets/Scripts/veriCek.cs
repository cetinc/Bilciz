using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Security;


public class veriCek : MonoBehaviour 
{

	public string GetUrl = "http://www.cetciz.com/unity3D/veriCek.php";//Server'da veri çekme özelliği bulunan php dosyasının yolu..
	public UILabel txtServer;//Ekrandaki Label objesi..

	
	void Start () 
	{	  
	}


	void Update () 
	{
	}

	void veriCekCagir(){
		StartCoroutine("GetData");//Veri çekme işlemi çağırılıyor..
	}

	
	IEnumerator GetData()//Veri çekme işlemleri
	{
				
   		txtServer.text = "Yükleniyor..";//Bekleme..
		
		WWWForm form = new WWWForm();

		form.AddField("cumle","SELECT * FROM cetciz_unity.SORULAR ORDER by soruID ASC");//Veri çekme cümlesi..
		
		WWW www = new WWW(GetUrl,form);

    	yield return www;
		Debug.Log("gelen text:"+www.text);

		if(www.text == "") //Gelen veri boş ise hataya düşer..
    	{
			Debug.Log("Hata: " + www.error);
		
    	}
		else 
		{       	
			txtServer.text = www.text;//Label'a geleni yaz..

			//Burada gelen string verisi parse ediliyor ve liste adındaki diziye aktarılıyor..
			string[] splitSorular  = www.text.Split("=="[1]);
			string[] splitSoruici  = new string[]{};
			List<string> liste = new List<string>();

			
			foreach (string s in splitSorular) {
				splitSoruici = s.Split("|"[0]);
				foreach (string s1 in splitSoruici) {
					if (s1!="") liste.Add(s1);//Veriler listeye satır satır aktarılıyor..
				}
			}

			foreach (string item in liste) {
				Debug.Log(item);//Parse edilen verileri tutan liste adlı dizi içeriği okunup debu ediliyor..				
			}
					

		}
	}
		

}
