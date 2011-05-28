IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'proc_LittleGeek_CreateContent')
	BEGIN
		DROP  Procedure  proc_LittleGeek_CreateContent
	END

GO

CREATE Procedure proc_LittleGeek_CreateContent
(
	@ContentType int,
	@Status int
)


AS

	INSERT INTO Content (
		ContentType, 
		Status
	) VALUES (
		@ContentType, 
		@Status
	)
	
	SELECT TOP 1 SCOPE_IDENTITY() AS ContentID
	  FROM Content
	
GO

