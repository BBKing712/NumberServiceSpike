CREATE TRIGGER dbo.TRG_Datentypen_AfterUpdate 
   ON  dbo.Datentypen 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets
	SET NOCOUNT ON;
UPDATE dbo.Datentypen
  SET AktualisiertAm = (getutcdate())
  WHERE ID IN (SELECT DISTINCT ID FROM Inserted)

END
GO