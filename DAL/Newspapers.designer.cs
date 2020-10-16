﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParsetCrawler.DAL
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Parset")]
	public partial class NewspapersDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertNPFirstPages(NPFirstPages instance);
    partial void UpdateNPFirstPages(NPFirstPages instance);
    partial void DeleteNPFirstPages(NPFirstPages instance);
    partial void InsertNewspapers(Newspapers instance);
    partial void UpdateNewspapers(Newspapers instance);
    partial void DeleteNewspapers(Newspapers instance);
    #endregion
		
		public NewspapersDataContext() : 
				base(global::ParsetCrawler.Properties.Settings.Default.ParsetConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public NewspapersDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NewspapersDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NewspapersDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NewspapersDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<NPFirstPages> NPFirstPages
		{
			get
			{
				return this.GetTable<NPFirstPages>();
			}
		}
		
		public System.Data.Linq.Table<vNewspapers> vNewspapers
		{
			get
			{
				return this.GetTable<vNewspapers>();
			}
		}
		
		public System.Data.Linq.Table<Newspapers> Newspapers
		{
			get
			{
				return this.GetTable<Newspapers>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.NPFirstPages")]
	public partial class NPFirstPages : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Code;
		
		private string _PersianDate;
		
		private string _PicUrl;
		
		private int _NewspaperCode;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCodeChanging(int value);
    partial void OnCodeChanged();
    partial void OnPersianDateChanging(string value);
    partial void OnPersianDateChanged();
    partial void OnPicUrlChanging(string value);
    partial void OnPicUrlChanged();
    partial void OnNewspaperCodeChanging(int value);
    partial void OnNewspaperCodeChanged();
    #endregion
		
		public NPFirstPages()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PersianDate", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string PersianDate
		{
			get
			{
				return this._PersianDate;
			}
			set
			{
				if ((this._PersianDate != value))
				{
					this.OnPersianDateChanging(value);
					this.SendPropertyChanging();
					this._PersianDate = value;
					this.SendPropertyChanged("PersianDate");
					this.OnPersianDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PicUrl", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string PicUrl
		{
			get
			{
				return this._PicUrl;
			}
			set
			{
				if ((this._PicUrl != value))
				{
					this.OnPicUrlChanging(value);
					this.SendPropertyChanging();
					this._PicUrl = value;
					this.SendPropertyChanged("PicUrl");
					this.OnPicUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NewspaperCode", DbType="Int NOT NULL")]
		public int NewspaperCode
		{
			get
			{
				return this._NewspaperCode;
			}
			set
			{
				if ((this._NewspaperCode != value))
				{
					this.OnNewspaperCodeChanging(value);
					this.SendPropertyChanging();
					this._NewspaperCode = value;
					this.SendPropertyChanged("NewspaperCode");
					this.OnNewspaperCodeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.vNewspapers")]
	public partial class vNewspapers
	{
		
		private int _Code;
		
		private string _Title;
		
		private string _REPic;
		
		private string _URL;
		
		private string _PersianDate;
		
		public vNewspapers()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", DbType="Int NOT NULL")]
		public int Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this._Code = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this._Title = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_REPic", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string REPic
		{
			get
			{
				return this._REPic;
			}
			set
			{
				if ((this._REPic != value))
				{
					this._REPic = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string URL
		{
			get
			{
				return this._URL;
			}
			set
			{
				if ((this._URL != value))
				{
					this._URL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PersianDate", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string PersianDate
		{
			get
			{
				return this._PersianDate;
			}
			set
			{
				if ((this._PersianDate != value))
				{
					this._PersianDate = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Newspapers")]
	public partial class Newspapers : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Code;
		
		private string _Title;
		
		private string _REPic;
		
		private string _URL;
		
		private string _NextPageRE;
		
		private string _BaseUrl;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCodeChanging(int value);
    partial void OnCodeChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    partial void OnREPicChanging(string value);
    partial void OnREPicChanged();
    partial void OnURLChanging(string value);
    partial void OnURLChanged();
    partial void OnNextPageREChanging(string value);
    partial void OnNextPageREChanged();
    partial void OnBaseUrlChanging(string value);
    partial void OnBaseUrlChanged();
    #endregion
		
		public Newspapers()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_REPic", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string REPic
		{
			get
			{
				return this._REPic;
			}
			set
			{
				if ((this._REPic != value))
				{
					this.OnREPicChanging(value);
					this.SendPropertyChanging();
					this._REPic = value;
					this.SendPropertyChanged("REPic");
					this.OnREPicChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string URL
		{
			get
			{
				return this._URL;
			}
			set
			{
				if ((this._URL != value))
				{
					this.OnURLChanging(value);
					this.SendPropertyChanging();
					this._URL = value;
					this.SendPropertyChanged("URL");
					this.OnURLChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NextPageRE", DbType="NVarChar(500)")]
		public string NextPageRE
		{
			get
			{
				return this._NextPageRE;
			}
			set
			{
				if ((this._NextPageRE != value))
				{
					this.OnNextPageREChanging(value);
					this.SendPropertyChanging();
					this._NextPageRE = value;
					this.SendPropertyChanged("NextPageRE");
					this.OnNextPageREChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BaseUrl", DbType="NVarChar(500)")]
		public string BaseUrl
		{
			get
			{
				return this._BaseUrl;
			}
			set
			{
				if ((this._BaseUrl != value))
				{
					this.OnBaseUrlChanging(value);
					this.SendPropertyChanging();
					this._BaseUrl = value;
					this.SendPropertyChanged("BaseUrl");
					this.OnBaseUrlChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
