IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'proc_LittleGeek_CreateMedia')
	BEGIN
		DROP PROCEDURE proc_LittleGeek_CreateMedia
	END

GO

CREATE Procedure proc_LittleGeek_CreateMedia (	
	@ContentID int,
	@MediaType int,
	@MediaReference varchar(128)
)
	
AS

	DECLARE @TheCount int
	
	 SELECT @TheCount = COUNT(1) 
	   FROM Media 
	  WHERE MediaReference = @MediaReference
	
	IF @TheCount = 0
	
		BEGIN
		
			INSERT INTO Media (
				ContentID,
				MediaType, 
				MediaReference
			) VALUES (
				@ContentID,
				@MediaType, 
				@MediaReference
			)
			
			SELECT TOP 1 SCOPE_IDENTITY() AS MediaID 
			  FROM Media
		
		END
		
	ELSE
	
		BEGIN
		
			SELECT MediaID
			  FROM Media
			 WHERE MediaReference = @MediaReference
		
		END

GO

GRANT EXEC ON proc_LittleGeek_CreateMedia TO PUBLIC

GO
