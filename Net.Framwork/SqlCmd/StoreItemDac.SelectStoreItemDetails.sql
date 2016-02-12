select 
		A.[NO]
		, A.TEMP
		, A.TITLE
		, A.STORE_MEMBER_NO as StoreMemberNo
		, A.CODE_NO as CodeNo
		, A.MAIN_IMG as MainImg
		, A.CONTENTS
		, A.BASE_PRICE as BasePrice
		, A.SHIPPING_TYPE as ShippingType
		, A.SHIPPING_COST as ShippingCost
		, A.TAGS
		, A.VIEW_CNT as ViewCnt
		, A.VIDEO_SOURCE as VideoSource
		, A.FEATURED_YN as FeaturedYn
		, A.FEATURED_VISIBILITY as FeaturedYisibility
		, A.USE_YN as UseYn
		, A.REG_DT as RegDt
		, B.STORE_NAME as StoreName
		, C.RENAME as MainImgName
		, D.PROFILE_PIC as ProfilePic
		, (SELECT COUNT(0) FROM STORE_LIKES WHERE STORE_ITEM_NO = A.[NO] AND MEMBER_NO = @VISITOR_NO) AS IsLikes
		, (SELECT COUNT(0) FROM STORE_REVIEW WHERE STORE_ITEM_NO = A.[NO]) AS CommentCnt

	from STORE_ITEM A with(nolock)
	inner join STORE_MEMBER B with(nolock) on A.STORE_MEMBER_NO = B.[NO]
	inner join STORE_ITEM_FILE C with(nolock) on A.MAIN_IMG = C.[NO]
    inner join MEMBER D with(nolock) on B.MEMBER_NO = D.[NO] 
	where A.[NO] = @STORE_ITEM_NO