select 
		A.[NO]
		, A.TEMP
		, A.TITLE
		, A.MEMBER_NO as MemberNo
		, A.CODE_NO as CodeNo
		, A.MAIN_IMG as MainImg
		, A.CONTENTS
		, A.BASE_PRICE as BasePrice
		, A.DELIVERY_TYPE as DeliveryType
		, A.TAGS
		, A.VIEW_CNT as ViewCnt
		, A.VIDEO_SOURCE as VideoSource
		, A.FEATURED_YN as FeaturedYn
		, A.FEATURED_VISIBILITY as FeaturedYisibility
		, A.USE_YN as UseYn
		, A.REG_DT as RegDt
		, B.NAME as MemberName
		, B.PROFILE_PIC as ProfilePic
		, C.RENAME as MainImgName
		, (SELECT COUNT(0) FROM STORE_LIKES WHERE STORE_ITEM_NO = A.[NO] AND MEMBER_NO = @VISITOR_NO) AS IsLikes
		, (SELECT COUNT(0) FROM STORE_REVIEW WHERE STORE_ITEM_NO = A.[NO]) AS CommentCnt

	from STORE_ITEM A with(nolock)
	inner join MEMBER B with(nolock) on A.MEMBER_NO = B.[NO]
	inner join STORE_ITEM_FILE C with(nolock) on A.MAIN_IMG = C.[NO]
	where A.[NO] = @STORE_ITEM_NO