/*
 * Created by SharpDevelop.
 * User: nomo
 * Date: 29.05.2012
 * Time: 18:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Threading;
using System.IO;
using System.Globalization;
using System.ComponentModel;
//using System.Threading;

namespace fluffydemon
{
	
	class userData

	{

		private string _value;
		private string _name;

		public string Value
		{

			get { return _value; }set { _value = value; }
		}

		public string Name
		{

			get { return _name; }

			set { _name = value; }
		}

		public userData(string name, string value)
		{

			_name = name;

			_value = value;

		}

		public override string ToString()
		{

			return _name;
		}

	}
	
	
	
	
	
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		
		
		public int downloadfile(string d_url, string d_file)
		{
			try
			{
				using (WebClient Client = new WebClient ())
				{
					Client.DownloadFile(d_url, d_file);
				}
				return 0;
			}
			catch(Exception e)
			{
				statout("Exception in downloadfile "+d_url+" "+e);
				return -1;
			}
		}
		
		
		public  void statout(string towrite)
		{
			try
			{
				//	textBox1.AppendText(towrite+"\r\n");
				MessageBox.Show(towrite);
			}
			catch
			{
				
			}
		}
		
		public void parseList(string lpath)
		{
			bool inlist = false;
			
			_items.Clear();
			
			try
			{

				using (StreamReader sr = new StreamReader(lpath))
				{
					String line;
					// Read and display lines from the file until the end of
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						if(line.IndexOf( "folder-list") != -1)inlist=true;
						if(line.IndexOf("rightbanner")!= -1)break;
						
						if(inlist==true){
							
							if(line.IndexOf("title=")!= -1)
							{
								if(line.IndexOf("Folder Back")== -1)
									getfolderpath(line);
								//	statout(line);
							}
						}
					}
					checkedListBox1.DataSource = null;
					checkedListBox1.DataSource = _items;
				}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				statout("The file could not be read:");
				statout(e.Message);
			}
			
			
		}
		
		List<string> _items = new List<string>();
		
		public void getfolderpath(string theline)
		{
			string[] substrs = theline.Split('\"');
			//	statout(substrs[1]+" - "+substrs[3]);
			
			_items.Add(substrs[1]);
			
		}
		
		
		
		public void parsefolder(string furl, bool insub)
		{
			
			//	if(activesubfolder=="")	parsesubfolder(furl);
			
			
			downloadfile(furl, "list2.html");
			
			bool hassubfolder = true;
			
			
			
			try
			{

				using (StreamReader sr = new StreamReader("list2.html"))
				{
					String line;
					// Read and display lines from the file until the end of
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						if(line.IndexOf( "Click to go to the download page") != -1)
						{
							hassubfolder = false;
							string[] substrs = line.Split('\"');
							
							downloadfolder(substrs[1]);
							
							activesubfolder="";
							
							//	statout(line);
							break;
						}
						
					}
					
					
					
				}
				
				//	if(hassubfolder)
				//	{
				if(insub == false)parsesubfolder(furl);
				
				//	}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				statout("The file could not be read:");
				statout(e.Message);
			}
			
			
			
			
		}
		
		
		void parsesubfolder(string furl)
		{
			downloadfile(furl, "list2_5.html");
			
			bool inlist = false;
			
			
			
			try
			{

				using (StreamReader sr = new StreamReader("list2_5.html"))
				{
					String line;
					// Read and display lines from the file until the end of
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						if(line.IndexOf( "folder-list") != -1)inlist=true;
						if(line.IndexOf("rightbanner")!= -1)break;
						
						if(inlist==true){
							
							if(line.IndexOf("title=")!= -1)
							{
								
								if(line.IndexOf("http://fluffy.is/Files/")!= -1)
								{
									if(line.IndexOf("Folder Back")== -1)
									{
										string[] substrs = line.Split('\"');
										//	statout(substrs[1]+" - "+substrs[3]);
										
										//	substrs[1]
										activesubfolder = substrs[1];
										parsefolder(((userData)comboBox1.SelectedItem).Value+"/"+activefolder+"/"+activesubfolder, true);
										
										
									}
									
									//	statout(line);
								}
							}
						}
					}
					
				}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				statout("The file could not be read:");
				statout(e.Message);
			}
			
			
			
		}
		
		
		
		
		
		public void downloadfolder(string furl)
		{
			downloadfile(furl, "list3.html");
			int filenum=0;
			int filetotal=0;
			try
			{

				using (StreamReader sr = new StreamReader("list3.html"))
				{
					String line;
					while ((line = sr.ReadLine()) != null)
					{
						if(line.IndexOf( "watch-") != -1)
						{
							filetotal++;
							
						}
						
					}
				}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				statout("The file could not be read:");
				statout(e.Message);
			}
			
			try
			{

				using (StreamReader sr = new StreamReader("list3.html"))
				{
					String line;
					// Read and display lines from the file until the end of
					// the file is reached.
					while ((line = sr.ReadLine()) != null)
					{
						if(line.IndexOf( "watch-") != -1)
						{
							//statout(line);
							char[] delimiterChars = { '\"', '<', '>'};
							string[] substr = line.Split(delimiterChars);
							
							filenum++;
							label3.Text="File "+filenum+" of "+filetotal+"";
							
							downloadvideo(substr[2], substr[4]);
							
							
							
						}
						
					}
				}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				statout("The file could not be read:");
				statout(e.Message);
			}
		}
		
		
		public bool isdownloading;
		
		public void downloadvideo(string furl, string fname)
		{
			string tempfolder = activefolder;
			if(activesubfolder !="")tempfolder=activefolder+"\\"+activesubfolder;
			
			//	statout(activefolder +": Downloading "+fname+" from "+furl);
			System.IO.Directory.CreateDirectory(tempfolder);
			
			if(File.Exists(tempfolder+"\\"+fname) == false)
			{
				

				WebClient client = new WebClient();
				client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
				client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
				
				label1.Text=""+fname;
				// Starts the download
				isdownloading=true;
				//   client.DownloadFileAsync(new Uri("http://fluffy.is/"+furl), activefolder+"\\"+fname);
				client.DownloadFileAsync(new Uri("http://fluffy.is/"+furl), tempfolder+"\\"+furl);
				
				// client.DownloadFile(new Uri("http://fluffy.is/"+furl), activefolder+"\\"+fname);

				//  btnStartDownload.Text = "Download In Process";
				//  btnStartDownload.Enabled = false;

				while(isdownloading)
				{
					Application.DoEvents();
					System.Threading.Thread.Sleep(5);
					
				}
				
				System.IO.File.Move(tempfolder+"\\"+furl, tempfolder+"\\"+fname);

			}
			
		}
		
		public double oldbytes=0;
		public double bytedif =0;
		public int oldsecs=0;
		
		void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			double bytesIn = double.Parse(e.BytesReceived.ToString());
			double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
			double percentage = bytesIn / totalBytes * 100;
			
			if(DateTime.Now.Second != oldsecs)
			{
				oldsecs = DateTime.Now.Second;
				bytedif = bytesIn - oldbytes;
				oldbytes=bytesIn;
			}
			
			label4.Text = (int)(bytesIn/1024)+" KB / "+(int)(totalBytes/1024)+" KB at "+(int)(bytedif/1024)+" KB/s";

			progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
		}


		
		void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			//	MessageBox.Show("Download Completed");

			isdownloading=false;
			//  btnStartDownload.Text = "Start Download";
			//  btnStartDownload.Enabled = true;
		}

		
		
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			comboBox1.Items.Add(new userData("Anime-A","http://fluffy.is/Files/Anime-A"));
			comboBox1.Items.Add(new userData("Anime-E","http://fluffy.is/Files/Anime-E"));
			comboBox1.Items.Add(new userData("Anime-Z","http://fluffy.is/Files/Anime-Z"));
			comboBox1.Items.Add(new userData("Movies and OVA","http://fluffy.is/Files/Movies%20and%20OVA"));
			comboBox1.Items.Add(new userData("Unfinished","http://fluffy.is/Files/Unfinished"));
			
			comboBox1.SelectedIndex = 0;
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			string sourcelist = ((userData)comboBox1.SelectedItem).Value;
			
			downloadfile(sourcelist, "list1.html");
			parseList("list1.html");
			button2.Enabled = true;
			groupBox2.Visible = true;
			
			
		}
		
		public string activefolder;
		public string activesubfolder;
		
		void Button2Click(object sender, EventArgs e)
		{
			
			groupBox3.Visible = true;
			button2.Enabled = false;
			button1.Enabled = false;
			comboBox1.Enabled = false;
			checkedListBox1.Enabled = false;
			
			if(checkedListBox1.CheckedItems.Count != 0)
			{
				// If so, loop through all checked items and print results.
				
				for(int x = 0; x <= checkedListBox1.CheckedItems.Count - 1 ; x++)
				{
					activefolder = checkedListBox1.CheckedItems[x].ToString();
					
					label2.Text = "Anime "+(x+1)+" of "+checkedListBox1.CheckedItems.Count+" ("+activefolder+")";
					
					//statout(checkedListBox1.CheckedItems[x].ToString());
					
					parsefolder(((userData)comboBox1.SelectedItem).Value + "/"+activefolder, false);
					
				}

			}
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}
	}
}
