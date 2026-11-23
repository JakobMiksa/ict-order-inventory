USE ICTOrderDb;
GO

INSERT INTO dbo.Product (Name, Price, StockQuantity, Hidden) VALUES
('Ost', 45.50, 10, 0),
('Mælk', 12.00, 25, 0),
('Rugbrød', 30.00, 0, 0),
('Gulerødder', 15.00, 1, 0),
('Æg', 28.00, 12, 1);
GO

INSERT INTO dbo.OrderRound (Start, Deadline) VALUES
('2026-11-05 18:00', '2026-11-19 18:00'),
('2026-11-19 18:00', '2026-12-03 18:00');
GO
