select 
		A.[NO]
		, A.TEMP
		, A.ITEM_NAME as ItemName
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
		, B.RENAME as MainImgName
		, C.STORE_NAME as StoreName
		, D.PROFILE_PIC as ProfilePic
		, (SELECT COUNT(0) FROM STORE_REVIEW WHERE STORE_ITEM_NO = A.[NO]) AS CommentCnt

	from STORE_ITEM A with(nolock)
	inner join STORE_ITEM_FILE B with(nolock) on A.MAIN_IMG = B.[NO]
	left join STORE_MEMBER C with(nolock) on A.STORE_MEMBER_NO = C.[NO]
    left join MEMBER D with(nolock) on C.MEMBER_NO = D.[NO] 
	where A.[NO] = @STORE_ITEM_NO
		--, (SELECT COUNT(0) FROM STORE_LIKES WHERE STORE_ITEM_NO = A.[NO] AND MEMBER_NO = @VISITOR_NO) AS IsLikes