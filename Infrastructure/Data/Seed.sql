INSERT INTO `MovieDB`.`Users` (`Id`, `FirstName`, `LastName`, `Username`) VALUES ('1', 'Andrew', 'Stears', 'astears');
INSERT INTO `MovieDB`.`Users` (`Id`, `FirstName`, `LastName`, `Username`) VALUES ('2', 'John', 'Doe', 'jdoe');


INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('1', '1');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('2', '2');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('3', '3');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('4', '4');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('5', '5');

INSERT INTO `MovieDB`.`MovieCollections` (`Id`, `Name`, `Description`, `UserId`) VALUES ('1', 'Favorites', 'Favorite Movies', '1');
INSERT INTO `MovieDB`.`MovieCollections` (`Id`, `Name`, `Description`, `UserId`) VALUES ('2', 'Watchlist', 'Watchlist', '1');
INSERT INTO `MovieDB`.`MovieCollections` (`Id`, `Name`, `Description`, `UserId`) VALUES ('3', 'Favorites 2', 'Favorite Movies 2', '2');
INSERT INTO `MovieDB`.`MovieCollections` (`Id`, `Name`, `Description`, `UserId`) VALUES ('4', 'Watchlist 2', 'Watchlist 2', '2');
INSERT INTO `MovieDB`.`MovieCollections` (`Id`, `Name`, `Description`, `UserId`) VALUES ('5', 'Horror', 'Horror movies', '1');
INSERT INTO `MovieDB`.`MovieCollections` (`Id`, `Name`, `Description`, `UserId`) VALUES ('6', 'Comedy', 'Comedy movies', '2');

INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('1', '1');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('2', '2');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('3', '3');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('4', '4');
INSERT INTO `MovieDB`.`Ratings` (`Id`, `Value`) VALUES ('5', '5');

INSERT INTO `MovieDB`.`MovieRatings` (`MovieId`, `RatingId`, `UserId`, `Id`) VALUES ('1', '1', '1', '1');
INSERT INTO `MovieDB`.`MovieRatings` (`MovieId`, `RatingId`, `UserId`, `Id`) VALUES ('2', '2', '1', '2');
INSERT INTO `MovieDB`.`MovieRatings` (`MovieId`, `RatingId`, `UserId`, `Id`) VALUES ('3', '3', '1', '3');
INSERT INTO `MovieDB`.`MovieRatings` (`MovieId`, `RatingId`, `UserId`, `Id`) VALUES ('1', '4', '2', '4');
INSERT INTO `MovieDB`.`MovieRatings` (`MovieId`, `RatingId`, `UserId`, `Id`) VALUES ('2', '5', '2', '5');

INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('1', '1');
INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('2', '1');
INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('3', '1');
INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('1', '3');
INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('1', '4');
INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('4', '2');
INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('5', '2');
INSERT INTO `MovieDB`.`MovieCollectionItems` (`MovieId`, `MovieCollectionId`) VALUES ('6', '2');
