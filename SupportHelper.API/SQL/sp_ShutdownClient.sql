-- =============================================
-- Author:		<Guian>
-- Create date: <24/10/2025>
-- Description:	<Busca hostname que chega pelo parametro e atualiza informações.>
-- =============================================
CREATE PROCEDURE [dbo].[sp_shutdown_client]
	@hostname CHAR(15),
	@lastupdate NVARCHAR(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF NOT EXISTS ( SELECT 1 FROM TB_MACHINE WHERE COL_HOSTNAME = @hostname )
	BEGIN
		;THROW 50002, 'Hostname não encontrado', 1;
	END

	UPDATE TB_MACHINE
	SET COL_ISCONNECTED = 0,
		COL_SGPISRUNNING = 0,
		COL_UPTIME = '00:00:0000',
		COL_LASTUPDATE = @lastupdate,
		COL_SIGNALR_CONNECTIONID = 'OFF',
		COL_SIGNALR_ISACTIVE = 0,
		COL_SIGNALR_LASTUPDATE = @lastupdate
	WHERE COL_HOSTNAME = @hostname
END
GO