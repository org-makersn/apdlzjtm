DECLARE @CNT INT
SELECT @CNT = COUNT(0) FROM STORE_CART WITH(NOLOCK)

IF @CNT > 0
BEGIN
	SELECT 
		MAX(CART_NO) + 1 AS MAX_CART_NO
	FROM STORE_CART WITH(NOLOCK)
END
ELSE
BEGIN
	SELECT 1000000000
END
