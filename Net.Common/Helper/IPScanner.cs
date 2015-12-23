using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Net.Common.Helper
{
    public class IPScanner
    {
        #region private entity
        private string dataPath;
        private string ip;
        private string country;
        private string local;
        private int _state = 0;
        private long firstStartIp = 0;
        private long lastStartIp = 0;
        private FileStream objfs = null;
        private long startIp = 0;
        private long endIp = 0;
        private int countryFlag = 0;
        private long endIpOff = 0;
        private string errMsg = null;
        #endregion

        #region public
        public string DataPath
        {
            set { dataPath = value; }
        }
        public string IP
        {
            set { ip = value; }
        }
        public string Country
        {
            get { return country; }
        }
        public string Local
        {
            get { return local; }
        }
        public string ErrMsg
        {
            get { return errMsg; }
        }
        public int State
        {
            get { return _state; }
        }
        #endregion

        #region Constructor
        public IPScanner()
        {
            //
            // TODO:
            //
        }
        #endregion

        #region IP to Int
        private long IpToInt(string ip)
        {
            char[] dot = new char[] { '.' };
            string[] ipArr = ip.Split(dot);
            if (ipArr.Length == 3)
                ip = ip + ".0";
            ipArr = ip.Split(dot);

            long ip_Int = 0;
            long p1 = long.Parse(ipArr[0]) * 256 * 256 * 256;
            long p2 = long.Parse(ipArr[1]) * 256 * 256;
            long p3 = long.Parse(ipArr[2]) * 256;
            long p4 = long.Parse(ipArr[3]);
            ip_Int = p1 + p2 + p3 + p4;
            return ip_Int;
        }
        #endregion

        #region int to IP
        private string IntToIP(long ip_Int)
        {
            long seg1 = (ip_Int & 0xff000000) >> 24;
            if (seg1 < 0)
                seg1 += 0x100;
            long seg2 = (ip_Int & 0x00ff0000) >> 16;
            if (seg2 < 0)
                seg2 += 0x100;
            long seg3 = (ip_Int & 0x0000ff00) >> 8;
            if (seg3 < 0)
                seg3 += 0x100;
            long seg4 = (ip_Int & 0x000000ff);
            if (seg4 < 0)
                seg4 += 0x100;
            string ip = seg1.ToString() + "." + seg2.ToString() + "." + seg3.ToString() + "." + seg4.ToString();

            return ip;
        }
        #endregion

        #region Get Start IP range
        private long GetStartIp(long recNO)
        {
            long offSet = firstStartIp + recNO * 7;
            //objfs.Seek(offSet,SeekOrigin.Begin);
            objfs.Position = offSet;
            byte[] buff = new Byte[7];
            objfs.Read(buff, 0, 7);

            endIpOff = Convert.ToInt64(buff[4].ToString()) + Convert.ToInt64(buff[5].ToString()) * 256 + Convert.ToInt64(buff[6].ToString()) * 256 * 256;
            startIp = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256 + Convert.ToInt64(buff[3].ToString()) * 256 * 256 * 256;
            return startIp;
        }
        #endregion

        #region Get End IP
        private long GetEndIp()
        {
            //objfs.Seek(endIpOff,SeekOrigin.Begin);
            objfs.Position = endIpOff;
            byte[] buff = new Byte[5];
            objfs.Read(buff, 0, 5);
            this.endIp = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256 + Convert.ToInt64(buff[3].ToString()) * 256 * 256 * 256;
            this.countryFlag = buff[4];
            return this.endIp;
        }
        #endregion

        #region Access to national / regional Offset
        private string GetCountry()
        {
            switch (this.countryFlag)
            {
                case 1:
                case 2:
                    this.country = GetFlagStr(this.endIpOff + 4);
                    this.local = (1 == this.countryFlag) ? " " : this.GetFlagStr(this.endIpOff + 8);
                    break;
                default:
                    this.country = this.GetFlagStr(this.endIpOff + 4);
                    this.local = this.GetFlagStr(objfs.Position);
                    break;
            }
            return " ";
        }
        #endregion

        #region Access to national / regional string
        private string GetFlagStr(long offSet)
        {
            int flag = 0;
            byte[] buff = new Byte[3];
            while (1 == 1)
            {
                //objfs.Seek(offSet,SeekOrigin.Begin);
                objfs.Position = offSet;
                flag = objfs.ReadByte();
                if (flag == 1 || flag == 2)
                {
                    objfs.Read(buff, 0, 3);
                    if (flag == 2)
                    {
                        this.countryFlag = 2;
                        this.endIpOff = offSet - 4;
                    }
                    offSet = Convert.ToInt64(buff[0].ToString()) + Convert.ToInt64(buff[1].ToString()) * 256 + Convert.ToInt64(buff[2].ToString()) * 256 * 256;
                }
                else
                {
                    break;
                }
            }
            if (offSet < 12)
                return " ";
            objfs.Position = offSet;
            return GetStr();
        }
        #endregion

        #region GetStr
        private string GetStr()
        {
            byte lowC = 0;
            byte upC = 0;
            string str = "";
            byte[] buff = new byte[2];
            while (1 == 1)
            {
                lowC = (Byte)objfs.ReadByte();
                if (lowC == 0)
                    break;
                if (lowC > 127)
                {
                    upC = (byte)objfs.ReadByte();
                    buff[0] = lowC;
                    buff[1] = upC;
                    System.Text.Encoding enc = System.Text.Encoding.GetEncoding("GB2312");
                    str += enc.GetString(buff);
                }
                else
                {
                    str += (char)lowC;
                }
            }
            return str;
        }
        #endregion

        #region Search matching data
        /// <summary>
        /// Search matching data
        /// </summary>
        /// <returns></returns>
        private void Searchwry()
        {
            string pattern = @"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))";
            Regex objRe = new Regex(pattern);
            Match objMa = objRe.Match(ip);
            if (!objMa.Success)
            {
                _state = 1;//IP type error
                return;
            }

            long ip_Int = this.IpToInt(ip);
            if (ip_Int >= IpToInt("127.0.0.0") && ip_Int <= IpToInt("127.255.255.255"))
            {
                this.country = "Local IP Address";
                this.local = "";
            }
            else if ((ip_Int >= IpToInt("0.0.0.0") && ip_Int <= IpToInt("2.255.255.255")) || (ip_Int >= IpToInt("64.0.0.0") && ip_Int <= IpToInt("126.255.255.255")) || (ip_Int >= IpToInt("58.0.0.0") && ip_Int <= IpToInt("60.255.255.255")))
            {
                this.country = "Internet IP Address";
                this.local = "";
            }
            objfs = new FileStream(this.dataPath, FileMode.Open, FileAccess.Read);
            try
            {
                objfs.Position = 0;
                byte[] buff = new Byte[8];
                objfs.Read(buff, 0, 8);
                firstStartIp = buff[0] + buff[1] * 256 + buff[2] * 256 * 256 + buff[3] * 256 * 256 * 256;
                lastStartIp = buff[4] * 1 + buff[5] * 256 + buff[6] * 256 * 256 + buff[7] * 256 * 256 * 256;
                long recordCount = Convert.ToInt64((lastStartIp - firstStartIp) / 7.0);
                if (recordCount <= 1)
                {
                    country = "FileDataError";
                    _state = 2;//FileDataError
                    objfs.Close();
                    return;
                }
                long rangE = recordCount;
                long rangB = 0;
                long recNO = 0;
                while (rangB < rangE - 1)
                {
                    recNO = (rangE + rangB) / 2;
                    this.GetStartIp(recNO);
                    if (ip_Int == this.startIp)
                    {
                        rangB = recNO;
                        break;
                    }
                    if (ip_Int > this.startIp)
                        rangB = recNO;
                    else
                        rangE = recNO;
                }
                this.GetStartIp(rangB);
                this.GetEndIp();
                if (this.startIp <= ip_Int && this.endIp >= ip_Int)
                {
                    this.GetCountry();
                    this.local = this.local.Replace("（is TaiWan[대만]）", "");
                }
                else
                {
                    this.country = "not found";
                    this.local = "";
                }
                objfs.Close();
                _state = 0;
            }
            catch
            {
                _state = 3;//not found error
            }
        }
        #endregion

        #region Get IP Address
        public string IPLocation()
        {
            this.Searchwry();
            return this.country + this.local;
        }

        public string IPLocation(string dataPath, string ip)
        {
            this.dataPath = dataPath;
            this.ip = ip;
            this.Searchwry();
            return this.country + this.local;
        }
        #endregion
    }
}