USE ChessGame;
GO

INSERT INTO Users(Username, Password, Rights) VALUES ('admin','admin',0);
INSERT INTO Users(Username, Password, Rights) VALUES ('stan','1234',1);
INSERT INTO Users(Username, Password, Rights) VALUES ('kyle','1234',2);
INSERT INTO Users(Username, Password, Rights) VALUES ('cartman','1234',3);
INSERT INTO Users(Username, Password, Rights) VALUES ('kenny','1234',1);

INSERT INTO Messages VALUES('2018-11-18 18:44:52','kenny','cartman','mm mmh mmh mmhmh mmhmh');
INSERT INTO Messages VALUES('2018-11-18 18:45:35','cartman','kenny','what do you want Kenny?');
INSERT INTO Messages VALUES('2018-11-18 18:46:15','kenny','cartman','mm mmh mmhmh mmh mmm mmhmh');
INSERT INTO Messages VALUES('2018-11-18 18:47:01','cartman','kenny','shut up Kenny!');

INSERT INTO Games VALUES(1,'kenny','stan','kenny',null,'00.Rook.Black-10.Knight.Black-20.Bishop.Black-30.Queen.Black-40.King.Black-50.Bishop.Black-60..-70.Rook.Black-01.Pawn.Black-11.Pawn.Black-21.Pawn.Black-31.Pawn.Black-41..-51.Pawn.Black-61.Pawn.Black-71.Pawn.Black-02..-12..-22..-32..-42..-52..-62..-72..-03..-13..-23..-33..-43.Pawn.Black-53..-63..-73..-04..-14..-24..-34.Pawn.White-44.Knight.Black-54..-64..-74..-05..-15..-25.Pawn.White-35..-45.Pawn.White-55..-65..-75..-06.Pawn.White-16.Pawn.White-26..-36..-46..-56.Pawn.White-66.Pawn.White-76.Pawn.White-07.Rook.White-17.Knight.White-27.Bishop.White-37.Queen.White-47.King.White-57.Bishop.White-67.Knight.White-77.Rook.White');
INSERT INTO Games VALUES(1,'stan','kyle','kyle',null,'00..-10.Rook.Black-20.Bishop.Black-30.Queen.Black-40.King.Black-50.Bishop.Black-60.Knight.Black-70.Rook.Black-01.Pawn.Black-11..-21.Pawn.Black-31.Pawn.Black-41..-51.Queen.White-61.Pawn.Black-71.Pawn.Black-02.Knight.Black-12.Pawn.Black-22..-32..-42..-52..-62..-72..-03..-13..-23..-33..-43.Pawn.Black-53..-63..-73..-04..-14..-24.Bishop.White-34.Pawn.White-44.Pawn.White-54..-64..-74..-05..-15..-25..-35..-45..-55.Knight.White-65..-75..-06.Pawn.White-16.Pawn.White-26.Pawn.White-36..-46..-56.Pawn.White-66.Pawn.White-76.Pawn.White-07.Rook.White-17.Knight.White-27.Bishop.White-37..-47.King.White-57..-67..-77.Rook.White');

SELECT * FROM Users;
SELECT * FROM Messages;
SELECT * FROM Games;

USE master;
GO