select 
	A.[NO]
	, A.MEMBER_NO as MemberNo
	, A.STORE_NAME as StoreName
	, A.OFFICE_PHONE as OfficePhone
	, A.CELL_PHONE as CellPhone
	, A.STORE_PROFILE_MSG as StoreProfileMsg
	, A.STORE_URL as StoreUrl
	, A.BANK_NAME as BankName
	, A.BANK_USER_NAME as BankUserName
	, A.BANK_ACCOUNT as BankAccount
	, A.DEL_YN as DelYn
	, A.REG_DT as RegDt
	, A.REG_ID as RegId
	, B.NAME as MemberName 
	, B.PROFILE_PIC as ProfilePic
	, B.COVER_PIC as CoverPic

from STORE_MEMBER A with(nolock)
inner join MEMBER B with(nolock) on A.MEMBER_NO = B.[NO]
where A.[NO] = @STORE_MEMBER_NO