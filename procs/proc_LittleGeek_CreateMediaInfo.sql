 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'proc_LittleGeek_CreateMediaInfo')
	BEGIN
		DROP PROCEDURE proc_LittleGeek_CreateMediaInfo
	END

GO

CREATE Procedure proc_LittleGeek_CreateMediaInfo (
	@MediaID int,
	@InfoType int,
	@Info varchar(4000)
)
	
AS

	DECLARE @TheCount int
	
	 SELECT @TheCount = COUNT(1) 
	   FROM MediaInfo 
	  WHERE MediaID = @MediaID
	    AND InfoType = @InfoType
	
	IF @TheCount = 0 AND @Info <> ''
	
		BEGIN
		
			INSERT INTO MediaInfo (
				MediaID, 
				InfoType,
				Info
			) VALUES (
				@MediaID, 
				@InfoType,
				@Info
			)
		
		END
		
	ELSE
	
		BEGIN
		
			UPDATE MediaInfo
			   SET Info = @Info
			 WHERE MediaID = @MediaID
			   AND InfoType = @InfoType
		
		END

GO