
namespace Net.Common.Define
{
    public static class Constant
    {
        /// <summary>
        /// 
        /// </summary>
        public static class AjaxReturnMsg
        {
            public const string Success_Msg = "성공하였습니다.";
            public const string Error_Msg = "실패하였습니다.";
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Product
        {
            public const string STL_FileUpload_Validate_Ext_Msg = "stl, obj 형식 파일만 가능합니다.";
            public const string STL_FileUpload_Max_Size__Msg = "최대 사이즈 100MB 파일만 가능합니다.";
        }

        /// <summary>
        /// 다른데로 옮길시 편하게하기 위해 임시
        /// </summary>
        public static class StoreUploadDir
        {
            public const string ModelingDir = @"product\3d-files";
            public const string JsonToJsDir = @"product\js-files";
        }

        /// <summary>
        /// 
        /// </summary>
        public static class DesignUploadDir
        {
            public const string ModelingDir = @"design\3d-files";
            public const string JsonToJsDir = @"design\js-files";
        }

        /// <summary>
        /// 
        /// </summary>
        public static class CommonUploadDir
        {
            public const string ProfileDir = @"common\profiles\thumb";
        }
    }
}
