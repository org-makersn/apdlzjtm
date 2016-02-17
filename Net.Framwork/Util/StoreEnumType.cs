using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Net.Framework.Util
{

    public enum OrderStatus
    {
        /// <summary>
        /// 주문완료
        /// </summary>
        [StringValue("00"), Description("주문완료")]
        Complete,

        /// <summary>
        /// 주문취소
        /// </summary>
        [StringValue("01"), Description("주문취소")]
        Cancel
    }

  public enum PaymentStatus
  {
      /// <summary>
      /// 결제완료
      /// </summary>
      [StringValue("00"), Description("결제완료")]
      Complete,

      /// <summary>
      /// 결제취소
      /// </summary>
      [StringValue("01"), Description("결제취소")]
      Cancel,

      /// <summary>
      /// 결제대기
      /// </summary>
      [StringValue("02"), Description("결제대기")]
      Waiting,

      /// <summary>
      /// 결제진행중
      /// </summary>
      [StringValue("03"), Description("결제진행중")]
      InProgress
  }

  public enum PrintingStatus
  {
      /// <summary>
      /// 출력완료
      /// </summary>
      [StringValue("00"), Description("출력완료")]
      Complete,

      /// <summary>
      /// 출력취소
      /// </summary>
      [StringValue("01"), Description("출력취소")]
      Cancel,

      /// <summary>
      /// 출력대기
      /// </summary>
      [StringValue("02"), Description("출력대기")]
      Waiting,

      /// <summary>
      /// 출력진행중
      /// </summary>
      [StringValue("03"), Description("출력진행중")]
      InProgress,
  }

  public enum ShippingStatus
  {
      /// <summary>
      /// 배송완료
      /// </summary>
      [StringValue("00"), Description("배송완료")]
      Complete,

      /// <summary>
      /// 배송취소
      /// </summary>
      [StringValue("01"), Description("배송취소")]
      Cancel,

      /// <summary>
      /// 배송대기
      /// </summary>
      [StringValue("02"), Description("배송대기")]
      Waiting,

      /// <summary>
      /// 배송중
      /// </summary>
      [StringValue("03"), Description("배송중")]
      InProgress,
  }

    
  public enum StoreShippingType
  {
      [EnumTitle("무료배송")]
      무료배송 = 1,
      [EnumTitle("상품별 배송비")]
      상품별배송비 = 2,
      [EnumTitle("수신자 부담")]
      수신자부담 = 3
  }


}