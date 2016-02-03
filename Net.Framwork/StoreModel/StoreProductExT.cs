using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace Net.Framework.StoreModel
{
    /// <summary>
    /// 
    /// </summary>
    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.STORE_PRODUCT")]
    public partial class StoreProductExT : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private long _NO;

        private string _VAR_NO;

        private string _NAME;

        private string _FILE_NAME;

        private string _FILE_RENAME;

        private string _FILE_EXT;

        private string _MIME_TYPE;

        private System.Nullable<double> _FILE_SIZE;

        private System.Nullable<double> _MATERIAL_VOLUME;

        private System.Nullable<double> _OBJECT_VOLUME;

        private System.Nullable<double> _SIZE_X;

        private System.Nullable<double> _SIZE_Y;

        private System.Nullable<double> _SIZE_Z;

        private System.Nullable<double> _SCALE;

        private string _SHORT_LINK;

        private string _VIDEO_URL;

        private System.Nullable<int> _CATEGORY_NO;

        private string _CONTENTS;

        private string _DESCRIPTION;

        private System.Nullable<int> _PART_CNT;

        private System.Nullable<char> _CUSTERMIZE_YN;

        private System.Nullable<char> _SELL_YN;

        private string _TAG_NAME;

        private System.Nullable<char> _CERTIFICATE_YN;

        private System.Nullable<char> _VISIBILITY_YN;

        private System.Nullable<char> _USE_YN;

        private System.Nullable<int> _MEMBER_NO;

        private System.Nullable<double> _TXT_SIZE_X;

        private System.Nullable<double> _TXT_SIZE_Y;

        private System.Nullable<int> _DETAIL_TYPE;

        private System.Nullable<int> _DETAIL_DEPTH;

        private string _TXT_LOC;

        private System.Nullable<System.DateTime> _REG_DT;

        private string _REG_ID;

        private System.Nullable<System.DateTime> _UPD_DT;

        private string _UPD_ID;

        private System.Nullable<int> _TOTAL_PRICE;

        #region 확장성 메서드 정의
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnNOChanging(long value);
        partial void OnNOChanged();
        partial void OnVAR_NOChanging(string value);
        partial void OnVAR_NOChanged();
        partial void OnNAMEChanging(string value);
        partial void OnNAMEChanged();
        partial void OnFILE_NAMEChanging(string value);
        partial void OnFILE_NAMEChanged();
        partial void OnFILE_RENAMEChanging(string value);
        partial void OnFILE_RENAMEChanged();
        partial void OnFILE_EXTChanging(string value);
        partial void OnFILE_EXTChanged();
        partial void OnMIME_TYPEChanging(string value);
        partial void OnMIME_TYPEChanged();
        partial void OnFILE_SIZEChanging(System.Nullable<double> value);
        partial void OnFILE_SIZEChanged();
        partial void OnMATERIAL_VOLUMEChanging(System.Nullable<double> value);
        partial void OnMATERIAL_VOLUMEChanged();
        partial void OnOBJECT_VOLUMEChanging(System.Nullable<double> value);
        partial void OnOBJECT_VOLUMEChanged();
        partial void OnSIZE_XChanging(System.Nullable<double> value);
        partial void OnSIZE_XChanged();
        partial void OnSIZE_YChanging(System.Nullable<double> value);
        partial void OnSIZE_YChanged();
        partial void OnSIZE_ZChanging(System.Nullable<double> value);
        partial void OnSIZE_ZChanged();
        partial void OnSCALEChanging(System.Nullable<double> value);
        partial void OnSCALEChanged();
        partial void OnSHORT_LINKChanging(string value);
        partial void OnSHORT_LINKChanged();
        partial void OnVIDEO_URLChanging(string value);
        partial void OnVIDEO_URLChanged();
        partial void OnCATEGORY_NOChanging(System.Nullable<int> value);
        partial void OnCATEGORY_NOChanged();
        partial void OnCONTENTSChanging(string value);
        partial void OnCONTENTSChanged();
        partial void OnDESCRIPTIONChanging(string value);
        partial void OnDESCRIPTIONChanged();
        partial void OnPART_CNTChanging(System.Nullable<int> value);
        partial void OnPART_CNTChanged();
        partial void OnCUSTERMIZE_YNChanging(System.Nullable<char> value);
        partial void OnCUSTERMIZE_YNChanged();
        partial void OnSELL_YNChanging(System.Nullable<char> value);
        partial void OnSELL_YNChanged();
        partial void OnTAG_NAMEChanging(string value);
        partial void OnTAG_NAMEChanged();
        partial void OnCERTIFICATE_YNChanging(System.Nullable<char> value);
        partial void OnCERTIFICATE_YNChanged();
        partial void OnVISIBILITY_YNChanging(System.Nullable<char> value);
        partial void OnVISIBILITY_YNChanged();
        partial void OnUSE_YNChanging(System.Nullable<char> value);
        partial void OnUSE_YNChanged();
        partial void OnMEMBER_NOChanging(System.Nullable<int> value);
        partial void OnMEMBER_NOChanged();
        partial void OnTXT_SIZE_XChanging(System.Nullable<double> value);
        partial void OnTXT_SIZE_XChanged();
        partial void OnTXT_SIZE_YChanging(System.Nullable<double> value);
        partial void OnTXT_SIZE_YChanged();
        partial void OnDETAIL_TYPEChanging(System.Nullable<int> value);
        partial void OnDETAIL_TYPEChanged();
        partial void OnDETAIL_DEPTHChanging(System.Nullable<int> value);
        partial void OnDETAIL_DEPTHChanged();
        partial void OnTXT_LOCChanging(string value);
        partial void OnTXT_LOCChanged();
        partial void OnREG_DTChanging(System.Nullable<System.DateTime> value);
        partial void OnREG_DTChanged();
        partial void OnREG_IDChanging(string value);
        partial void OnREG_IDChanged();
        partial void OnUPD_DTChanging(System.Nullable<System.DateTime> value);
        partial void OnUPD_DTChanged();
        partial void OnUPD_IDChanging(string value);
        partial void OnUPD_IDChanged();
        #endregion

        public StoreProductExT()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NO", AutoSync = AutoSync.OnInsert, DbType = "BigInt NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public long NO
        {
            get
            {
                return this._NO;
            }
            set
            {
                if ((this._NO != value))
                {
                    this.OnNOChanging(value);
                    this.SendPropertyChanging();
                    this._NO = value;
                    this.SendPropertyChanged("NO");
                    this.OnNOChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_VAR_NO", DbType = "VarChar(10)")]
        public string VAR_NO
        {
            get
            {
                return this._VAR_NO;
            }
            set
            {
                if ((this._VAR_NO != value))
                {
                    this.OnVAR_NOChanging(value);
                    this.SendPropertyChanging();
                    this._VAR_NO = value;
                    this.SendPropertyChanged("VAR_NO");
                    this.OnVAR_NOChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NAME", DbType = "VarChar(100)")]
        public string NAME
        {
            get
            {
                return this._NAME;
            }
            set
            {
                if ((this._NAME != value))
                {
                    this.OnNAMEChanging(value);
                    this.SendPropertyChanging();
                    this._NAME = value;
                    this.SendPropertyChanged("NAME");
                    this.OnNAMEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FILE_NAME", DbType = "VarChar(100)")]
        public string FILE_NAME
        {
            get
            {
                return this._FILE_NAME;
            }
            set
            {
                if ((this._FILE_NAME != value))
                {
                    this.OnFILE_NAMEChanging(value);
                    this.SendPropertyChanging();
                    this._FILE_NAME = value;
                    this.SendPropertyChanged("FILE_NAME");
                    this.OnFILE_NAMEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FILE_RENAME", DbType = "VarChar(50)")]
        public string FILE_RENAME
        {
            get
            {
                return this._FILE_RENAME;
            }
            set
            {
                if ((this._FILE_RENAME != value))
                {
                    this.OnFILE_RENAMEChanging(value);
                    this.SendPropertyChanging();
                    this._FILE_RENAME = value;
                    this.SendPropertyChanged("FILE_RENAME");
                    this.OnFILE_RENAMEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FILE_EXT", DbType = "VarChar(10)")]
        public string FILE_EXT
        {
            get
            {
                return this._FILE_EXT;
            }
            set
            {
                if ((this._FILE_EXT != value))
                {
                    this.OnFILE_EXTChanging(value);
                    this.SendPropertyChanging();
                    this._FILE_EXT = value;
                    this.SendPropertyChanged("FILE_EXT");
                    this.OnFILE_EXTChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MIME_TYPE", DbType = "VarChar(50)")]
        public string MIME_TYPE
        {
            get
            {
                return this._MIME_TYPE;
            }
            set
            {
                if ((this._MIME_TYPE != value))
                {
                    this.OnMIME_TYPEChanging(value);
                    this.SendPropertyChanging();
                    this._MIME_TYPE = value;
                    this.SendPropertyChanged("MIME_TYPE");
                    this.OnMIME_TYPEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FILE_SIZE", DbType = "Float")]
        public System.Nullable<double> FILE_SIZE
        {
            get
            {
                return this._FILE_SIZE;
            }
            set
            {
                if ((this._FILE_SIZE != value))
                {
                    this.OnFILE_SIZEChanging(value);
                    this.SendPropertyChanging();
                    this._FILE_SIZE = value;
                    this.SendPropertyChanged("FILE_SIZE");
                    this.OnFILE_SIZEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MATERIAL_VOLUME", DbType = "Float")]
        public System.Nullable<double> MATERIAL_VOLUME
        {
            get
            {
                return this._MATERIAL_VOLUME;
            }
            set
            {
                if ((this._MATERIAL_VOLUME != value))
                {
                    this.OnMATERIAL_VOLUMEChanging(value);
                    this.SendPropertyChanging();
                    this._MATERIAL_VOLUME = value;
                    this.SendPropertyChanged("MATERIAL_VOLUME");
                    this.OnMATERIAL_VOLUMEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OBJECT_VOLUME", DbType = "Float")]
        public System.Nullable<double> OBJECT_VOLUME
        {
            get
            {
                return this._OBJECT_VOLUME;
            }
            set
            {
                if ((this._OBJECT_VOLUME != value))
                {
                    this.OnOBJECT_VOLUMEChanging(value);
                    this.SendPropertyChanging();
                    this._OBJECT_VOLUME = value;
                    this.SendPropertyChanged("OBJECT_VOLUME");
                    this.OnOBJECT_VOLUMEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SIZE_X", DbType = "Float")]
        public System.Nullable<double> SIZE_X
        {
            get
            {
                return this._SIZE_X;
            }
            set
            {
                if ((this._SIZE_X != value))
                {
                    this.OnSIZE_XChanging(value);
                    this.SendPropertyChanging();
                    this._SIZE_X = value;
                    this.SendPropertyChanged("SIZE_X");
                    this.OnSIZE_XChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SIZE_Y", DbType = "Float")]
        public System.Nullable<double> SIZE_Y
        {
            get
            {
                return this._SIZE_Y;
            }
            set
            {
                if ((this._SIZE_Y != value))
                {
                    this.OnSIZE_YChanging(value);
                    this.SendPropertyChanging();
                    this._SIZE_Y = value;
                    this.SendPropertyChanged("SIZE_Y");
                    this.OnSIZE_YChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SIZE_Z", DbType = "Float")]
        public System.Nullable<double> SIZE_Z
        {
            get
            {
                return this._SIZE_Z;
            }
            set
            {
                if ((this._SIZE_Z != value))
                {
                    this.OnSIZE_ZChanging(value);
                    this.SendPropertyChanging();
                    this._SIZE_Z = value;
                    this.SendPropertyChanged("SIZE_Z");
                    this.OnSIZE_ZChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SCALE", DbType = "Float")]
        public System.Nullable<double> SCALE
        {
            get
            {
                return this._SCALE;
            }
            set
            {
                if ((this._SCALE != value))
                {
                    this.OnSCALEChanging(value);
                    this.SendPropertyChanging();
                    this._SCALE = value;
                    this.SendPropertyChanged("SCALE");
                    this.OnSCALEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SHORT_LINK", DbType = "VarChar(50)")]
        public string SHORT_LINK
        {
            get
            {
                return this._SHORT_LINK;
            }
            set
            {
                if ((this._SHORT_LINK != value))
                {
                    this.OnSHORT_LINKChanging(value);
                    this.SendPropertyChanging();
                    this._SHORT_LINK = value;
                    this.SendPropertyChanged("SHORT_LINK");
                    this.OnSHORT_LINKChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_VIDEO_URL", DbType = "VarChar(100)")]
        public string VIDEO_URL
        {
            get
            {
                return this._VIDEO_URL;
            }
            set
            {
                if ((this._VIDEO_URL != value))
                {
                    this.OnVIDEO_URLChanging(value);
                    this.SendPropertyChanging();
                    this._VIDEO_URL = value;
                    this.SendPropertyChanged("VIDEO_URL");
                    this.OnVIDEO_URLChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CATEGORY_NO", DbType = "Int")]
        public System.Nullable<int> CATEGORY_NO
        {
            get
            {
                return this._CATEGORY_NO;
            }
            set
            {
                if ((this._CATEGORY_NO != value))
                {
                    this.OnCATEGORY_NOChanging(value);
                    this.SendPropertyChanging();
                    this._CATEGORY_NO = value;
                    this.SendPropertyChanged("CATEGORY_NO");
                    this.OnCATEGORY_NOChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CONTENTS", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string CONTENTS
        {
            get
            {
                return this._CONTENTS;
            }
            set
            {
                if ((this._CONTENTS != value))
                {
                    this.OnCONTENTSChanging(value);
                    this.SendPropertyChanging();
                    this._CONTENTS = value;
                    this.SendPropertyChanged("CONTENTS");
                    this.OnCONTENTSChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DESCRIPTION", DbType = "VarChar(MAX)")]
        public string DESCRIPTION
        {
            get
            {
                return this._DESCRIPTION;
            }
            set
            {
                if ((this._DESCRIPTION != value))
                {
                    this.OnDESCRIPTIONChanging(value);
                    this.SendPropertyChanging();
                    this._DESCRIPTION = value;
                    this.SendPropertyChanged("DESCRIPTION");
                    this.OnDESCRIPTIONChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PART_CNT", DbType = "Int")]
        public System.Nullable<int> PART_CNT
        {
            get
            {
                return this._PART_CNT;
            }
            set
            {
                if ((this._PART_CNT != value))
                {
                    this.OnPART_CNTChanging(value);
                    this.SendPropertyChanging();
                    this._PART_CNT = value;
                    this.SendPropertyChanged("PART_CNT");
                    this.OnPART_CNTChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CUSTERMIZE_YN", DbType = "Char(1)")]
        public System.Nullable<char> CUSTERMIZE_YN
        {
            get
            {
                return this._CUSTERMIZE_YN;
            }
            set
            {
                if ((this._CUSTERMIZE_YN != value))
                {
                    this.OnCUSTERMIZE_YNChanging(value);
                    this.SendPropertyChanging();
                    this._CUSTERMIZE_YN = value;
                    this.SendPropertyChanged("CUSTERMIZE_YN");
                    this.OnCUSTERMIZE_YNChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SELL_YN", DbType = "Char(1)")]
        public System.Nullable<char> SELL_YN
        {
            get
            {
                return this._SELL_YN;
            }
            set
            {
                if ((this._SELL_YN != value))
                {
                    this.OnSELL_YNChanging(value);
                    this.SendPropertyChanging();
                    this._SELL_YN = value;
                    this.SendPropertyChanged("SELL_YN");
                    this.OnSELL_YNChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TAG_NAME", DbType = "VarChar(250)")]
        public string TAG_NAME
        {
            get
            {
                return this._TAG_NAME;
            }
            set
            {
                if ((this._TAG_NAME != value))
                {
                    this.OnTAG_NAMEChanging(value);
                    this.SendPropertyChanging();
                    this._TAG_NAME = value;
                    this.SendPropertyChanged("TAG_NAME");
                    this.OnTAG_NAMEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CERTIFICATE_YN", DbType = "Char(1)")]
        public System.Nullable<char> CERTIFICATE_YN
        {
            get
            {
                return this._CERTIFICATE_YN;
            }
            set
            {
                if ((this._CERTIFICATE_YN != value))
                {
                    this.OnCERTIFICATE_YNChanging(value);
                    this.SendPropertyChanging();
                    this._CERTIFICATE_YN = value;
                    this.SendPropertyChanged("CERTIFICATE_YN");
                    this.OnCERTIFICATE_YNChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_VISIBILITY_YN", DbType = "Char(1)")]
        public System.Nullable<char> VISIBILITY_YN
        {
            get
            {
                return this._VISIBILITY_YN;
            }
            set
            {
                if ((this._VISIBILITY_YN != value))
                {
                    this.OnVISIBILITY_YNChanging(value);
                    this.SendPropertyChanging();
                    this._VISIBILITY_YN = value;
                    this.SendPropertyChanged("VISIBILITY_YN");
                    this.OnVISIBILITY_YNChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_USE_YN", DbType = "Char(1)")]
        public System.Nullable<char> USE_YN
        {
            get
            {
                return this._USE_YN;
            }
            set
            {
                if ((this._USE_YN != value))
                {
                    this.OnUSE_YNChanging(value);
                    this.SendPropertyChanging();
                    this._USE_YN = value;
                    this.SendPropertyChanged("USE_YN");
                    this.OnUSE_YNChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MEMBER_NO", DbType = "Int")]
        public System.Nullable<int> MEMBER_NO
        {
            get
            {
                return this._MEMBER_NO;
            }
            set
            {
                if ((this._MEMBER_NO != value))
                {
                    this.OnMEMBER_NOChanging(value);
                    this.SendPropertyChanging();
                    this._MEMBER_NO = value;
                    this.SendPropertyChanged("MEMBER_NO");
                    this.OnMEMBER_NOChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TXT_SIZE_X", DbType = "Float")]
        public System.Nullable<double> TXT_SIZE_X
        {
            get
            {
                return this._TXT_SIZE_X;
            }
            set
            {
                if ((this._TXT_SIZE_X != value))
                {
                    this.OnTXT_SIZE_XChanging(value);
                    this.SendPropertyChanging();
                    this._TXT_SIZE_X = value;
                    this.SendPropertyChanged("TXT_SIZE_X");
                    this.OnTXT_SIZE_XChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TXT_SIZE_Y", DbType = "Float")]
        public System.Nullable<double> TXT_SIZE_Y
        {
            get
            {
                return this._TXT_SIZE_Y;
            }
            set
            {
                if ((this._TXT_SIZE_Y != value))
                {
                    this.OnTXT_SIZE_YChanging(value);
                    this.SendPropertyChanging();
                    this._TXT_SIZE_Y = value;
                    this.SendPropertyChanged("TXT_SIZE_Y");
                    this.OnTXT_SIZE_YChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DETAIL_TYPE", DbType = "Int")]
        public System.Nullable<int> DETAIL_TYPE
        {
            get
            {
                return this._DETAIL_TYPE;
            }
            set
            {
                if ((this._DETAIL_TYPE != value))
                {
                    this.OnDETAIL_TYPEChanging(value);
                    this.SendPropertyChanging();
                    this._DETAIL_TYPE = value;
                    this.SendPropertyChanged("DETAIL_TYPE");
                    this.OnDETAIL_TYPEChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DETAIL_DEPTH", DbType = "Int")]
        public System.Nullable<int> DETAIL_DEPTH
        {
            get
            {
                return this._DETAIL_DEPTH;
            }
            set
            {
                if ((this._DETAIL_DEPTH != value))
                {
                    this.OnDETAIL_DEPTHChanging(value);
                    this.SendPropertyChanging();
                    this._DETAIL_DEPTH = value;
                    this.SendPropertyChanged("DETAIL_DEPTH");
                    this.OnDETAIL_DEPTHChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TXT_LOC", DbType = "VarChar(20)")]
        public string TXT_LOC
        {
            get
            {
                return this._TXT_LOC;
            }
            set
            {
                if ((this._TXT_LOC != value))
                {
                    this.OnTXT_LOCChanging(value);
                    this.SendPropertyChanging();
                    this._TXT_LOC = value;
                    this.SendPropertyChanged("TXT_LOC");
                    this.OnTXT_LOCChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_REG_DT", DbType = "DateTime")]
        public System.Nullable<System.DateTime> REG_DT
        {
            get
            {
                return this._REG_DT;
            }
            set
            {
                if ((this._REG_DT != value))
                {
                    this.OnREG_DTChanging(value);
                    this.SendPropertyChanging();
                    this._REG_DT = value;
                    this.SendPropertyChanged("REG_DT");
                    this.OnREG_DTChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_REG_ID", DbType = "VarChar(50)")]
        public string REG_ID
        {
            get
            {
                return this._REG_ID;
            }
            set
            {
                if ((this._REG_ID != value))
                {
                    this.OnREG_IDChanging(value);
                    this.SendPropertyChanging();
                    this._REG_ID = value;
                    this.SendPropertyChanged("REG_ID");
                    this.OnREG_IDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UPD_DT", DbType = "DateTime")]
        public System.Nullable<System.DateTime> UPD_DT
        {
            get
            {
                return this._UPD_DT;
            }
            set
            {
                if ((this._UPD_DT != value))
                {
                    this.OnUPD_DTChanging(value);
                    this.SendPropertyChanging();
                    this._UPD_DT = value;
                    this.SendPropertyChanged("UPD_DT");
                    this.OnUPD_DTChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UPD_ID", DbType = "VarChar(50)")]
        public string UPD_ID
        {
            get
            {
                return this._UPD_ID;
            }
            set
            {
                if ((this._UPD_ID != value))
                {
                    this.OnUPD_IDChanging(value);
                    this.SendPropertyChanging();
                    this._UPD_ID = value;
                    this.SendPropertyChanged("UPD_ID");
                    this.OnUPD_IDChanged();
                }
            }
        }

        public System.Nullable<int> TOTAL_PRICE
        {
            get
            {
                return this._TOTAL_PRICE;
            }
            set
            {
                if ((this._TOTAL_PRICE != value))
                {
                    this._TOTAL_PRICE = value;
                    this.SendPropertyChanged("TOTAL_PRICE");
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
