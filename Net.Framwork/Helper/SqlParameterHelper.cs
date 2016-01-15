using System;
using System.Data;
using System.Data.SqlClient;

namespace Net.Framwork.Helper
{
    public class SqlParameterHelper
    {
        #region [Construct]
        private SqlParameterHelper()
        {
        }
        #endregion

        #region Generic CreateParameter
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType dbType)
        {
            SqlParameter ret = new SqlParameter(parameterName, dbType);
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType dbType, int size)
        {
            SqlParameter ret = CreateParameter(parameterName, dbType);
            ret.Size = size;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="sourceColumn">소스 열의 이름</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType dbType, int size, string sourceColumn)
        {
            SqlParameter ret = CreateParameter(parameterName, dbType);
            ret.Size = size;
            ret.SourceColumn = sourceColumn;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="direction"><c>ParameterDirection</c>값 중 하나</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter ret = CreateParameter(parameterName, dbType);
            ret.Direction = direction;
            ret.Size = size;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="direction"><c>ParameterDirection</c>값 중 하나</param>
        /// <param name="sourceColumn">소스 열의 이름</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, string sourceColumn)
        {
            SqlParameter ret = CreateParameter(parameterName, dbType);
            ret.Direction = direction;
            ret.Size = size;
            ret.SourceColumn = sourceColumn;
            return ret;
        }


        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, object value)
        {

            return new SqlParameter(parameterName, ConvertToDBValue(value));
        }

        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType)
        {
            SqlParameter ret = CreateParameter(parameterName, value);
            ret.SqlDbType = dbType;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType, int size)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType);
            ret.Size = size;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="sourceColumn">소스 열의 이름</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType, int size, string sourceColumn)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType);
            ret.Size = size;
            ret.SourceColumn = sourceColumn;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="direction"><c>ParameterDirection</c>값 중 하나</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType, size);
            ret.Direction = direction;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="direction"><c>ParameterDirection</c>값 중 하나</param>
        /// <param name="sourceColumn">소스 열의 이름</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType, int size, ParameterDirection direction, string sourceColumn)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType, size, direction);
            ret.SourceColumn = sourceColumn;
            return ret;
        }


        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, bool value)
        {
            return CreateParameter(parameterName, value, true);
        }

        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, bool value, bool convertToYN)
        {
            return convertToYN ? new SqlParameter(parameterName, ConvertToYNChar(value)) : new SqlParameter(parameterName, value);
        }

        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, bool value, SqlDbType dbType)
        {
            bool convertToYN =
                (dbType == SqlDbType.Char || dbType == SqlDbType.NChar || dbType == SqlDbType.NText ||
                dbType == SqlDbType.NVarChar || dbType == SqlDbType.Text || dbType == SqlDbType.VarChar);
            SqlParameter ret = CreateParameter(parameterName, value, convertToYN);
            ret.SqlDbType = dbType;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, bool value, SqlDbType dbType, int size)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType);
            ret.Size = size;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="sourceColumn">소스 열의 이름</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, bool value, SqlDbType dbType, int size, string sourceColumn)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType);
            ret.Size = size;
            ret.SourceColumn = sourceColumn;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="direction"><c>ParameterDirection</c>값 중 하나</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, bool value, SqlDbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType, size);
            ret.Direction = direction;
            return ret;
        }
        /// <summary>
        /// 매개 변수를 사용하여 <c>SqlParameter</c>클래스의 새 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="parameterName">매핑할 매개 변수의 이름</param>
        /// <param name="value"><c>SqlParameter</c>의 값이 되는 <c>Object</c></param>
        /// <param name="dbType"><c>SqlDbType</c>값 중 하나</param>
        /// <param name="size">매개 변수의 길이</param>
        /// <param name="direction"><c>ParameterDirection</c>값 중 하나</param>
        /// <param name="sourceColumn">소스 열의 이름</param>
        /// <returns><c>SqlParameter</c>클래스의 새 인스턴스</returns>
        public static SqlParameter CreateParameter(string parameterName, bool value, SqlDbType dbType, int size, ParameterDirection direction, string sourceColumn)
        {
            SqlParameter ret = CreateParameter(parameterName, value, dbType, size, direction);
            ret.SourceColumn = sourceColumn;
            return ret;
        }
        #endregion

        #region Util Method

        private static object ConvertToDBValue(object value)
        {
            return (value == null) ? DBNull.Value : value;
        }

        /// <summary>
        /// 파라메터값에 따라 "Y", "N" 둘 중 하나의 값을 갖는 문자열을 반환합니다.
        /// </summary>
        /// <param name="value">부울 값</param>
        /// <returns>파라메터값이 true면 "Y", false면 "N"</returns>
        private static string ConvertToYNChar(bool value)
        {
            return value ? "Y" : "N";
        }
        #endregion
    }
}