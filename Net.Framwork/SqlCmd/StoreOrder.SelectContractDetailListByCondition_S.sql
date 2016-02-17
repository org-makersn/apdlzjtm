SELECT
	SO.NO,
	SPH.T_ID,
	SO.OID,
	SO.BUYER_NAME,
	SO.BUYER_TEL,
	SO.BUYER_EMAIL,
	SI.ITEM_NAME,
	SOD.ITEM_PRICE,
	SOD.ITEM_CNT,
	SOD.ITEM_PRICE * SOD.ITEM_CNT AS PAY_PRICE,
	SOD.ORDER_STATUS,
	SO.ORDER_DATE,
	SO.ORDER_CANCEL_YN,	
	SO.SHIPPING_ADDR_NO,
	SSA.ADDR1,
	SSA.ADDR2,
	SSA.ADDR_DETAIL,
	SSA.POST_NO
FROM STORE_ORDER AS SO WITH(NOLOCK) 
INNER JOIN STORE_ORDER_DETAIL AS SOD WITH(NOLOCK) ON SO.NO=SOD.ORDER_MASTER_NO
INNER JOIN STORE_PAYMENT_HISTORY AS SPH WITH(NOLOCK) ON SPH.ORDER_MASTER_NO=SO.NO
LEFT JOIN STORE_ITEM AS SI WITH(NOLOCK) ON SI.NO=SOD.ITEM_NO
LEFT JOIN STORE_SHIPPING_ADDR AS SSA WITH(NOLOCK) ON SSA.NO=SO.SHIPPING_ADDR_NO
WHERE SO.ORDER_DATE BETWEEN @ST_DT AND @ED_DT
AND SO.MEMBER_NO = @MEMBER_NO
ORDER BY SO.REG_DT DESC
