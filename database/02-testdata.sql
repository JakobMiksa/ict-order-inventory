USE ICTOrderDb;
GO

INSERT INTO dbo.Product (Name, Price, StockQuantity) VALUES
('Ost', 45.50, 10),
('Mælk', 12.00, 25),
('Rugbrød', 30.00, 0),
('Gulerødder', 15.00, 1),
('Æg', 28.00, 12);
GO

INSERT INTO dbo.OrderRound (Start, Deadline) VALUES
('2026-11-05 18:00', '2026-11-19 18:00'),
('2026-11-19 18:00', '2026-12-03 18:00');
GO
